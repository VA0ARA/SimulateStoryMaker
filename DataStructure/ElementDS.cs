using System;
using System.Collections.Generic;

namespace StoryMaker.DataStructure
{
    /// <summary>
    /// This Structure is consist of at least requirements of an element data
    /// </summary>
    [Serializable]
    public class ElementDS:IComponent<SceneDS>
    {
        public string Name { get; set; }
        // This version of element uses version 1 of transform component by default
        [Newtonsoft.Json.JsonProperty]
        List<IComponent<ElementDS>> _components = new List<IComponent<ElementDS>>();
        public string ID { get; set; }

        [Newtonsoft.Json.JsonConstructor]
        ElementDS()
        {
        }

        public ElementDS(TransformDS transform)
        {
            _transform = transform;
            _components.Add(_transform);
        }


        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<IComponent<ElementDS>> Components
        {
            get
            {
                return _components;
            }
        }

        public void AddComponent(IComponent<ElementDS> component)
        {
            //Check for transform component is unique
            if (component is TransformDS transform)
                Transform = transform;
            else
                _components.Add(component);
        }

        public void AddComponents(IEnumerable<IComponent<ElementDS>> components)
        {
            foreach (var c in components)
                AddComponent(c);
        }

        [Newtonsoft.Json.JsonProperty]
        TransformDS _transform;//=new DataStructure.Transform.TransformDS();
        /// <summary>
        /// This version of element uses version 1 of transform component by default
        /// </summary>
        /// 
        [Newtonsoft.Json.JsonIgnore]
        public virtual TransformDS Transform
        {
            get => _transform;
            set
            {
                _components.Remove(_transform);
                _transform = value;
                _components.Add(value);
            }

        }
    }
}
