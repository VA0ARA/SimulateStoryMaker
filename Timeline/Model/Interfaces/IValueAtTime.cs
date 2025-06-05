namespace Timeline.Model.Interfaces
{
    public interface IValueAtTime
    {
        object GetValue(float t);
    }
}