using System;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using StoryMaker.Helpers;

namespace Timeline.Class
{
    class HorizontalGridLine : Grid
    {
        #region Properties

        public bool Ruler
        {
            get { return (bool) GetValue(RulerProperty); }
            set { SetValue(RulerProperty, value); }
        }

        public static readonly DependencyProperty RulerProperty =
            DependencyProperty.Register(nameof(Ruler), typeof(bool), typeof(HorizontalGridLine),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));


        public FontFamily RulerFont
        {
            get { return (FontFamily) GetValue(RulerFontProperty); }
            set { SetValue(RulerFontProperty, value); }
        }

        public static readonly DependencyProperty RulerFontProperty =
            DependencyProperty.Register(nameof(RulerFont), typeof(FontFamily), typeof(HorizontalGridLine),
                new FrameworkPropertyMetadata(new FontFamily(""), FrameworkPropertyMetadataOptions.AffectsRender));

        public int RulerFontSize
        {
            get { return (int) GetValue(RulerFontSizeProperty); }
            set { SetValue(RulerFontSizeProperty, value); }
        }

        public static readonly DependencyProperty RulerFontSizeProperty =
            DependencyProperty.Register(nameof(RulerFontSize), typeof(int), typeof(HorizontalGridLine),
                new FrameworkPropertyMetadata(10, FrameworkPropertyMetadataOptions.AffectsRender));

        public LinearGradientBrush RulerFontColor
        {
            get { return (LinearGradientBrush) GetValue(RulerFontColorProperty); }
            set { SetValue(RulerFontColorProperty, value); }
        }

        public static readonly DependencyProperty RulerFontColorProperty =
            DependencyProperty.Register(nameof(RulerFontColor), typeof(LinearGradientBrush), typeof(HorizontalGridLine),
                new FrameworkPropertyMetadata(new LinearGradientBrush(),
                    FrameworkPropertyMetadataOptions.AffectsRender));

        public int RulerColumnPerUnit
        {
            get { return (int) GetValue(RulerColumnPerUnitProperty); }
            set { SetValue(RulerColumnPerUnitProperty, value); }
        }

        public static readonly DependencyProperty RulerColumnPerUnitProperty =
            DependencyProperty.Register(nameof(RulerColumnPerUnit), typeof(int), typeof(HorizontalGridLine),
                new FrameworkPropertyMetadata(30, FrameworkPropertyMetadataOptions.AffectsRender));

        public Brush ColumnColor
        {
            get { return (Brush) GetValue(ColumnColorProperty); }
            set { SetValue(ColumnColorProperty, value); }
        }

        public static readonly DependencyProperty ColumnColorProperty =
            DependencyProperty.Register(nameof(ColumnColor), typeof(Brush), typeof(HorizontalGridLine),
                new FrameworkPropertyMetadata(Brushes.DarkGray, FrameworkPropertyMetadataOptions.AffectsRender));

