using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.VersionConverter
{
    public interface IVersionProvider
    {
        string GetVersion(string path);
    }
}
