using BinarySerialization;
using UmbralRealm.Core.Network;
using UmbralRealm.Core.Network.Packet;
using UmbralRealm.Core.Network.Packet.Interfaces;
using UmbralRealm.Core.Network.Packet.Model.Generic;
using UmbralRealm.Core.Utilities.Interfaces;

namespace UmbralRealm.Proxy
{
    public class PacketLogger : IDataSubscriber<IPacket>
    {
        private readonly OpcodeMapping _mapping;

        public PacketLogger(OpcodeMapping mapping)
        {
            _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        }

        public Task Handle(IPacket data)
        {
            if (_mapping.TryGetByModel(data.GetType(), out var map))
            {
                Console.WriteLine($"ServerType: {map.ServerType}");
                Console.WriteLine($"Origin: {map.Origin}");

                if (map.ServerType == ServerType.Login)
                {
                    var opcode = (Login.Packet.PacketOpcode)map.Opcode;
                    Console.WriteLine($"Opcode: {opcode}");
                }

                if (map.ServerType == ServerType.World)
                {
                    var opcode = (World.Packet.PacketOpcode)map.Opcode;
                    Console.WriteLine($"Opcode: {opcode}");
                }
            }
            else if (data is UnknownPacket unknownPacket)
            {
                //Console.WriteLine($"ServerType: {unknownPacket.ServerType}");
                Console.WriteLine($"Origin: {unknownPacket.Origin}");
                Console.WriteLine($"Opcode: {unknownPacket.Opcode}");
            }

            using var stream = new MemoryStream();
            var serializer = new BinarySerializer();
            serializer.Serialize(stream, data);

            Console.WriteLine($"{BitConverter.ToString(stream.ToArray())}");
            Console.WriteLine();

            return Task.CompletedTask;
        }
    }
}
