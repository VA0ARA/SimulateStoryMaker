namespace StoryMaker.DataStructure.Assets
{
    public class SpritesheetAsset : ImageAsset
    {
        public SpritesheetAsset(string path,int width,int height,int fps):base(path)
        {
            Width = width;
            Height = height;
            FPS = fps;
        }
        public int Width { get; }
        public int Height { get; }
        public int FPS { get; }
    }
}
