using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.ComponentModel;

namespace StoryMaker.Inspector.Models
{
    public abstract class BaseProperty<T>:INotifyPropertyChanged,IProperty
    {
        protected object _obj;
        protected PropertyInfo _property;
        public event PropertyChangedEventHandler PropertyChanged;

        public string PropertyName
        {
            get => _property.Name;
        }

        public T PropertyValue
        {
            get
            {
                return (T)_property.GetValue(_obj);
            }
            set
            {
                _property.SetValue(_obj, value);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(PropertyValue)));
            }
        }
    }
}
