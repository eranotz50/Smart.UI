using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Smart.UI.Panels;


namespace Smart.UI.Controls.Scrollers
{
    [TemplatePart(Name = "Root", Type = typeof (SmartGrid))]
    [TemplatePart(Name = "Substract", Type = typeof (Rectangle))]
    [TemplatePart(Name = "Shadow", Type = typeof (Rectangle))]
    [TemplatePart(Name = "Slider", Type = typeof (Rectangle))]
    [TemplatePart(Name = "BackwardArrow", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "ForwardArrow", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "BackwardFastArrow", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "ForwardFastArrow", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "ForwardSplitter", Type = typeof (FrameworkElement))]
    [TemplatePart(Name = "BackwardSplitter", Type = typeof (FrameworkElement))]
    public class HorizontalScrollBar : AbstractScrollBar
    {
        public HorizontalScrollBar()
        {
            Orientation = Orientation.Horizontal;
            DefaultStyleKey = typeof (HorizontalScrollBar);
        }

#if WPF
        static HorizontalScrollBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HorizontalScrollBar), new FrameworkPropertyMetadata(typeof(HorizontalScrollBar)));
        }
#endif
    }
}