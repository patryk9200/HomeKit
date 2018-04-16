using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace ConsoleApp
{
    public class HttpServer
    {

        private Thread _serverThread;


        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.UseUrls("http://*:5000")
                .Build();



        public HttpServer()
        {
            _serverThread = new Thread(this.Start);
            _serverThread.Start();

        }

        private void Start()
        {
            //BuildWebHost(args).Run();
            string[] args = new string[0];
            BuildWebHost(args).Run();
        }

        public void Stop()
        {
            _serverThread.Abort();

        }

    }

}