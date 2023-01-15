using UmbralRealm.Login.Packet.Server;

namespace UmbralRealm.Login.Service.Interfaces
{
    public interface IServerInfoService
    {
        public WorldSelectionPacket BuildWorldSelectionPacket();
    }
}
