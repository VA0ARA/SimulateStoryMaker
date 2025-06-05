namespace StoryMaker.DataStructure
{
    public class DropCondition:InteractivityDS
    {
        public DropCondition() : base(TriggerType.DropCondition)
        {
            
        }

        public string DraggingID, DropContainerID;
        public bool AnyDropContainer, NoDropContainer;
    }
}