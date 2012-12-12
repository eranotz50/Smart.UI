using System.Windows;
using Smart.UI.Panels;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Widgets.PanelAdorners
{
    public static class Adorners
    {
        public static readonly DependencyProperty FlexCanvasAdornersProperty =
            DependencyProperty.RegisterAttached("FlexCanvasAdorners", typeof (AdornerCollection<FlexCanvas>),
                                                typeof (FlexCanvas),
                                                new PropertyMetadata(default(AdornerCollection<FlexCanvas>),
                                                                     FlexCanvasAdornersCallback));

        public static readonly DependencyProperty SmartGridAdornersProperty =
            DependencyProperty.RegisterAttached("SmartGridAdorners", typeof (AdornerCollection<SmartGrid>),
                                                typeof (SmartGrid),
                                                new PropertyMetadata(default(AdornerCollection<SmartGrid>),
                                                                     SmartGridAdornersCallback));

        public static readonly DependencyProperty WidgetGridAdornersProperty =
            DependencyProperty.RegisterAttached("WidgetGridAdorners", typeof (AdornerCollection<WidgetGrid>),
                                                typeof (WidgetGrid),
                                                new PropertyMetadata(default(AdornerCollection<WidgetGrid>),
                                                                     WidgetGridAdornersCallback));

      

        public static readonly DependencyProperty WallAdornersProperty =
            DependencyProperty.RegisterAttached("WallAdorners", typeof (AdornerCollection<Wall>), typeof (Wall),
                                                new PropertyMetadata(default(AdornerCollection<Wall>),
                                                                     WallAdornersCallback));

        public static void AdornersCallback<T>(DependencyObject d, DependencyPropertyChangedEventArgs e)
            where T : FlexCanvas
        {
            var p = d as T;
            if (p != null) p.InvalidateMeasure();
            var oldVal = e.OldValue as AdornerCollection<T>;
            var newVal = e.NewValue as AdornerCollection<T>;
            if (newVal == null) return;
            if (newVal.Host == null) newVal.Host = p;
        }

        public static void SetFlexCanvasAdorners(FlexCanvas element, AdornerCollection<FlexCanvas> value)
        {
            element.SetValue(FlexCanvasAdornersProperty, value);
        }

        public static AdornerCollection<FlexCanvas> GetFlexCanvasAdorners(FlexCanvas element)
        {
            return element.GetOrDefault<AdornerCollection<FlexCanvas>>(FlexCanvasAdornersProperty);
        }


        /// <summary>
        /// Callback than invalidates measure of the element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void FlexCanvasAdornersCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AdornersCallback<FlexCanvas>(d, e);
        }


        public static void SetSmartGridAdorners(SmartGrid element, AdornerCollection<SmartGrid> value)
        {
            element.SetValue(SmartGridAdornersProperty, value);
        }

        public static AdornerCollection<SmartGrid> GetSmartGridAdorners(SmartGrid element)
        {
            return element.GetOrDefault<AdornerCollection<SmartGrid>>(SmartGridAdornersProperty);
        }

        /// <summary>
        /// Callback than invalidates measure of the element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void SmartGridAdornersCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AdornersCallback<SmartGrid>(d, e);
        }

        public static void SetWidgetGridAdorners(WidgetGrid element, AdornerCollection<WidgetGrid> value)
        {
            element.SetValue(WidgetGridAdornersProperty, value);
        }

        public static AdornerCollection<WidgetGrid> GetWidgetGridAdorners(WidgetGrid element)
        {
            return element.GetOrDefault<AdornerCollection<WidgetGrid>>(WidgetGridAdornersProperty);
        }

        /// <summary>
        /// Callback than invalidates measure of the element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void WidgetGridAdornersCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AdornersCallback<WidgetGrid>(d, e);
        }       


        public static void SetWallAdorners(Wall element, AdornerCollection<Wall> value)
        {
            element.SetValue(WallAdornersProperty, value);
        }

        public static AdornerCollection<Wall> GetWallAdorners(Wall element)
        {
            return element.GetOrDefault<AdornerCollection<Wall>>(WallAdornersProperty);
        }


        public static void WallAdornersCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AdornersCallback<Wall>(d, e);
        }
    }
}