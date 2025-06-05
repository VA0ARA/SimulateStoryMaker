using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Inspector.Models.Version1
{
    public class PropertyFactory : IPropertyFactory
    {
        public IProperty Create(object obj,PropertyInfo property)
        {
            Type t = property.PropertyType;

            if (t == typeof(int))
                return new IntProperty(obj,property);

            return null;
        }
    }
}
