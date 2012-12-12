using System.Windows;
using Smart.UI.Controls.Scrollers;
using Smart.UI.Panels;
using Smart.UI.Widgets.PanelAdorners;

namespace Smart.UI.Widgets
{
    public class WallScrollManager : ScrollManager<Wall>
    {
    }

    /// <summary>
    /// EXPERIMENTAL
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ScrollManager<T> : SimpleAdorner<T> where T : SimplePanel
    {
        public HorizontalScrollBar HorizontalScroll;
        public T Target;
        public VerticalScrollBar VerticalScroll;

        public ScrollManager()
        {
            HorizontalScroll = new HorizontalScrollBar();
            VerticalScroll = new VerticalScrollBar();
        }

        public override void Attach(T panel)
        {
            base.Attach(panel);
            panel.AddChild(HorizontalScroll);
            panel.AddChild(VerticalScroll);
        }

        public override void Deactivate()
        {
            Host.RemoveChild(HorizontalScroll);
            Host.RemoveChild(VerticalScroll);
            base.Deactivate();
        }


        protected override void OnBeforeMeasurement(Size constrains)
        {
            //throw new NotImplementedException();
        }

        protected override void OnAfterMeasurement(Size constrains)
        {
            //throw new NotImplementedException();
        }

        protected override void OnBeforeArrange(Size constrains)
        {
            //throw new NotImplementedException();
        }

        protected override void OnAfterArrange(Size constrains)
        {
            //throw new NotImplementedException();
        }
    }
}