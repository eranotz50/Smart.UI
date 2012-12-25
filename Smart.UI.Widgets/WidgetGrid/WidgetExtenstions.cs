using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Media.Animation;
using Smart.Classes.Extensions;
using System.Linq;
using Smart.UI.Panels;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Widgets
{
    /// <summary>
    /// Extensions for framework elements using functionality of widgegrids
    /// </summary>
    public static class WidgetExtenstions
    {
      
        public static FrameworkElement ParkToGrid(this FrameworkElement element, CellsRegion region)
        {
            return
                element.SetRow(region.Row).SetRowSpan(region.RowSpan).SetColumn(region.Col).SetColumnSpan(region.ColSpan);
        }

        /// <summary>
        /// Pos Columns, Rows and spans by cell region info
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static T SetCellsRegion<T>(this T source, CellsRegion region) where T : FrameworkElement
        {
            FlexGrid.SetColumn(source, region.Col);
            FlexGrid.SetRow(source, region.Row);
            FlexGrid.SetColumnSpan(source, region.ColSpan);
            FlexGrid.SetRowSpan(source, region.RowSpan);
            return source;
        }

        public static T SetPosition<T>(this CellsRegion region, T source) where T : FrameworkElement
        {
            return source.SetCellsRegion(region);
        }

        public static Animation GetMovement(this FrameworkElement element)
        {
            return WidgetGrid.GetMovement(element);
        }

        /// <summary>
        /// Animatevly moves and resize element somewhere not leaving the cell region it is attached to
        /// </summary>
        /// <param name="element"></param>
        /// <param name="to"> </param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public static Animation MoveInCells(this FrameworkElement element, Rect to, TimeSpan howLong = default(TimeSpan),
                                            IEasingFunction easing = null)
        {
            var p = element.Parent as WidgetGrid;
            if (p == null) return null;
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                        CellsRegion region = p.GetCellsRegion(element);
                        Rect from = element.GetSlot();
                        if (from.Equals(Rect.Empty)) from = element.GetBounds();
                        double x = to.X - from.X;
                        double y = to.Y - from.Y;
                        double w = to.Width - from.Width;
                        double h = to.Height - from.Height;
                        ani.DoOnNext +=
                            q =>
                            p.PlaceInCells(element,
                                           new Rect(from.X + x*q, from.Y + y*q, from.Width + w*q, from.Height + h*q),
                                           region);
                        ani.DoOnCompleted += () => p.PlaceInCells(element, to, region);
                        ani.OnNext(i);
                    });
        }


        /// <summary>
        /// This function if often used when we want to change the relative size of the element
        /// for instance, make cool apearance or select effects
        /// </summary>
        /// <param name="element"></param>
        /// <param name="relative"></param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public static Animation ResizeInCellsRelatively(this FrameworkElement element, Point relative,
                                                        TimeSpan howLong = default(TimeSpan),
                                                        IEasingFunction easing = null)
        {
            Rect from = element.GetSlot();
            if (from.Equals(Rect.Empty)) from = element.GetBounds();
            return element.MoveInCells(from.ResizeRectRelatively(relative), howLong, easing);
        }

        /// <summary>
        /// This animation changes elements size in relative to its
        /// </summary>
        /// <param name="elements"></param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public static Animation ResizeInCellsRelatively(this IEnumerable<Tuple<FrameworkElement,Point>> elements,
                                                       TimeSpan howLong = default(TimeSpan),
                                                       IEasingFunction easing = null)
        {
            Contract.Requires(elements.Any());
            var p = elements.First().Item1.GetParent<WidgetGrid>();
            if (p == null) return null;
            List<Tuple<FrameworkElement, Rect>> objs = elements.Select(i =>
                                          {
                                              Rect from = i.Item1.GetSlot();
                                              if (from.Equals(Rect.Empty)) from = i.Item1.GetBounds();
                                              return new Tuple<FrameworkElement, Rect>(i.Item1,
                                                                                       from.ResizeRectRelatively(i.Item2));
                                          }).ToList();
            return p.MoveInCellsMassive(objs, howLong, easing);

        }
    }
}