using System;
using System.Collections.Generic;
using System.Linq;
using StoryMaker.DataStructure.SceneGate;
using StoryMaker.DataStructure.Assets;

namespace StoryMaker.DataStructure
{
    /// <summary>
    /// This class contains assets & elements of a scene
    /// </summary>
    [Serializable]
    public class SceneDS : IComponent<ProjectDS>,SceneGate.ISceneNode
    {
#if UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL 
        SceneGateDS _defaultInputGate = new SceneGateDS(SceneGateDS.InputGateName, GateType.Start),
            _defaultOutputGate = new SceneGateDS(SceneGateDS.OutputGateName, GateType.Exit);
        
        [Newtonsoft.Json.JsonIgnore]
        public SceneGateDS DefaultInputGateDS
        {
            get
            {
                return _defaultInputGate;
            }
        }
        
        [Newtonsoft.Json.JsonIgnore]
        public SceneGateDS DefaultOutputGateDS
        {
            get
            {
                return _defaultOutputGate;
            }
        }
#endif
        public string Name { get; set; }
        public float EndTime { get; set; } = 30;
        public string ID { get; set; }
        /// <summary>
        /// This list is consist of all assets used in scene
        /// </summary>
        public List<IAsset> Assets { get; set; } = new List<IAsset>();

        /// <summary>
        /// This list consist of all element used in secene
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<ElementDS> Elements
        {
            get
            {
                return _components.Where(c => c is ElementDS).Select(e => (ElementDS)e);
            }
        }

        /// <summary>
        /// This property returns the variable defined in scene
        /// </summary>
        /// 

        [Newtonsoft.Json.JsonIgnore]

        public IEnumerable<VariableDS> Variables
        {
            get
            {
                return _components.Where(c => c is VariableDS).Select(c => (VariableDS)c);
            }
        }

        /// <summary>
        /// This property returns all scene gates
        /// </summary>

        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<SceneGateDS> SceneGates
        {
            get
            {
// #if UNITY_ANDROID || UNITY_STANDALONE || UNITY_WEBGL 
//                 return Components.OfType<SceneGateDS>().Concat(new SceneGateDS[] {DefaultInputGateDS,DefaultOutputGateDS});
// #else
                return Components.OfType<SceneGateDS>();
// #endif
            }
        }

        /// <summary>
        /// This property returns scene start gates
        /// </summary>
        /// 

        [Newtonsoft.Json.JsonIgnore]
        SceneGateDS _defaultEntryGate, _defaultExitGate;
        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<SceneGateDS> StartGates
        {
            get
            {
                if (_defaultEntryGate == null)
                    _defaultEntryGate = new SceneGateDS(SceneGateDS.DefaultInputGateName, GateType.Start);
                return SceneGates.Where(g => g.GateType == GateType.Start).Concat(new SceneGateDS[] { _defaultEntryGate });
            }
        }


        /// <summary>
        /// This property returns scene exit gates
        /// </summary>
        /// 

        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<SceneGateDS> ExitGates
        {
            get
            {
                if (_defaultExitGate == null)
                    _defaultExitGate = new SceneGateDS(SceneGateDS.DefaultOutputGateName, GateType.Exit);
                return SceneGates.Where(g => g.GateType == GateType.Exit).Concat(new SceneGateDS[] { _defaultExitGate });
            }
        }

        [Newtonsoft.Json.JsonProperty]
        List<IComponent<SceneDS>> _components = new List<IComponent<SceneDS>>();

        /// <summary>
        /// This property returns all components
        /// </summary>
        /// 

        [Newtonsoft.Json.JsonIgnore]
        public IEnumerable<IComponent<SceneDS>> Components
        {
            get
            {
#if UNITY_ANDROID || UNITY_STANDALONE
                return _components.Concat(new IComponent<SceneDS>[] { _defaultInputGate, _defaultOutputGate });
#else
                return _components;
#endif
            }
        }


        /// <summary>
        /// This method is responsible for add a new component
        /// </summary>
        /// <param name="component"></param>
        public void AddComponent(IComponent<SceneDS> component)
        {
            _components.Add(component);
        }

        /// <summary>
        /// This method adds multiple components
        /// </summary>
        /// <param name="components"></param>
        public void AddComponents(IEnumerable<IComponent<SceneDS>> components)
        {
            foreach (var c in components)
                AddComponent(c);
        }

        public static float GetSceneDuration(SceneDS scene)
        {
            var animators = scene.Elements.SelectMany(e => e.Components).OfType<DataStructure.AnimatorDS>();
            var clips = animators.SelectMany(a => a.AnimatorControllerDs.Clips);
            return clips.Count() > 0 ? clips.Max(c => ClipDS.GetClipDuration(c)) : 0;
        }
    }
}
