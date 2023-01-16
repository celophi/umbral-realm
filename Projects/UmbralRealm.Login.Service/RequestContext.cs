using MediatR;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Login.Service
{
    public class RequestContext<TInput> : IGenericRequest<RequestResult> where TInput : IPacket
    {
        public IWriteConnection Connection { get; private set; }

        public readonly TInput Request;

        public RequestContext(IWriteConnection connection, TInput packet)
        {
            this.Connection = connection;
            this.Request = packet;
        }
    }

    public interface IGenericRequest<TResponse> : IRequest<TResponse>
    {
        IWriteConnection Connection { get; }
    }
}
