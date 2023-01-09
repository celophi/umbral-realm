using System.Threading.Tasks.Dataflow;
using UmbralRealm.Core.Utilities.Interfaces;

namespace UmbralRealm.Core.Utilities
{
    public class BufferBlockMediator<T> : IDataMediator<T>
    {
        /// <summary>
        /// Buffer holding data that allows observers to subscribe.
        /// Currently passes data by reference. Does not clone.
        /// </summary>
        private readonly BroadcastBlock<T> _broadcaster = new(data => data);

        /// <inheritdoc/>
        public async Task Publish(T data)
        {
            await _broadcaster.SendAsync(data);
        }

        /// <inheritdoc/>
        public void Subscribe(IDataSubscriber<T> subscriber)
        {
            var bufferBlock = new BufferBlock<T>();
            bufferBlock.LinkTo(this.CreateActionBlock(subscriber.Handle));
            _broadcaster.LinkTo(bufferBlock);
        }

        /// <summary>
        /// Used for creating TPL dataflow action blocks from function and wrapping them in an exception handler.
        /// Dataflow blocks cannot be used if an exception happens.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        private ActionBlock<T> CreateActionBlock(Func<T, Task> func)
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
