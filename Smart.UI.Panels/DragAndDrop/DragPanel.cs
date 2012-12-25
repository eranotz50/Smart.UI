using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Markup;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.Classes.Extensions;
using Smart.Classes.Subjects;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
    /// <summary>
    /// Drag panel class, dels with dragging and docking
    /// inherits from SimplePanel, so can track children changes and pos stuff using Pos3D
    /// </summary>
    [ContentProperty("Children")]
    public class DragPanel : SimplePanel, IDragPanel
    {
        #region DEPENDENCY PROPS Депенденси проперти, а также гетеры и сетеры к ним

        /// <summary>
        /// Проперти для быстрого позиционирования
        /// </summary>        
        public static readonly DependencyProperty DragEnabledProperty = DependencyProperty.Register("DragEnabled",
                                                                                           typeof (Boolean),
                                                                                           typeof (DragPanel),
                                                                                           new PropertyMetadata(true,
                                                                                                                InvalidateArrangeCallback));


        public Boolean DragEnabled
        {
            get { return (Boolean) GetValue(DragEnabledProperty); }
            set { SetValue(DragEnabledProperty, value); }
        }

        public static readonly DependencyProperty WaitBeforeDragProperty =
    DependencyProperty.Register("WaitBeforeDrag", typeof(TimeSpan), typeof(DragPanel),
                                new PropertyMetadata( /*default(TimeSpan)*/new TimeSpan(0, 0, 0, 0, 100)));

        public TimeSpan WaitBeforeDrag
        {
            get { return (TimeSpan) GetValue(WaitBeforeDragProperty); }
            set { SetValue(WaitBeforeDragProperty, value); }
        }

        #endregion

        /// <summary>
        /// Defines dockind mode: is it free or it takes into consideration other elements or is prohibited
        /// </summary>
        public DockMode DockingMode = DockMode.DockEverywhere;


        public DragPanel()
        {
            DragPanels = new SmartCollection<DragPanel>();
            Children.LeadByType(DragPanels);
            InitDragAndDrop();
        }

        /// <summary>
        /// Дочерние драг панельки, выделены в отдельную коллекцию для более удобной реализации драг энд дропа
        /// </summary>
        public SmartCollection<DragPanel> DragPanels { get; set; }


        protected virtual void InitDragAndDrop()
        {
            InitDragManager();
            InitDragging();
            BindDragging();
            InitDocking();
        }

        protected virtual void InitDragManager()
        {
            DragManager = new DragManager(this);
        }

        public virtual Rect ExtractBounds(FrameworkElement target)
        {
            return GetPlace(target);
        }

        #region DRAGGING            

        /// <summary>
        /// class that deals with making fly objects
        /// </summary>
        public IDragManager DragManager;

        public IObservable<ObjectFly> OnTheFly;

        /// <summary>
        /// If there is an element dragged under this panel (the element might not be a child of this panel) underflight.onnext(flyobject) occurs
        /// </summary>
        public SimpleSubject<ObjectFly> UnderFlight;

        /// <summary>
        /// Subject for all elements that are currenty dragged
        /// </summary>
        public MemoryPipe<ObjectFly> Dragged { get; set; }


        /// <summary>
        /// Инициализируем все необходимое для драг энд дропа,
        /// возможно в наследниках прийдется овверайдить
        /// </summary>
        protected virtual void BindDragging()
        {
            /*ОСТОРОЖНО - БЫДЛОКОД! Потом его перепишу*/
            var noMove = new Animation(WaitBeforeDrag.TotalMilliseconds);
            Action detach = () =>
                                {
                                    noMove.Detach();
                                    noMove.DoOnCompleted = null;
                                };

            MouseMoveSubject.Add(i => detach());
            MouseUpSubject.Add(i => detach());
            MouseDownSubject
                .Where(
                    d =>
                    DragEnabled && WaitBeforeDrag != default(TimeSpan) &&
                   !IsIntantlyDragged(d.EventArgs.OriginalSource as FrameworkElement))
                //.Where(d=>d.Sender==this)
                .Chain(d =>
                           {
                               //   var parent = this.Parent;
                               //   var name = this.Name;                                    
                               if (IsReadyForDrag(d.EventArgs.OriginalSource)) noMove.Go();
                               noMove.DoOnCompleted += () =>
                                                       DragManager.StartDrag(d.EventArgs);
                           });


            MouseDownSubject
                .Where(d => DragEnabled)
                .Where(
                    d =>
                    WaitBeforeDrag == default(TimeSpan) ||
                    IsIntantlyDragged(d.EventArgs.OriginalSource as FrameworkElement))
                .Where(d => IsReadyForDrag(d.EventArgs.OriginalSource))
                .Chain(d => DragManager.StartDrag(d.EventArgs));
        }

        /// <summary>
        /// Tells if an element should be dragged with delay or instantly
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        protected Boolean IsIntantlyDragged(FrameworkElement child)
        {
            Contract.Requires(child!=null);
            return !GetElementType(child).Equals(ElementType.Normal);
        }

        /// <summary>
        /// FromString all dragging subjects
        /// </summary>
        protected /*virtual*/ void InitDragging()
        {
            Dragged = new MemoryPipe<ObjectFly>();
            OnTheFly = from d in Dragged
                       from m in MouseMoveSubject.TakeUntil(MouseUpSubject)
                       where m.Sender == this
                       select d.WithCurrent(m.EventArgs.GetPosition(this));
            OnTheFly.Chain(OnFly);
            UnderFlight = new SimpleSubject<ObjectFly>();
        }

      

        /// <summary>
        /// Проверка подходит ли элемент для драга
        /// </summary>
        /// <param name="element">Элемент, что драгится</param>
        /// <returns></returns>
        public virtual Boolean IsReadyForDrag(Object element)
        {
            if ((element is FrameworkElement) == false) return false;
            return ((element as FrameworkElement).GetNearestDragEnabledParent<DragPanel>() == this);
        }

        /// <summary>
        /// Function that runs when dragged element is moved
        /// </summary>
        /// <param name="objectFly">Dragged element covered into flyobject</param>
        public void OnFly(ObjectFly objectFly)
        {
            objectFly.Pos(); //calls position changes
            if (objectFly.DockMode == DockMode.NoDock) return;
            //if the object is dockable finds under what child panel it flies
            DragPanel dock = FindDock(objectFly.Target);
            DragPanel last = Dockers.LastInput;
            if (last != dock)
            {
                if (last != null) last.DockingLeave.OnNext(objectFly);
                dock.DockingEnter.OnNext(objectFly);
            }
            Dockers.OnNext(dock);
            dock.UnderFlight.OnNext(objectFly);
            //objectFly.Pos();
        }

        #endregion

        #region DOCKING

        public MemoryPipe<DragPanel> Dockers;

        public SimpleSubject<Tuple<DragPanel, FrameworkElement>> Dock { get; set; }
        public SimpleSubject<ObjectFly> DockingEnter { get; set; }
        public SimpleSubject<ObjectFly> DockingLeave { get; set; }


        /// <summary>
        /// Docks child to the panel
        /// </summary>
        /// <param name="fly"></param>
        public virtual void DockChild(ObjectFly fly)
        {
          //  ElementType type = fly.Target.GetElementType();

            if (fly.DockMode == DockMode.NoDock)
            {

                //returns element if it is not dockable
                if(!SimplyDocked(fly.Target))
                    fly.PosSubject.OnError(new Exception("Element is not Dockable!"));
                return;
            }

            FrameworkElement child = fly.Target;
            child.ReleaseMouseCapture();
            DragPanel target = FindDock(child);
            target.DockingLeave.OnNext(fly);
            if (!SimplyDocked(fly.Target))
                if (target.AllowDock(fly))
                    target.Dock.OnNext(new Tuple<DragPanel, FrameworkElement>(this, child));
                else
                    fly.PosSubject.OnError(new Exception("No free space!"));
                        //returns the element to its original position
        }

        /// <summary>
        /// Simply docked is usually used for adorners
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        protected virtual Boolean SimplyDocked(FrameworkElement child)
        {
            return !GetElementType(child).Equals(ElementType.Normal);
        }


        public DragPanel FindIntersection(FrameworkElement el)
        {
            return FindIntersection(el.GetRelativeRect(this), el);
        }

        /// <summary>
        /// Находим панельку, над которой находится большая часть нашего драгдропабельного контролза
        /// </summary>
        /// <param name="place"></param>
        /// <param name="el">Элемент, нужен для того, чтобы не было докинга на самого себя</param>
        /// <returns></returns>
        public DragPanel FindIntersection(Rect place, FrameworkElement el = null)
        {
            double square = 0.0;
            DragPanel biggest = this;
            foreach (DragPanel item in DragPanels)
            {
                if (item == el || item.DockingMode == DockMode.NoDock) continue;
                double currentSquare = place.GetIntersectionSquare( /*item.GetBounds()*/item.GetSlot());
                if (currentSquare <= square) continue;
                square = currentSquare;
                biggest = item;
            }
            return biggest;
        }

        /// <summary>
        /// Докинг с элементом вместо ректангла
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public DragPanel FindDock(FrameworkElement element)
        {
            DragPanel panel = FindIntersection(element);
            return panel == this ? this : panel.FindDock(element);
        }

        /// <summary>
        /// Находим элемент над которым находится драгабельное существо
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        public DragPanel FindDock(Rect place)
        {
            DragPanel panel = FindIntersection(place);
            return panel == this ? this : panel.FindDock(place.ToOtherCoords(this, panel));
        }

        protected virtual void InitDocking()
        {
            Dockers = new MemoryPipe<DragPanel>();
            Dock = new SimpleSubject<Tuple<DragPanel, FrameworkElement>>(i => OnChildDock(i.Item1, i.Item2));
            DockingEnter = new SimpleSubject<ObjectFly>();
            DockingLeave = new SimpleSubject<ObjectFly>();
            Dockers.MemorySelector = i => i != Dockers.LastInput;
            MouseUpSubject.Where(i => i.Sender == this && DragEnabled).Chain(i =>
                                                                                 {
                                                                                     Dragged.GetOutOfMemory().Chain(
                                                                                         DockChild);
                                                                                     Dockers.GetOutOfMemory();
                                                                                 });
            this.AllowDock = this.AllowDockHandler;
        }

        public Func<ObjectFly, Boolean> AllowDock;
       
        /// <summary>
        /// Allows docking of selected control
        /// </summary>
        /// <param name="fly"></param>
        /// <returns></returns>
        protected virtual bool AllowDockHandler(ObjectFly fly)
        {
            return fly.DockMode != DockMode.DockOnFreeSpace || CheckFreeSpace(fly.Target);
        }
       
        public virtual Boolean CheckFreeSpace<T>(T target) where T : FrameworkElement
        {
            int q = target.Parent == this ? 1 : 0;
            return ChildrenInPlace<T>(target.GetRelativeRect(this)).Count <= q;
        }


        /// <summary>
        /// Chechs wether there are some children on defined place
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rect"></param>
        /// <param name="selector"> </param>
        /// <returns></returns>
        public SmartCollection<T> ChildrenInPlace<T>(Rect rect, Func<T,Boolean> selector = null ) where T : FrameworkElement
        {
            if (selector == null) selector = DefaultChildrenSelector;
            return
                Children.OfType<T>().Where(selector)
                    .Where(child => child.GetSlot()
                                        .GetIntersection(rect) != Intersection.None).
                    ToCollection();
        }

        protected virtual bool  DefaultChildrenSelector<T>(T child) where T:FrameworkElement
        {
            return GetElementType(child) == ElementType.Normal;
        }

       

        /// <summary>
        /// fires on child being docked to this panel from another
        /// </summary>
        /// <param name="from">panel from which the child was docked</param>
        /// <param name="child"></param>
        protected virtual void OnChildDock(DragPanel from, FrameworkElement child)
        {
            if (from == this) return;
            from.ChildToPanel(child, this);
        }

        #endregion
    }
}