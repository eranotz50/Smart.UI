using System;
using System.Linq;
using System.Windows;
using Smart.UI.Panels;
using Smart.UI.Widgets.PanelAdorners;

namespace Smart.UI.Widgets.PanelAdorners
{
    /// <summary>
    /// Для быстрого добавления самых распостраненных адорнеров
    /// </summary>
    public static class AdornerHelper
    {
        #region HELPFUL DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ResizableProperty =
            DependencyProperty.RegisterAttached("Resizable", typeof (Boolean), typeof (FlexCanvas),
                                                new PropertyMetadata(default(Boolean)));

        public static readonly DependencyProperty LinesVisibleProperty =
            DependencyProperty.RegisterAttached("LinesVisible", typeof (Boolean), typeof (SmartGrid),
                                                new PropertyMetadata(default(Boolean)));

        public static Boolean GetResizable(FlexCanvas panel)
        {
            return (bool) panel.GetValue(ResizableProperty);
        }

        public static void SetResizable(FlexCanvas panel, Boolean value)
        {
            if (GetResizable(panel) == value) return;
            panel.SetValue(ResizableProperty, value);
            SwitchAdorner<Resizer, FlexCanvas>(value, panel, Adorners.GetFlexCanvasAdorners);
        }

        public static Boolean GetLinesVisible(SmartGrid panel)
        {
            return (bool) panel.GetValue(LinesVisibleProperty);
        }

        public static void SetLinesVisible(SmartGrid panel, Boolean value)
        {
            if (GetLinesVisible(panel) == value) return;
            panel.SetValue(LinesVisibleProperty, value);
            SwitchAdorner<GridLines, SmartGrid>(value, panel, Adorners.GetSmartGridAdorners);
        }

        #endregion

        #region MAIN DEPROPRS

        /*
        public static AdornerCollection<FlexCanvas> GetAdorners<T>(this T panel) where T : FlexCanvas
        {
            return Adorners.GetFlexCanvasAdorners(panel);
        }
        */


        public static T SetAdorners<T>(this T panel, AdornerCollection<FlexCanvas> adornerCollection)
            where T : FlexCanvas
        {
            Adorners.SetFlexCanvasAdorners(panel, adornerCollection);
            return panel;
        }

        public static T SwitchAdorner<T, TPanel>(Boolean value, TPanel panel,
                                                 Func<TPanel, AdornerCollection<TPanel>> getter)
            where T : CanvasAdorner<TPanel>, new()
            where TPanel : FlexCanvas, new()
        {
            AdornerCollection<TPanel> adorners = getter(panel);
            T ad = adorners.OfType<T>().FirstOrDefault();
            if (value)
                if (ad == null)
                    adorners.Add(new T());
                else ad.Activate();
            else
            {
                if (ad != null) ad.Deactivate();
            }
            return ad;
        }

        #endregion

        #region EXTENSIONS

        public static AdornerCollection<FlexCanvas> GetFlexCanvasAdorners(this FlexCanvas source)
        {
            return Adorners.GetFlexCanvasAdorners(source);
        }

        public static AdornerCollection<UI.Widgets.WidgetGrid> GetWidgetGridAdorners(this UI.Widgets.WidgetGrid source)
        {
            return Adorners.GetWidgetGridAdorners(source);
        }

        public static AdornerCollection<SmartGrid> GetSmartGridAdorners(this SmartGrid source)
        {
            return Adorners.GetSmartGridAdorners(source);
        }

        #endregion
    }
}