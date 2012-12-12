using System;
using System.Windows;
using System.Windows.Media;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
    /// <summary>
    /// Класс, который вытягивает канвасные параметры из чаилда
    /// заполняет плейсхолдер
    /// </summary>
    public class CanvasExtractor : Placeholder<RelativeValue>
    {
        public double Height = double.PositiveInfinity;
        public Action OnUpdate;
        public double Width = double.PositiveInfinity;

        public CanvasExtractor()
        {
        }

        public CanvasExtractor(Action onUpdate)
        {
            OnUpdate = onUpdate;
        }

        public CanvasExtractor(FrameworkElement child, Action onUpdate)
        {
            ExtractFrom(child);
            OnUpdate = onUpdate;
        }

        public CanvasExtractor Update()
        {
            if (OnUpdate != null) OnUpdate();
            return this;
        }

        /// <summary>
        /// Заполняем плейсхолдер канваса
        /// </summary>
        /// <param name="placer">Плейсхолдер канваса</param>
        /// <param name="constrains">Ограничения по размеру</param>
        public T Populate<T>(T placer, Size constrains) where T : Placeholder
        {
            placer.CleanUp();
            placer.Left = Left.StaredLength(constrains.Width);
            placer.Center = Center.StaredLength(constrains.Width);
            placer.Right = Right.StaredLength(constrains.Width);
            placer.Top = Top.StaredLength(constrains.Height);
            placer.Middle = Middle.StaredLength(constrains.Height);
            placer.Bottom = Bottom.StaredLength(constrains.Height);
            return placer;
        }

        #region EXTRACT FUNCTIONS

        /// <summary>
        /// Вытягивает параметры канвасные из чаилда
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public CanvasExtractor ExtractHorizontalFrom(FrameworkElement child)
        {
            Left = FlexCanvas.GetLeft(child);
            Center = FlexCanvas.GetCenter(child);
            Right = FlexCanvas.GetRight(child);
            Width = child.Width;
            return this;
        }

        /// <summary>
        /// Экстракция вертикальная
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public CanvasExtractor ExtractVerticalFrom(FrameworkElement child)
        {
            Top = FlexCanvas.GetTop(child);
            Middle = FlexCanvas.GetMiddle(child);
            Bottom = FlexCanvas.GetBottom(child);
            Height = child.Height;
            return this;
        }

        public CanvasExtractor CleanUpHorizontal()
        {
            Left = Right = Center = null;
            Width = double.PositiveInfinity;
            return this;
        }


        public CanvasExtractor CleanUpVertical()
        {
            Top = Bottom = Middle = null;
            Height = double.PositiveInfinity;
            return this;
        }


        public CanvasExtractor CleanUp()
        {
            return CleanUpHorizontal().CleanUpVertical();
        }

        #endregion

        #region EXPERIMENTAL

        /// <summary>
        /// Вытягивает все параметры канвасные из чаилда
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        public CanvasExtractor ExtractFrom(FrameworkElement child)
        {
            Left = FlexCanvas.GetLeft(child);
            Center = FlexCanvas.GetCenter(child);
            Right = FlexCanvas.GetRight(child);
            Top = FlexCanvas.GetTop(child);
            Middle = FlexCanvas.GetMiddle(child);
            Bottom = FlexCanvas.GetBottom(child);
            Width = child.Width;
            Height = child.Height;
            return this;
        }

        /// <summary>
        /// Extract from slot
        /// </summary>
        /// <param name="slot">elements slot</param>
        /// <param name="constrains">size of the panel</param>
        /// <returns></returns>
        public CanvasExtractor ExtractFrom(Rect slot, Size constrains)
        {
            Left = new RelativeValue(slot.Left);
            Right = new RelativeValue(constrains.Width - slot.Left - slot.Width);
            Top = new RelativeValue(slot.Top);
            Bottom = new RelativeValue(constrains.Height - slot.Top - slot.Height);
            return this;
        }

        #endregion

        #region GROW FUNCTIONS

        /// <summary>
        /// Растем  вправо
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        public CanvasExtractor GrowRight(double dx)
        {
            if (!dx.IsValidNotNull()) return this;
            if (Width.IsMoreThanNull()) Width += dx;
            if (Right.Value.IsValid()) Right.AbsoluteValue -= dx;
            if (Center.Value.IsValid()) Center.AbsoluteValue += dx/2;
            return this;
        }

        /// <summary>
        /// Растем в разные стороны
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        public CanvasExtractor GrowCenter(double dx)
        {
            if (!dx.IsValidNotNull()) return this;
            if (Width.IsMoreThanNull()) Width += dx;
            if (Left.Value.IsValid()) Left.AbsoluteValue -= dx/2;
            if (Right.Value.IsValid()) Right.AbsoluteValue -= dx/2;
            return this;
        }

        /// <summary>
        /// Растем влево
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        /// 
        public CanvasExtractor GrowLeft(double dx)
        {
            if (!dx.IsValidNotNull()) return this;
            if (Width.IsMoreThanNull()) Width += dx;
            if (Left.Value.IsValid()) Left.AbsoluteValue -= dx;
            if (Center.Value.IsValid()) Center.AbsoluteValue -= dx/2;
            return this;
        }

        /// <summary>
        /// Растем вниз
        /// </summary>
        /// <param name="dy"></param>
        /// <returns></returns>
        public CanvasExtractor GrowBottom(double dy)
        {
            if (!dy.IsValidNotNull()) return this;
            if (Height.IsMoreThanNull()) Height += dy;
            if (Bottom.Value.IsValid()) Bottom.AbsoluteValue -= dy;
            if (Middle.Value.IsValid()) Middle.AbsoluteValue += dy/2;
            return this;
        }

        /// <summary>
        /// Растем равномерно по вертикали
        /// </summary>
        /// <param name="dy"></param>
        /// <returns></returns>
        public CanvasExtractor GrowMiddle(double dy)
        {
            if (!dy.IsValidNotNull()) return this;
            if (Height.IsMoreThanNull()) Height += dy;
            if (Top.Value.IsValid()) Top.AbsoluteValue -= dy/2;
            if (Bottom.Value.IsValid()) Bottom.AbsoluteValue -= dy/2;
            return this;
        }

        /// <summary>
        /// Растем вверх
        /// </summary>
        /// <param name="dy"></param>
        /// <returns></returns>
        public CanvasExtractor GrowTop(double dy)
        {
            if (!dy.IsValidNotNull()) return this;
            if (Height.IsMoreThanNull()) Height += dy;
            if (Top.Value.IsValid()) Top.AbsoluteValue -= dy;
            if (Middle.Value.IsValid()) Middle.AbsoluteValue -= dy/2;
            return this;
        }

        #endregion

        #region GROW REGION

        public CanvasExtractor GrowLeft(FrameworkElement child, double delta)
        {
            return CleanUpHorizontal().ExtractHorizontalFrom(child).GrowLeft(delta).SetHorizontalCoords(child).Update();
        }

        public CanvasExtractor GrowCenter(FrameworkElement child, double delta)
        {
            return
                CleanUpHorizontal().ExtractHorizontalFrom(child).GrowCenter(delta).SetHorizontalCoords(child).Update();
        }

        public CanvasExtractor GrowRight(FrameworkElement child, double delta)
        {
            return CleanUpHorizontal().ExtractHorizontalFrom(child).GrowRight(delta).SetHorizontalCoords(child).Update();
        }

        public CanvasExtractor GrowTop(FrameworkElement child, double delta)
        {
            return CleanUpVertical().ExtractVerticalFrom(child).GrowTop(delta).SetVerticalCoords(child).Update();
        }

        public CanvasExtractor GrowMiddle(FrameworkElement child, double delta)
        {
            return CleanUpVertical().ExtractVerticalFrom(child).GrowMiddle(delta).SetVerticalCoords(child).Update();
        }

        public CanvasExtractor GrowBottom(FrameworkElement child, double delta)
        {
            return CleanUpVertical().ExtractVerticalFrom(child).GrowBottom(delta).SetVerticalCoords(child).Update();
        }

        public CanvasExtractor Grow(FrameworkElement child, Point shift)
        {
            return CleanUp().ExtractFrom(child).GrowCenter(shift.X).GrowMiddle(shift.Y).SetCoords(child).Update();
        }

        public CanvasExtractor Grow(FrameworkElement child, Point shift, AlignmentX hor, AlignmentY ver)
        {
            return GrowHorizontal(child, shift.X, hor).GrowVertical(child, shift.Y, ver);
        }

        public CanvasExtractor GrowHorizontal(FrameworkElement child, double delta, AlignmentX hor)
        {
            CleanUpHorizontal().ExtractHorizontalFrom(child);
            switch (hor)
            {
                case AlignmentX.Left:
                    GrowLeft(delta);
                    break;
                case AlignmentX.Right:
                    GrowRight(delta);
                    break;
                case AlignmentX.Center:
                    GrowCenter(delta);
                    break;
            }
            return SetHorizontalCoords(child).Update();
        }

        public CanvasExtractor GrowVertical(FrameworkElement child, double delta, AlignmentY ver)
        {
            CleanUpVertical().ExtractVerticalFrom(child);
            switch (ver)
            {
                case AlignmentY.Top:
                    GrowTop(delta);
                    break;
                case AlignmentY.Bottom:
                    GrowBottom(delta);
                    break;
                case AlignmentY.Center:
                    GrowMiddle(delta);
                    break;
            }
            return SetVerticalCoords(child).Update();
        }

        #endregion

        #region SHIFT FUNCTIONS

        /// <summary>
        /// Сдвигает контрол на канвасе по горизонтали
        /// </summary>
        /// <param name="dx"></param>
        /// <returns></returns>
        public CanvasExtractor ShiftHorizontal(double dx)
        {
            if (!dx.IsValidNotNull()) return this;
            if (Left.Value.IsValid()) Left.AbsoluteValue += dx;
            if (Right.Value.IsValid()) Right.AbsoluteValue -= dx;
            if (Center.Value.IsValid()) Center.AbsoluteValue += dx;
            return this;
        }

        /// <summary>
        /// Сдвигает контрол на канвасе по вертикали
        /// </summary>
        /// <param name="dy"></param>
        /// <returns></returns>
        public CanvasExtractor ShiftVertical(double dy)
        {
            if (!dy.IsValidNotNull()) return this;
            if (Top.Value.IsValid()) Top.AbsoluteValue += dy;
            if (Bottom.Value.IsValid()) Bottom.AbsoluteValue -= dy;
            if (Middle.Value.IsValid()) Middle.AbsoluteValue += dy;
            return this;
        }

        public CanvasExtractor Shift(FrameworkElement child, Point shift)
        {
            return
                CleanUp().ExtractFrom(child).ShiftHorizontal(shift.X).ShiftVertical(shift.Y).SetCoords(child).Update();
        }

        #endregion

        #region TRANSFORM

        public CanvasExtractor SetCoords(Rect slot, Size constrains, FrameworkElement child)
        {
            return ExtractFrom(slot, constrains).SetCoords(child);
        }

        #endregion

        #region Установка координат в элемент

        public CanvasExtractor SetHorizontalCoords(FrameworkElement child)
        {
            child.SetLeft(Left).SetRight(Right).SetCenter(Center).SetWidth(Width);
            return this;
        }

        public CanvasExtractor SetVerticalCoords(FrameworkElement child)
        {
            child.SetTop(Top).SetBottom(Bottom).SetMiddle(Middle).SetHeight(Height);
            return this;
        }


        public CanvasExtractor SetCoords(FrameworkElement child)
        {
            child.SetLeft(Left).SetRight(Right).SetCenter(Center).SetWidth(Width);
            child.SetTop(Top).SetBottom(Bottom).SetMiddle(Middle).SetHeight(Height);
            return this;
        }

        #endregion
    }
}