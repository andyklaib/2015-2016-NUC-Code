using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LisaControlerCsh
{
    class Vector2DStack
    {
        private int Size, Index, Capacity;
        private Vector2D[] data;

        public Vector2DStack(int Cap)
        {
            Capacity = Cap;
            Size = 0;
            Index = 0;
            data = new Vector2D[Capacity];
        }
        public Vector2DStack()
        {
            Capacity = 10;
            Size = 0;
            Index = 0;
            data = new Vector2D[Capacity];
        }
        public void Push(Vector2D datain)
        {
            if(datain != null)
                if (Index < Capacity)
                {
                    data[Index] = datain;
                    Index++;
                    Size++;
                }
        
        }
        public Vector2D Pop()
        {
            if (Index > 0)
            {
                Index--;
                Size--;
                return data[Index];
            }
            else
                return null;
        }
        public bool IsFull()
        {
            return (Index == Capacity);
        }
        public bool IsEmpty()
        {
            return (Index == 0);
        }
        public int GetSize()
        {
            return Index;
        }
        public int GetCapacity()
        {
            return Capacity;
        }
    }
}
