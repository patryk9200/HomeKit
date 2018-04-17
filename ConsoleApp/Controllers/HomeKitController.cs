using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleApp.Controllers
{

    public class HomeKitController : Controller
    {
        [HttpGet("/")]
        public string Test()
        {
            return "OK";
        }

        [HttpPost("/pair-setup")]
        public async Task<HttpResponseMessage> Pair()
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

                byte[][] chunks = contentBytes
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / 3)
                    .Select(grp => grp.Select(x => x.Value).ToArray())
                    .ToArray();


                string hex = BitConverter.ToString(contentBytes);

                var tlv = new Mono.Security.ASN1Element(chunks[1], 0); //this works but you need to make sure the bytes are split up BEFORE inputting here..whoop!

                var tlvString = tlv.ToString();
            }


            //var tlv = new StreamReader(Request.Body). .ReadToEnd();

            byte[] m2Resposne = new byte[] { 0x07, 0x01, 0x02 };

            //return m2Resposne;

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

            Stream stream = new MemoryStream(m2Resposne);

            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                //new MediaTypeHeaderValue("application/octet-stream");
                new MediaTypeHeaderValue("application/pairing+tlv8");

            return result;

            //HttpResponseMessage h = new HttpResponseMessage();
            //h.Headers.Add("Content-Type","")

            //return new HttpResponseMessage { Headers Content = StringContent("my text/plain response") }

            //return contentBytes; // $"Host: {Host} ";
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