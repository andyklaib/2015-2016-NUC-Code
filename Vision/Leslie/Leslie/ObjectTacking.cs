using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LisaControlerCsh
{
    class ObjectTacking
    {
        private const int ObjectBuffer = 50;
        private long MAP_Bounds;
        private bool[] Map;
        private bool[] CollisionMap;
        private bool[] Checked;
        private Vector2D Top;
        private Vector2D Left;
        private Vector2D Right;
        private Vector2D Bottom;

        private LinkedList Pending;
        private LinkedList Collision;
        private LinkedList LandMark;

        private int Depth;
        private const int RINGSIZE = 5;
        private void BoundObject(int Position, Vector2D Start)
        {
            Top = Start;
	        Left = Start;
	        Right  = Start;
	        Bottom = Start;
            int ring = RINGSIZE;
            int X = -ring;
            int Y = -ring;
            bool left = false;
            while (X != 0 || Y != 0)
            {
                if (((int)Start.GetY() + Y) > -1 && ((int)Start.GetX() + X) > -1 && ((int)Start.GetY() + Y) < MAP_Bounds && ((int)Start.GetX() + X) < MAP_Bounds)
                {
                    if (!Checked[((int)Start.GetY() + Y) * MAP_Bounds + ((int)Start.GetX() + X)])
                    {
                        if (Map[((int)Start.GetY() + Y) * MAP_Bounds + ((int)Start.GetX() + X)] == true)
                        {
                            int cX = (int)Start.GetX() + X;
                            int cY = (int)Start.GetY() + Y;
                            while (cX != Start.GetX() || cY != Start.GetY())
                            {
                                Checked[cY * MAP_Bounds + cX] = true;
                                if (cX > Start.GetX())
                                    cX--;
                                else if (cX < Start.GetX())
                                    cX++;
                                if (cY > Start.GetY())
                                    cY--;
                                else if (cY < Start.GetY())
                                    cY++;
                            }
                            Depth = 0;
                            FindMax(new Vector2D(Start.GetX() + X, Start.GetY() + Y));
                        }
                    }
                }
                    if (X < ring && !left)
                        X++;
                    else if (Y <= ring && !left)
                        Y++;
                    else if (!left)
                    {
                        left = true;
                        X--;
                    }
                    else if (X >= -ring)
                        X--;
                    else if (Y > -ring)
                        Y--;
                    else
                    {
                        ring--;
                        left = false;
                        X++;
                    }
                
            }
        }
        private void FindMax(Vector2D Start)
        {
            Depth++;
            if(Start.GetY() < Top.GetY())
	        {
		        Top = Start;
	        }
	        if(Start.GetX() < Left.GetX())
	        {
		        Left = Start;
	        }
	        if (Start.GetX() > Right.GetX())
	        {
		        Right = Start;
	        }
	        if(Start.GetY() > Bottom.GetY())
	        {
		        Bottom = Start;
	        }
            int ring = RINGSIZE;
            int X = -ring;
            int Y = -ring;
            bool left = false; 
            while (X != 0 || Y != 0)
            {
                if (((int)Start.GetY() + Y) > -1 && ((int)Start.GetX() + X) > -1 && ((int)Start.GetY() + Y) < MAP_Bounds && ((int)Start.GetX() + X) < MAP_Bounds)
                {
                    if (!Checked[((int)Start.GetY() + Y) * MAP_Bounds + ((int)Start.GetX() + X)])
                    {
                        if (Map[((int)Start.GetY() + Y) * MAP_Bounds + ((int)Start.GetX() + X)] == true)
                        {
                            int cX = (int)Start.GetX() + X;
                            int cY = (int)Start.GetY() + Y;
                            while (cX != Start.GetX() || cY != Start.GetY())
                            {
                                Checked[cY * MAP_Bounds + cX] = true;
                                if (cX > Start.GetX())
                                    cX--;
                                else if (cX < Start.GetX())
                                    cX++;
                                if (cY > Start.GetY())
                                    cY--;
                                else if (cY < Start.GetY())
                                    cY++;
                            }
                            FindMax(new Vector2D(Start.GetX() + X, Start.GetY() + Y));
                        }
                    }
                }
                if (X < ring && !left)
                    X++;
                else if (Y <= ring && !left)
                    Y++;
                else if (!left)
                {
                    left = true;
                    X--;
                }
                else if (X >= -ring)
                    X--;
                else if (Y > -ring)
                    Y--;
                else
                {
                    ring--;
                    left = false;
                    X++;
                }
            }

        }
        private void AnalyzeBounds()
        {
            Rectangle NewBound = new Rectangle((int)Left.GetX(),(int)Top.GetY(),(int)Right.GetX() - (int)Left.GetX(),(int)Bottom.GetY() - (int)Top.GetY());
            if (NewBound.GetPoint(3).GetX() == NewBound.GetPoint(1).GetX())
                return;
            if (LandMark.GetLength() > 0)//check if we have found this obeject and it is in the landmark list
            {
                ListObject Current = LandMark.GetHead();
                while (Current != null)
                {
                    int overlap = Current.Object.GetBounds().Overlap(NewBound);
                    if (overlap > 55)
                    {
                        Current.Object.Update(NewBound, overlap);
                        return;
                    }
                    Current = Current.Previous;
                }
            }
            if (Collision.GetLength() > 0)//check if we have found this object already and it is in the Collsion item list
            {
                ListObject Current = Collision.GetHead();
                while (Current != null)
                {
                    int overlap = Current.Object.GetBounds().Overlap(NewBound);
                    if (overlap > 55)
                    {
                        Current.Object.Update(NewBound, overlap);
                        return;
                    }
                    Current = Current.Previous;
                }
            }
	        if(Pending.GetLength() > 0)//check if we have found this object already and it is in  the pending list
	        {
		        ListObject Current = Pending.GetHead();
		        while (Current != null)
		        {
			        int overlap = Current.Object.GetBounds().Overlap(NewBound);
			        if(overlap > 55)
			        {
				        Current.Object.Update(NewBound,overlap);
				        return;
			        }
                    Current = Current.Previous;
		        }
	        }
	        
	        
	        Pending.Add(new CollisionObject(NewBound));
        }

        public ObjectTacking()
        {
            MAP_Bounds = 0;
	        Pending = new LinkedList();
	        Collision = new LinkedList();
	        LandMark = new LinkedList();
        }

        public ObjectTacking(long MapSize)
        {
            MAP_Bounds = MapSize;
	        Pending = new LinkedList();
	        Collision = new LinkedList();
	        LandMark = new LinkedList();
            Map = new bool[MAP_Bounds * MAP_Bounds];
            CollisionMap = new bool[MAP_Bounds * MAP_Bounds];
        }
        private void ClearMap(Array map)
        {
            //for (int i = 0; i < map.Length; i++)
            //{
            //    map[i] = 0;
            //}
            Array.Clear(map, 0, map.Length);
            
        }
        public bool[] CreateMap(int[] data, Vector2D Position, double Bearing, float scale, float SlitAngle)
        {
            //Map = new int[MAP_Bounds * MAP_Bounds];
            ClearMap(Map);
            for(int i = 0; i < data.Length; i++)
            {
                if (data[i] < 30000)
                {
                    int X = (int)((data[i] * Math.Sin((Bearing + (i * SlitAngle) - 45 + 90) * (Math.PI / 180)) + Position.GetX()) * scale + MAP_Bounds / 2);
                    int Y = (int)((data[i] * Math.Cos((Bearing + (i * SlitAngle) - 45 + 90) * (Math.PI / 180)) + Position.GetY()) * scale + MAP_Bounds / 2);
                    if (X >= MAP_Bounds)
                    {
                        X = (int)MAP_Bounds - 1;
                    }
                    if (X <= 0)
                    {
                        X = 0;
                    }
                    if (Y >= MAP_Bounds)
                    {
                        Y = (int)MAP_Bounds - 1;
                    }
                    if (Y <= 0)
                    {
                        Y = 0;
                    }
                    Map[Y * MAP_Bounds + X] = true;
                }

            }
            return Map;
        }
        public void FindObjects(Array Mapin)
        {
            Map = (bool[])Mapin;
            ClearMap(CollisionMap); //= new int[MAP_Bounds*MAP_Bounds];
	        Checked = new bool[MAP_Bounds*MAP_Bounds];
	        for(int Y = 0; Y < MAP_Bounds;Y++)
	        {
		        for(int X = Y % 2;X < MAP_Bounds;X = X + 2)//test everyother pixel alternating between even and odd every line
		        {
			        int Position = (int)MAP_Bounds*Y+X;
			        if(Map[Position] == true)//Check for hit
			        {
				        if(Checked[Position] == false)
				        {
					        BoundObject(Position,new  Vector2D(X,Y));
					        AnalyzeBounds();
				        }
			        }
		        }
	        }
        }

        public void UpdateLists()
        {
            if(Pending.GetLength() > 0)//check if we have found this object already and it is in  the pending list
	        {
		        ListObject Current = Pending.GetHead();
		        while (Current != null)
		        {
                    Current.Object.Update();
			        if(Current.Object.IsGood() > 85 && Current.Object.GetLifeTime() > 25)
			        {
				        LandMark.Add(Current.Object);
				        Pending.Remove(Current);
			        }
			        else if(Current.Object.IsGood() > 0)
			        {
				        LandMark.Add(Current.Object);
				        Pending.Remove(Current);
			        }
                    else if (Current.Object.GetTimeSinceChange() > 5)
                    {
                        Pending.Remove(Current);
                    }
                    Current = Current.Previous;
		        }

	        }
	        if(Collision.GetLength() > 0)//check if we have found this object already and it is in the Collsion item list
	        {
		        ListObject Current = Collision.GetHead();
		        while (Current != null)
		        {
                    Current.Object.Update();
			        if(Current.Object.IsGood() > 75)
			        {
				        LandMark.Add(Current.Object);
				        Collision.Remove(Current);
			        }
                    else if (Current.Object.GetTimeSinceChange() > 10)
                    {
                        Pending.Remove(Current);
                    }
                    Current = Current.Previous;
		        }

	        }
	        if(LandMark.GetLength() > 0)//check if we have found this obeject and it is in the landmark list
	        {
		        ListObject Current = LandMark.GetHead();
		        while (Current != null)
		        {
                    Current.Object.Update();
			        if(Current.Object.IsGood() < 75)
			        {
				        Collision.Add(Current.Object);
				        LandMark.Remove(Current);
			        }
                    else if (Current.Object.GetTimeSinceChange() > 20)
                    {
                        LandMark.Remove(Current);
                    }
                    Current = Current.Previous;
		        }

	        }
        }
        public List<Rectangle>[] GetBoundings()
        {

            List<Rectangle>[] output = new List<Rectangle>[3];
            output[0] = new List<Rectangle>();
            output[1] = new List<Rectangle>();
            output[2] = new List<Rectangle>();
            if (Pending.GetLength() > 0)//check if we have found this object already and it is in  the pending list
            {
                ListObject Current = Pending.GetHead();
                while (Current != null)
                {
                    output[0].Add(Current.Object.GetBounds());
                    Current = Current.Previous;
                }

            }
            if (Collision.GetLength() > 0)//check if we have found this object already and it is in the Collsion item list
            {
                ListObject Current = Collision.GetHead();
                while (Current != null)
                {
                    output[1].Add(Current.Object.GetBounds());
                    Current = Current.Previous;
                }

            }
            if (LandMark.GetLength() > 0)//check if we have found this obeject and it is in the landmark list
            {
                ListObject Current = LandMark.GetHead();
                while (Current != null)
                {
                    output[2].Add(Current.Object.GetBounds());
                    Current = Current.Previous;
                }

            }
            return output;
        }
        public int[] GetObjectCounts()
        {
            int[] output = new int[3];
            output[0] = Pending.GetLength();
            output[1] = Collision.GetLength();
            output[2] = LandMark.GetLength();
            return output;
        }
        public bool[] FindCollisionMaps()
        {
            Array.Clear(CollisionMap, 0, CollisionMap.Length - 1);
            if(Collision.GetLength() > 0)//check if we have found this object already and it is in the Collsion item list
	        {
		        ListObject Current = Collision.GetHead();
		        while (Current != null)
		        {
			        for(int y = (int)Current.Object.GetBounds().GetPoint(3).GetY() - ObjectBuffer;y < Current.Object.GetBounds().GetPoint(0).GetY() + ObjectBuffer;y++)
			        {
				        for(int x = (int)Current.Object.GetBounds().GetPoint(3).GetX() - ObjectBuffer;x < Current.Object.GetBounds().GetPoint(2).GetX() + ObjectBuffer;x++)
				        {
					        CollisionMap[y*MAP_Bounds + x] = true;
				        }
			        }
                    Current = Current.Next;
		        }

	        }
	        if(LandMark.GetLength() > 0)//check if we have found this obeject and it is in the landmark list
	        {
		        ListObject Current = LandMark.GetHead();
		        while (Current != null)
		        {
			        for(int y = (int)Current.Object.GetBounds().GetPoint(3).GetY() - ObjectBuffer;y < Current.Object.GetBounds().GetPoint(0).GetY() + ObjectBuffer;y++)
			        {
				        for(int x = (int)Current.Object.GetBounds().GetPoint(3).GetX() -ObjectBuffer;x < Current.Object.GetBounds().GetPoint(2).GetX() +ObjectBuffer;x++)
				        {
					        CollisionMap[y*MAP_Bounds + x] = true;
				        }
			        }
                    Current = Current.Next;
		        }
	        }
            return CollisionMap;
        }
        //int Distance(int X1,int Y1, int X2, int Y2) Depricated by Vector2D.Distance()
    }
}
