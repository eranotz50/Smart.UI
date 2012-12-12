using System;
using System.Windows;
using Smart.UI.Panels;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Relatives
{
    public static class RelativeExtensions
    {
        #region EASY RELATIVE SETTERS

        public static T SetRelativeTo<T>(this T source, String value) where T : FrameworkElement
        {
            Relative.SetTo(source, value);
            return source;
        }

        public static String GetRelativeTo<T>(this T source) where T : FrameworkElement
        {
            return Relative.GetTo(source);
        }

        public static T SetRelativeElement<T>(this T source, FrameworkElement value) where T : FrameworkElement
        {
            Relative.SetRelativeElement(source, value);
            return source;
        }

        public static FrameworkElement GetRelativeElement<T>(this T source) where T : FrameworkElement
        {
            return Relative.GetRelativeElement(source);
        }

        public static T SetPosition<T>(this T source, Action<Args<FrameworkElement, Rect, Size>, FrameworkElement> value)
            where T : FrameworkElement
        {
            Relative.SetPosition(source, value.AsString());
            return source;
        }

        public static String GetPosition<T>(this T source) where T : FrameworkElement
        {
            return Relative.GetPosition(source);
        }

        public static T SetRelativePosition<T>(this T source,
                                               ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>
                                                   value) where T : FrameworkElement
        {
            Relative.SetRelativePosition(source, value);
            return source;
        }

        public static T SetStringPosition<T>(this T source, String value) where T : FrameworkElement
        {
            Relative.SetPosition(source, value);
            return source;
        }

        public static T SetDragGrowthMode<T>(this T source, GrowthMode value) where T : FrameworkElement
        {
            Relative.SetDragGrowthMode(source, value);
            return source;
        }

        public static ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement> GetRelativePosition<T>(
            this T source) where T : FrameworkElement
        {
            return Relative.GetRelativePosition(source);
        }

        #endregion

        #region RELATIVE POSITIONING

        public static T PosLeft<T>(this T element, double value) where T : FrameworkElement
        {
            Pos3D pos = element.GetPos();
            if (pos.NotEmpty)
            {
                pos.X = value;
                element.SetPos(pos /*.Clone*/);
                return element;
            }
            Rect slot = element.GetSlot();
            if (!slot.IsEmpty)
                element.SetSlot(new Rect(value, slot.Y, slot.Width, slot.Height));
                    //.GetParent<SimplePanel>().InvalidateArrange();
            return element;
        }

        public static T PosTop<T>(this T element, double top) where T : FrameworkElement
        {
            /*
            Pos3D pos = element.GetPos();
            if (pos.NotEmpty)
            {
                pos.Y = value;
                element.SetPos(pos.Clone);
                return element;
            }*/
            Rect slot = element.GetSlot();
            if (slot.IsEmpty == false)
                element.SetSlot(new Rect(slot.X, top, slot.Width, slot.Height));
                    //.GetParent<SimplePanel>().InvalidateArrange();
            return element;
        }

        public static T PosLeftTop<T>(this T element, double left, double top) where T : FrameworkElement
        {
            /*
            Pos3D pos = element.GetPos();
            if (pos.NotEmpty)
            {
                pos.X = left;
                pos.Y = top;
                element.SetPos(pos.Clone);
                return element;
            }*/
            Rect slot = element.GetSlot();
            if (!slot.IsEmpty)
                element.SetSlot(new Rect(left, top, slot.Width, slot.Height));
                    //.GetParent<SimplePanel>().InvalidateArrange(); ;
            ;
            return element;
        }

        public static T PosLeftMiddle<T>(this T element, double top, double height, double left) where T : FrameworkElement
        {
            Rect slot = element.GetSlot();
            if (!slot.IsEmpty)
            {
               var delta = height - slot.Height;
               element.SetSlot(new Rect(left, top+delta/2, slot.Width, slot.Height));

            }
            //.GetParent<SimplePanel>().InvalidateArrange(); ;
            ;
            return element;
        }

        public static T PosCenterTop<T>(this T element, double left, double width, double top) where T : FrameworkElement
        {
            Rect slot = element.GetSlot();
            if (!slot.IsEmpty)
            {
                var delta = width - slot.Width;
                element.SetSlot(new Rect(left+ delta/2, top, slot.Width, slot.Height));
            }
            //.GetParent<SimplePanel>().InvalidateArrange(); ;
            ;
            return element;
        }


        /*
        public static T PosLeftMiddle<T>(this T element, Point middle) where T : FrameworkElement
        {            
            Rect slot = element.GetSlot();
            if (!slot.IsEmpty)
            {
                var center = slot.Center();
                var shift = center.Substract(middle);
                element.SetSlot(new Rect(slot.X+shift.X, slot.Y+shift.Y, slot.Width, slot.Height));
            
            }
            //.GetParent<SimplePanel>().InvalidateArrange(); ;
            ;
            return element;
        }
        */
        #endregion

        #region PUT RELATIVE TO

        public static T PutLeftFrom<T>(this T source, FrameworkElement target, double value = 0.0)
            where T : FrameworkElement
        {
            target.UpdateOnArrange(i =>
                                   source.PosLeft(i.Param1.X - source.ActualWidth - value));
            return source;
        }

        public static T PutRightFrom<T>(this T source, FrameworkElement target, double value = 0.0)
            where T : FrameworkElement
        {
            target.UpdateOnArrange(i =>
                                   source.PosLeft(value + i.Param1.X + i.Param1.Width));
            return source;
        }

        public static T PutOnTopOf<T>(this T source, FrameworkElement target, double value = 0.0)
            where T : FrameworkElement
        {
            target.UpdateOnArrange(i =>
                                   source.PosTop(i.Param1.Y - source.ActualHeight - value));
            return source;
        }

        public static T PutTheBottomOf<T>(this T source, FrameworkElement target, double value = 0.0)
            where T : FrameworkElement
        {
            target.UpdateOnArrange(i =>
                                   source.PosTop(value + i.Param1.Y + i.Param1.Height));
            return source;
        }

        #endregion

        #region SET FROM

        /// <summary>
        /// Different from PutLeftFrom that does not take into account target's width
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T SetLeftFrom<T>(this T source, FrameworkElement target, double value = 0.0)
            where T : FrameworkElement
        {
            target.UpdateOnArrange(i => source.PosLeft(i.Param1.X - value));
            return source;
        }

        /// <summary>
        /// Different from PutLeftFrom that does not take into account target's height
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T SetTopFrom<T>(this T source, FrameworkElement target, double value = 0.0)
            where T : FrameworkElement
        {
            target.UpdateOnArrange(i => source.PosTop(i.Param1.Y - value));
            return source;
        }

        /// <summary>
        /// Positions an element relative to another;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="coords"></param>
        /// <returns></returns>
        public static T SetPositionFrom<T>(this T source, FrameworkElement target, Point coords = new Point())
            where T : FrameworkElement
        {
            if (coords.X < 0) source.PutLeftFrom(target, -coords.X);
            else source.PutRightFrom(target, coords.X);
            if (coords.Y < 0) source.PutOnTopOf(target, -coords.Y);
            else source.PutTheBottomOf(target, coords.Y);
            return source;
        }

        /// <summary>
        /// Sets width taken from another element and adds some value to it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Our element</param>
        /// <param name="target">Element wich actual width we shall take</param>
        /// <param name="value">value to be added to target's actual width</param>
        /// <returns></returns>
        public static T SetWidthFrom<T>(this T source, FrameworkElement target, double value = 0.0)
            where T : FrameworkElement
        {
            target.UpdateOnArrange(i =>
                                       {
                                           double w = target.ActualWidth + value;
                                           if (source.Width.Equals(w) == false) source.Width = w;
                                       });
            return source;
        }

        /// <summary>
        ///  Sets width taken from another element, multiplies it and adds some value to it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">Element wich actual width we shall take</param>
        /// <param name="target">Element wich width we shall take</param>
        /// <param name="q"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T SetWidthMultipliedFrom<T>(this T source, FrameworkElement target, double q = 1.0,
                                                  double value = 0.0) where T : FrameworkElement
        {
            target.UpdateOnArrange(i =>
                                       {
                                           double w = target.ActualWidth*q + value;
                                           //in order to avoid layout cycle
                                           if (source.Width.Equals(w) == false) source.Width = w;
                                       });
            return source;
        }

        public static T SetHeightFrom<T>(this T source, FrameworkElement target, double value = 0.0)
            where T : FrameworkElement
        {
            target.UpdateOnArrange(i =>
                                       {
                                           double h = target.ActualHeight + value;
                                           //in order to avoid layout cycle
                                           if (source.Height.Equals(h) == false) source.Height = h;
                                       });


            return source;
        }

        public static T SetHeightMultipliedFrom<T>(this T source, FrameworkElement target, double q = 1.0,
                                                   double value = 0.0) where T : FrameworkElement
        {
            target.UpdateOnArrange(i =>
                                       {
                                           double h = target.ActualHeight*q + value;
                                           //in order to avoid layout cycle
                                           if (source.Height.Equals(h) == false) source.Height = h;
                                       });
            return source;
        }

        /// <summary>
        /// Sets element's size and position based on another element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <param name="modifier">sets element's position by modifying target's position</param>
        /// <returns></returns>
        public static T SetLayoutSlotFrom<T>(this T source, FrameworkElement target, Func<Rect, Rect> modifier)
            where T : FrameworkElement
        {
            target.UpdateOnArrange(
                i => source.SetSlot(modifier(target.GetSlot())).GetParent<SimplePanel>().InvalidateArrange());
            return source;
        }

        #endregion
    }
}