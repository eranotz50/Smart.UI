using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
//#if SILVERLIGHT
namespace Smart.UI.Panels
{
    /// <summary>
    /// Class for lines with arrows drawing
    /// </summary>
    public class LinesPath
#if SILVERLIGHT
        :Path
#else
        :FreePath
#endif
    {
        public LinesPath()
        {
            Data = new GeometryGroup();
        }

        public GeometryGroup Geometry
        {
            get { return Data as GeometryGroup; }
            set { Data = value; }
        }

        public GeometryCollection Lines
        {
            get { return Geometry.Children; }
        }


        protected override Size MeasureOverride(Size availableSize)
        {
            return availableSize; // availableSize.IsValid() ? availableSize : new Size(1, 1);
        }

        public void AddLine(Point start, Point end)
        {
            Geometry.Children.Add(new LineGeometry {StartPoint = start, EndPoint = end});
        }

        public void MakeLine(Point start, Point end, int index = -1)
        {
            if (index == -1 || index >= Geometry.Children.Count)
                AddLine(start, end);
            else
            {
                if (Geometry.Children[index] is LineGeometry)
                {
                    var line = (LineGeometry) Geometry.Children[index];
                    line.StartPoint = start;
                    line.EndPoint = end;
                }
                else Geometry.Children[index] = new LineGeometry {StartPoint = start, EndPoint = end};
            }
        }
    }
}
//#endif