using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LisaControlerCsh;

namespace Leslie
{
    public class WayPoint
    {
        private float _X;
        private float _Y;
        private int _ID;
        private int _Next;
        private int _Flags;
        public float X
        {
            get { return _X; }
            set { _X = value; }
        }
        public float Y{
            get{ return _Y;}
            set { _Y = value; }
        }
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public int Next
        {
            get { return _Next; }
            set { _Next = value; }
        }
        public int Flags
        {
            get { return _Flags; }
            set { _Flags = value; }
        }
        Line CreateLine(WayPoint other)
        {
            return new Line(new Vector2D(_X, _Y), new Vector2D(other.X, other.Y));
        }
        
    }
}
