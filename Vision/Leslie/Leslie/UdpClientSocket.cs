//Code Originated from MSDN
//Modified by Steven Song
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Collections.Generic;
using System.Collections.Concurrent;


namespace NGCP.IO
{
    public class UdpClientSocket : Link
    {
        #region Properties
        /// <summary>
        /// IP address or Host Name of Tcp Client
        /// </summary>
        public IPAddress IpAddress { get; private set; }
        /// <summary>
        /// IP address of Tcp Client
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// ManualResetEvent instances signal completion for terminate
        /// </summary>
        ManualResetEvent TerminateFlag = new ManualResetEvent(false);
        /// <summary>
        /// Socket if connection
        /// </summary>
        UdpClient Udp;
        #endregion Properties


        #region Contructor
        /// <summary>
        /// Construct a Tcp Client Socket
        /// </summary>
        /// <param name="Port">Port Number</param>
        public UdpClientSocket(string Host, int Port)
        {
            this.TerminateFlag.Reset();
            // Find out the local endpoint for the socket.
            IPHostEntry ipHostInfo = Dns.Resolve(Host);
            IpAddress = ipHostInfo.AddressList[0];
            this.Port = Port;
            this.EscapeToken = Encoding.ASCII.GetBytes("<EOF/>");
        }
        /// <summary>
        /// Construct a Tcp Client Socket
        /// </summary>
        /// <param name="Port">Port Number</param>
        public UdpClientSocket(IPAddress IpAddress, int Port)
        {
            this.TerminateFlag.Reset();
            this.IpAddress = IpAddress;
            this.Port = Port;
            this.EscapeToken = Encoding.ASCII.GetBytes("<EOF/>");
        }
        #endregion Contructor


        #region Control Methods
        /// <summary>
        /// Stop a Tcp Client from listening to connection
        /// </summary>
        public override void Stop()
        {
            TerminateFlag.Set();
        }
        /// <summary>
        /// Start a thread of
        /// </summary>
        public override void Start()
        {
            Thread workThread = new Thread(StartWork);
            workThread.Start();
        }
        /// <summary>
        /// Start a connection of Tcp Client Socket
        /// </summary>
        void StartWork()
        {
            //set Terminate flag
            TerminateFlag.Reset();
            // Connect to a remote device.
            try
            {
                IPEndPoint remoteEP = new IPEndPoint(IpAddress, Port);
                // Create Udp Client
                Udp = new UdpClient(remoteEP);
                // Create the state object.
                UdpStateObject state = new UdpStateObject();
                state.client = Udp;
                state.ep = remoteEP;
                // Connect to the remote endpoint
                Udp.BeginReceive(ReadCallback, state);
                //wait until stop is called
                TerminateFlag.WaitOne();
                // Release the socket.
                Udp.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        /// <summary>
        /// Add a message to server queue that will be send to server
        /// </summary>
        /// <param name="Message"></param>
        public override void Send(byte[] Message)
        {
            SendMessage(Udp, Message);
        }
        #endregion Control Methods


        #region Private Functions
        /// <summary>
        /// Read a Callback send from client
        /// </summary>
        /// <param name="ar"></param>
        void ReadCallback(IAsyncResult ar)
        {
            try
            {
                UdpStateObject state = (UdpStateObject)(ar.AsyncState);
                // Read data from the remote device.
                UdpClient client = state.client;
                IPEndPoint ep = state.ep;
                byte[] buffer = client.EndReceive(ar, ref ep);
                if (buffer.Length > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.message.AddRange(buffer);
                    //if eof detected
                    int loc = findEOF(state.message,EscapeToken);;
                    if (loc > -1)
                    {
                        //fetch package
                        byte[] package = new byte[loc];
                        for (int i = 0; i < loc; i++)
                            package[i] = state.message[i];
                        //take off read package
                        state.message.RemoveRange(0, loc + EscapeToken.Length);
                        // All the data has been read from the client. 
                        //trigger new message event
                        if (PackageReceived != null)
                            PackageReceived(package);
                        //establish new message and read more
                        UdpStateObject newState = new UdpStateObject();
                        newState.client = client;
                        newState.ep = ep;
                        client.BeginReceive(new AsyncCallback(ReadCallback), newState);
                    }
                    else
                    {
                        // Get the rest of the data.
                        client.BeginReceive(new AsyncCallback(ReadCallback), state);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private int findEOF(List<byte> message, byte[] token)
        {
            int i = 0;
            int tokenI = 0;
            //int index = -1;
            for(i = 0; i < message.Count;i++)
            {
                if (message[i] == token[tokenI])
                {
                    if (tokenI == token.Length)
                        return i - token.Length;
                    else
                    {
                        tokenI++;
                    }
                }
                else
                    tokenI = 0;
            }
            return -1;
        }
        private void SendMessage(UdpClient client, byte[] data)
        {
            // Begin sending the data to the remote device.
            client.BeginSend(data, data.Length,
                new AsyncCallback(SendCallback), client);
        }


        private void SendCallback(IAsyncResult ar)
        {
            
            ////Create the state object.
            //UdpStateObject state = new UdpStateObject();
            //state.client = Udp;
            //state.ep = remoteEP;
            
            //try
            //{
            //    // Retrieve the socket from the state object.
            //    Socket client = (Socket)ar.AsyncState;
            //    // Complete sending the data to the remote device.
            //    int bytesSent = client.EndSend(ar);
            //    Console.WriteLine("Sent {0} bytes to server.", bytesSent);
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.ToString());
            //}
            
        }
        #endregion Private Functions
    }


    // State object for reading client data asynchronously
    public class UdpStateObject
    {
        // Ip end point
        public IPEndPoint ep;
        // Client  socket.
        public UdpClient client;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public List<byte> message = new List<byte>();
    }
}


