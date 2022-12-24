using System.Diagnostics.CodeAnalysis;
using UmbralRealm.Core.Network.Packet.Interfaces;

namespace UmbralRealm.Core.Network.Packet
{
    /// <summary>
    /// Represents a global mapping between packet opcode and other models, functions, and metadata.
    /// This mapping should be registered at application startup and is expected to stay constant.
    /// </summary>
    public class OpcodeMapping
    {
        /// <summary>
        /// Uses the <see cref="Map.Opcode"/> as the key to <see cref="Map"/> instances.
        /// </summary>
        private readonly IReadOnlyDictionary<ushort, IOpcodeMap> _opcodeMap;

        /// <summary>
        /// Uses the <see cref="Map.Model"/> as the key to <see cref="Map"/> instances.
        /// </summary>
        private readonly IReadOnlyDictionary<Type, IOpcodeMap> _modelMap;

        private OpcodeMapping(List<IOpcodeMap> maps)
        {
            _opcodeMap = maps.ToDictionary(map => map.Opcode, map => map);
            _modelMap = maps.Where(map => map.Model != null).ToDictionary(map => map.Model!, map => map);
        }

        public static OpcodeMapping Create(Enum opcode)
        {
            var maps = new List<OpcodeMap>();

            // Get all attributes decored on the enumeration and build a map.
            var enumFields = opcode.GetType().GetFields();

            foreach (var field in enumFields)
            {
                if (field.GetCustomAttributes(typeof(PacketOpcodeMetadataAttribute), inherit: false).FirstOrDefault() is PacketOpcodeMetadataAttribute attribute)
                {
                    var map = new OpcodeMap();
                    map.Opcode = (ushort)field.GetRawConstantValue()!;
                    map.ServerType = attribute.ServerType;
                    map.Origin = attribute.Origin;
                    maps.Add(map);
                }
            }

            // TODO: If there are models outside of the assembly, need to change something here.
            // Add the model type for all existing maps found.
            var types = opcode.GetType().Assembly.GetTypes();

            foreach (var type in types)
            {
                if (type.GetCustomAttributes(typeof(PacketOpcodeMappingAttribute), inherit: false).FirstOrDefault() is PacketOpcodeMappingAttribute attribute)
                {
                    var found = maps.FirstOrDefault(map => map.Opcode == attribute.Opcode);
                    if (found != null)
                    {
                        found.Model = type;
                    }
                }
            }

            return new OpcodeMapping(maps.Cast<IOpcodeMap>().ToList());
        }

        /// <summary>
        /// Returns an opcode mapping if one has been registered indexed by the opcode.
        /// </summary>
        /// <param name="opcode"></param>
        /// <returns></returns>
        public bool TryGetByOpcode(ushort opcode, [MaybeNullWhen(false)] out IOpcodeMap map) => 
            _opcodeMap.TryGetValue(opcode, out map);
       

        /// <summary>
        /// Returns an opcode mapping if one has been registered indexed by the model type.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool TryGetByModel(Type model, [MaybeNullWhen(false)] out IOpcodeMap map) =>
            _modelMap.TryGetValue(model, out map);
    }
}
