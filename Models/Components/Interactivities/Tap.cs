using StoryMaker.DataStructure;

namespace StoryMaker.Models.Components.Interactivities
{
    public class Tap : InterActivity
    {
        public Tap()
        {
            ID = System.Guid.NewGuid().ToString();
        }

        public Tap(string id)
        {
            this.ID = id;
        }
        public override TriggerType GetTriggerType()
        {
            return TriggerType.Tap;
        }

        public override void MouseDown()
        {
            Execute();
        }

        public override bool CanAdd(StoryObject storyObject)
        {
            return storyObject.GetComponent<Tap>() == null;
        }

        public override bool CanRemove(StoryObject storyObject)
        {
            return true;
        }
    }
}