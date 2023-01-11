using UmbralRealm.Login.Packet.Server;

namespace UmbralRealm.Login.Interfaces
{
    public interface IServerInfoService
    {
        public WorldSelectionPacket BuildWorldSelectionPacket();
    }
}
