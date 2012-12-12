using System;
using System.Windows;
using System.Windows.Media;
using Smart.UI.Panels;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Widgets.PanelAdorners
{
    /// <summary>
    /// Subjest for different resizers
    /// </summary>
    public abstract class GrowSubject : DragSubject<ObjectFly>
    {
        public Func<FrameworkElement> TargetGetter;

        protected GrowSubject()
        {
        }

        protected GrowSubject(Func<FrameworkElement> getter)
        {
            TargetGetter = getter;
        }

        protected override void OnNextHandler(ObjectFly f)
        {
            Grow(f, TargetGetter == null ? f.Target : TargetGetter() ?? f.Target);
        }

        public abstract void Grow(ObjectFly f, FrameworkElement target);
    }

    public class GrowRightSubject : GrowSubject
    {
        public override void Grow(ObjectFly f, FrameworkElement target)
        {
            target.GrowHorizontal(f.ShiftPos.X, AlignmentX.Right);
        }
    }

    public class GrowLeftSubject : GrowSubject
    {
        public override void Grow(ObjectFly f, FrameworkElement target)
        {
            target.GrowHorizontal(-f.ShiftPos.X, AlignmentX.Left);
        }
    }

    public class GrowTopSubject : GrowSubject
    {
        public override void Grow(ObjectFly f, FrameworkElement target)
        {
            target.GrowVertical(-f.ShiftPos.Y, AlignmentY.Top);
        }
    }

    public class GrowBottomSubject : GrowSubject
    {
        public override void Grow(ObjectFly f, FrameworkElement target)
        {
            target.GrowVertical(f.ShiftPos.Y, AlignmentY.Bottom);
        }
    }


    public class GrowTopRightSubject : GrowSubject
    {
        public override void Grow(ObjectFly f, FrameworkElement target)
        {
            target.Grow(f.ShiftPos.InvertY(), AlignmentX.Right, AlignmentY.Top);
        }
    }

    public class GrowTopLeftSubject : GrowSubject
    {
        public override void Grow(ObjectFly f, FrameworkElement target)
        {
            Point shift = f.ShiftPos.Invert();
            target.Grow(shift, AlignmentX.Left, AlignmentY.Top);
            f.LastMouse = f.LastMouse.Add(shift);
        }
    }

    public class GrowBottomRightSubject : GrowSubject
    {
        public override void Grow(ObjectFly f, FrameworkElement target)
        {
            target.Grow(f.ShiftPos, AlignmentX.Right, AlignmentY.Bottom);
        }
    }

    public class GrowBottomLeftSubject : GrowSubject
    {
        public override void Grow(ObjectFly f, FrameworkElement target)
        {
            target.Grow(f.ShiftPos.InvertX(), AlignmentX.Left, AlignmentY.Bottom);
        }
    }
}