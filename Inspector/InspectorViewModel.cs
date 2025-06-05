using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Reflection;

namespace StoryMaker.Inspector
{
    public class InspectorViewModel
    {
        object _object;
        public object PresentedObject
        {
            get => _object;
            set
            {
                _object = value;
                GenerateFieldList();
            }
        }

        public Models.IPropertyFactory PropertyFactory { get; set; }

        void GenerateFieldList()
        {
            Properties.Clear();
            var properties = _object.GetType().GetProperties().Where(p => p.GetCustomAttribute<HideInInspector>()==null);
            foreach (var p in properties)
                Properties.Add(PropertyFactory.Create(_object, p));
        }

        public ObservableCollection<Models.IProperty> Properties { get; } = new ObservableCollection<Models.IProperty>();
    }
}
