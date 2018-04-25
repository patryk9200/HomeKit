using System.Collections.Generic;

namespace HomeKit.Protocol.TypeLengthValue
{
    public static class TypeLengthValue
    {

        public static byte[] Encode(params TypeValue[] typeValues)
        {
            List<byte> bufferList = new List<byte>();

            //TODO: Add support for type values exceeding 255 bytes which must be split as per homekit specification
            foreach (var item in typeValues)
            {
                bufferList.Add(item.Type);
                bufferList.Add((byte)item.Value.Length);
                bufferList.AddRange(item.Value);
            }

            var result = bufferList.ToArray();

            return result;
        }


        public static List<TypeValue> Decode(byte[] bytes)
        {
            var results = new List<TypeValue>();

            for (int i = 0; i < bytes.Length; i++)
            {
                var tv = new TypeValue();

                tv.Type = bytes[i];

                var tmpBytes = new List<byte>();
                var tmpLength = bytes[i + 1];
                for (int j = 1; j <= tmpLength; j++)
                {
                    tmpBytes.Add(bytes[i + j + 1]);
                }

                tv.Value = tmpBytes.ToArray();

                results.Add(tv);

                int delta = tmpLength + 1;
                i += delta;
            }

            return results;
        }


        public class TypeValue
        {
            public byte Type { get; set; }
            //public byte Length { get; set; }
            public byte[] Value { get; set; }

        }

    }
}
