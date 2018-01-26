using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LisaControlerCsh
{
    class Rectangle
    {
        private int Width, Height;
        private double Theta;
        Vector2D Center;
        Vector2D P0;
        Vector2D P1;
        Vector2D P2;
        Vector2D P3;
        Vector2D Org;
        public Rectangle()
        {

        }
        public Rectangle(int Xin, int Yin, int Widthin, int Heightin)
        {
            //Center = new Vector2D(Xin, Yin);
            //P0 = new Vector2D(Xin - Widthin / 2, Yin - Heightin / 2);
            //P1 = new Vector2D(Xin + Widthin /2, Yin - Heightin / 2);
            //P2 = new Vector2D(Xin + Widthin / 2, Yin + Heightin / 2);
            //P3 = new Vector2D(Xin - Widthin /2, Yin + Heightin /2);

            if (Widthin > 1)
                Width = Widthin;
            else
                Width = 1;
            if (Heightin > 1)
                Height = Heightin;
            else
                Height = 1;
            P0 = new Vector2D(Xin , Yin + Heightin);
            P1 = new Vector2D(Xin + Widthin, Yin + Heightin);
            P2 = new Vector2D(Xin + Widthin, Yin);
            P3 = new Vector2D(Xin, Yin);
            
            
            Theta = 0;
        }
        public Rectangle(int Xin, int Yin, int Widthin, int Heightin,double Thetain)
        {
            Center = new Vector2D(Xin, Yin);
            P0 = new Vector2D(Xin - Widthin / 2, Yin - Heightin / 2);
            P1 = new Vector2D(Xin + Widthin / 2, Yin - Heightin / 2);
            P2 = new Vector2D(Xin + Widthin / 2, Yin + Heightin / 2);
            P3 = new Vector2D(Xin - Widthin / 2, Yin + Heightin / 2);
            Width = Widthin;
            Height = Heightin;
            Theta = Thetain;
        }
        public Rectangle(Vector2D P0in, Vector2D P1in, Vector2D P2in, Vector2D P3in, double Thetain, Vector2D Orgin)
        {
            P0 = P0in;
            P1 = P1in;
            P2 = P2in;
            P3 = P3in;
            Org = Orgin;

            Width = (int)P0.Distance(P1);
            Height = (int)P1.Distance(P2);
            Theta = Thetain;
        }
        public Rectangle(Vector2D P0in, Vector2D P1in, Vector2D P2in, Vector2D P3in)
        {
            P0 = P0in;
            P1 = P1in;
            P2 = P2in;
            P3 = P3in;
            Org = P0in;

            Width = (int)P0.Distance(P1);
            Height = (int)P1.Distance(P2);
            Theta = 0;
        }
        public void Rest(int Xin, int Yin, int Widthin, int Heightin)
        {
            Center = new Vector2D(Xin, Yin);
            P0 = new Vector2D(Xin - Widthin / 2, Yin - Heightin / 2);
            P1 = new Vector2D(Xin + Widthin / 2, Yin - Heightin / 2);
            P2 = new Vector2D(Xin + Widthin / 2, Yin + Heightin / 2);
            P3 = new Vector2D(Xin - Widthin / 2, Yin + Heightin / 2);
            Width = Widthin;
            Height = Heightin;
            Theta = 0;
        }
        public Vector2D GetCenter()
        {
            return Center;
        }
        public int Overlap(Rectangle Other)
        {
            bool[] PointContained= new bool[4];
            if (GetPoint(0) == Other.GetPoint(0) && GetPoint(1) == Other.GetPoint(1) && GetPoint(2) == Other.GetPoint(2) && GetPoint(3) == Other.GetPoint(3))
            {
                return 100;
            }
            Vector2DStack PointStack = new Vector2DStack(4);
            for (int i = 0; i < 4; i++)
            {
                PointContained[i] = Other.Collision(GetPoint(i));
                if (PointContained[i])
                {
                    PointStack.Push(GetPoint(i));

                }
            }
            if (PointStack.GetSize() == 4)
            {
                return ((Height * Width) * 100) / (Other.Height * Other.Width);
            }
            //PointStack = new Vector2DStack(4);
            for (int i = 0; i < 4; i++)
            {
                PointContained[i] = Collision(Other.GetPoint(i));
                if (PointContained[i])
                {
                    PointStack.Push(Other.GetPoint(i));

                }
            }
            if (PointStack.GetSize() == 4)
            {
                return ((Other.Height * Other.Width) * 100) / (Height * Width);
            }
            else if(PointStack.IsEmpty())
            {
                return 0;
            }
            //Cannot have case of three points contained
            //Test for line intercepts
            //Test Other Rectanle top and bottom first
            PointStack.Push(Intercept(Other.GetPoint(0), Other.GetPoint(1), GetPoint(3), GetPoint(0)));
            PointStack.Push(Intercept(Other.GetPoint(3), Other.GetPoint(2), GetPoint(3), GetPoint(0)));
            PointStack.Push(Intercept(Other.GetPoint(0), Other.GetPoint(1), GetPoint(2), GetPoint(1)));
            PointStack.Push(Intercept(Other.GetPoint(3), Other.GetPoint(2), GetPoint(2), GetPoint(1)));
            //Test Other Rectangle Left and Right
            PointStack.Push(Intercept(Other.GetPoint(3), Other.GetPoint(0), GetPoint(0), GetPoint(1)));
            PointStack.Push(Intercept(Other.GetPoint(2), Other.GetPoint(1), GetPoint(0), GetPoint(1)));
            PointStack.Push(Intercept(Other.GetPoint(3), Other.GetPoint(0), GetPoint(3), GetPoint(2)));
            PointStack.Push(Intercept(Other.GetPoint(2), Other.GetPoint(1), GetPoint(3), GetPoint(2)));
            //Rectangle* Over = new Rectangle(PointStack.Pop(),PointStack.Pop(),PointStack.Pop(),PointStack.Pop());
            if (PointStack.GetSize() < 3)
            {
                return 0;
            }
            Vector2D p0 = PointStack.Pop();
            float overArea = p0.Distance(PointStack.Pop()) * p0.Distance(PointStack.Pop());
            return (int)((overArea * 100) / (Height * Width + (Other.Height * Other.Width - overArea)));
            
        }
        public bool CollisionRot(Vector2D Other)
        {
	        Vector2D P0_P3 = P0 - P3;
	        Vector2D P2_P3 = P2 - P3;
	        Vector2D TWO_P_C = (Other*2.0f - P0 - P2);

	        return (P2_P3.Dot(TWO_P_C - P2_P3) <= 0 && P2_P3.Dot(TWO_P_C + P2_P3) >= 0) &&
		        (P0_P3.Dot(TWO_P_C - P0_P3) <= 0 && P0_P3.Dot(TWO_P_C + P0_P3) >= 0);
        }
         public bool Collision(Vector2D point)
        {
            if (point != null)
                if (point.GetX() >= P0.GetX() - 0.01f && point.GetX() <= P2.GetX() + 0.01f && point.GetY() <= P0.GetY() + 0.01f && point.GetY() >= P2.GetY() - 0.01f)
                    return true;
                else
                    return false;
            else
                return false;
        }
        public Vector2D GetPoint(int index)
        {
	        switch (index)
	        {
	        case 0:
		        return  P0;
	        case 1:
		        return  P1;
	        case 2:
		        return P2;
	        case 3:
		        return P3;
	        default:
		        return null;
	        }
	
        }
        public Vector2D Intercept(Vector2D Point0,Vector2D Point1, Vector2D Point2, Vector2D Point3)
        {
	        if(Point2.GetX() > Point0.GetX() && Point2.GetX() < Point1.GetX() && Point0.GetY() < Point2.GetY() && Point0.GetY() > Point3.GetY())
		        return new Vector2D(Point2.GetX(),Point0.GetY());
	        else
		        return null;
        }
        public Vector2D InterceptAng(Vector2D Point0, Vector2D Point1, Vector2D Point2, Vector2D Point3)
        {
            

            float TestM1 = (Point2.GetY() - Point3.GetY()) / (Point2.GetX() - Point3.GetX());
            if (Point2.GetX() - Point3.GetX() == 0)
            {
                TestM1 = 100000000;
            }
            float TestB1 = -(Point2.GetX() * TestM1) + Point2.GetY();
            float TestM2 = (Point0.GetY() - Point1.GetY()) / (Point0.GetX() - Point1.GetX());
            if (Point0.GetX() - Point1.GetX() == 0)
            {
                TestM2 = 100000000;
            }
            float TestB2 = -(Point0.GetX() * TestM2) + Point0.GetY();
            return new Vector2D((TestB2 - TestB1) / (TestM1 - TestM2), TestM1 * ((TestB2 - TestB1) / (TestM1 - TestM2)) + TestB1);
            
            //if (Point2.GetY() > Point3.GetY())
            //{
            //    if(output.GetX() < 
            //}
            //else
            //{

            //}
        }
        public Vector2D Intercept(Vector2D Point0, Vector2D Point1)
        {
            //if (Point2.GetX() > Point0.GetX() && Point2.GetX() < Point1.GetX() && Point0.GetY() < Point2.GetY() && Point0.GetY() > Point3.GetY())
            //    return new Vector2D(Point2.GetX(), Point0.GetY());
            //else
            //    return null;
            Vector2D output = null;
            Stack<Vector2D> Intercepts = new Stack<Vector2D>();
            Intercepts.Push(InterceptAng(Point0, Point1, GetPoint(0), GetPoint(1)));
            Intercepts.Push(InterceptAng(Point0, Point1, GetPoint(0), GetPoint(3)));
            Intercepts.Push(InterceptAng(Point0, Point1, GetPoint(2), GetPoint(1)));
            Intercepts.Push(InterceptAng(Point0, Point1, GetPoint(2), GetPoint(3)));
            float distance = 10000000f;
            while (Intercepts.Count > 0)
            {
                Vector2D point = Intercepts.Pop();
                if (point != null && Collision(point) && point.Distance(Point0) + point.Distance(Point1) <= Point0.Distance(Point1) + 0.1f)
                {
                    float dis = point.Distance(Point0);
                    if (dis < distance)
                    {
                        output = point;
                        distance = dis;
                    }
                    //output = point;
                }
            }
            return output;
        }
        
        public int GetWidth() { return Width; }
        public int GetHeight() { return Height; }
    }
}
