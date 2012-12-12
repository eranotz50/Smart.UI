using System.Windows;

//#if SILVERLIGHT
namespace Smart.UI.Panels
{
    /// <summary>
    /// Class for horizontal line drawing
    /// </summary>
    public class HorizontalArrow : Arrow
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            var start = new Point();
            var end = new Point();
            start.X = StartPoint.X;
            end.X = (EndPoint.X.Equals(0.0) && EndPoint.Y.Equals(0)) ? finalSize.Width : EndPoint.X;
                // если конечные координаты не указаны, в качестве правой точки ставим ширину канваса
            switch (VerticalAlignment)
            {
                case VerticalAlignment.Top:
                case VerticalAlignment.Stretch:
                    start.Y = end.Y = 0;
                    break;
                case VerticalAlignment.Center:
                    start.Y = end.Y = finalSize.Height/2;
                    break;
                case VerticalAlignment.Bottom:
                    start.Y = end.Y = finalSize.Height;
                    break;
            }
            CreateFigure(start, end);
            return finalSize;
        }
    }
}
//#endif