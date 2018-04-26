using System;
using Mono.Zeroconf;

namespace ConsoleApp
{
    class Program
    {
        private const int PORT = 51820;

        //use simple name, no spaces, no specials
        private const string ServiceName = "HomeKitSharpBridge";

        static void Main(string[] args)
        {

            Console.WriteLine("Starting Bonjour");
            StartBonjourService();

            var httpServer = new HttpServer();

            var tcpServer = new TcpServer(PORT);

            Console.ReadLine();

            //Stop method should be called before exit.
            httpServer.Stop();
            tcpServer.Stop();

        }

        private static void StartBonjourService()
        {
            RegisterService service = new RegisterService();
            service.Name = ServiceName;
            service.RegType = "_hap._tcp";
            service.ReplyDomain = "local.";
            service.Port = unchecked((short)PORT); //documentated work around
                                                   // TxtRecords are optional
            TxtRecord txt_record = new TxtRecord();
            txt_record.Add("c#", "1");
            txt_record.Add("ff", "0");
            txt_record.Add("id", "22:32:43:54:54:01");
            txt_record.Add("md", ServiceName);
            txt_record.Add("pv", "1.0");
            txt_record.Add("s#", "1");
            txt_record.Add("sf", "1");
            txt_record.Add("ci", "2");

            service.TxtRecord = txt_record;

            service.Response += OnRegisterServiceResponse;
            service.Register();

            //ServiceBrowser browser = new ServiceBrowser();
            //browser.ServiceAdded += delegate (object o, ServiceBrowseEventArgs aargs)
            //{
            //    Console.WriteLine("Found Service: {0}", aargs.Service.Name);

            //    aargs.Service.Resolved += delegate (object oo, ServiceResolvedEventArgs argss)
            //    {
            //        IResolvableService s = (IResolvableService)argss.Service;
            //        Console.WriteLine("Resolved Service: {0} - {1}:{2} ({3} TXT record entries)",
            //            s.FullName, s.HostEntry.AddressList[0], s.Port, s.TxtRecord.Count);
            //    };

            //    aargs.Service.Resolve();
            //};

            //browser.Browse("_hap._tcp", "local");
        }

        private static void OnRegisterServiceResponse(object o, RegisterServiceEventArgs args)
        {
            switch (args.ServiceError)
            {
                case ServiceErrorCode.NameConflict:
                    Console.WriteLine("*** Name Collision! '{0}' is already registered",
                        args.Service.Name);
                    break;
                case ServiceErrorCode.None:
                    Console.WriteLine("*** Registered name = '{0}'", args.Service.Name);
                    break;
                case ServiceErrorCode.Unknown:
                    Console.WriteLine("*** Error registering name = '{0}'", args.Service.Name);
                    break;
            }
        }
    }
}