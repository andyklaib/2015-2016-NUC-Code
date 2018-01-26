using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LisaControlerCsh
{
    class WayPoint
    {
        private Vector2D Target;
        private WayPoint Next;
        private int TimeToLive;
        private int TimeLived;
        private bool Alive;

        public WayPoint()
        {

        }
        public WayPoint(Vector2D des, int Time)
        {
            Target = des;
            TimeToLive = Time;
            TimeLived = 0;
            Alive = true;
        }
        public WayPoint(index next)
        {
            Target = new Vector2D(next.x, next.y);
            TimeToLive = 10000;
            TimeLived = 0;
            Alive = true;
        }
        public void AddWayPoint(WayPoint next)
        {
            Next = next;
        }
        public void Add(index next)
        {
            Next = new WayPoint(new Vector2D(next.x, next.y), 1000);
        }
        public Vector2D GetDest()
        {
            return Target;
        }
        public void Update()
        {
            if (Alive)
            {
                TimeLived++;
                if (TimeLived >= TimeToLive)
                {
                    Alive = false;
                }
            }
        }
        public bool IsAlive()
        {
            return Alive;
        }
        public WayPoint GetNext()
        {
            return Next;
        }
    }
}
