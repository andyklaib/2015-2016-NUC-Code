using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LisaControlerCsh
{
    class LinkedList
    {
        private int Length;
        private ListObject Head;
        private ListObject Tail;
        
        public LinkedList()
        {
            Head = null;
            Tail = null;
            Length = 0;
        }
        public LinkedList(CollisionObject Objectin)
        {
            Head = new ListObject();
            Head.Object = Objectin;
            Head.Next = null;
            Head.Previous = null;
            Tail = Head;
            Length = 1;
        }
        public void Add(CollisionObject Objectin)
        {
            ListObject NewObject = new ListObject();
            NewObject.Object = Objectin;
            NewObject.Previous = Head;
            if(Head != null)
                Head.Next = NewObject;
            Head = NewObject;
            if (Length == 0)
                Tail = Head;
            Length++;
        }
        public void Remove(ListObject Objectin)
        {
            if (Length > 1)
            {
                if (Length == 1)
                {
                    Head = null;
                    Tail = null;
                }
                else if (Objectin.Next != null)
                {
                    if (Objectin.Previous != null)
                    {
                        Objectin.Next.Previous = Objectin.Previous;
                        Objectin.Previous.Next = Objectin.Next;
                    }
                    else
                    {
                        Tail = Objectin.Next;
                        Objectin.Next.Previous = null;
                    }

                }
                else
                {
                    Head = Objectin.Previous;
                    Objectin.Previous.Next = null;
                }
                Length--;
            }
            else
            {
                Length--;
                Head = null;
                Tail = null;
            }
        }
        public ListObject GetHead()
        {
            return Head;
        }
        public int GetLength() { return Length; }
        
    }
    class ListObject
    {
        public ListObject Previous;
        public CollisionObject Object;
        public ListObject Next;
    }
}
