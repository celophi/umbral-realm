namespace UmbralRealm.Core.Player
{
    /// <summary>
    /// Bitfield that represents how a player looks.
    /// </summary>
    [Flags]
    public enum PlayerAppearance : ulong
    {
        /// <summary>
        /// The default value represents the first value of all attributes, and a female player.
        /// </summary>
        Default =       0x0000000000000000,

        Hair2 =         0x0000000000000040,
        Hair3 =         0x0000000000000080,
        Hair4 =         0x00000000000000C0,
        Hair5 =         0x0000000000000100,
        Hair6 =         0x0000000000000140,
        Hair7 =         0x0000000000000180,
        Hair8 =         0x00000000000001C0,

        Face2 =         0x0000000000001000,
        Face3 =         0x0000000000002000,
        Face4 =         0x0000000000003000,
        Face5 =         0x0000000000004000,
        Face6 =         0x0000000000005000,
        Face7 =         0x0000000000006000,
        Face8 =         0x0000000000007000,
        Face9 =         0x0000000000008000,
        Face10 =        0x0000000000009000,

        HairColor2 =    0x0000000000040000,
        HairColor3 =    0x0000000000080000,
        HairColor4 =    0x00000000000C0000,
        HairColor5 =    0x0000000000100000,
        HairColor6 =    0x0000000000140000,
        HairColor7 =    0x0000000000180000,
        HairColor8 =    0x00000000001C0000,
        HairColor9 =    0x0000000000200000,
        HairColor10 =   0x0000000000240000,
        HairColor11 =   0x0000000000280000,
        HairColor12 =   0x00000000002C0000,

        Skin2 =         0x0000000001000000,
        Skin3 =         0x0000000002000000,
        Skin4 =         0x0000000003000000,
        Skin5 =         0x0000000004000000,

        EyeColor2 =     0x0000000040000000,
        EyeColor3 =     0x0000000080000000,
        EyeColor4 =     0x00000000C0000000,
        EyeColor5 =     0x0000000100000000,
        EyeColor6 =     0x0000000140000000,
        EyeColor7 =     0x0000000180000000,
        EyeColor8 =     0x00000001C0000000,
        EyeColor9 =     0x0000000200000000,
        EyeColor10 =    0x0000000240000000,
        EyeColor11 =    0x0000000280000000,
        EyeColor12 =    0x00000002C0000000,
        EyeColor13 =    0x0000000300000000,
        EyeColor14 =    0x0000000340000000,
        EyeColor15 =    0x0000000380000000,
        EyeColor16 =    0x00000003C0000000,
        EyeColor17 =    0x0000000400000000,
        EyeColor18 =    0x0000000440000000,
        EyeColor19 =    0x0000000480000000,
        EyeColor20 =    0x00000004C0000000,
        EyeColor21 =    0x0000000500000000,
        EyeColor22 =    0x0000000540000000,
        EyeColor23 =    0x0000000580000000,
        EyeColor24 =    0x00000005C0000000,
        EyeColor25 =    0x0000000600000000,

        Male =          0x0000001000000000,
    }
}
