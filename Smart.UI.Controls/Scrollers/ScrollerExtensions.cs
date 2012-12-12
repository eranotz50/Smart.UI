using System.Windows.Controls;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Controls.Scrollers
{
    public static class ScrollerExtensions
    {
        public static AbstractScrollBar GetMainScrollBar<T>(this T source) where T : IOrientable, IScrollHolder
        {
            return source.Orientation == Orientation.Horizontal
                       ? (AbstractScrollBar) source.HorizontalScroll
                       : source.VerticalScroll;
        }
    }
}