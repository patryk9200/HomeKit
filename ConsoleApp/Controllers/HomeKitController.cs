using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Eneter.SecureRemotePassword;
using Microsoft.AspNetCore.Mvc;

//using System.Security.Cryptography .Asn1;
using static HomeKit.Protocol.TLV.Values;
using static HomeKit.Protocol.TypeLengthValue.TypeLengthValue;

namespace ConsoleApp.Controllers
{

    public class HomeKitController : Controller
    {
        //[HttpGet("/")]
        //public string Test()
        //{
        //    return "OK";
        //}

        public List<TypeValue> Decode()
        {
            var result = new List<TypeValue>();

            if (Request.Body.CanSeek)
            {
                // Reset the position to zero to read from the beginning.
                Request.Body.Position = 0;
            }

            byte[] contentBytes;

            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = Request.Body.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                contentBytes = ms.ToArray();

                result = HomeKit.Protocol.TypeLengthValue.TypeLengthValue.Decode(contentBytes);

                //string hex = BitConverter.ToString(contentBytes);
            }

            return result;
        }

        [HttpPost("/pair-setup")]
        public async void Pair()
        {

            var request = Decode();

            var sequence = request.FirstOrDefault(x => x.Type == kTLVType_State.Type);

            byte[] responseBytes = new byte[0];

            if (sequence.Value[0] == 0x01)
                responseBytes = handlePairStepOne(request); //._handlePairStepOne(request, response, session);
            else if (sequence.Value[0] == 0x03)
                handlePairStepTwo(request); //, response, session, objects);
            else if (sequence.Value[0] == 0x05)
                handlePairStepThree(request);//, response, session, objects);


            //var response = TestErrorResponse(); // M1M2Response();


            Response.Headers.Add("Content-Type", "application/pairing+tlv8");
            Response.Headers.Add("Content-Length", responseBytes.Length.ToString());

            await Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
        }

        public static byte[] _SRP_b;  //Service Secret 'b'
        public static byte[] _SRP_Salt; //session salt

        private byte[] handlePairStepOne(List<TypeValue> requestValue)
        {
            _SRP_Salt = SRP.s();

            _SRP_b = SRP.b();
            var vBytes = Encoding.UTF8.GetBytes("Pair-Setup");
            var publicKey = SRP.B(_SRP_b, vBytes);


            var sequence = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_State.Type, Value = new byte[] { 0x02 } };
            var saltMessage = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_Salt.Type, Value = _SRP_Salt };
            var publicKeyMessage = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_PublicKey.Type, Value = publicKey };

            var result = HomeKit.Protocol.TypeLengthValue.TypeLengthValue.Encode(sequence, saltMessage, publicKeyMessage);

            return result;
        }

        private byte[] handlePairStepTwo(List<TypeValue> requestValue)
        {
            //var newSalt = SRP.s();
            var a = requestValue.FirstOrDefault(x => x.Type == kTLVType_PublicKey.Type); // "A is a public key that exists only for a single login session."
            var M1 = requestValue.FirstOrDefault(x => x.Type == kTLVType_Proof.Type);    // "M1 is the proof that you actually know your own password."

            var clientsA = SRP.A(a.Value);

            var session = SRP.K_Service(a.Value, Encoding.UTF8.GetBytes("Pair-Setup"), Encoding.UTF8.GetBytes("11111111"), _SRP_b);

            var _M2 = SRP.M2(clientsA, M1.Value, session);

            var sequence = new TypeValue() { Type = kTLVType_State.Type, Value = new byte[] { 0x04 } };
            var passowrdProof_M2 = new TypeValue() { Type = kTLVType_Proof.Type, Value = _M2 };

            var result = HomeKit.Protocol.TypeLengthValue.TypeLengthValue.Encode(sequence, passowrdProof_M2);

            return result;
        }

        private byte[] handlePairStepThree(List<TypeValue> requestValue)
        {
            var newSalt = SRP.s();

            return new byte[0];
        }

        private byte[] TestErrorResponse()
        {
            //spec says should abort user
            var statem2 = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_State.Type, Value = new byte[] { 0x02 } };
            var erro = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_Error.Type, Value = new byte[] { 0x07 } };
            var response = HomeKit.Protocol.TypeLengthValue.TypeLengthValue.Encode(statem2, erro);
            return response;
            //byte[] control = new byte[] {
            //    0x06,0x01,0x02, //State M2
            //    0x07, 0x01, 0x07, //Error BUSY
            //};
        }

        //private byte[] M1M2Response()
        //{
        //    //salt needs persisting somewhere?
        //    var newSalt = SRP.s();

        //    var m2Message = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_State.Type, Value = new byte[] { 0x02 } };
        //    var publicKeyMessage = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_PublicKey.Type, Value = new byte[] { 0x07 } };
        //    var saltMessage = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_Salt.Type, Value = newSalt };

        //    var result = HomeKit.Protocol.TypeLengthValue.TypeLengthValue.Encode(m2Message, publicKeyMessage, saltMessage);

        //    return result;

        //    //byte[] m2Resposne = new byte[] {
        //    //    0x06 ,0x01,0x02, //State M2
        //    //    0x03, 0x01, 0x07, //Public Key
        //    //    0x02 //Salt
        //    //};

        //}

        //[HttpPost("/pair-verify")]
        //public async Task<string> PairVerify([FromHeader]string Host, [FromBody]byte[] content)
        //{
        //    return $"Host: {Host} ";
        //}
    }


}


//Tag 00 Length 1 Value 00 

//010 611