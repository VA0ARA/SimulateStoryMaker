using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Core
{
    public interface IBoundary
    {
        Bounds? GetBounds();
    }
}
