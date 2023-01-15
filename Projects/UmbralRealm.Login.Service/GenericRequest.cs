using MediatR;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Service
{
    public class GenericRequest<TInput> : IGenericRequest<IPacket> where TInput : IPacket
    {
        public IWriteConnection Connection { get; private set; }

        public readonly TInput Packet;

        public GenericRequest(IWriteConnection connection, TInput packet)
        {
            this.Connection = connection;
            this.Packet = packet;
        }
    }

    public interface IGenericRequest<TResponse> : IRequest<TResponse>
    {
        IWriteConnection Connection { get; }
    }
}
