using System;
using System.Reactive;
using System.Windows;
using System.Windows.Input;
using Smart.UI.Controls.Scrollers;
using Smart.UI.Panels;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Widgets.PanelAdorners
{
    public class ZoomAdorner : BasicResizer
    {
        public HorizontalScrollBar HScroll;
        public FrameworkElement Slot;
        public VerticalScrollBar VScroll;


        public ZoomAdorner()
        {
            VScroll = new VerticalScrollBar().SetZIndex(DefZindex).SetElementType(ElementType.DecorationAdorner);
            HScroll = new HorizontalScrollBar().SetZIndex(DefZindex).SetElementType(ElementType.DecorationAdorner);
        }

        public override void Activate()
        {
            base.Activate();
            Host.MouseDownSubject.AddAction(MouseDownHandler);
            Host.MouseWheelSubject.AddAction(MouseWheelHandler);
            Host.ChildMouseEnterSubject.AddAction(MouseEnterHandler);
            Host.ChildMouseLeaveSubject.AddAction(MouseLeaveHandler);

            Host.AddChild(VScroll);
            Host.AddChild(HScroll);
        }

        protected virtual void MouseDownHandler(EventPattern<MouseButtonEventArgs> e)
        {
            var source = e.Sender as FrameworkElement;
            source = source.GetNearestDragPanelChild<Widgets.WidgetGrid>();
            if (source == null || source.Parent != Host) return;
            var h = (Host as UI.Widgets.WidgetGrid);
            if (h != null && source.GetElementType() == ElementType.Normal)
                h.ZoomAt(source, new TimeSpan(0, 0, 0, 0, 500)).Go();
        }

        public virtual void MouseEnterHandler(EventPattern<FrameworkElement, MouseEventArgs> e)
        {
            if (e.Sender == Host) return;
            FrameworkElement source = e.Sender;
            source = source.GetNearestDragPanelChild<Widgets.WidgetGrid>();
            if (Host == null) return;
            if (source.GetElementType() == ElementType.Normal /*&& source.Parent.Equals(Host)*/)
            {
                AddBoundary(source).AddChildren();
                Slot = source;
            }
        }

        public virtual void MouseLeaveHandler(EventPattern<FrameworkElement, MouseEventArgs> e)
        {
            if (e.Sender == Host) return;
            FrameworkElement source = e.Sender;
            source = source.GetNearestDragPanelChild<Widgets.WidgetGrid>();
            if (Host == null) return;
            if (source.GetElementType() == ElementType.Normal /* && source.Parent.Equals(Host)*/)
            {
                RemoveBoundaryByTarget(source);
                Slot = null;
            }
        }


        public override void Deactivate()
        {
            base.Deactivate();
            Host.MouseDownSubject.RemoveAction(MouseDownHandler);
            Host.MouseWheelSubject.RemoveAction(MouseWheelHandler);
            Host.ChildMouseEnterSubject.RemoveAction(MouseEnterHandler);
            Host.ChildMouseLeaveSubject.RemoveAction(MouseLeaveHandler);
            Host.RemoveChild(VScroll);
            Host.RemoveChild(HScroll);
        }

        private void MouseWheelHandler(EventPattern<MouseWheelEventArgs> e)
        {
            var h = (Host as UI.Widgets.WidgetGrid);
            var delta = (double) e.EventArgs.Delta;
            if (Slot != null && h != null)
                h.ZoomAt(new Point(delta/1200, delta/1200), Slot);
        }
    }
}