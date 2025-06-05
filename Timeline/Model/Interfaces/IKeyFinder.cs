namespace Timeline.Model.Interfaces
{
    public interface IKeyFinder
    {
        int PreviousKey(int key, bool containsSelf);
        int NextKey(int key, bool containsSelf);
    }
}