using System.ComponentModel;

namespace StoryMaker.Models.Interfaces
{
    public interface IDrawable : IOrder, INotifyPropertyChanged 
    {
        Vector2 DrawableBound { get; }
        Vector2 Pivot { get; }

    }
}