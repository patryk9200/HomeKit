
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleApp
{
    public class SimpleHTTPServer
    {
        private Thread _serverThread;
        private HttpListener _listener;
        private TcpListener _listenerTCP;
        private int _port;

        public int Port
        {
            get { return _port; }
            private set { }
        }

        /// <summary>
        /// Construct server with suitable port.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        public SimpleHTTPServer(int port)
        {
            //get an empty port
            //TcpListener l = new TcpListener(IPAddress.Any, port);
            //l.Start();
            //int port = ((IPEndPoint)l.LocalEndpoint).Port;
            //l.Stop();
            this.Initialize(port);
        }

        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        public void Stop()
        {
            _serverThread.Abort();
            _listener.Stop();
        }

        private void ListenTCP()
        {
            _listenerTCP = new TcpListener(IPAddress.Any, _port);
            //_listener.Prefixes.Add("http://*:" + _port.ToString() + "/");
            _listenerTCP.Start();


            // Buffer for reading data
            Byte[] bytes = new Byte[512];


            Console.WriteLine("Starting TCP Listener");
            while (true)
            {
                try
                {
                    TcpClient client = _listenerTCP.AcceptTcpClient();
                    //var context = _listenerTCP. .GetContext();
                    NetworkStream stream = client.GetStream();
                    //Process(context);



                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {

                        var request = new Request(bytes, i);

                        Console.WriteLine("Received: {0}", "");


                        // Process the data sent by the client.
                        //data = data.ToUpper();

                        //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        //// Send back a response.
                        //stream.Write(msg, 0, msg.Length);
                        //Console.WriteLine("Sent: {0}", data);
                    }

                    client.Close();
                }
                catch (Exception ex)
                {

                }
            }
        }

        public class Request
        {

            public Request(byte[] bytes, int length)
            {
                Headers = new Dictionary<string, string>();

                var ascii = Encoding.ASCII.GetString(bytes, 0, length);

                string[] lines = ascii.Split(new[] { "\r\n" }, StringSplitOptions.None);

                for (int i = 0; i < lines.Length; i++)
                {

                    if (i == 0)
                    {
                        var hdr = lines[i].Split(" ");
                        Type = hdr[0];
                        Path = hdr[1];
                        continue;
                    }

                    if (lines[i].Contains(":"))
                    {
                        var hdrs = lines[i].Split(':');
                        Headers.Add(hdrs[0], hdrs[1]);
                    }

                    if (lines[i].Length == 0)
                    {
                        Body = Encoding.ASCII.GetBytes(lines[i + 1]);
                        break;
                    }

                }
            }


            public string Type { get; set; }
            public string Path { get; set; }


            public Dictionary<string, string> Headers { get; set; }

            public byte[] Body { get; set; }



        }

        //private void Listen()
        //{
        //    _listener = new HttpListener();
        //    //_listener.Prefixes.Add("http://*:" + _port.ToString() + "/");
        //    _listener.Start();
        //    while (true)
        //    {
        //        try
        //        {
        //            HttpListenerContext context = _listener.GetContext();
        //            Process(context);
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //    }
        //}

        private void Process(HttpListenerContext context)
        {
            string path = context.Request.Url.AbsolutePath;
            Console.WriteLine($"[{context.Request.HttpMethod}] {path}");



            HttpListenerResponse response = context.Response;
            //response.ContentType = "application/pairing+tlv8";
            // Construct a response.
            //string clientInformation = ClientInformation(context);
            response.StatusCode = (int)HttpStatusCode.OK;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes("<HTML><BODY> " + "TEST" + "</BODY></HTML>");
            // Get a response stream and write the response to it.
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            //listener.Stop();


            //string mime;
            //context.Response.ContentType = "application/pairing+tlv8";
            ////context.Response.ContentLength64 = input.Length;
            //context.Response.AddHeader("Date", DateTime.Now.ToString("r"));
            ////context.Response.AddHeader("Last-Modified", System.IO.File.GetLastWriteTime(filename).ToString("r"));
            //byte[] byteArray = Encoding.UTF8.GetBytes("[{test}]");
            //context.Response.ContentLength64 = byteArray.Length;
            //context.Response.OutputStream.Write(byteArray, 0, byteArray.Length);
            //context.Response.StatusCode = (int)HttpStatusCode.OK;
            //context.Response.OutputStream.Flush();
            //context.Response.StatusCode = (int)HttpStatusCode.NotFound;

        }


        private void Initialize(int port)
        {
            this._port = port;
            _serverThread = new Thread(this.ListenTCP);
            _serverThread.Start();
        }

    }
}
