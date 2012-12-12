using System.Windows;

namespace Smart.UI.Widgets.PanelAdorners
{
    public class Resizer : BasicResizer
    {
        public RectangleAndEllipseBoundary Boundary
        {
            get { return Boundaries.Count > 0 ? Boundaries[0] : null; }
        }

        public override FrameworkElement Target
        {
            set
            {
                if (value == Target) return;
                _target = value;
                if (Host == null || Target == null) return;
                //_target.UpdateArrange();
                Host.InvalidateArrange();
                Boundary.Activate(Host, value);
            }
        }
    }
}