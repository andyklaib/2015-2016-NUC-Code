using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using LeahDLL;
using System.Runtime.InteropServices;
using NGCP.IO;
using LisaControlerCsh;

//using namespace LeahDLL;

namespace Leslie
{
    public partial class Form1 : Form
    {
        Leah video;
        LidarCom Lidar;
        Thread videoThread;

        bool Simulate = false;


        bool requestSent;

        const float slitangle = 0.25f;
        const float PlotSize = 512;
        const float DrawingScale = (PlotSize / 2) / 30000f;
        const long MAPSIZE = 1000;
        const int imageSize = (int)MAPSIZE;/// 2;
        const float resolution = 0.1f;
        volatile bool videoStop = false;

        Calibration calWindow;

        Graphics g;
        Pen DataPen;

        byte[] map = new byte[1000000];
        public Form1()
        {
            InitializeComponent();
            video = new Leah();
            Lidar = new LidarCom();
            refreshPorts();
            g = lidarvisPB.CreateGraphics();
            DataPen = new Pen(System.Drawing.Color.Aquamarine, 1);
        }

        private void camLaunchBT_Click(object sender, EventArgs e)
        {
            if (videoThread == null || camLaunchBT.Text == "Launch Camera")
            {
                videoThread = new Thread(new ThreadStart(VideoLoop));
                videoStop = false;
                videoThread.Start();
                camLaunchBT.Text = "Stop Camera";
            }
            else
            {
                videoStop = true;
                camLaunchBT.Text = "Launch Camera";
            }
        }

        private void brefreshBT_Click(object sender, EventArgs e)
        {
            refreshPorts();
        }
        private void refreshPorts()
        {
            portCB.Items.Clear();
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                portCB.Items.Add(port);
            }
        }
        private unsafe void VideoLoop()
        {
            int source = (int)sourceNUD.Value;
            int width = (int)widthNUD.Value;
            int height = (int)heightNUD.Value;
            int threshold = (int)threshNUD.Value;
            bool debug = debugCB.Checked;
            unsafe{
                fixed(byte* point = map){

                    video.run(source, width, height, threshold, debug,point,1000);
                }

            }
            //videoStream.Invoke(new EventHandler(delegate
            //{
            //    videoStream.Width = width;
            //    videoStream.Height = height;
            //}));
            //byte[] data = new byte[height * width * 3];
            Bitmap image = new Bitmap(1280, 720);
            int size = 1280 * 720 * 3;
            byte[] data = new byte[size];
            while (!videoStop)
            {
               unsafe
               {

                fixed (byte* point = data)
                {


                    video.process(point,size);
                }
               // MemoryStream sm = new MemoryStream(data);
                
                //Image img = // (Image)ic.ConvertFrom(data);


                
                //    for (int y = 0; y < 720; y++)
                //    {
                //        int index = 0;
                //        for (int x = 0; x < 1280; x++)
                //        {
                //            //image.SetPixel(x, y, Color.FromArgb(data[y * 1280 + (x+2) * 3], data[y * 1280 + (x+1) * 3 ], data[y * 1280 + (x+0) *3]));
                //            image.SetPixel(x, y, Color.FromArgb(data[(y * 1280 * 3) + index + 2],data[(y * 1280 * 3) + index + 1], data[(y * 1280 *3) + index]));
                //            index += 3;
                //        }
                //    }
                //videoStream.Invoke(new EventHandler(delegate
                //{
                //    videoStream.Image = image;
                //}));
                //image.Dispose();
                
                }
            }
           // frametimer.Enabled = true;
        }
        private void connectBT_Click(object sender, EventArgs e)
        {
            if (!Lidar.Connected())
            {
                Lidar.Connect(portCB.SelectedItem.ToString());
                updateTM.Enabled = true;
                connectBT.Text = "Disconnect";
            }
            else
            {
                updateTM.Enabled = false;
                Lidar.Disconnect();
                connectBT.Text = "Connect";
            }
        }

        private void frametimer_Tick(object sender, EventArgs e)
        {
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoThread != null)
                videoStop = true;
        }


        private void DrawData(int[] data, int start, int stop, Bitmap image)
        {
            g = Graphics.FromImage(image);
            for (int i = start; i < data.Length; i++)
            {
                g.DrawLine(DataPen, new System.Drawing.Point((int)(PlotSize / 2), (int)(PlotSize / 2)), new System.Drawing.Point((int)(DrawingScale * data[i] * Math.Cos((45 - i * slitangle) * (Math.PI / 180f)) + PlotSize / 2), (int)(DrawingScale * data[i] * Math.Sin((45 - i * slitangle) * (Math.PI / 180f)) + PlotSize / 2)));
            }
        }

        private void updateTM_Tick_1(object sender, EventArgs e)
        {
            if (!requestSent)
            {
                Lidar.RequestData(0, 1080);
                requestSent = true;
            }
            else if (Lidar.IsDataReady())
            {
                if (!Lidar.dataBad)
                {
                    int[] data = Lidar.GetData();
                    Bitmap lid = new Bitmap((int)PlotSize, (int)PlotSize);
                    DrawData(data, 0, 1080, lid);
                    lidarvisPB.Image = lid;
                    unsafe{
                        fixed(int* point = data)
                        {
                            video.Mapping(point, 1080, 270 , 0, 0, 0);
                        }
                    }
                }
                requestSent = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (videoThread != null)
            {
                videoStop = true;
                calWindow = new Calibration(Lidar,video,1280,720,(int)(270/0.25),1);
                calWindow.Show();
            }
            else
            {
                MessageBox.Show("Start the camera fisrt");
            }
        }
    }
}
