using System.Windows;
using System.Windows.Media;

//#if SILVERLIGHT
namespace Smart.UI.Panels
{
    public enum TriangleOrientation
    {
        Top,
        Left,
        Right,
        Bottom
    }

    public class Triangle : LinesPath
    {
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof (TriangleOrientation), typeof (Triangle),
                                        new PropertyMetadata(TriangleOrientation.Top, InvalidateArrangeCallback));

        public Point A;
        public Point B;
        public Point C;
        public PathFigure Figure;
        public PathGeometry Path;
        public PolyLineSegment Segment;

        public Triangle()
        {
            Path = new PathGeometry();
            Figure = new PathFigure {IsClosed = true, IsFilled = true};
            Segment = new PolyLineSegment();
            Figure.Segments.Add(Segment);
            Path.Figures.Add(Figure);
            Lines.Add(Path);
            Segment.Points = new PointCollection {A, B, C, A};
        }

        public TriangleOrientation Orientation
        {
            get { return (TriangleOrientation) GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// Callback than invalidates arrange of the element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void InvalidateArrangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe != null) fe.InvalidateArrange();
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            switch (Orientation)
            {
                case TriangleOrientation.Bottom:
                    A = new Point();
                    B = new Point(finalSize.Width, 0.0);
                    C = new Point(finalSize.Width/2, finalSize.Height);
                    break;
                case TriangleOrientation.Top:
                    A = new Point(finalSize.Width, finalSize.Height);
                    B = new Point(0.0, finalSize.Height);
                    C = new Point(finalSize.Width/2, 0.0);
                    break;
                case TriangleOrientation.Left:
                    A = new Point(finalSize.Width, 0.0);
                    B = new Point(finalSize.Width, finalSize.Height);
                    C = new Point(0.0, finalSize.Height/2);
                    break;
                case TriangleOrientation.Right:
                    A = new Point(0.0, finalSize.Height);
                    B = new Point(0.0, 0.0);
                    C = new Point(finalSize.Width, finalSize.Height/2);
                    break;
            }
            Segment.Points[0] = A;
            Segment.Points[1] = B;
            Segment.Points[2] = C;
            Segment.Points[3] = A;
            return finalSize;
            // return base.ArrangeOverride(finalSize);
        }
    }
}
//#endif