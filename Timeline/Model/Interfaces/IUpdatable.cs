namespace Timeline.Model.Interfaces
{
    public interface IUpdatable
    {
        bool EnableUpdate { get; }
        int CurrentFrame { get; set; }
        void FrameChanged(float frame, bool changedByUser);
    }
}