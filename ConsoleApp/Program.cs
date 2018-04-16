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


//var connString = $"tcp://0.0.0.0:51820";
//using (var server = new MessageWire.Host(connString))
//{
//    Console.WriteLine($"SRP Server Started");

//    server.MessageReceived += (s, e) =>
//    {
//        Console.WriteLine($"SRP Server message recieved...");

//        foreach (var frame in e.Message.Frames)
//        {
//            Console.WriteLine(frame.ConvertToString());
//        }
//        //Console.WriteLine($"SRP: {e.Message.Frames[0].ConvertToString()}")
//        //Assert.Equal("Hello, I'm the client.", e.Message.Frames[0].ConvertToString());
//        //Assert.Equal("This is my second line.", e.Message.Frames[1].ConvertToString());

//        //var replyData = new List<byte[]>();
//        //replyData.Add("Hello, I'm the server. You sent.".ConvertToBytes());
//        //replyData.AddRange(e.Message.Frames);
//        //server.Send(e.Message.ClientId, replyData);
//        //serverReceived = true;
//        //Assert.True(e.Message.Frames.Count == 2, "Server received message did not have 2 frames.");
//        //Assert.True(replyData.Count == 3, "Server message did not have 3 frames.");
//    };

//    //using (var client = new Client(connString))
//    //{
//    //    Console.WriteLine($"SRP Client Started");

//    //    client.MessageReceived += (s, e) =>
//    //    {
//    //        //clientReceived = true;
//    //        Console.WriteLine($"SRP Client message recieved...");
//    //    };

//    //    var clientMessageData = new List<byte[]>();
//    //    clientMessageData.Add("Hello, I'm the client.".ConvertToBytes());
//    //    clientMessageData.Add("This is my second line.".ConvertToBytes());
//    //    client.Send(clientMessageData);

//    //    //var count = 0;
//    //    //while (count < 20 && (!clientReceived || !serverReceived))
//    //    //{
//    //    //    Thread.Sleep(20);
//    //    //    count++;
//    //    //}
//    //}
//}


//private static async Task StartMultiCast()
//{
//    var mcastListener = await _httpListener.UdpMulticastHttpRequestObservable("239.255.255.250", PORT, false);

//    mcastListener.Subscribe(msg =>
//    {
//        System.Console.WriteLine($"Method: {msg.Method}, Request type: {msg.RequestType}");
//    });

//}
//private static async void StartTcpListener()
//{
//    System.Console.WriteLine("Start Listener");

//    var listenerConfig = Initializer.GetListener("192.168.1.98", PORT);
//    _httpListener = listenerConfig.httpListener;

//    var observerListener = await _httpListener.TcpHttpRequestObservable(
//        port: 8000,
//        allowMultipleBindToSamePort: true);

//    await StartMultiCast();

//    System.Console.WriteLine("Listener Started");

//    // Rx Subscribe
//    observerListener.Subscribe(
//        async request =>
//        {

//            //Enter your code handling each incoming Http request here.

//            //Example response
//            System.Console.WriteLine($"Remote Address: {request.RemoteAddress}");
//            System.Console.WriteLine($"Remote Port: {request.RemotePort}");
//            System.Console.WriteLine("--------------***-------------");
//            if (request.RequestType == RequestType.TCP)
//            {
//                //var response = new SimpleHttpServer.Model.HttpResponse
//                //{ 
//                //    StatusCode = (int)System.Net.HttpStatusCode.OK,
//                //    ResponseReason = System.Net.HttpStatusCode.OK.ToString(),
//                //    Headers = new Dictionary<string, string>
//                //            {
//                //            {"Date", DateTime.UtcNow.ToString("r")},
//                //            {"Content-Type", "text/html; charset=UTF-8" },
//                //            },
//                //    Body = new MemoryStream(Encoding.UTF8.GetBytes($"<html>\r\n<body>\r\n<h1>Hello, World! {DateTime.Now}</h1>\r\n</body>\r\n</html>"))
//                //};

//                //await _httpListener.HttpSendReponseAsync(request, response).ConfigureAwait(false);
//            }

//        });
//}