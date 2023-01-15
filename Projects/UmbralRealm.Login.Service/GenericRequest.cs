using MediatR;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Service
{
    public class GenericRequest<TPacket> : IRequest<IPacket> where TPacket : IPacket
    {
        public readonly IWriteConnection Connection;

        public readonly TPacket Packet;

        public GenericRequest(IWriteConnection connection, TPacket packet)
        {
            this.Connection = connection;
            this.Packet = packet;
        }
    }

    public interface IGenericRequest<TResponse> : IRequest<TResponse>
    {

    }
}
