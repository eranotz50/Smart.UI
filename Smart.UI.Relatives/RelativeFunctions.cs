using System;
using System.Diagnostics.Contracts;
using System.Windows;
using System.Windows.Media;
using Smart.UI.Panels;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Relatives
{
    public enum GrowthMode
    {
        None,
        Left,
        Right,
        Top,
        Bottom,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    public static class RelativeFunctions
    {
        

        #region ONESIDEPOSES

        public static void PosSameCoords(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            element.PosLeftTop(args.Param1.X, args.Param1.Y);
        }

        public static void PosSameLeft(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            element.PosLeft(args.Param1.Left);
        }

        public static void PosSameTop(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            element.PosTop(args.Param1.Top);
        }

        /// <summary>
        /// One of the default relative positioning functions
        /// </summary>
        /// <param name="args"></param>
        /// <param name="element"></param>
        public static void PosLeft(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            Rect slot = element.GetSlot();
            if (slot.IsEmpty) element.PosLeft(args.Param1.Left); else element.PosLeft(args.Param1.Left - slot.Width);
        }

        public static void PosRight(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            element.PosLeft(args.Param1.Right);
        }

        public static void PosTop(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            Rect slot = element.GetSlot();
            if (slot.IsEmpty) element.PosTop(args.Param1.Top);else element.PosTop(args.Param1.Top - slot.Height);
            //PosTop(element, args.Param1.Top);
        }

        public static void PosBottom(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            element.PosTop(args.Param1.Bottom);
        }
        #endregion

        #region CORNER POSES

        public static void PosLeftTop(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            Rect slot = element.GetSlot();
            double left = (slot.IsEmpty) ? args.Param1.Left : args.Param1.Left - slot.Width;
            double top = (slot.IsEmpty) ? args.Param1.Top : args.Param1.Top - slot.Height;
            element.PosLeftTop(left, top);
        }

        public static void PosRightTop(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            Rect slot = element.GetSlot();
            double right = args.Param1.Right;
            double top = (slot.IsEmpty) ? args.Param1.Top : args.Param1.Top - slot.Height;
            element.PosLeftTop(right, top);
        }

        public static void PosLeftBottom(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            Rect slot = element.GetSlot();
            double left = (slot.IsEmpty) ? args.Param1.Left : args.Param1.Left - slot.Width;
            double bottom = args.Param1.Bottom;
            element.PosLeftTop(left, bottom);
        }

        public static void PosRightBottom(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            double right = args.Param1.Right;
            double bottom = args.Param1.Bottom;
            element.PosLeftTop(right, bottom);
        }

        #endregion

        #region MIDDLE/CENTER POSES

        public static void PosLeftMiddle(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
            {
                Contract.Requires(args != null && args.Param1 != Rect.Empty);
                Contract.Ensures(element.GetSlot().Right.Equals(args.Param1.X));
                double left = args.Param1.Left;
                var slot = element.GetSlot();
                if (!slot.IsEmpty) left -= slot.Width;
                element.PosLeftMiddle(args.Param1.Y, args.Param1.Height, left);
            }

      
            public static void PosRightMiddle(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
            {
                Contract.Requires(args!=null && args.Param1!=Rect.Empty);
                double right = args.Param1.Right;
                element.PosLeftMiddle(args.Param1.Y, args.Param1.Height, right);
            }

            public static void PosCenterBottom(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
            {
                Contract.Requires(args != null && args.Param1 != Rect.Empty);            
                double bottom = args.Param1.Bottom;
                element.PosCenterTop(args.Param1.X, args.Param1.Width, bottom);
            }

            public static void PosCenterTop(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
            {
                Contract.Requires(args != null && args.Param1 != Rect.Empty);
                Contract.Ensures(element.GetSlot().Bottom.Equals(args.Param1.Y));
                double top = args.Param1.Y;
                var slot = element.GetSlot();
                if (!slot.IsEmpty) top -= slot.Height;
                
                element.PosCenterTop(args.Param1.X, args.Param1.Width, top);
            }
        #endregion

        #region SIDES POSITIONING

        /// <summary>
        /// One of the default relative positioning functions
        /// </summary>
        /// <param name="args"></param>
        /// <param name="element"></param>
        public static void PosLeftSide(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            element.PosLeft(args.Param1.Left);
        }

        public static void PosRightSide(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            Rect slot = element.GetSlot();
            if (slot.IsEmpty) element.PosLeft(args.Param1.Right); else element.PosLeft(args.Param1.Right - slot.Width);
        }

        public static void PosTopSide(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            element.PosTop(args.Param1.Top);
        }

        public static void PosBottomSide(Args<FrameworkElement, Rect, Size> args, FrameworkElement element)
        {
            Rect slot = element.GetSlot();
            if (slot.IsEmpty) element.PosTop(args.Param1.Bottom); else element.PosTop(args.Param1.Bottom - slot.Height);
        }


        #endregion

       
        public static Action<ObjectFly> ActionByGrowth(GrowthMode mode)
        {
            switch (mode)
            {
                case GrowthMode.Left:
                    return GrowLeft;
                case GrowthMode.Right:
                    return GrowRight;
                case GrowthMode.Top:
                    return GrowTop;
                case GrowthMode.Bottom:
                    return GrowBottom;
                case GrowthMode.TopLeft:
                    return GrowTopLeft;
                case GrowthMode.TopRight:
                    return GrowTopRight;
                case GrowthMode.BottomLeft:
                    return GrowBottomLeft;
                case GrowthMode.BottomRight:
                    return GrowBottomRight;
            }
            return null;
        }

        #region DEFAULT DRAG RELATIVE FUNCTIONS

        public static void GrowLeft(ObjectFly f)
        {
            FrameworkElement el = f.Target.GetRelativeElement();
            if (el != null) el.GrowHorizontal(-f.ShiftPos.X, AlignmentX.Left);
        }

        public static void GrowRight(ObjectFly f)
        {
            FrameworkElement el = f.Target.GetRelativeElement();
            if (el != null) el.GrowHorizontal(f.ShiftPos.X, AlignmentX.Right);
        }

        public static void GrowTop(ObjectFly f)
        {
            FrameworkElement el = f.Target.GetRelativeElement();
            if (el != null) el.GrowVertical(-f.ShiftPos.Y, AlignmentY.Top);
        }

        public static void GrowBottom(ObjectFly f)
        {
            FrameworkElement el = f.Target.GetRelativeElement();
            if (el != null) el.GrowVertical(f.ShiftPos.Y, AlignmentY.Bottom);
        }

        public static void GrowTopLeft(ObjectFly f)
        {
            FrameworkElement el = f.Target.GetRelativeElement();
            if (el != null) el.Grow(f.ShiftPos.Invert(), AlignmentX.Left, AlignmentY.Top);
        }

        public static void GrowTopRight(ObjectFly f)
        {
            FrameworkElement el = f.Target.GetRelativeElement();
            if (el != null) el.Grow(f.ShiftPos.InvertY(), AlignmentX.Right, AlignmentY.Top);
        }

        public static void GrowBottomLeft(ObjectFly f)
        {
            FrameworkElement el = f.Target.GetRelativeElement();
            if (el != null) el.Grow(f.ShiftPos.InvertX(), AlignmentX.Left, AlignmentY.Bottom);
        }

        public static void GrowBottomRight(ObjectFly f)
        {
            FrameworkElement el = f.Target.GetRelativeElement();
            if (el != null) el.Grow(f.ShiftPos, AlignmentX.Right, AlignmentY.Bottom);
        }

        #endregion

        /*
        static public PanelUpdateMode GrowthByAction(Action<ObjectFly> action)
        {
            return null;
        }
        */

    }
}