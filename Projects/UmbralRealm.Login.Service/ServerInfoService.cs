using UmbralRealm.Login.Packet.Server;
using UmbralRealm.Login.Service.Interfaces;

namespace UmbralRealm.Login
{
    public class ServerInfoService : IServerInfoService
    {
        public WorldSelectionPacket BuildWorldSelectionPacket()
        {
            var packet = new WorldSelectionPacket();

            var info = new Packet.WorldSelectionInfo
            {
                WorldId = 1010,
                WorldName = new Core.IO.LengthPrefixedString
                {
                    Text = "Umbral Realm",
                },
                Unknown2 = new Core.IO.LengthPrefixedString
                {
                    Text = "Test",
                }
            };

            info.WorldName.Length = (ushort)info.WorldName.Text.Length;
            info.Unknown2.Length = (ushort)info.Unknown2.Text.Length;
            info.Status = 1;
            info.Unknown6 = 1;
            info.Population = 3;


            packet.DefaultWorldId = 1010;
            packet.WorldCount = 1;
            packet.WorldSelectionInfoList.Add(info);

            return packet;
        }
    }
}
