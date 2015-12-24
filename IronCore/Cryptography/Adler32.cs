namespace IronCore.Cryptography
{
    public class Adler32
    {
        private const ushort CHAR_OFFSET = 31;

        public static uint Calculate(byte[] data)
        {
            return Calculate(data, 0, data.Length);
        }

        public static uint Calculate(byte[] data, int offset, int count)
        {
            ushort s1 = 0;
            ushort s2 = 0;

            int leftoverdata = (count % 4);
            long rounds = count - leftoverdata;

            for (int i = 0; i < rounds; i += 4)
            {
                s2 += (ushort) (
                                   4 * (s1 + data[offset]) +
                                   3 * data[offset + 1] +
                                   2 * data[offset + 2] +
                                   data[offset + 3] +
                                   10 * CHAR_OFFSET);


                s1 += (ushort) (
                                   data[offset + 0] +
                                   data[offset + 1] +
                                   data[offset + 2] +
                                   data[offset + 3] +
                                   4 * CHAR_OFFSET);

                offset += 4;
            }

            for (int i = 0; i < leftoverdata; i++)
            {
                s1 += (ushort) (data[offset + i] + CHAR_OFFSET);
                s2 += s1;
            }

            return (uint) ((s1 & 0xffff) + (s2 << 16));
        }


        public static uint Roll(byte outByte, byte inByte, uint checksum, long bytecount)
        {
            var s1 = (ushort) (checksum & 0xffff);
            var s2 = (ushort) (checksum >> 16);

            s1 += (ushort) (inByte - outByte);
            s2 += (ushort) (s1 - (bytecount * (outByte + CHAR_OFFSET)));

            return (uint) ((s1 & 0xffff) + (s2 << 16));
        }
    }
}