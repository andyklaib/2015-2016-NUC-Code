using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LisaControlerCsh
{
    class TestLIDAR
    {
        private int MaxRange;
        private int MinStep;
        private int MaxStep;
        private int NumberOfSteps;
        private float slitAngle;
        private List<Rectangle> ObjectList;

        private Vector2D StartPoint;
        private float Bearing;
        public TestLIDAR(int Range, int Start, int Stop, int NumSteps)
        {
            MaxRange = Range;
            MinStep = Start;
            MaxStep = Stop;
            NumberOfSteps = NumSteps;
            slitAngle = (float)(Stop - Start) / (float)NumSteps;
            ObjectList = new List<Rectangle>();
        }
        public void SetPosition(Vector2D Pos,float Heading)
        {
            StartPoint = Pos;
            Bearing = Heading;
        }
        public Vector2D GetPosition()
        {
            return StartPoint;
        }
        public float GetBearing()
        {
            return Bearing;
        }
        public void AddObject(Rectangle newObject)
        {
            ObjectList.Add(newObject);
        }
        public int[] RequestData(int Start, int Stop)
        {
            int[] data = new int[Stop - Start];
            for( int i = Start;i < Stop; i++)
            {
                data[i] = MaxRange;
                Vector2D EndPoint = new Vector2D(MaxRange * (float)Math.Cos((slitAngle * i + Bearing - 45) * (Math.PI / 180)) + StartPoint.GetX(),- MaxRange * (float)Math.Sin((slitAngle * i + Bearing - 45) * (Math.PI / 180)) + StartPoint.GetY());
                foreach(Rectangle obj in ObjectList)
                {
                    Vector2D intercept = obj.Intercept(StartPoint, EndPoint);
                    if (intercept != null)
                    {
                        int dis = (int)intercept.Distance(StartPoint);
                        if (dis <= MaxRange)
                        {
                            data[i] = dis;
                        }
                    }

                }

            }
            return data;

        }
    }
}
