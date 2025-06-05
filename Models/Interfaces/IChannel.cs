using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Models.Interfaces
{
    public interface IChannel
    {
        string Name { get; }
        /// <summary>
        /// All fields are based on frame
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="times"></param>
        /// <param name="gapTime"></param>
        void Loop(int startTime,int endTime, int times, int gapTime);
        int GetLength();
    }
}
