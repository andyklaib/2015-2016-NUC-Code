using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LisaControlerCsh
{
    class circularArray
    {
        
        dataObj current;
        public void push(object newdata)
        {
            current.data = newdata;
            current = current.next;
        }
        public circularArray(int Size)
        {
            current = new dataObj();
            current.tag = 0;
            current.data = new byte();
            dataObj head = current;

            for(int i = 1; i < Size; i++)
            {
                current.next = new dataObj();
                current = current.next;
                current.tag = i;
                current.data = new byte();
            }
            current.next = head;
        }
        public string read()
        {
            string output = "";
            dataObj start = current;
            do
            {
                output += ((byte)current.data).ToString("x2");
                current = current.next;
            } while (current.tag != start.tag);
            return output;
        }
    }
     class dataObj
    {
        public dataObj next;
        public object data;
        public int tag;
    }
}
