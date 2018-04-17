namespace HomeKit.Protocol.TLV
{
    public class Values
    {


        public class kTLVType_Method
        {
            public int Type => 0x00;

            int _value;
            public kTLVType_Method(int value)
            {
                _value = value;
            }

            public int Value => _value;
        }

        public class kTLVType_Identifier
        {
            string _value; public int Type => 0x01;

            public kTLVType_Identifier(string value)
            {
                _value = value;
            }

            public string Value => _value;
        }

        public class kTLVType_Salt
        {
            byte[] _value; public int Type => 0x02;

            public kTLVType_Salt(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_PublicKey
        {
            byte[] _value; public int Type => 0x03;

            public kTLVType_PublicKey(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_Proof
        {
            byte[] _value; public int Type => 0x04;

            public kTLVType_Proof(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_EncryptedData
        {
            byte[] _value; public int Type => 0x05;

            public kTLVType_EncryptedData(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }


        public class kTLVType_State
        {
            int _value; public int Type => 0x06;

            public kTLVType_State(int value)
            {
                _value = value;
            }

            public int Value => _value;
        }

        public class kTLVType_Error
        {
            int _value; public int Type => 0x07;

            public kTLVType_Error(int value)
            {
                _value = value;
            }

            public int Value => _value;
        }

        public class kTLVType_RetryDelay
        {
            int _value; public int Type => 0x08;

            public kTLVType_RetryDelay(int value)
            {
                _value = value;
            }

            public int Value => _value;
        }

        public class kTLVType_Certificate
        {
            byte[] _value; public int Type => 0x09;

            public kTLVType_Certificate(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_Signature
        {
            byte[] _value; public int Type => 0x0A;

            public kTLVType_Signature(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_Permissions
        {
            int _value; public int Type => 0x0B;

            public kTLVType_Permissions(int value)
            {
                _value = value;
            }

            public int Value => _value;
        }

        public class kTLVType_FragmentData
        {
            public int Type => 0x0C;

            byte[] _value;
            public kTLVType_FragmentData(byte[] value)
            {
                _value = value;

            }

            public byte[] Value => _value;
        }

        public class kTLVType_FragmentLast
        {
            byte[] _value; public int Type => 0x0D;

            public kTLVType_FragmentLast(byte[] value)
            {
                _value = value;
            }

            public byte[] Value => _value;
        }

        public class kTLVType_Seperator
        {
            public int Type => 0xFF;
            public char Value => '\0';

        }


    }
}
