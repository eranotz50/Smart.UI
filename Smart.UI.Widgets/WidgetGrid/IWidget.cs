using System.Windows;

namespace Smart.UI.Widgets
{
    /// <summary>
    /// виджеты, которые могут менять свое отображение содержимого должны наследоваться от этого интерфейса и оборачиваться WidgetWrapper
    /// </summary>
    public interface IWidget
    {
        /// <summary>
        /// отображение мелкого списка
        /// </summary>
        FrameworkElement SmallImage { get; set; }

        /// <summary>
        /// отображение среднего списка
        /// </summary>
        FrameworkElement MediumImage { get; set; }

        /// <summary>
        /// отображение большого списка
        /// </summary>
        FrameworkElement LargeImage { get; set; }

        /// <summary>
        /// корень контрола. для управления им.
        /// </summary>
        FrameworkElement Root { get; set; }
    }
}