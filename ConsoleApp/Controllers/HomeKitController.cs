using System.IO;
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
        public async Task<string> Pair()
        {
            if (Request.Body.CanSeek)
            {
                // Reset the position to zero to read from the beginning.
                Request.Body.Position = 0;
            }

            var input = new StreamReader(Request.Body).ReadToEnd();

            return ""; // $"Host: {Host} ";
        }

        //[HttpPost("/pair-verify")]
        //public async Task<string> PairVerify([FromHeader]string Host, [FromBody]byte[] content)
        //{
        //    return $"Host: {Host} ";
        //}
    }
}
