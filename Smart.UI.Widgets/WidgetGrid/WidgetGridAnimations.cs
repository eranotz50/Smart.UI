using System;
using System.Collections.Generic;
using Smart.UI.Classes.Extensions;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using Smart.UI.Classes.Animations;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Layout;
using Smart.UI.Panels;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Widgets
{
    /// <summary>
    /// Extention to widgetgriditself related to animations
    /// </summary>
    public static class WidgetGridAnimations
    {
      
        /// <summary>
        /// Moves element without changing its row and columnh params (so it stays in the same cell)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="destR"></param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        static public Animation MoveInCellsTo<T>(this T source, FrameworkElement element, CellsRegion destR, TimeSpan howLong = default(TimeSpan),
                                       IEasingFunction easing = null) where T:WidgetGrid
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                {
                    Rect from = source.InvisibleChange(element, destR);
                    if (from.Equals(Rect.Empty)) from = element.GetBounds();
                    element.SetPos(default(Pos3D));

                    Rect cell = source.GetCellsRect(destR);
                    double x = cell.X - from.X;
                    double y = cell.Y - from.Y;
                    double w = cell.Width - from.Width;
                    double h = cell.Height - from.Height;

                    ani.DoOnNext +=
                        q =>
                        source.PlaceInCells(element,
                                     new Rect(from.X + x * q, from.Y + y * q, from.Width + w * q, from.Height + h * q));
                    ani.OnNext(i);
                });
        }

        /// <summary>
        /// Moves several elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">WidgetGrid</param>
        /// <param name="objects">elements with their target positions</param>
        /// <param name="howLong">duration of animation</param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public static Animation MoveInCellsMassive<T>(this T source, IEnumerable<Tuple<FrameworkElement,Rect>> objects, TimeSpan howLong = default(TimeSpan),
                                       IEasingFunction easing = null) where T:WidgetGrid
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                         var targets =objects.Select(
                            o =>
                            new{
                                    Element = o.Item1,
                                    From = o.Item1.GetBounds()                                                                        ,
                                    To = o.Item2
                                }).Select(s=>new
                                                 {
                                                     s.Element, s.From, s.To,
                                                     deltaX = s.To.X - s.From.X,
                                                     deltaY = s.To.Y - s.From.Y,
                                                     deltaWidth = s.To.Width - s.From.Width,
                                                     deltaHeight = s.To.Height - s.From.Height,
  
                                                     
                                                 });

                        ani.DoOnNext +=
                            q =>
                                targets.ForEach(t=>                                    
                                        source.PlaceInCells(t.Element,
                                            new Rect(
                                                t.From.X+t.deltaX*q,
                                                t.From.Y+t.deltaY*q,
                                                t.From.Width+t.deltaWidth*q,
                                                t.From.Height+t.deltaHeight*q))
                                                );
                                                                                                        
                         ani.OnNext(i);
                    });        
        }

        #region MOVEMENTS UP DOWN RIGHT

        static public Animation MoveUp<T>(this T source, FrameworkElement element, int shift = 1, TimeSpan howlong = default(TimeSpan)) where T:WidgetGrid
        {
            return source.MoveDown(element, -shift, howlong);
        }

        static public Animation MoveDown<T>(this T source, FrameworkElement element, int shift = 1, TimeSpan howlong = default(TimeSpan)) where T:WidgetGrid
        {
            int row = FlexGrid.GetRow(element);
            int sum = row + shift;
            int span = FlexGrid.GetRowSpan(element);
            row = row + shift < 0 || sum + span >= source.RowDefinitions.Length ? row : sum;
            return source.MoveToCell(element, FlexGrid.GetColumn(element), row, FlexGrid.GetColumnSpan(element), span, howlong);
        }

        static public Animation MoveRight<T>(this T source, FrameworkElement element, int shift = 1, TimeSpan howlong = default(TimeSpan)) where T:WidgetGrid
        {
            int col = FlexGrid.GetColumn(element);
            int sum = col + shift;
            int span = FlexGrid.GetColumnSpan(element);
            col = col + shift < 0 || sum + span >= source.ColumnDefinitions.Length ? col : sum;
            return source.MoveToCell(element, col, FlexGrid.GetRow(element), span, FlexGrid.GetRowSpan(element), howlong);
        }

        static public Animation MoveLeft<T>(this T source, FrameworkElement element, int shift = 1, TimeSpan howlong = default(TimeSpan)) where T : WidgetGrid
        {
            return source.MoveRight(element, -shift, howlong);
        }

        #endregion

        #region ZOOMING

        static public Animation ZoomAt<T>(this T source, FrameworkElement element, TimeSpan howLong, IEasingFunction easing = null) where T:WidgetGrid
        {
            return source.ZoomAt(element.GetRelativeRect(source), howLong, easing);
        }

        static public Animation ZoomAt<T>(this T source, Rect bounds, TimeSpan howLong, IEasingFunction easing = null) where T:WidgetGrid
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                {
                    Rect rect = source.Space.Panel;
                    //var bounds = element.GetRelativeRect(this);
                    Size size = source.Space.Canvas.Size();
                    var factor = new Size(rect.Width / bounds.Width, rect.Height / bounds.Height);
                    Size newCanvasSize = size.MultiplyBy(factor);
                    Point delta = newCanvasSize.Substract(size);
                    var endShift = new Point(factor.Width * bounds.X, factor.Height * bounds.Y);
                    ani.DoOnNext += q =>
                    {
                        var f = new Point(q * (factor.Width - 1) + 1, q * (factor.Height - 1) + 1);
                        //this.CanvasSize = size.MultiplyBy(f);

                        source.Space.CanvasChange.FireChange(
                            source.Space.Canvas.SetSize(size.Width + delta.X * q, size.Height + delta.Y * q));
                        source.Space.PanelChange.FireChange(
                            source.Space.Panel.SetCoords(rect.X * f.X + endShift.X * q,
                                                  rect.Y * f.Y + endShift.Y * q));
                    };
                    ani.OnNext(i);
                });
        }

        #endregion

        #region OLD (USED TO BE INSIDE A CLASS
        /*
        #region MOVEMENTS UP DOWN RIGHT

        /// <summary>
        /// Moves element without changing its row and columnh params (so it stays in the same cell)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="destR"></param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public Animation MoveInCellsTo(FrameworkElement element, CellsRegion destR, TimeSpan howLong = default(TimeSpan),
                                       IEasingFunction easing = null)
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                {
                    Rect from = InvisibleChange(element, destR);
                    if (from.Equals(Rect.Empty)) from = element.GetBounds();
                    element.SetPos(default(Pos3D));

                    Rect cell = GetCellsRect(destR);
                    double x = cell.X - from.X;
                    double y = cell.Y - from.Y;
                    double w = cell.Width - from.Width;
                    double h = cell.Height - from.Height;

                    ani.DoOnNext +=
                        q =>
                        PlaceInCells(element,
                                     new Rect(from.X + x * q, from.Y + y * q, from.Width + w * q, from.Height + h * q));
                    ani.OnNext(i);
                });
        }

        public Animation MoveUp(FrameworkElement element, int shift = 1, TimeSpan howlong = default(TimeSpan))
        {
            return MoveDown(element, -shift, howlong);
        }

        public Animation MoveDown(FrameworkElement element, int shift = 1, TimeSpan howlong = default(TimeSpan))
        {
            int row = GetRow(element);
            int sum = row + shift;
            int span = GetRowSpan(element);
            row = row + shift < 0 || sum + span >= RowDefinitions.Length ? row : sum;
            return MoveToCell(element, GetColumn(element), row, GetColumnSpan(element), span, howlong);
        }

        public Animation MoveRight(FrameworkElement element, int shift = 1, TimeSpan howlong = default(TimeSpan))
        {
            int col = GetColumn(element);
            int sum = col + shift;
            int span = GetColumnSpan(element);
            col = col + shift < 0 || sum + span >= ColumnDefinitions.Length ? col : sum;
            return MoveToCell(element, col, GetRow(element), span, GetRowSpan(element), howlong);
        }

        public Animation MoveLeft(FrameworkElement element, int shift = 1, TimeSpan howlong = default(TimeSpan))
        {
            return MoveRight(element, -shift, howlong);
        }
        #endregion
        */
        #endregion
    }
}
