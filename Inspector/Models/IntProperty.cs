using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace StoryMaker.Inspector.Models
{
    public class IntProperty : BaseProperty<int>
    {
        public IntProperty(object obj,PropertyInfo property)
        {
            _obj = obj;
            _property=property;
        }
    }
}
