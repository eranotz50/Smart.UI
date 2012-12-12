using System;
using System.Windows;
using System.Windows.Media;
using Smart.Classes.Subjects;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Panels
{
    public static class PanelExtensions
    {
        public static void RunOnFirstArrange<T>(this T element, Action<Args<FrameworkElement, Rect, Size>> func)
            where T : FrameworkElement
        {
            SimpleSubject<Args<FrameworkElement, Rect, Size>> sub = element.GetOnArrange() ??
                                                                    element.UpdateValue(
                                                                        BasicSmartPanel.OnArrangeProperty,
                                                                        new SimpleSubject
                                                                            <Args<FrameworkElement, Rect, Size>>());
            sub.At().DoOnNext += func;
        }

        public static T ShiftMe<T>(this T child, Point shift) where T : FrameworkElement
        {
            child.GetParent<SimplePanel>().Shift(child, shift);
            return child;
        }

        #region EASY UPDATES

        /*
            public static T UpdateArrange<T>(this T element) where T:FrameworkElement
            {
                if(element==null) return null;
                var dirtyness = BasicSmartPanel.GetDirty(element);
                if (dirtyness != Dirtiness.Clean) return element;
                element.InvalidateArrange();
                var p = element.GetParent<SimplePanel>();
                if (p == null) return element;
                BasicSmartPanel.SetDirty(element,Dirtiness.Arrange);
                p.ForArrangeUpdate.Add(element);
                        // element.LayoutUpdated -= p.MeasureUpdatedHandler; element.LayoutUpdated += p.MeasureUpdatedHandler;
                //element.LayoutUpdated -= p.LayoutUpdatedHandler; element.LayoutUpdated += p.LayoutUpdatedHandler;
                return element;
            }

            public static T UpdateMeasure<T>(this T element) where T : FrameworkElement
            {
                if (element == null) return null;
                var dirtyness = BasicSmartPanel.GetDirty(element);
                if (dirtyness == Dirtiness.Measure) return element;
                element.InvalidateMeasure();
                var p = element.GetParent<SimplePanel>();
                if (p == null) return element;
                if (dirtyness == Dirtiness.Arrange) p.ForArrangeUpdate.Remove(element);
                BasicSmartPanel.SetDirty(element, Dirtiness.Measure);
                p.ForMeasureUpdate.Add(element);
                        //element.LayoutUpdated -= p.ArrangeUpdatedHandler; element.LayoutUpdated += p.ArrangeUpdatedHandler;
                //element.LayoutUpdated -= p.LayoutUpdatedHandler; element.LayoutUpdated += p.LayoutUpdatedHandler;                               
                return element;
            }
        */

        #endregion

        #region EASY CANVAS SETTINGS

        public static T SetCanvasSize<T>(this T panel, Size size) where T : BasicSmartPanel
        {
            panel.Space.Canvas.Width = size.Width;
            panel.Space.Canvas.Height = size.Height;
            panel.InvalidateMeasure();
            return panel;
        }

        public static T SetPanelShift<T>(this T panel, Point shift) where T : BasicSmartPanel
        {
            panel.Space.Panel.X = shift.X;
            panel.Space.Panel.Y = shift.Y;
            panel.InvalidateMeasure();
            return panel;
        }

        public static T SetRelativeShift<T>(this T panel, Point shift) where T : BasicSmartPanel
        {
            panel.Space.RelativeShift = shift;
            panel.InvalidateMeasure();
            return panel;
        }

        #endregion

        #region EASY ONARRANGE SETTING

        public static SimpleSubject<Args<FrameworkElement, Rect, Size>> GetOnArrange<T>(this T element)
            where T : FrameworkElement
        {
            return BasicSmartPanel.GetOnArrange(element);
        }

        public static SimpleSubject<Args<FrameworkElement, Rect, Size>> GetOrNewOnArrange<T>(this T element)
            where T : FrameworkElement
        {
            return
                element.GetOrDefault<SimpleSubject<Args<FrameworkElement, Rect, Size>>>(
                    BasicSmartPanel.OnArrangeProperty);
        }

        public static T SetOnArrange<T>(this T element, SimpleSubject<Args<FrameworkElement, Rect, Size>> value)
            where T : FrameworkElement
        {
            BasicSmartPanel.SetOnArrange(element, value);
            return element;
        }

        public static T SetOnArrange<T>(this T element, Action<Args<FrameworkElement, Rect, Size>> value)
            where T : FrameworkElement
        {
            BasicSmartPanel.SetOnArrange(element, value);
            return element;
        }


        /// <summary>
        /// Updates subjects attached to onarrange subject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T UpdateOnArrange<T>(this T element, SimpleSubject<Args<FrameworkElement, Rect, Size>> value)
            where T : FrameworkElement
        {
            SimpleSubject<Args<FrameworkElement, Rect, Size>> subject = BasicSmartPanel.GetOnArrange(element) ??
                                                                        element.UpdateValue(
                                                                            BasicSmartPanel.OnArrangeProperty,
                                                                            new SimpleSubject
                                                                                <Args<FrameworkElement, Rect, Size>>());
            subject.Update(value); //value = subject.Merge(value);
            //  element.UpdateArrange();
            var p = element.GetParent<SimplePanel>();
            if (p != null) p.InvalidateArrange();
            return element;
        }


        /// <summary>
        /// Updates on of the handlers attached to onarrange subject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T UpdateOnArrange<T>(this T element, Action<Args<FrameworkElement, Rect, Size>> value)
            where T : FrameworkElement
        {
            SimpleSubject<Args<FrameworkElement, Rect, Size>> subject = BasicSmartPanel.GetOnArrange(element) ??
                                                                        element.UpdateValue(
                                                                            BasicSmartPanel.OnArrangeProperty,
                                                                            new SimpleSubject
                                                                                <Args<FrameworkElement, Rect, Size>>());
            subject.Update(value);
            //element.UpdateArrange();
            var p = element.GetParent<SimplePanel>();
            if (p != null) p.InvalidateMeasure();
            return element;
        }

        public static T DeleteFromOnArrange<T>(this T element, Action<Args<FrameworkElement, Rect, Size>> value)
            where T : FrameworkElement
        {
            SimpleSubject<Args<FrameworkElement, Rect, Size>> subject = BasicSmartPanel.GetOnArrange(element);
            if (subject == null) return element;
            subject.DoOnNext -= value;
            //element.UpdateArrange();
            var p = element.GetParent<SimplePanel>();
            if (p != null) p.InvalidateMeasure();
            return element;
        }

        #endregion

        #region HIERARCHY RELATED

        public static T GetHighestParent<T>(this FrameworkElement element) where T : BasicSmartPanel
        {
            T host = element is T ? (element as T) : null;
            FrameworkElement f = element;
            while (f.Parent as FrameworkElement != null || VisualTreeHelper.GetParent(f) as FrameworkElement != null)
            {
                f = (f.Parent ?? VisualTreeHelper.GetParent(f)) as FrameworkElement;
                if (f is T) host = f as T;
            }
            return host;
        }

        public static FrameworkElement GetNearestPanelChild<T>(this FrameworkElement element)
            where T : BasicSmartPanel
        {
            FrameworkElement f = element;
            while (f.Parent as FrameworkElement != null || VisualTreeHelper.GetParent(f) as FrameworkElement != null)
            {
                var parent = (f.Parent ?? VisualTreeHelper.GetParent(f)) as FrameworkElement;
                if (parent is T) return f;
                f = parent;
            }
            return null; //f;
        }

        #endregion
    }
}