using System;
using System.IO;
using Eneter.SecureRemotePassword;
using Microsoft.AspNetCore.Mvc;

//using System.Security.Cryptography .Asn1;
using static HomeKit.Protocol.TLV.Values;

namespace ConsoleApp.Controllers
{

    public class HomeKitController : Controller
    {
        //[HttpGet("/")]
        //public string Test()
        //{
        //    return "OK";
        //}

        [HttpPost("/pair-setup")]
        public async void Pair()
        {
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

                var requestValues = HomeKit.Protocol.TypeLengthValue.TypeLengthValue.Decode(contentBytes);

                string hex = BitConverter.ToString(contentBytes);
            }


            var response = TestErrorResponse(); // M1M2Response();




            Response.Headers.Add("Content-Type", "application/pairing+tlv8");
            Response.Headers.Add("Content-Length", response.Length.ToString());

            await Response.Body.WriteAsync(response, 0, response.Length);
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

        private byte[] M1M2Response()
        {
            //salt needs persisting somewhere?
            var newSalt = SRP.s();

            var m2Message = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_State.Type, Value = new byte[] { 0x02 } };
            var publicKeyMessage = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_PublicKey.Type, Value = new byte[] { 0x07 } };
            var saltMessage = new HomeKit.Protocol.TypeLengthValue.TypeLengthValue.TypeValue() { Type = kTLVType_Salt.Type, Value = newSalt };

            var result = HomeKit.Protocol.TypeLengthValue.TypeLengthValue.Encode(m2Message, publicKeyMessage, saltMessage);

            return result;

            //byte[] m2Resposne = new byte[] {
            //    0x06 ,0x01,0x02, //State M2
            //    0x03, 0x01, 0x07, //Public Key
            //    0x02 //Salt
            //};

        }

        //[HttpPost("/pair-verify")]
        //public async Task<string> PairVerify([FromHeader]string Host, [FromBody]byte[] content)
        //{
        //    return $"Host: {Host} ";
        //}
    }


}


//Tag 00 Length 1 Value 00 

//010 611