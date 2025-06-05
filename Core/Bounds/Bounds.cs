using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryMaker.Core
{
    public struct Bounds
    {
        public Bounds(int x,int y,int width,int height)
        {
            Center = new IntPoint(x, y);
            this.Width =Math.Abs(width);
            this.Height =Math.Abs(height);
        }

        public Bounds(IntPoint center,int width,int height)
        {
            this.Center = center;
            this.Width =Math.Abs(width);
            this.Height =Math.Abs(height);
        }

        public Bounds(IntPoint point1,IntPoint point2)
        {
            Center = new IntPoint((point1.X + point2.X) / 2, (point1.Y + point2.Y) / 2);
            Width = Math.Abs(point2.X - point1.X);
            Height = Math.Abs(point2.Y - point1.Y);
        }

        public IntPoint Center { get; }
        public int X => Center.X;
        public int Y => Center.Y;
        public int Width { get; }
        public int Height { get; }

        public IntPoint Min => new IntPoint(X - Width / 2, Y - Height / 2);
        public IntPoint Max=> new IntPoint(X + Width / 2, Y + Height / 2);

        public static bool Intersects(Bounds bounds1,Bounds bounds2)
        {
            IntPoint left_up, left_down, right_up, _right_down;

            //check bound2 intersects bound1
            GetCorners(bounds1, out left_up, out left_down, out right_up, out _right_down);
            IntPoint[] c1 = { left_up, left_down, right_up, _right_down };

            foreach (var p in c1)
                if (bounds2.ContainsPoint(p))
                    return true;

            //check bound1 intersects bound2
            GetCorners(bounds2, out left_up, out left_down, out right_up, out _right_down);
            IntPoint[] c2 = { left_up, left_down, right_up, _right_down };

            foreach (var p in c2)
                if (bounds1.ContainsPoint(p))
                    return true;

            return false;
        }

        public bool ContainsPoint(IntPoint point)
        {
            IntPoint min = this.Min, max = this.Max;

            return point.X <= max.X && point.X >= min.X && point.Y <= max.Y && point.Y >= min.Y;

        }

        public static void GetCorners(Bounds bounds,out IntPoint left_up,out IntPoint left_down,out IntPoint right_up,out IntPoint right_down)
        {
            IntPoint c = bounds.Center;
            int w = bounds.Width, h = bounds.Height;
            left_up = new IntPoint(c.X - w / 2, c.Y - h / 2);
            left_down= new IntPoint(c.X - w / 2, c.Y + h / 2);
            right_up= new IntPoint(c.X + w / 2, c.Y - h / 2);
            right_down= new IntPoint(c.X + w / 2, c.Y + h / 2);
        }

        public static Bounds? GetIntersection(Bounds bounds1, Bounds bounds2)
        {
            if (!Intersects(bounds1, bounds2))
                return null;

            IntPoint min1 = bounds1.Min, min2 = bounds2.Min,
                max1 = bounds1.Max, max2 = bounds2.Max;

            IntPoint min = new IntPoint(Math.Max(min1.X, min2.X), Math.Max(min1.Y, min2.Y))
                , max = new IntPoint(Math.Min(max1.X, max2.X), Math.Min(max1.Y, max2.Y));

            return new Bounds(min, max);
        }

        public static Bounds GetBound(Bounds bounds1,Bounds bounds2)
        {
            IntPoint min1=bounds1.Min, min2=bounds2.Min, max1=bounds1.Max, max2=bounds2.Max;

            return new Bounds(new IntPoint(Math.Min(min1.X, min2.X), Math.Min(min1.Y, min2.Y))
                , new IntPoint(Math.Max(max1.X, max2.X), Math.Max(max1.Y, max2.Y)));

        }

        public override string ToString()
        {
            return $"Center : {Center} , Width : {Width} ,Height : {Height}";
        }
    }
}
