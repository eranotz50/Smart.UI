using Smart.UI.Panels;
using Smart.Classes.Subjects;

#if SILVERLIGHT

namespace Smart.UI.Panels
{
    public class ArrowSubject : NamedSubject<ObjectFly>
    {
        public Arrow Arrow;

        public ArrowSubject()
        {
            Arrow = new Arrow();
            At().DoOnNext += PlaceArrow;
        }

        protected void PlaceArrow(ObjectFly value)
        {
            if (Arrow == null) Arrow = new Arrow();
            if (Arrow.Parent != value.Target.Parent)
                value.Target.GetNearestDragParent().AddChild(Arrow);
        }

        protected void PosArrow(ObjectFly value)
        {
            Arrow.StartPoint = value.StartMouse;
            Arrow.EndPoint = value.CurrentMouse;
        }


        protected override void OnNextHandler(ObjectFly value)
        {
            PosArrow(value);
        }

        protected override void OnCompletedHandler()
        {
            if (Arrow.Parent is SimplePanel) (Arrow.Parent as SimplePanel).RemoveChild(Arrow);
            At().DoOnNext += PlaceArrow;
        }
    }
}
#endif