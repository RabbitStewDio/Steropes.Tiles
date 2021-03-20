namespace Steropes.Tiles.Demo.Core.GameData.Util
{
    public static class BitVectors
    {
        static readonly uint[] readMasks;
        static readonly uint[] writeMasks;

        static BitVectors()
        {
            readMasks = new uint[32];
            writeMasks = new uint[32];
            uint one = 1;
            for (int i = 0; i < 31; i += 1)
            {
                uint bit = one << i;
                readMasks[i] = bit;
                writeMasks[i] = ~bit;
            }
        }

        public static bool Read(this byte data, int index)
        {
            return (data & readMasks[index]) != 0;
        }

        public static byte Write(this byte data, int index, bool val)
        {
            if (val)
            {
                return (byte)((data | readMasks[index]) & 0xFF);
            }
            else
            {
                return (byte)((data & writeMasks[index]) & 0xFF);
            }
        }

        public static bool Read(this ushort data, int index)
        {
            return (data & readMasks[index]) == readMasks[index];
        }

        public static ushort Write(this ushort data, int index, bool val)
        {
            if (val)
            {
                return (ushort)((data | readMasks[index]) & 0xFFFF);
            }
            else
            {
                return (ushort)((data & writeMasks[index]) & 0xFFFF);
            }
        }

        public static bool Read(this uint data, int index)
        {
            return (data & readMasks[index]) != 0;
        }

        public static uint Write(this uint data, int index, bool val)
        {
            if (val)
            {
                return ((data | readMasks[index]) & 0xFFFF);
            }
            else
            {
                return ((data & writeMasks[index]) & 0xFFFF);
            }
        }
    }
}
