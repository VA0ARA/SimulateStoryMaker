using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.TextMap
{
    public interface ITitleProvider
    {
        string GetTitle(Type type);
    }
}
