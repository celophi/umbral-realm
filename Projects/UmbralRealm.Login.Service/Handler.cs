using System.Threading.Tasks.Dataflow;
using MediatR;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Utilities.Interfaces;
using UmbralRealm.Login.Packet.Server;

namespace UmbralRealm.Login.Service
{
    public class Handler : IDataSubscriber<IWriteConnection>
    {
        private BufferBlock<IWriteConnection> _requestQueue = new BufferBlock<IWriteConnection>();

        private Dictionary<Guid, LoginState> _loginStatuses = new();

        private readonly IMediator _mediator;

        public Handler(IMediator mediator)
        {
            _requestQueue.LinkTo(this.CreateActionBlock<IWriteConnection>(this.Handle));
            _mediator = mediator;
        }

        public async Task Handle(IWriteConnection connection)
        {
            if (connection?.IsConnected != true)
            {
                connection?.Disconnect();
                return;
            }

            if (!_loginStatuses.TryGetValue(connection.Id, out LoginState state))
            {
                _loginStatuses.Add(connection.Id, LoginState.VerifyCredentials);

                await connection.SendAsync(new ClientVersionPacket
                {
                    Version = 7.0f
                });

                await _requestQueue.SendAsync(connection);
                return;
            }

            if (connection.TryGetPacket(out var packet))
            {
                await this.Process(connection, packet);
            }

            await _requestQueue.SendAsync(connection);
        }

        private async Task Process(IWriteConnection connection, IPacket packet)
        {
            // TODO: Perhaps there is a way to accomplish this without activator.

            var packetType = packet.GetType();
            var gen = typeof(RequestContext<>).MakeGenericType(new[] { packetType });
            var request = Activator.CreateInstance(gen, new object[] { connection, packet });


            try
            {
                var result = await _mediator.Send(request!);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Used for creating TPL dataflow action blocks from function and wrapping them in an exception handler.
        /// Dataflow blocks cannot be used if an exception happens.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private ActionBlock<T> CreateActionBlock<T>(Func<T, Task> func)
        {
            try
            {
                return new ActionBlock<T>(func);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