        public double ColumnWidth
        {
            get { return (double) GetValue(ColumnWidthProperty); }
            set
            {
                value = value <= 0 ? 1 : value;
                SetValue(ColumnWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ColumnWidthProperty =
            DependencyProperty.Register(nameof(ColumnWidth), typeof(double), typeof(HorizontalGridLine),
                new FrameworkPropertyMetadata((double) 20.0f, FrameworkPropertyMetadataOptions.AffectsRender));

        public Vector Viewport
        {
            get { return (Vector) GetValue(ViewportProperty); }
            set { SetValue(ViewportProperty, value); }
        }

        public static readonly DependencyProperty ViewportProperty =
            DependencyProperty.Register(nameof(Viewport), typeof(Vector), typeof(HorizontalGridLine),
                new FrameworkPropertyMetadata(new Vector(0, 0), FrameworkPropertyMetadataOptions.AffectsRender));

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(
            nameof(Zoom), typeof(float), typeof(HorizontalGridLine),
            new FrameworkPropertyMetadata(1f, FrameworkPropertyMetadataOptions.AffectsRender,
                null, CoerceValueCallback));

        private static object CoerceValueCallback(DependencyObject d, object value)
        {
            var v = (float) value;
            var newV = (float) Mathf.Clamp(0.000001, 20, v);
            return newV;
        }

        public float Zoom
        {
            get { return (float) GetValue(ZoomProperty); }
            set { SetValue(ZoomProperty, value); }
        }

        private int minZoomLevel = 0;

        private int maxZoomLevel = 20;

        private int zoomLevel;

        #endregion Properties

        #region Constructors

        #endregion Constructors

        #region Overrides

        protected override void OnRender(DrawingContext drawingContext)
        {
            // this code does not draw lines that are out of width range
            var startFrame = (int) (Viewport.X / (ColumnWidth * Zoom));
            var endFrame = (int) (Viewport.Y / (ColumnWidth * Zoom));

            var minHeight = RenderSize.Height * 0.25;
            var maxHeight = RenderSize.Height;

            zoomLevel = GetZoomLevel();
            
            var pluser = 1;
            
            for(int i = 0 ; i <= zoomLevel ; i++)
            {
                double dX = GetDistanceBetweenSameLevel(i);
                
                if (dX < 5)
                {
                    var nextLevel = i + 1;
                    pluser = GetShowRange(nextLevel); 
                }
            }

            for (var i = (startFrame / pluser ) * pluser; i < endFrame; i += pluser)
            {
                var x1 = i * ColumnWidth * Zoom;
                double y1 = 0;

                var x2 = i * ColumnWidth * Zoom;
                var y2 = RenderSize.Height;
                
                var level = GetColumnLevel(i);
                var distance = GetDistanceBetweenSameLevel(level);
                var penThickness = Mathf.Clamp(0, 1, distance / ColumnWidth / 4);

                if (Ruler)
                {
                    var unit = i / RulerColumnPerUnit;
                    var minUnit = i % RulerColumnPerUnit;

                    var newY1 = minHeight;
                    y1 = Mathf.Clamp(minHeight, maxHeight, newY1 / (distance / ColumnWidth));

                    var formattedText = new FormattedText(
                        $"{(unit >= 10 ? unit.ToString() : "0" + unit)}:{(minUnit >= 10 ? minUnit.ToString() : "0" + minUnit)}",
                        CultureInfo.CurrentCulture,
                        FlowDirection.LeftToRight,
                        new Typeface(RulerFont, FontStyles.Normal, FontWeights.Normal, FontStretches.Normal),
                        RulerFontSize,
                        RulerFontColor);

                    // set text x post
                    var xText = x1 + formattedText.Width / 4;
                    // set text y post
                    var yText = (minHeight) - formattedText.Height / 2;
                    
                    if(distance > 50)
                        drawingContext.DrawText(formattedText, new Point(xText, yText));
                }

                var pen = new Pen(ColumnColor, penThickness);

                drawingContext.DrawLine(pen, new Point(x1, y1), new Point(x2, y2));
            }

            // send back the drawingContext object
            base.OnRender(drawingContext);
        }

        private int GetZoomLevel()
        {
            return Zoom < 1 ? ((int) Math.Log(Zoom, 0.5) < 20 ?  (int) Math.Log(Zoom, 0.5) : maxZoomLevel) : minZoomLevel;
        }

        private int GetShowRange(int level)
        {
            int showRange = (int) (level == 0 ? 1 :
                level == 1 ? 5 :
                (int) Math.Round((float) RulerColumnPerUnit / 2) * Math.Pow(2, level - 2));
            return showRange;
        }

        private int GetColumnLevel(int index)
        {
            if (index == 0) return 100;
            if (index % 5 != 0) return 0;

            var level = 0;

            for (var i = 1; i <= maxZoomLevel; i++)
            {
                var showRange = GetShowRange(i);

                if (index % showRange == 0)
                    level++;
            }

            return level;
        }

        private double GetDistanceBetweenSameLevel(int level)
        {
            int showRange = GetShowRange(level);
                
            double x1 = showRange * ColumnWidth * Zoom;
            double x2 = showRange * ColumnWidth * Zoom * 2;

            double dX = x2 - x1;
            return Math.Abs(dX);
        }

        #endregion Overrides
    }
}