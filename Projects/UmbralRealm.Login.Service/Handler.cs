using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Login.Data;
using UmbralRealm.Login.Packet.Client;
using UmbralRealm.Login.Packet.Server;

namespace UmbralRealm.Login.Service
{
    public class Handler : IConnectionSubscriber
    {
        private BufferBlock<IWriteConnection> _requestQueue = new BufferBlock<IWriteConnection>();

        private Dictionary<Guid, LoginState> _loginStatuses = new();

        public Handler()
        {
            _requestQueue.LinkTo(this.CreateActionBlock<IWriteConnection>(this.HandleConnection));
        }

        public async Task HandleConnection(IWriteConnection connection)
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
                this.Process(connection, packet);
            }

            await _requestQueue.SendAsync(connection);
        }

        private void Process(IWriteConnection connection, IPacket packet)
        {
            if (packet is LoginAuthenticatePacket)
            {
                this.HandleLoginAuthenticatePacket(connection, (LoginAuthenticatePacket)packet);
                return;
            }

            if (packet is WorldAuthenticatePacket)
            {
                this.HandleWorldAuthenticatePacket(connection, (WorldAuthenticatePacket)packet);
                return;
            }
        }

        private void HandleLoginAuthenticatePacket(IWriteConnection connection, LoginAuthenticatePacket packet)
        {

            // TODO: handle

        }

        private void HandleWorldAuthenticatePacket(IWriteConnection connection, WorldAuthenticatePacket packet)
        {
            // TODO: handle

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
