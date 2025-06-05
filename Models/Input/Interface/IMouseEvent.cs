using StoryMaker.Models.Input.Enum;

namespace StoryMaker.Models.Input.Interface
{
    public interface IMouseEvent
    {
        void RaiseEvent(MouseEventType eventType);
    }
}