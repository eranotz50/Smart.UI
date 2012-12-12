using System;
using System.Windows;

namespace Smart.UI.Widgets
{
    /// <summary>
    /// класс для хранения динамических свойств контролов виджета в разных состояниях
    /// </summary>
    public class WidgetDynamicDependancyProperty
    {
        public static readonly DependencyProperty ParticipateInDynamicLayoutProperty =
            DependencyProperty.RegisterAttached("ParticipateInDynamicLayout", typeof (bool), typeof (DependencyObject),
                                                new PropertyMetadata(false));

        public static readonly DependencyProperty VisibleInSmallPanelProperty =
            DependencyProperty.RegisterAttached("VisibleInSmallPanel", typeof (bool), typeof (DependencyObject),
                                                new PropertyMetadata(true));

        public static readonly DependencyProperty VisibleInMediumPanelProperty =
            DependencyProperty.RegisterAttached("VisibleInMediumPanel", typeof (bool), typeof (DependencyObject),
                                                new PropertyMetadata(true));

        public static readonly DependencyProperty VisibleInLargePanelProperty =
            DependencyProperty.RegisterAttached("VisibleInLargePanel", typeof (bool), typeof (DependencyObject),
                                                new PropertyMetadata(true));

        public static void SetParticipateInDynamicLayout(DependencyObject element, bool value)
        {
            element.SetValue(ParticipateInDynamicLayoutProperty, value);
        }

        public static bool GetParticipateInDynamicLayout(DependencyObject element)
        {
            return (bool) element.GetValue(ParticipateInDynamicLayoutProperty);
        }


        public static Boolean GetVisibleInSmallPanel(DependencyObject element)
        {
            return (bool) element.GetValue(VisibleInSmallPanelProperty);
        }

        public static void SetVisibleInSmallPanel(DependencyObject element, Boolean value)
        {
            element.SetValue(VisibleInSmallPanelProperty, value);
            SetParticipateInDynamicLayout(element, true);
        }


        public static Boolean GetVisibleInMediumPanel(DependencyObject element)
        {
            return (bool) element.GetValue(VisibleInMediumPanelProperty);
        }

        public static void SetVisibleInMediumPanel(DependencyObject element, Boolean value)
        {
            element.SetValue(VisibleInMediumPanelProperty, value);
            SetParticipateInDynamicLayout(element, true);
        }


        public static Boolean GetVisibleInLargePanel(DependencyObject element)
        {
            return (bool) element.GetValue(VisibleInLargePanelProperty);
        }

        public static void SetVisibleInLargePanel(DependencyObject element, Boolean value)
        {
            element.SetValue(VisibleInLargePanelProperty, value);
            SetParticipateInDynamicLayout(element, true);
        }
    }


    /// <summary>
    /// екстеншин для выставления динамических свойств видимости разных контролов на виджете в разных размерных состояниях
    /// </summary>
    public static class WidgetDynamicDependancyPropertyExtention
    {
        public static T SetVisibleInSmallPanel<T>(this T source, bool value) where T : DependencyObject
        {
            WidgetDynamicDependancyProperty.SetVisibleInSmallPanel(source, value);
            return source;
        }

        public static T SetVisibleInMediumPanel<T>(this T source, bool value) where T : DependencyObject
        {
            WidgetDynamicDependancyProperty.SetVisibleInMediumPanel(source, value);
            return source;
        }

        public static T SetVisibleInLargePanel<T>(this T source, bool value) where T : DependencyObject
        {
            WidgetDynamicDependancyProperty.SetVisibleInLargePanel(source, value);
            return source;
        }

        public static bool GetVisibleInSmallPanel<T>(this T source) where T : DependencyObject
        {
            return WidgetDynamicDependancyProperty.GetVisibleInSmallPanel(source);
        }

        public static bool GetVisibleInMediumPanel<T>(this T source) where T : DependencyObject
        {
            return WidgetDynamicDependancyProperty.GetVisibleInMediumPanel(source);
        }

        public static bool GetVisibleInLargePanel<T>(this T source) where T : DependencyObject
        {
            return WidgetDynamicDependancyProperty.GetVisibleInLargePanel(source);
        }

        public static bool GetParticipateInDynamicLayout<T>(this T source) where T : DependencyObject
        {
            return WidgetDynamicDependancyProperty.GetParticipateInDynamicLayout(source);
        }
    }
}