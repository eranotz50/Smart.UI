using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Smart.UI.Classes.Animations;

namespace Smart.UI.Panels
{
/*
    /// <summary>
    /// обертка для контрола на виджет-панели
    /// </summary>
    public class WidgetWrapper : UserControl
    {
        private readonly IWidget _widget;

        private FrameworkElement _currentView;
        private PanelSize _currentWidgetSize;
        private Size _mediumWidgetSize;

        /// <summary>
        /// разные размеры панелек по достижении которых виджет должен изменить свое отображение
        /// </summary>
        private Size _smallWidgetSize;

        /// <summary>
        /// Создание обертки виджета. Пока не знаю как указывать разные размеры, при которых виджет должен менять свое отображение. 
        /// То ли при создании стола рассчитывать, то ли динамически как-то. Пока просто будем задавать примерные размеры
        /// </summary>
        /// <param name="widget"></param>
        /// <param name="smallWidgetSize"></param>
        /// <param name="mediumWidgetSize"></param>
        /// <param name="largeWidgetSize"></param>
        public WidgetWrapper(IWidget widget, Size smallWidgetSize, Size mediumWidgetSize)
        {
            _widget = widget;
            Content = _widget.Root;
            _smallWidgetSize = smallWidgetSize;
            _mediumWidgetSize = mediumWidgetSize;
            SizeChanged += WidgetWrapper_SizeChanged;
            //сначала спрячем все контролы
            if (_widget.SmallImage != null)
                _widget.SmallImage.Visibility = Visibility.Collapsed;
            if (_widget.MediumImage != null)
                _widget.MediumImage.Visibility = Visibility.Collapsed;
            if (_widget.LargeImage != null)
                _widget.LargeImage.Visibility = Visibility.Collapsed;

            // выставим показ в соответствии с текущим состоянием
            WidgetWrapper_SizeChanged(null, null); // сразу поставить видимость
        }

        private void WidgetWrapper_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Size widgetSize = RenderSize;
            if (widgetSize.Width == 0 && widgetSize.Height == 0) return;
            // если размеры меньше нижней границы, показываем мелкое отображение
            if (
                (widgetSize.Height <= _smallWidgetSize.Height || widgetSize.Width <= _smallWidgetSize.Width)
                && _widget.SmallImage != null)
            {
                _currentWidgetSize = PanelSize.Small;
                if (_widget.SmallImage.Visibility == Visibility.Collapsed)
                {
                    if (_widget.MediumImage != null)
                        _widget.MediumImage.AnimateProperty(OpacityProperty, 0, new TimeSpan(0, 0, 0, 0, 300)).Go().Then
                            (() => _widget.MediumImage.Visibility = Visibility.Collapsed);
                    if (_widget.LargeImage != null)
                        _widget.LargeImage.AnimateProperty(OpacityProperty, 0, new TimeSpan(0, 0, 0, 0, 300)).Go().Then(
                            () => _widget.LargeImage.Visibility = Visibility.Collapsed);
                    _widget.SmallImage.Opacity = 0.0;
                    _widget.SmallImage.Visibility = Visibility.Visible;
                    _widget.SmallImage.AnimateProperty(OpacityProperty, 1.0, new TimeSpan(0, 0, 0, 0, 300)).Go();
                    _currentView = _widget.SmallImage;
                }
            }
                // если размеры меньше среднего размера, но больше мелкого, показываем среднее отображение
            else if (
                (widgetSize.Height <= _mediumWidgetSize.Height || widgetSize.Width <= _mediumWidgetSize.Width)
                && _widget.MediumImage != null)
            {
                _currentWidgetSize = PanelSize.Medium;
                if (_widget.MediumImage.Visibility == Visibility.Collapsed)
                {
                    if (_widget.SmallImage != null)
                        _widget.SmallImage.AnimateProperty(OpacityProperty, 0, new TimeSpan(0, 0, 0, 0, 300)).Go().Then(
                            () => _widget.SmallImage.Visibility = Visibility.Collapsed);
                    if (_widget.LargeImage != null)
                        _widget.LargeImage.AnimateProperty(OpacityProperty, 0, new TimeSpan(0, 0, 0, 0, 300)).Go().Then(
                            () => _widget.LargeImage.Visibility = Visibility.Collapsed);
                    _widget.MediumImage.Opacity = 0.0;
                    _widget.MediumImage.Visibility = Visibility.Visible;
                    _widget.MediumImage.AnimateProperty(OpacityProperty, 1.0, new TimeSpan(0, 0, 0, 0, 300)).Go();
                    _currentView = _widget.MediumImage;
                }
            }
            else // иначе показываем большое отображение
            {
                _currentWidgetSize = PanelSize.Large;
                if (_widget.LargeImage != null && _widget.LargeImage.Visibility == Visibility.Collapsed)
                {
                    if (_widget.SmallImage != null)
                        _widget.SmallImage.AnimateProperty(OpacityProperty, 0, new TimeSpan(0, 0, 0, 0, 300)).Go().Then(
                            () => _widget.SmallImage.Visibility = Visibility.Collapsed);
                    if (_widget.MediumImage != null)
                        _widget.MediumImage.AnimateProperty(OpacityProperty, 0, new TimeSpan(0, 0, 0, 0, 300)).Go().Then
                            (() => _widget.MediumImage.Visibility = Visibility.Collapsed);
                    _widget.LargeImage.Opacity = 0.0;
                    _widget.LargeImage.Visibility = Visibility.Visible;
                    _widget.LargeImage.AnimateProperty(OpacityProperty, 1.0, new TimeSpan(0, 0, 0, 0, 300)).Go();
                    _currentView = _widget.LargeImage;
                }
            }
            if (_currentView != null)
                SetPanelControlsVisibility(_currentView, _currentWidgetSize);
        }

        private void SetPanelControlsVisibility(UIElement panel, PanelSize size)
        {
            // _SetPanelControlsVisibility(panel, size);
            IEnumerable<DependencyObject> elements = panel.GetVisualDescendants();
            foreach (UIElement el in elements)
            {
                if (el.GetParticipateInDynamicLayout())
                {
                    bool visible = false;
                    switch (size)
                    {
                        case PanelSize.Small:
                            visible = el.GetVisibleInSmallPanel();
                            break;
                        case PanelSize.Medium:
                            visible = el.GetVisibleInMediumPanel();
                            break;
                        case PanelSize.Large:
                            visible = el.GetVisibleInLargePanel();
                            break;
                    }
                    el.Visibility = visible ? Visibility.Visible : Visibility.Collapsed;
                    // че-то какой-то косяк плавно проявлять контролы и прятать
                    //if (visible && el.Visibility == Visibility.Collapsed)
                    //{
                    //    el.Opacity = 0;
                    //    el.Visibility = Visibility.Visible;
                    //    el.AnimateProperty(OpacityProperty, 1.0, new TimeSpan(0, 0, 0, 0, 100)).Go();
                    //}
                    //else if (!visible && el.Visibility == Visibility.Visible)
                    //    el.AnimateProperty(OpacityProperty, 0, new TimeSpan(0, 0, 0, 0, 100)).Go().Then(() => el.Visibility = Visibility.Collapsed);
                }
            }
        }

        #region Nested type: PanelSize

        private enum PanelSize
        {
            Small,
            Medium,
            Large
        }

        #endregion
    }
 * */
}