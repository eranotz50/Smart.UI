using System;
using System.Windows;
using System.Windows.Input;
using Smart.UI.Panels;
using Smart.UI.Widgets;
using Smart.Classes.Subjects;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Events;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Widgets.PanelAdorners
{
    /// <summary>
    /// Needed to inject animations int wall
    /// </summary>
    public class WallAnimator : CanvasAdorner<UI.Widgets.Wall>
    {
        public const String AbsoluteCellResize = "AbsoluteCellResize";

        /// <summary>
        /// When wallanimator is attached to panel and then activated
        /// </summary>
        public override void Activate()
        {
            base.Activate();
            if (Panel.Items == null) return;
            foreach (FrameworkElement item in Panel.Items) AddHandlers(item);
            Panel.Items.ItemsAdded.DoOnNext += AddHandlers;
            //Panel.ItemPositionUpdate.DoOnNext += ItemPositionChangeHandler;
            Panel.EventManager.AddEventHandler<RelativeResizeEvent>(ResizeHandler);
            Panel.EventManager.AddEventHandler<RelativeResizeEvent>(AbsoluteCellResize, AbsoluteResizeHandler);
        }
        /*
        /// <summary>
        /// When item changes its position
        /// </summary>
        /// <param name="args"></param>
        protected void ItemPositionChangeHandler(Args<FrameworkElement, OrientedCellsRegion, OrientedCellsRegion> args)
        {
            FrameworkElement item = args.Param0;
            CellsRegion region = args.Param2.ToCellsRegion(Panel.Orientation);
            Panel.InvisibleChange(item, region);
            Animation ani = Panel.MoveInCellsTo(item, region,
                                                new TimeSpan(0, 0, 0, 1), Easings.CubicEaseInOut);
            WidgetGrid.SetMovement(item, ani.Go());
        }
        */
        protected void ResizeHandler(RelativeResizeEvent args)
        {
            args.Target.ResizeInCellsRelatively(args.RelativeSize, args.Duration, args.Easing).Go();
        }

        protected void AbsoluteResizeHandler(RelativeResizeEvent args)
        {
            args.Target.MoveInCells(Panel.GetCellsRect(args.Target).ResizeRectRelatively(args.RelativeSize),
                                    args.Duration, args.Easing).Go();
        }

        public void AddHandlers(FrameworkElement item)
        {
            SimpleSubject<Args<FrameworkElement, Rect, Size>> onArrange = item.GetOrNewOnArrange();
            onArrange.OnceIf(i => i.Param0.Visibility == Visibility.Visible, i =>
                                                                                 {
                                                                                     Panel.RunEvent(
                                                                                         new RelativeResizeEvent(item,
                                                                                                                 new Point
                                                                                                                     (1,
                                                                                                                      1),
                                                                                                                 UI.Widgets.WidgetGrid
                                                                                                                     .
                                                                                                                     DefaultMovementTime,
                                                                                                                 Easings
                                                                                                                     .
                                                                                                                     CubicEaseInOut));
                                                                                     Panel.PlaceInCells(item,
                                                                                                        item.GetSlot().
                                                                                                            SetSize(10,
                                                                                                                    10));
                                                                                 }
                );
            item.MouseEnter += item_MouseEnter;
            item.MouseLeave += item_MouseLeave;
        }

        private void item_MouseLeave(object sender, MouseEventArgs e)
        {
            var item = sender as FrameworkElement;
            if (item == null) return;
            item.GetEventsRaiser().RaiseEvent(AbsoluteCellResize,
                                              new RelativeResizeEvent(item, new Point(1, 1),
                                                                      UI.Widgets.WidgetGrid.DefaultMovementTime,
                                                                      Easings.CubicEaseInOut));
            item.AddZIndex(-10);
        }

        private void item_MouseEnter(object sender, MouseEventArgs e)
        {
            var item = sender as FrameworkElement;
            if (item == null) return;
            item.GetEventsRaiser().RaiseEvent(AbsoluteCellResize,
                                              new RelativeResizeEvent(item, new Point(1.1, 1.1),
                                                                      UI.Widgets.WidgetGrid.DefaultMovementTime,
                                                                      Easings.CubicEaseInOut));
            item.AddZIndex(10);
        }


        public void RemoveHandlers(FrameworkElement item)
        {
            item.MouseEnter -= item_MouseEnter;
            item.MouseLeave -= item_MouseLeave;
        }

        public override void Deactivate()
        {
            base.Deactivate();
           // Panel.ItemPositionUpdate.DoOnNext -= ItemPositionChangeHandler;
            Panel.Items.ItemsAdded.DoOnNext -= AddHandlers;
            foreach (FrameworkElement item in Panel.Items) RemoveHandlers(item);
            Panel.EventManager.RemoveHandler<RelativeResizeEvent>(ResizeHandler);
            Panel.EventManager.RemoveHandler<RelativeResizeEvent>(AbsoluteCellResize, AbsoluteResizeHandler);
        }
    }
}