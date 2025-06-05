using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace StoryMaker.Inspector.Models
{
    public interface IPropertyFactory
    {
        IProperty Create(object obj,PropertyInfo property);
    }
}
