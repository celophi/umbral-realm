using UmbralRealm.Core.Network;
using UmbralRealm.Core.Network.Packet;

namespace UmbralRealm.World.Packet
{
    /// <summary>
    /// Unique identifier for the packet.
    /// </summary>
    public enum PacketOpcode : ushort
    {
        /// <summary>
        /// Includes MAC address, selected login server IP address and port.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Client, ServerType.World)]
        WorldAuthenticate = 0x0001,

        /// <summary>
        /// Sent from the client regularly to indicate it is still connected to the server.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Client, ServerType.World)]
        Ping = 0x0002,

        /// <summary>
        /// Indicates intent to start the game with selected player.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Client, ServerType.World)]
        StartGame = 0x0003,

        /// <summary>
        /// Creates a new player.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Client, ServerType.World)]
        CreatePlayer = 0x0005,

        /// <summary>
        /// After <see cref="PlayerDeleteAcknowledge"/> has been received, this confirms the player deletion.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Client, ServerType.World)]
        PlayerDeleteConfirm = 0x0006,

        /// <summary>
        /// Sent from the client when the account requests to delete a player.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Client, ServerType.World)]
        PlayerDeleteIntent = 0x001E,

        /// <summary>
        /// Server response indicating a login request has been received.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Server, ServerType.World)]
        WorldLoginAcknowledge = 0x0025,

        /// <summary>
        /// Not sure, but I think this indicates failure. from the server.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Server, ServerType.World)]
        WorldLoginFailure = 0x0026,

        /// <summary>
        /// Sent from the server with all the player info to select from.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Server, ServerType.World)]
        PlayerSelectionList = 0x0029,

        /// <summary>
        /// Response from the server indicating an acknowledgement of <see cref="PlayerDeleteIntent"/>.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Server, ServerType.World)]
        PlayerDeleteAcknowledge = 0x0050,

        /// <summary>
        /// Contains hotkey configuration values found in "connect.ini".
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Client, ServerType.World)]
        ClientConfiguration = 0x005B,

        /// <summary>
        /// Indicates that login to the world server was successful.
        /// From server.
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Server, ServerType.World)]
        WorldLoginSuccess = 0x005D,

        /// <summary>
        /// 
        /// </summary>
        [PacketOpcodeMetadata(PacketOrigin.Server, ServerType.World)]
        StartGameAcknowledge = 0x005F,
    }
}
