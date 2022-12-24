using UmbralRealm.Core.Network.Interfaces;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Security;

namespace UmbralRealm.Core.Network
{
    public class ConnectionFactory : IConnectionFactory
    {
        private readonly IPacketConverter _packetFactory;

        public ConnectionFactory(IPacketConverter packetFactory)
        {
            _packetFactory = packetFactory ?? throw new ArgumentNullException(nameof(packetFactory));
        }

        public Connection Create(ISocketConnection socketConnection, NetworkCipher networkCipher) => new(socketConnection, networkCipher, _packetFactory);
    }
}
