using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LisaControlerCsh;

namespace LeslieCon
{
    public class VisionWayPoint
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
        public VisionWayPoint(float x, float y)
        {
            _X = x;
            _Y = y;
            _Flags = 0;
            _ID = 0;
            _Next = 0;
        }
        public VisionWayPoint(float x, float y,int ID, int next)
        {
            _X = x;
            _Y = y;
            _Flags = 0;
            _ID = 0;
            _Next = 0;
        }
    }
}
