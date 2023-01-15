using MediatR;
using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Login.Packet.Client;

namespace UmbralRealm.Login.Service.Requests
{
    public class LoginAuthenticateRequestHandler : IRequestHandler<GenericRequest<LoginAuthenticatePacket>, IPacket>
    {
        public Task<IPacket> Handle(GenericRequest<LoginAuthenticatePacket> request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
