
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace HomeKit.ConsoleApp
{
    public class SimpleHTTPServer
    {
        private Thread _serverThread;
        private HttpListener _listener;
        private int _port;

        public int Port
        {
            get { return _port; }
            private set { }
        }

        /// <summary>
        /// Construct server with given port.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        /// <param name="port">Port of the server.</param>
        public SimpleHTTPServer(int port)
        {
            this.Initialize(port);
        }

        /// <summary>
        /// Construct server with suitable port.
        /// </summary>
        /// <param name="path">Directory path to serve.</param>
        public SimpleHTTPServer()
        {
            //get an empty port
            TcpListener l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
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

        private void Listen()
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://*:" + _port.ToString() + "/");
            _listener.Start();
            while (true)
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext();
                    Process(context);
                }
                catch (Exception ex)
                {

                }
            }
        }

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
            _serverThread = new Thread(this.Listen);
            _serverThread.Start();
        }

    }
}
