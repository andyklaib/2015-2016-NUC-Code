using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LisaControlerCsh
{
    class CollisionObject
    {
        private int NumberOfUpdates;
        private int LifeTime;
        private int PercentChange;
        private bool Updated;
        private int TimeSinceChange;
        private Rectangle Bounds;
        private Rectangle SmallestArea;
        public CollisionObject(Rectangle Boundsin)
        {
            Bounds = Boundsin;
            NumberOfUpdates = 0;
            PercentChange = 0;
            LifeTime = 0;
        }
        CollisionObject()
        {

        }
        public int slamX;
        public int slamY;
        public int colX;
        public int colY;
        public Rectangle GetBounds() { return Bounds; }
        public void Update(Rectangle newBounds, int overlap)
        {
            Bounds = newBounds;
            NumberOfUpdates++;
            PercentChange += 100 - overlap;
            Updated = true;
        }
        public int GetLifeTime() { return NumberOfUpdates + LifeTime; }
        public int GetTimeSinceChange()
        {
            return TimeSinceChange;
        }
        public void Update()
        {
            if (!Updated)
            {
                //NumberOfUpdates++;
                
                TimeSinceChange++;
            }
            else
                TimeSinceChange = 0;
            LifeTime++;
            Updated = false;
        }
        public int IsGood()
        {
            if (LifeTime > 3)
            {

                return 100 - PercentChange / (NumberOfUpdates + LifeTime);


            }
            return 0;
        }
        public bool WasUpdated()
        {
            bool output = Updated;
	        Updated = false;
	        return output;
        }
    }
}
