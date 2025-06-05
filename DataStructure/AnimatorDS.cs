using System;

namespace StoryMaker.DataStructure
{
    [Serializable]
    public class AnimatorDS : IComponent<ElementDS>
    {
        public bool ApplyRootMotion;
        public bool PlayOnAwake;
        public string ID { get; set; }
        public AnimatorControllerDS AnimatorControllerDs { get; set; } = new AnimatorControllerDS();
    }
}
