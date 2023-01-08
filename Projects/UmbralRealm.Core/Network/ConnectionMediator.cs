using System.Threading.Tasks.Dataflow;
using UmbralRealm.Core.Network.Interfaces;

namespace UmbralRealm.Core.Network
{
    public class ConnectionMediator : IConnectionMediator
    {
        /// <summary>
        /// Buffer holding connections that allows observers to subscribe.
        /// </summary>
        private readonly BufferBlock<IWriteConnection> connections = new();

        /// <inheritdoc/>
        public async Task Publish(IWriteConnection connection)
        {
            await connections.SendAsync(connection);
        }

        /// <inheritdoc/>
        public void Subscribe(IConnectionSubscriber subscriber)
        {
            connections.LinkTo(this.CreateActionBlock<IWriteConnection>(subscriber.HandleConnection));
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
