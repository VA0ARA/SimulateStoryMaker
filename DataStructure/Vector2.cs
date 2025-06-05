using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public struct Vector2
    {
        public float X, Y;

        public Vector2(float x,float y)
        {
            X = x;
            Y = y;
        }
    }
}
