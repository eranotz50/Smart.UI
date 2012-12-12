using System.Windows;
using Smart.Classes.Subjects;

namespace Smart.UI.Panels
{
    /// <summary>
    /// Обертка управляющая драгом летающих объектов
    /// </summary>
    public class ObjectFly //: IObjectFly
    {
        public DockMode DockMode;
        public Point ObjectShift;
        public Size Space;

        public ObjectFly(FrameworkElement target, Point start = default(Point))
        {
            StartMouse = start;
            Target = target;
            PosSubject = new SimpleSubject<ObjectFly>();
        }

        public SimpleSubject<ObjectFly> PosSubject { get; set; }
        public FrameworkElement Target { get; set; }


        public Point StartMouse { get; set; }
        public Point LastMouse { get; set; }
        public Point CurrentMouse { get; set; }

        public Point DragPos
        {
            get { return new Point(CurrentMouse.X - StartMouse.X, CurrentMouse.Y - StartMouse.Y); }
        }


        /// <summary>
        /// Element's displacement since latest movement
        /// </summary>
        public Point ShiftPos
        {
            get
            {
                return new Point(CurrentMouse.X - LastMouse.X /*+ ObjectShift.X*/, CurrentMouse.Y - LastMouse.Y
                    /* + ObjectShift.Y*/);
            }
        }

        public virtual ObjectFly Clone
        {
            get { return new ObjectFly(Target, StartMouse) {DockMode = DockMode}; }
        }

        public ObjectFly WithCurrent(Point current)
        {
            LastMouse = CurrentMouse;
            CurrentMouse = current;
            return this;
        }

        /// <summary>
        /// Сюда можно вешать любую логику через подписку
        /// </summary>
        public virtual void Pos()
        {
            PosSubject.OnNext(this);
        }
    }
}