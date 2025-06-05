using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Core
{
    public struct IntPoint
    {
        public IntPoint(int x,int y)
        {
            this.X = x;
            this.Y = y;
        }
        public int X, Y;

        public override string ToString()
        {
            return $"X : {X} , Y : {Y}"; 
        }
    }
}
