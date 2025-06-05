namespace StoryMaker.Models.Interfaces
{
    public interface IViewport
    {
        IDrawable Drawable { get; }
        bool IsFocus { get; set; }
    }
}