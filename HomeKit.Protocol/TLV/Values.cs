namespace HomeKit.Protocol.TLV
{
    public class Values
    {

        public class kTLVType_Method
        {
            public static byte Type => 0x00;

            int _value;
            public kTLVType_Method(int value)
            {
                _value = value;
            }

            public int Value => _value;
        }

        public class kTLVType_Identifier
        {
            public static byte Type => 0x01;

            string _value;

            public kTLVType_Identifier(string value)
            {
                _value = value;
            }

            public string Value => _value;
        }

        public class kTLVType_Salt
        {
            public static byte Type => 0x02;

            byte[] _value;

            public kTLVType_Salt(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_PublicKey
        {
            public static byte Type => 0x03;

            byte[] _value;

            public kTLVType_PublicKey(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_Proof
        {
            public static byte Type => 0x04;

            byte[] _value;

            public kTLVType_Proof(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_EncryptedData
        {
            public static byte Type => 0x05;

            byte[] _value;

            public kTLVType_EncryptedData(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }


        public class kTLVType_State
        {
            public static byte Type => 0x06;

            int _value;

            public kTLVType_State(int value)
            {
                _value = value;
            }

            public int Value => _value;
        }

        public class kTLVType_Error
        {
            public static byte Type => 0x07;

            int _value;

            public kTLVType_Error(int value)
            {
                _value = value;
            }

            public int Value => _value;
        }

        public class kTLVType_RetryDelay
        {
            public static byte Type => 0x08;

            int _value;

            public kTLVType_RetryDelay(int value)
            {
                _value = value;
            }

            public int Value => _value;
        }

        public class kTLVType_Certificate
        {
            public static byte Type => 0x09;

            byte[] _value;

            public kTLVType_Certificate(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_Signature
        {
            public static byte Type => 0x0A;

            byte[] _value;

            public kTLVType_Signature(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_Permissions
        {
            public static byte Type => 0x0B;

            int _value;

            public kTLVType_Permissions(int value)
            {
                _value = value;
            }

            public int Value => _value;
        }

        public class kTLVType_FragmentData
        {
            public static byte Type => 0x0C;

            byte[] _value;
            public kTLVType_FragmentData(byte[] value)
            {
                _value = value;

            }

            public byte[] Value => _value;
        }

        public class kTLVType_FragmentLast
        {
            public static byte Type => 0x0D;

            byte[] _value;


            public kTLVType_FragmentLast(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_Seperator
        {
            public static byte Type => 0xFF;
            public char Value => '\0';

        }


    }
}
