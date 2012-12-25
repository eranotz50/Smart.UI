using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media.Animation;
using Smart.UI.Panels;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
    /// <summary>
    /// /Animations for movement of elements on smart panels
    /// </summary>
    public static class MovementAnimations
    {
        /// <summary>
        /// Creates expanding animation for the collection of elements
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="elements"></param>
        /// <param name="point"></param>
        /// <param name="len"></param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public static Animation CreateExpand<T>(this IEnumerable<T> elements, Pos3D point, double len,
                                                TimeSpan howLong = default(TimeSpan), IEasingFunction easing = null)
            where T : FrameworkElement
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                        foreach (T element in elements)
                        {
                            T el = element;
                            Pos3D from = element.ExtractPos();
                            Pos3D to = point + point.DirectionTo(from)*len;
                            Pos3D delta = to - from;
                            ani.DoOnNext += q => SimplePanel.SetPos(el, delta*q + from);
                            ani.DoOnCompleted += () => SimplePanel.SetPos(el, to);
                            ani.OnNext(i);
                        }
                    });
        }

        /// <summary>
        /// Movement animation that use Pos3D
        /// </summary>
        /// <param name="element"></param>
        /// <param name="to"></param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public static Animation MoveTo(this FrameworkElement element, Pos3D to, TimeSpan howLong = default(TimeSpan),
                                       IEasingFunction easing = null)
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                        Pos3D from = element.ExtractPos();
                        Pos3D delta = to - from;
                        ani.DoOnNext += q => SimplePanel.SetPos(element, delta*q + from);
                        ani.DoOnCompleted += () => SimplePanel.SetPos(element, to);
                        ani.OnNext(i);
                    });
        }

        /// <summary>
        /// Movement without animation
        /// </summary>
        /// <param name="q"></param>
        /// <param name="element"></param>
        /// <param name="to"></param>
        public static void JustMoveTo(double q, FrameworkElement element, Pos3D to)
        {
            Pos3D from = element.ExtractPos();
            Pos3D delta = to - from;
            SimplePanel.SetPos(element, delta*q + from);
        }

        /// <summary>
        /// Movement animation that uses place as movement mechanism, it not only can change scale together with coordinates
        /// </summary>
        /// <param name="element"></param>
        /// <param name="to"></param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public static Animation MoveTo(this FrameworkElement element, Rect to, TimeSpan howLong = default(TimeSpan),
                                       IEasingFunction easing = null)
        {
            if (to.IsEmpty) throw new Exception("Going to empty place");
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                        Rect from = SimplePanel.GetPlace(element);
                        if (from.Equals(Rect.Empty)) from = element.GetBounds();
                        double x = to.X - from.X;
                        double y = to.Y - from.Y;
                        double w = to.Width - from.Width;
                        double h = to.Height - from.Height;
                        ani.DoOnNext +=
                            q =>
                            SimplePanel.SetPlace(element,
                                                 new Rect(from.X + x*q, from.Y + y*q, from.Width + w*q,
                                                          from.Height + h*q));
                        ani.DoOnCompleted += () => SimplePanel.SetPlace(element, to);
                        ani.OnNext(i);
                    });
        }

        /// <summary>
        /// не могу понять нафиг столько переменных регить
        /// </summary>
        /// <param name="element"></param>
        /// <param name="to"></param>
        /// <param name="howLong"></param>
        /// <param name="easing"></param>
        /// <returns></returns>
        public static Animation ResizeTo(this FrameworkElement element, Rect to, TimeSpan howLong = default(TimeSpan),
                                         IEasingFunction easing = null)
        {
            return new Animation(howLong, easing).OnEachStart(
                (i, ani) =>
                    {
                        Rect from = SimplePanel.GetSlot(element);
                        Pos3D fromPos = element.ExtractPos();
                        var toPos = new Pos3D(to.X, to.Y, 0);
                        Pos3D deltaPos = toPos - fromPos;
                        double fromX = from.X;
                        double toX = to.X;
                        double fromY = from.Y;
                        double toY = to.Y;
                        double fromWidth = from.Width;
                        double toWidth = to.Width;
                        double fromHeight = from.Height;
                        double toHeight = to.Height;
                        double deltaX = toX - fromX;
                        double deltaY = toY - fromY;
                        double deltaWidth = toWidth - fromWidth;
                        double deltaHeight = toHeight - fromHeight;
                        ani.DoOnNext += q =>
                                            {
                                                SimplePanel.SetPos(element, deltaPos*q + fromPos);
                                                SimplePanel.SetSlot(element,
                                                                    new Rect(q*deltaX + fromX, q*deltaY + fromY,
                                                                             q*deltaWidth + fromWidth,
                                                                             q*deltaHeight + fromHeight));
                                                element.Width = q*deltaWidth + fromWidth;
                                                element.Height = q*deltaHeight + fromHeight;
                                                element.UpdateLayout();
                                            };
                        ani.DoOnCompleted += () => SimplePanel.SetSlot(element, to);
                        ani.OnNext(i);
                    });
        }
    }
}