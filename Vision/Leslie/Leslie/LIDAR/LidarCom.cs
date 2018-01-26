using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;

namespace LisaControlerCsh
{
    public class LidarCom
    {
        private static SerialPort serialPort;
        private TextBox textBox;
        private int byteCount;
        private int charCount;
        private byte[] value;
        private int dataCount;
        private int[] recievedData;
        private bool DataReady;
        //private int LineCount;
        private int dataTime;
        public bool dataBad;
        //private string header;

        private circularArray buffer;
        private struct Command
        {
            public byte[] CommandSymbol;
            public byte Parameter;
            public char[] StringCharacters;
            public byte[] Termination;
        }
        private struct Reply
        {
            public byte[] CommandSysmbol;
            public byte Parameter;
            public char[] StringCharacters;
            public byte[] Status;
            public byte Sum1;
            public char[][] Data;
            public byte[] Sum2;
        }
        public void Connect(string COMport)
        {
            textBox = null;
            if (serialPort == null)
            {
                serialPort = new SerialPort();
            }

            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.PortName = COMport;
                    serialPort.BaudRate = 115200;
                    serialPort.ReadBufferSize = 10000;
                    serialPort.Parity = Parity.None;
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
                    serialPort.ReceivedBytesThreshold = 2;
                    serialPort.Open();
                    //textBox.AppendText("Port Connected \r\n");
                }
                catch(Exception e)
                {
                    throw e;
                    //textBox.AppendText("Error connecting Port \r\n");
                }
            }


        }
        public void Connect(string COMport, TextBox log)
        {
            textBox = log;
            if (serialPort == null)
            {
                serialPort = new SerialPort();
            }

            if (!serialPort.IsOpen)
            {
                try
                {
                    serialPort.PortName = COMport;
                    serialPort.BaudRate = 115200;
                    serialPort.ReadBufferSize = 10000;
                    serialPort.Parity = Parity.None;
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
                    serialPort.ReceivedBytesThreshold = 2;
                    serialPort.Open();
                    textBox.AppendText("Port Connected \r\n");
                }
                catch
                {
                    textBox.AppendText("Error connecting Port \r\n");
                }
            }


        }
        public void Disconnect()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
                if (textBox != null)
                {
                    textBox.AppendText("Serial Port Closed");
                }
            }
        }
        public bool Connected()
        {
            if (serialPort != null)
                return serialPort.IsOpen;
            else
                return false;
        }
        public void RequestData(int start, int stop)
        {
            byte[] dataPacket = new byte[17];
            //byte[] dataRecieved;
            dataPacket[0] = 0x00;//command byte 1
            dataPacket[1] = 0x4d;//command byte 0
            dataPacket[2] = 0x44;//Prameter byte two character encoding
            //byte[] startByte = BitConverter.GetBytes(start);
            //dataPacket[3] = startByte[3];
            //dataPacket[4] = startByte[2];
            //dataPacket[5] = startByte[1];
            //dataPacket[6] = startByte[0];
            //byte[] stopByte = BitConverter.GetBytes(stop);
            //dataPacket[7] = stopByte[3];
            //dataPacket[8] = stopByte[2];
            //dataPacket[9] = stopByte[1];
            
            //dataPacket[10] = stopByte[0];
            dataPacket[3] = 0x30;
            dataPacket[4] = 0x30;
            dataPacket[5] = 0x30;
            dataPacket[6] = 0x30;
            dataPacket[7] = 0x31;
            dataPacket[8] = 0x30;
            dataPacket[9] = 0x38;
            dataPacket[10] = 0x30;
            dataPacket[11] = 0x30;//cluster count byte 1
            dataPacket[12] = 0x31;//cluster count byte 0
            dataPacket[13] = 0x30;//scna interval
            dataPacket[14] = 0x30;//Number of scans byte 1
            dataPacket[15] = 0x31;//Number of scans byte 0
            //No string characters are sent
            //dataPacket[16] = BitConverter.GetBytes('D');
            dataPacket[16] = (byte)'\n';//inset Line terminator
            serialPort.Write(dataPacket, 0, 17);//send request
            string logout = "";
            for (int i = 0; i < 17; i++)
            {
                logout += " " + dataPacket[i].ToString("x2") + " ";
            }
            if(textBox != null)
                textBox.AppendText("\r\nOutput: " + logout + "\r\n");
            byteCount = 0;
            charCount = 0;
            dataCount = 0;
            //LineCount = 0;
            dataBad = false;
            value =  new byte[3];
            recievedData = new int[1180];
            DataReady = false;
            buffer = new circularArray(3);
            dataTime = 0;
            //Recieve scan data
            //dataRecieved = ReadPacket();
            return;

        }
        void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                while (serialPort.BytesToRead > 0)
                {
                    // byte[] data = serialPort.Read
                    for (int i = 0; i < serialPort.BytesToRead; i++)
                    {
                        byte data = (byte)serialPort.ReadByte();

                        if (dataTime > 7)//byteCount > 45)
                        {
                            if (data == 0x0a || (byteCount - 45) % 66 == 0)
                            {
                                //reject line breakse and sum totals
                                //if (charCount > 0)
                                //{
                                //    charCount--;
                                //    //value.Remove(charCount * 2, 2);
                                //}
                                //else
                                //{
                                //    charCount = 0;
                                //    value = new byte[3];
                                //}
                            }
                            else if (charCount < 2)
                            {
                                //value += (data - 0x30)
                                value[charCount] = (byte)(data - 0x30);
                                charCount++;

                            }
                            else
                            {

                                value[charCount] = (byte)(data - 0x30);
                                recievedData[dataCount] = (int)((value[0] << 12) + (value[1] << 6) + value[2]);
                                charCount = 0;
                                value = new byte[3];
                                dataCount++;
                            }
                        }
                        else
                        {
                            //if (data == 0x0a)
                            //{
                            //    if (LineCount == 11)
                            //    {
                            //        //byteCount = 46;
                            //    }
                            //    LineCount++;
                            //}
                            buffer.push(data);
                            string bu = buffer.read();
                            if (bu == "393962")
                            {
                                dataTime = 1;
                                if (byteCount != 39)
                                    dataBad = true;
                            }
                            if (dataTime > 0)
                                dataTime++;
                            //textBox.Invoke(new EventHandler(delegate
                            //{
                            //    if (data == 0x0a)
                            //        textBox.AppendText("\r\n" + byteCount.ToString() + "\r\n");
                            //    else
                            //        textBox.AppendText(" " + data.ToString("x2") + " ");
                            //}));
                        }
                        if (dataCount >= 1000)
                        {
                            DataReady = true;
                        }
                        byteCount++;
                        //if(byteCount > 
                    }

                }
            }
            catch
            {

            }
        }
        public bool IsDataReady()
        {
            return DataReady;
        }
        public int[] GetData()
        {

            if (DataReady)
            {
                return recievedData;
            }
            else return new int[1];
        }
        private byte[] ReadPacket()
        {
            bool done = false;
            byte[] output = new byte[1024];
            int bytecount = 0;
            while (!done)
            {
                
                string line = serialPort.ReadLine();
                if (line.Length >= 1)
                {
                    char[] linedata = line.ToCharArray();
                    for(int i =0;i < linedata.Length;i++)
                    {
                        output[i + bytecount] = (byte)linedata[i];
                    }
                    bytecount += linedata.Length;
                }
                else
                {
                    done = true;
                }
            }
            return output;
        }
    }
}
