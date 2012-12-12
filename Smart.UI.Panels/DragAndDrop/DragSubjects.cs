using System.Windows;
using Smart.UI.Panels;
using Smart.Classes.Subjects;

namespace Smart.UI.Panels
{
    #region DRAGSUBJECT

    public abstract class DragSubject<T> : NamedSubject<T>
    {
        protected DragSubject()
        {
            At().DoOnNext += OnStartHandler;
        }

        protected virtual void OnStartHandler(T value)
        {
        }

        protected override void OnCompletedHandler()
        {
            At().DoOnNext += OnStartHandler;
        }
    }


    public class FreeDrag : DragSubject<ObjectFly>
    {
        protected override void OnNextHandler(ObjectFly f)
        {
            f.Target.ShiftMe(f.ShiftPos);
        }
    }

    public class VerticalDrag : DragSubject<ObjectFly>
    {
        protected override void OnNextHandler(ObjectFly f)
        {
            f.Target.ShiftMe(new Point(0.0, f.ShiftPos.Y));
        }
    }

    public class HorizontalDrag : DragSubject<ObjectFly>
    {
        protected override void OnNextHandler(ObjectFly f)
        {
            f.Target.ShiftMe(new Point(f.ShiftPos.X, 0.0));
        }
    }

    #endregion
}
