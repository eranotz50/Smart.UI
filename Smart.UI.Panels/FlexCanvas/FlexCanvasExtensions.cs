using System.Windows;
using System.Windows.Controls;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
    public static class FlexCanvasExtensions
    {
        /*
        public static T ClearCanvas<T>(this T source) where T : FrameworkElement
        {
            return
                source.SetLeft(double.PositiveInfinity).SetRight(double.PositiveInfinity).SetTop(double.PositiveInfinity)
                    .SetBottom(double.PositiveInfinity);
        }
        */

        public static T SetLeft<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetLeft(source, new RelativeValue(value));
            return source;
        }

        public static T SetCenter<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetCenter(source, new RelativeValue(value));
            return source;
        }

        public static T SetRight<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetRight(source, new RelativeValue(value));
            return source;
        }


        public static T SetTop<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetTop(source, new RelativeValue(value));
            return source;
        }

        public static T SetMiddle<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetMiddle(source, new RelativeValue(value));
            return source;
        }

        public static T SetBottom<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetBottom(source, new RelativeValue(value));
            return source;
        }

        public static T SetLeft<T>(this T source, RelativeValue value) where T : FrameworkElement
        {
            FlexCanvas.SetLeft(source, value);
            return source;
        }

        public static T SetCenter<T>(this T source, RelativeValue value) where T : FrameworkElement
        {
            FlexCanvas.SetCenter(source, value);
            return source;
        }

        public static T SetRight<T>(this T source, RelativeValue value) where T : FrameworkElement
        {
            FlexCanvas.SetRight(source, value);
            return source;
        }

        public static T SetTop<T>(this T source, RelativeValue value) where T : FrameworkElement
        {
            FlexCanvas.SetTop(source, value);
            return source;
        }

        public static T SetMiddle<T>(this T source, RelativeValue value) where T : FrameworkElement
        {
            FlexCanvas.SetMiddle(source, value);
            return source;
        }

        public static T SetBottom<T>(this T source, RelativeValue value) where T : FrameworkElement
        {
            FlexCanvas.SetBottom(source, value);
            return source;
        }

        public static T SetLeftTop<T>(this T source, Point value) where T : FrameworkElement
        {
            FlexCanvas.SetLeft(source, new RelativeValue(value.X));
            FlexCanvas.SetTop(source, new RelativeValue(value.Y));
            return source;
        }

        #region EASYj CANVAS GETTING

        public static double GetLeft<T>(this T source) where T : FrameworkElement
        {
            return FlexCanvas.GetLeft(source).AbsoluteValue;
        }

        public static double GetCenter<T>(this T source) where T : FrameworkElement
        {
            return FlexCanvas.GetCenter(source).AbsoluteValue;
        }

        public static double GetRight<T>(this T source) where T : FrameworkElement
        {
            return FlexCanvas.GetRight(source).AbsoluteValue;
        }

        public static double GetTop<T>(this T source) where T : FrameworkElement
        {
            return FlexCanvas.GetTop(source).AbsoluteValue;
        }

        public static double GetMiddle<T>(this T source) where T : FrameworkElement
        {
            return FlexCanvas.GetMiddle(source).AbsoluteValue;
        }

        public static double GetBottom<T>(this T source) where T : FrameworkElement
        {
            return FlexCanvas.GetBottom(source).AbsoluteValue;
        }

        #endregion

        #region EASY FLEXCANVAS PARAMS ADDING

        public static T AddLeft<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetLeft(source, FlexCanvas.GetLeft(source).AddAbsolute(value));
            return source;
        }

        public static T AddCenter<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetCenter(source, FlexCanvas.GetCenter(source).AddAbsolute(value));
            return source;
        }

        public static T AddRight<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetRight(source, FlexCanvas.GetRight(source).AddAbsolute(value));
            return source;
        }


        public static T AddTop<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetTop(source, FlexCanvas.GetTop(source).AddAbsolute(value));
            return source;
        }

        public static T AddMiddle<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetMiddle(source, FlexCanvas.GetMiddle(source).AddAbsolute(value));
            return source;
        }

        public static T AddBottom<T>(this T source, double value = 0.0) where T : FrameworkElement
        {
            FlexCanvas.SetBottom(source, FlexCanvas.GetBottom(source).AddAbsolute(value));
            return source;
        }

        #endregion

        #region ORIENTABLE

        #region COORDS SETTINGS

        public static T SetCoord<T>(this T source, Point coord) where T : FrameworkElement, IOrientable
        {
            if (source.Orientation == Orientation.Horizontal) source.SetLeft(coord.X);
            else source.SetTop(coord.Y);
            return source;
        }

        public static T SetCoord<T>(this T source, Rect coord) where T : FrameworkElement, IOrientable
        {
            if (source.Orientation == Orientation.Horizontal) source.SetLeft(coord.X);
            else source.SetTop(coord.Y);
            return source;
        }

        public static T SetCoord<T>(this T source, double coord) where T : FrameworkElement, IOrientable
        {
            if (source.Orientation == Orientation.Horizontal) source.SetLeft(coord);
            else source.SetTop(coord);
            return source;
        }

        public static T SetOtherCoord<T>(this T source, Point coord) where T : FrameworkElement, IOrientable
        {
            if (source.Orientation == Orientation.Vertical) source.SetLeft(coord.X);
            else source.SetTop(coord.Y);
            return source;
        }

        public static T SetOtherCoord<T>(this T source, Rect coord) where T : FrameworkElement, IOrientable
        {
            if (source.Orientation == Orientation.Vertical) source.SetLeft(coord.X);
            else source.SetTop(coord.Y);
            return source;
        }

        public static T SetOtherCoord<T>(this T source, double coord) where T : FrameworkElement, IOrientable
        {
            if (source.Orientation == Orientation.Vertical) source.SetLeft(coord);
            else source.SetTop(coord);
            return source;
        }

        public static T SetCoord<T>(this T source, FrameworkElement element, double coord) where T : IOrientable
        {
            if (source.Orientation == Orientation.Horizontal) element.SetLeft(coord);
            else element.SetTop(coord);
            return source;
        }

        public static T SetOtherCoord<T>(this T source, FrameworkElement element, double coord) where T : IOrientable
        {
            if (source.Orientation == Orientation.Vertical) element.SetLeft(coord);
            else element.SetTop(coord);
            return source;
        }

        #endregion

        #endregion
    }
}