using HomeKit.Protocol.TypeLengthValue;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class TypeLengthValueTests
    {
        [TestMethod]
        public void Encode_SingleMessage()
        {
            TypeLengthValue.TypeValue message1 = new TypeLengthValue.TypeValue { Type = 0x01, Value = new byte[] { 0x01, 0x05, 0x03 } };

            var bytes = TypeLengthValue.Encode(message1);

            Assert.AreEqual(5, bytes.Length);
            Assert.AreEqual(3, bytes[1]);
        }

        [TestMethod]
        public void Encode_ManyMessages()
        {
            TypeLengthValue.TypeValue message1 = new TypeLengthValue.TypeValue { Type = 0x01, Value = new byte[] { 0x01, 0x05, 0x03 } };
            TypeLengthValue.TypeValue message2 = new TypeLengthValue.TypeValue { Type = 0x0A, Value = new byte[] { 0x01, 0x05, 0x03 } };
            TypeLengthValue.TypeValue message3 = new TypeLengthValue.TypeValue { Type = 0x0D, Value = new byte[] { 0x01, 0x05, 0x03 } };

            var bytes = TypeLengthValue.Encode(message1, message2, message3);

            Assert.AreEqual(15, bytes.Length);
            //All messages of length 3
            Assert.AreEqual(3, bytes[1]);
            Assert.AreEqual(3, bytes[6]);
            Assert.AreEqual(3, bytes[11]);
        }

        [TestMethod]
        public void Decode_SingleMessage()
        {
            var bytes = new byte[] { 0x01, 0x01, 0x03 };

            var messages = TypeLengthValue.Decode(bytes);

            Assert.AreEqual(messages.Count, 1, "Should be 1 message");
            Assert.AreEqual(messages[0].Type, 0x01, "Msg 1 Type");
            Assert.AreEqual(messages[0].Value[0], 0x03, "Msg 1 Byte 1");
        }

        [TestMethod]
        public void Decode_ManyMessage()
        {
            var bytes = new byte[] {
                0x01, 0x01, 0x03,
                0x02, 0x03, 0x07,0x07,0x07,
                0x03, 0x05, 0x01,0x02,0x03,0x04,0x05,
                0x04, 0x0A, 0x01,0x02,0x03,0x04,0x05,0x01,0x02,0x03,0x04,0x05
            };

            var messages = TypeLengthValue.Decode(bytes);

            Assert.AreEqual(messages.Count, 4, "Should be 4 messages");

            Assert.AreEqual(messages[1].Type, 0x02, "Msg 2 Type");
            Assert.AreEqual(messages[2].Type, 0x03, "Msg 3 Type");
            Assert.AreEqual(messages[3].Type, 0x04, "Msg 4 Type");

            Assert.AreEqual(messages[3].Value[0], 0x01, "Msg 4 Byte 1");
            Assert.AreEqual(messages[3].Value[1], 0x02, "Msg 4 Byte 2");
            Assert.AreEqual(messages[3].Value[2], 0x03, "Msg 4 Byte 3");
            Assert.AreEqual(messages[3].Value[3], 0x04, "Msg 4 Byte 4");
            Assert.AreEqual(messages[3].Value[4], 0x05, "Msg 4 Byte 5");
            Assert.AreEqual(messages[3].Value[9], 0x05, "Msg 4 Byte 10");
        }


    }
}
