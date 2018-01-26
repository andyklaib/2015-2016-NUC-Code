using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LisaControlerCsh;


namespace Leslie
{
    class Pathfinding
    {
        
        float Cost;
        Vector2D End;
        Vector2D Start;
        List<WayPoint> OldList;
        List<Line> path;
        List<Rectangle> ColisionObjects;
        int NumberOfPasses = 2;
        float safetyMargin = 5;
        public List<WayPoint> GetPath(int xp, int yp, int xt, int yt,int size)
        {
            List<WayPoint> output;
            path = new List<Line>();
            bool NewPath = true;
            if (OldList != null)
            {
                output = OldList;
                if (OldList[output.Count - 1].X == xt && OldList[output.Count - 1].Y == yt)//The route we calculated last time went to the same place. Just need to update the first way point then verify no collisons.
                {
                    NewPath = false;
                    for(int i = 0; i < OldList.Count -1;i++)
                    {
                        //path.Add(OldList[i].CreateLine(OldList[i+1]));
                    }
                }
                else
                {
                    output = new List<WayPoint>();//The old path went somewhere else
                }
            }
            else//No old path to go from
            {
                output = new List<WayPoint>();
            }
            if (NewPath)//Create a new minimum path
            {
                path = new List<Line>();
                End = new Vector2D(xp, yp);
                Start = new Vector2D(xt, yt);
                Cost = Start.Distance(End);
                Line first = new Line(Start, End);
                path.Add(first);
            }
            //Begin the Testing the path
            Vector2D hitLocation = new Vector2D();
            Rectangle hitObject = new Rectangle();
            Line newLine1;
            Line newLine2;
            for (int pass = 0; pass < NumberOfPasses; pass++)
            {
                for (int lineIndex = 0; lineIndex < path.Count; lineIndex++)
                {

                    if (path[lineIndex].FindCollisions(ref hitLocation, ref hitObject, ColisionObjects))
                    {
                        float Min = float.MaxValue;
                        float dis = 0;
                        int index = -1;
                        for (int i = 0; i < 4; i++)
                        {
                            dis = hitLocation.Distance(hitObject.GetPoint(0));
                            if (dis < Min)
                            {
                                Min = dis;
                                index = i;
                            }
                        }
                        Line off = new Line(hitObject.GetPoint((index + 2) % 4),hitObject.GetPoint(index));
                        newLine1 = new Line(path[lineIndex].start, off.Point(off.start.Distance(off.end) + safetyMargin));

                    }

                }
            }

            return output;
        }

    }
    class Line
    {
        public Vector2D start;
        public Vector2D end;
        public Vector2D Unit;
        public float Slope;
        public float b;
        public Line(Vector2D Start, Vector2D End)
        {
            start = Start;
            end = End;
            Unit = start.Unit(end);
            Slope = _Slope();
            b = _b();
        }
        public float Length
        {
            get { return start.Distance(end); }
        }
        public Vector2D Point(float distance)
        {
            return start + Unit * distance;
        }
        private float _Slope()
        {
            return (start.GetY() - end.GetY()) / (start.GetX() - end.GetX());
        }
        private float _b()
        {
            return -(Slope * start.GetX() - start.GetY());
        }
        public bool Intesect(Line other,ref Vector2D Location)
        {
            if (Slope != other.Slope)
            {
                Location.SetX( (b - other.b) / (other.Slope - Slope));
                Location.SetY(Slope * Location.GetX() + b);
                return true;
            }
            else
                return false;
        }
        public bool FindCollisions(ref Vector2D hitLocation, ref Rectangle hitObject, List<Rectangle> colObjs)
        {
            bool hit = false;

            foreach (Rectangle item in colObjs)
            {
                hit = item.CollisionRot(this,ref hitLocation);
                if(hit)
                {
                    hitObject = item;
                    return hit;
                }
            }
            return hit;
        }
    };
}
