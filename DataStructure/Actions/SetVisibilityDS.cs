namespace StoryMaker.DataStructure.Actions
{
    public class SetVisibilityDS:IComponent<InteractivityDS>
    {
        public string ID, ImageID;
        public float StartTime, Duration;
        public bool Visibility;
    }
}