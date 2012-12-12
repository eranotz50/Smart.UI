using System.Windows;

//#if SILVERLIGHT
namespace Smart.UI.Panels
{
    /// <summary>
    /// Class for vertical lines drawing
    /// </summary>
    public class VerticalLinesPath : Arrow
    {
        protected override Size ArrangeOverride(Size finalSize)
        {
            var start = new Point();
            var end = new Point();
            start.Y = StartPoint.Y;
            end.Y = (EndPoint.X.Equals(0.0) && EndPoint.Y.Equals(0.0)) ? finalSize.Height : EndPoint.Y;
                // если конечные координаты не указаны, в качестве конечной точки ставим высоту канваса
            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                case HorizontalAlignment.Stretch:
                    start.X = end.X = 0;
                    break;
                case HorizontalAlignment.Center:
                    start.X = end.X = finalSize.Width/2;
                    break;
                case HorizontalAlignment.Right:
                    start.X = end.X = finalSize.Width;
                    break;
            }
            CreateFigure(start, end);
            return finalSize;
        }
    }
}
//#endif