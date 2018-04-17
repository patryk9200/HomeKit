
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ConsoleApp
{
    public class TcpServer
    {
        private Thread _serverThread;
        private TcpListener _TcpListener;
        private int _port;

        public int Port
        {
            get { return _port; }
            private set { }
        }

        /// <summary>
        /// Constructs a TCP Listener on a specific port and proxies between an internal wrapped HttpClient
        /// </summary>
        public TcpServer(int port)
        {
            this._port = port;

            _serverThread = new Thread(this.ListenTCP);
            _serverThread.Start();
        }

        /// <summary>
        /// Stop server and dispose all functions.
        /// </summary>
        public void Stop()
        {
            _serverThread.Abort();
            _TcpListener.Stop();

        }

        private void ListenTCP()
        {
            _TcpListener = new TcpListener(IPAddress.Any, _port);
            _TcpListener.Start();


            // Buffer for reading data
            Byte[] bytes = new Byte[1024];

            Console.WriteLine("Starting TCP Listener");

            while (true)
            {
                try
                {
                    TcpClient client = _TcpListener.AcceptTcpClient();

                    NetworkStream stream = client.GetStream();


                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {

                        //debug 

                        var ascii = Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("** TCP REQUEST ** ");
                        Console.WriteLine("{0}", ascii);


                        //TODO: Detect and decrypt stage here then pass into proxy

                        TcpClient tcpProxy = new TcpClient("localhost", 5000);

                        //ascii = ascii.Replace($"Host: {_bonjourServiceName}._hap._tcp.local", "Host: 127.0.0.1:5000"); //why does Apple send the host like this?
                        //OMG my regex sux.. Can you fix this please. Thanks.
                        //string t = Regex.Replace(ascii, "^(Host:).*", "Host: 127.0.0.1:5000" + Environment.NewLine);


                        //byte[] request = Encoding.ASCII.GetBytes(ascii);

                        NetworkStream ns = tcpProxy.GetStream();
                        ns.Write(bytes, 0, i);
                        //ns.Write(request, 0, request.Length);

                        Console.WriteLine("");



                        // String to store the response ASCII representation.
                        //string responseData = String.Empty;
                        byte[] responseData = new Byte[1024];
                        //// Read the first batch of the TcpServer response bytes.
                        Int32 bytesRecieved = ns.Read(responseData, 0, responseData.Length);
                        string responseAscii = System.Text.Encoding.ASCII.GetString(responseData, 0, bytesRecieved);
                        Console.WriteLine("** TCP RESPSONE **");
                        Console.WriteLine(responseAscii);

                        Console.WriteLine("");

                        //stream.Close();
                        //tcpProxy.Close();

                    }

                    client.Close();
                }
                catch (Exception ex)
                {

                }
            }


        }

    }

}
