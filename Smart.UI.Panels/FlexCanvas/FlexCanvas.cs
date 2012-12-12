using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Smart.UI.Panels;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
    /// <summary>
    /// Canvas class
    /// It is much more than silverlight/wpf canvas because it can also position elements relatively to its right/bottom/middle/center borders
    /// </summary>
    public class FlexCanvas : DragPanel
    {
        #region DEPENDENCY PROPERTIES

        /// <summary>
        /// Ставлю по дефолту нули, так как есть особенность. Дефолтное значение берется из одного места. Если ты получил дефолтный топ, а затем поменял внутри него что-то, то во всех элементах с дефолтным топом тоже поменяется, что не есть хорошо
        /// </summary>
        public static readonly DependencyProperty TopProperty = DependencyProperty.RegisterAttached("Top", typeof (RelativeValue),
                                                                                           typeof (FlexCanvas),
                                                                                           new PropertyMetadata(null,
                                                                                                                /*UpdateMeasureCallback*/
                                                                                                                InvalidateParentMeasureCallback));

        public static readonly DependencyProperty CenterProperty = DependencyProperty.RegisterAttached("Center",
                                                                                              typeof (RelativeValue),
                                                                                              typeof (FlexCanvas),
                                                                                              new PropertyMetadata(
                                                                                                  null,
                                                                                                  /*UpdateMeasureCallback*/
                                                                                                  InvalidateParentMeasureCallback));

        public static readonly DependencyProperty RightProperty = DependencyProperty.RegisterAttached("Right",
                                                                                             typeof (RelativeValue),
                                                                                             typeof (FlexCanvas),
                                                                                             new PropertyMetadata(null,
                                                                                                                  /*UpdateMeasureCallback*/
                                                                                                                  InvalidateParentMeasureCallback));

        public static readonly DependencyProperty LeftProperty = DependencyProperty.RegisterAttached("Left",
                                                                                            typeof (RelativeValue),
                                                                                            typeof (FlexCanvas),
                                                                                            new PropertyMetadata(null,
                                                                                                                 /*UpdateMeasureCallback*/
                                                                                                                 InvalidateParentMeasureCallback));

        public static readonly DependencyProperty MiddleProperty = DependencyProperty.RegisterAttached("Middle",
                                                                                              typeof (RelativeValue),
                                                                                              typeof (FlexCanvas),
                                                                                              new PropertyMetadata(
                                                                                                  null,
                                                                                                  /*UpdateMeasureCallback*/
                                                                                                  InvalidateParentMeasureCallback));

        public static readonly DependencyProperty BottomProperty = DependencyProperty.RegisterAttached("Bottom",
                                                                                              typeof (RelativeValue),
                                                                                              typeof (FlexCanvas),
                                                                                              new PropertyMetadata(
                                                                                                  null,
                                                                                                  /*UpdateMeasureCallback*/
                                                                                                  InvalidateParentMeasureCallback));

        public CanvasExtractor Extractor { get; set; }


        /// <summary>
        /// Использую собственный конвертор типов для получения записей вроде звездочек или абсолютных значений
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [TypeConverter(typeof (RelativeValueConverter))]
        public static RelativeValue GetLeft(DependencyObject obj)
        {
            return obj.GetOrDefault<RelativeValue>(LeftProperty);
        }

        public static void SetLeft(DependencyObject obj, RelativeValue value)
        {
            obj.SetValue(LeftProperty, value);
        }

        [TypeConverter(typeof (RelativeValueConverter))]
        public static RelativeValue GetRight(DependencyObject obj)
        {
            return obj.GetOrDefault<RelativeValue>(RightProperty);
        }

        public static void SetRight(DependencyObject obj, RelativeValue value)
        {
            obj.SetValue(RightProperty, value);
        }

        [TypeConverter(typeof (RelativeValueConverter))]
        public static RelativeValue GetCenter(DependencyObject obj)
        {
            return obj.GetOrDefault<RelativeValue>(CenterProperty);
        }

        public static void SetCenter(DependencyObject obj, RelativeValue value)
        {
            obj.SetValue(CenterProperty, value);
        }

        [TypeConverter(typeof (RelativeValueConverter))]
        public static RelativeValue GetTop(DependencyObject obj)
        {
            return obj.GetOrDefault<RelativeValue>(TopProperty);
        }

        public static void SetTop(DependencyObject obj, RelativeValue value)
        {
            obj.SetValue(TopProperty, value);
        }

        [TypeConverter(typeof (RelativeValueConverter))]
        public static RelativeValue GetMiddle(DependencyObject obj)
        {
            return obj.GetOrDefault<RelativeValue>(MiddleProperty);
        }

        public static void SetMiddle(DependencyObject obj, RelativeValue value)
        {
            obj.SetValue(MiddleProperty, value);
        }

        [TypeConverter(typeof (RelativeValueConverter))]
        public static RelativeValue GetBottom(DependencyObject obj)
        {
            return obj.GetOrDefault<RelativeValue>(BottomProperty);
        }

        public static void SetBottom(DependencyObject obj, RelativeValue value)
        {
            obj.SetValue(BottomProperty, value);
        }

        #endregion

        public FlexCanvas()
        {
            Extractor = new CanvasExtractor(InvalidateMeasure);
        }

        /// <summary>
        /// Cleans children position
        /// </summary>
        /// <param name="child"></param>
        public override void ClearPosition(FrameworkElement child)
        {
            base.ClearPosition(child);            
            SetLeft(child, default(RelativeLength));
            SetRight(child, default(RelativeLength));
            SetCenter(child, default(RelativeLength));
            SetTop(child, default(RelativeLength));
            SetMiddle(child, default(RelativeLength));
            SetBottom(child, default(RelativeLength));            
        }

        #region DRAG & DROP RELATED

        /// <summary>
        /// Сдвигает канвасный элемент по икс/игрикуканвасно, используется в драг энд дропе
        /// </summary>
        /// <param name="child"></param>
        /// <param name="shift">сдвиг</param>
        /// <returns></returns>
        public override Boolean Shift(FrameworkElement child, Point shift)
        {
            if (base.Shift(child, shift)) return true;
            Extractor.Shift(child, shift);
            return true;
        }


        /// <summary>
        /// Fires when dragged element (usualy a child of another panel) is released over this panel
        /// </summary>
        /// <param name="from"></param>
        /// <param name="child"></param>
        protected override void OnChildDock(DragPanel from, FrameworkElement child)
        {
            if (from == this) return;
            from.ChildToPanel(child, this);
            ParkChild(child);
        }


        #endregion

        #region LAYOUT RELATED

        /// <summary>
        /// Заполнение плейсхолдера, класса высставляющего размеры
        /// </summary>
        /// <param name="child">позиционируемый элемент</param>
        /// <param name="constrains">в какую рамку нужно обязательно вписаться по размеру</param>
        /// <returns>прямоугольник, занимаемый элементом</returns>
        public virtual T Populate<T>(FrameworkElement child, Size constrains) where T : Placeholder, new()
        {
            return Extractor.ExtractFrom(child).Populate(new T(), constrains);
        }


        /// <summary>
        /// Populate with canvas info
        /// </summary>
        /// <param name="child"></param>
        /// <param name="constrains"></param>
        protected override Rect WriteSlot(FrameworkElement child, Size constrains)
        {
            Rect slot = base.WriteSlot(child, constrains);
            if (!slot.IsEmpty) return slot;
            var placer = Populate<CanvasPlaceholder>(child, constrains);
            Size size = placer.GetSize(constrains);
            child.Measure(size);
            return placer.GetBoundary(child.SizeForRender(size), constrains);
        }

        #endregion

        public override T GrowChild<T>(T child, Point delta, AlignmentX hor = AlignmentX.Center,
                                       AlignmentY ver = AlignmentY.Center)
        {
            Rect place = GetPlace(child);
            if (!place.IsEmpty)
                return child.SetPlace(place.GrowRect(delta, hor, ver));
            Extractor.Grow(child, delta, hor, ver);
            return child;
        }

        /// <summary>
        /// NAPILNIK работает лишь отчасти
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <param name="rect"></param>
        /// <returns></returns>
        public override T PlaceChild<T>(T child, Rect rect)
        {
            return (GetPlace(child).IsEmpty)
                       ? child.SetLeft(rect.X).SetTop(rect.Y).SetWidth(rect.Width).SetHeight(rect.Height)
                       : child.SetPlace(rect);
        }

        #region SWAPer

        /// <summary>
        /// Меняем элемент местами
        /// </summary>
        /// <param name="child"></param>
        public virtual void SwapElement(FrameworkElement child)
        {
            RelativeValue v1 = GetLeft(child);
            RelativeValue v2 = GetTop(child);
            SetLeft(child, v2);
            SetTop(child, v1);
            v1 = GetRight(child);
            v2 = GetBottom(child);
            SetRight(child, v2);
            SetBottom(child, v1);
        }

        public virtual void Swap(Boolean withThis = false)
        {
            foreach (FrameworkElement child in Children) SwapElement(child);
            if (withThis) if (Parent is FlexCanvas) (Parent as FlexCanvas).SwapElement(this);
            InvalidateMeasure();
        }

        #endregion
    }
}