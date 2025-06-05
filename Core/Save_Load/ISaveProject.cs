using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryMaker.Models;

namespace StoryMaker.Core.Save_Load
{
    public interface ISaveProject
    {
        void Save(ProjectModel project, string path);
    }
}
