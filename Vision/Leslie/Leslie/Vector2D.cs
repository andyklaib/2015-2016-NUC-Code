using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LisaControlerCsh
{
    class Vector2D
    {
        private float X, Y, R, Theta;
        public Vector2D()
        {
            X = 0;
            Y =0;
            R =0;
            Theta = 0;
        }
        public Vector2D(float Xin, float Yin)
        {
            X = Xin;
            Y = Yin;
            R = (float)Math.Sqrt(Xin * Xin + Yin * Yin);
	        Theta = (float)Math.Tan(Yin/Xin);
        }
        public float Dot(Vector2D Other)
        {
            return X*Other.GetX() + Y*Other.GetY();
        }
        public float GetX() { return X; }
        public float GetY() { return Y; }
        public void SetX(float value) { X = value; }
        public void SetY(float value) { Y = value; }
        public float GetR() { return R; }
        public float GetTheta() { return Theta; }
        public float Distance(Vector2D other)
        {
            return (float)Math.Sqrt((X - other.GetX())*(X - other.GetX()) + (Y - other.GetY())*(Y - other.GetY()));
        }
        public Vector2D MidPoint(Vector2D Other)
        {
            return null;
        }
        public static Vector2D operator +(Vector2D other,Vector2D other2)
        {
            return new Vector2D(other.GetX() + other2.GetX(), other.GetY() + other2.GetY());
        }
        public static Vector2D operator -(Vector2D other, Vector2D other2)
        {
            return new Vector2D(other.GetX() - other2.GetX(),other.GetY() - other2.GetY()); 
        }
        public static Vector2D operator *(Vector2D other, float other2)
        {
            return new Vector2D(other.GetX()*other2,other.GetY()*other2);
        }
        //public static void operator =(Vector2D other)
        //{
        //    return new Vector2D(
        //}
    }
}
