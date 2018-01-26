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
using LisaControlerCsh;
using LeahDLL;
using System.IO;

namespace Leslie
{
    public partial class Calibration : Form
    {
        LidarCom LIDAR;
        Leah Camera;
        calibrationMatrix CalibrationData;

        bool requestSent;

        const float slitangle = 0.25f;
        const float PlotSize = 256;
        const float DrawingScale = (PlotSize / 2) / 30000f;
        const long MAPSIZE = 1000;
        const int imageSize = (int)MAPSIZE;/// 2;
        const float resolution = 0.1f;

        Thread videoThread;
        Graphics g;
        Pen DataPen;
        private LidarCom Lidar;
        private Leah video;
        volatile bool videoStop;

        public Calibration(LidarCom lidar, Leah cam,int Width,int Height,int beams,int incraments)
        {
            InitializeComponent();
            Camera = cam;
            LIDAR = lidar;
            g = lidarvisPB.CreateGraphics();
            DataPen = new Pen(System.Drawing.Color.Aquamarine, 1);
            CalibrationData = new calibrationMatrix();
            CalibrationData.CamToLidar = new Vector2D[Width,Height];
            CalibrationData.LidarToCam = new Vector2D[beams,incraments];
            videoThread = new Thread(new ThreadStart(videoLogic));
            videoStop = false;
            videoThread.Start();
        }


        private void nextBT_Click(object sender, EventArgs e)
        {

        }
        private void videoLogic()
        {
            unsafe
            {
            int size = 1280 * 720 *3;
            byte[] data = new byte[size];
            ImageConverter ic = new ImageConverter();


            while (!videoStop)
            {

                fixed (byte* point = data)
                {


                    Camera.calDetect(point, size);
                }
               // MemoryStream sm = new MemoryStream(data);
                
                //Image img = // (Image)ic.ConvertFrom(data);
                    
                    Bitmap image = new Bitmap(1280,720);

                    for (int y = 0; y < 720; y++)
                    {
                        int index = 0;
                        for (int x = 0; x < 1280; x++)
                        {
                            //image.SetPixel(x, y, Color.FromArgb(data[y * 1280 + (x+2) * 3], data[y * 1280 + (x+1) * 3 ], data[y * 1280 + (x+0) *3]));
                            image.SetPixel(x, y, Color.FromArgb(data[(y * 1280 * 3) + index + 2], data[(y * 1280 * 3) + index + 1], data[(y * 1280 * 3) + index]));
                            index += 3;
                        }
                    }
                    videoPB.Image = image;
                }

            }
        }
        private void updateTM_Tick(object sender, EventArgs e)
        {
            if (!requestSent)
                {
                    LIDAR.RequestData(0, 1080);
                    requestSent = true;
                }
            else if (LIDAR.IsDataReady())
            {
                if (!LIDAR.dataBad)
                {
                    int[] data = LIDAR.GetData();
                    Bitmap lid = new Bitmap((int)PlotSize, (int)PlotSize);
                    DrawData(data, 0, 1080, lid);
                    lidarvisPB.Image = lid;

                }
            }
        }

        private void DrawData(int[] data, int start, int stop, Bitmap image)
        {
            g = Graphics.FromImage(image);
            for (int i = start; i < data.Length; i++)
            {
                g.DrawLine(DataPen, new System.Drawing.Point((int)(PlotSize / 2), (int)(PlotSize / 2)), new System.Drawing.Point((int)(DrawingScale * data[i] * Math.Cos((45 - i * slitangle) * (Math.PI / 180f)) + PlotSize / 2), (int)(DrawingScale * data[i] * Math.Sin((45 - i * slitangle) * (Math.PI / 180f)) + PlotSize / 2)));
            }
        }

        private void Calibration_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoThread != null)
            {
                videoStop = true;
            }
        }


    }
}
