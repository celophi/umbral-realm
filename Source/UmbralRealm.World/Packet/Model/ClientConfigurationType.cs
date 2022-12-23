namespace UmbralRealm.World.Packet.Model
{
    /// <summary>
    /// Represents "client.ini" configuration entry types.
    /// </summary>
    public enum ClientConfigurationType : ushort
    {
        HotkeyFunction = 0,
        HotkeyChannel = 1,
        HotkeyCharacter = 2,
        HotkeyMainShortcut = 3,
        HotkeySubShortcut = 4,
        HotkeyThirdShortcut = 5,
        UnknownA = 6,
        UnknownB = 7,
    }
}
