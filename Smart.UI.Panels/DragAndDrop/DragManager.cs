using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Smart.UI.Panels;
using Smart.Classes.Subjects;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Panels
{
    public interface IDragManager
    {
        ObjectFly StartDrag(MouseButtonEventArgs e);
        ObjectFly BuildDragObject(FrameworkElement target);
        ObjectFly StartFlight(ObjectFly fly, Func<UIElement, Point> getPos);

        void ComeBack(ObjectFly fly, Rect place);
    }

    public interface IDragManager<T> : IDragManager where T : DragPanel
    {
        T Sender { get; set; }
    }


    /// <summary>
    /// Класс отвечающий за драггинг
    /// </summary>
    public class DragManager : IDragManager
    {
        #region DEPENDENCY PROPERTIES

        #region DRAG SUBJECT

        public static readonly DependencyProperty DragSubjectProperty =
            DependencyProperty.RegisterAttached("DragSubject", typeof (SimpleSubject<ObjectFly>),
                                                typeof (FrameworkElement),
                                                new PropertyMetadata(default(SimpleSubject<ObjectFly>)));

        [TypeConverter(typeof (DragSubjectConverter))]
        public static SimpleSubject<ObjectFly> GetDragSubject(DependencyObject obj)
        {
            return obj.GetOrDefault<SimpleSubject<ObjectFly>>(DragSubjectProperty, () => new FreeDrag());
        }

        [TypeConverter(typeof (DragSubjectConverter))]
        public static void SetDragSubject(DependencyObject obj, SimpleSubject<ObjectFly> value)
        {
            obj.SetValue(DragSubjectProperty, value);
        }

        #endregion

        #region DRAGCANVASES

        public static readonly DependencyProperty ChildrenDragCanvasProperty =
            DependencyProperty.RegisterAttached("ChildrenDragCanvas", typeof (DragCanvasFunc), typeof (DragPanel),
                                                new PropertyMetadata(DragCanvasConverter.Instance.Default));

        public static readonly DependencyProperty DragCanvasProperty =
            DependencyProperty.RegisterAttached("DragCanvas", typeof (DragCanvasFunc), typeof (FrameworkElement),
                                                new PropertyMetadata(DragCanvasConverter.Instance.Default));

        [TypeConverter(typeof (DragCanvasConverter))]
        public static void SetChildrenDragCanvas(DragPanel element, DragCanvasFunc value)
        {
            element.SetValue(ChildrenDragCanvasProperty, value);
        }

        [TypeConverter(typeof (DragCanvasConverter))]
        public static DragCanvasFunc GetChildrenDragCanvas(DragPanel element)
        {
            return (DragCanvasFunc) element.GetValue(ChildrenDragCanvasProperty);
        }


        [TypeConverter(typeof (DragCanvasConverter))]
        public static void SetDragCanvas(FrameworkElement element, DragCanvasFunc value)
        {
            element.SetValue(DragCanvasProperty, value);
        }

        [TypeConverter(typeof (DragCanvasConverter))]
        public static DragCanvasFunc GetDragCanvas(FrameworkElement element)
        {
            return (DragCanvasFunc) element.GetValue(DragCanvasProperty) ??
                   GetChildrenDragCanvas(element.GetNearestDragParent());
        }

        #endregion

        #region DRAG TARGET

        public static readonly DependencyProperty DragTargetProperty =
            DependencyProperty.RegisterAttached("DragTarget", typeof (DragTargetFunc), typeof (FrameworkElement),
                                                new PropertyMetadata(DragTargetConverter.Instance.Default));

        [TypeConverter(typeof (DragTargetConverter))]
        public static void SetDragTarget(UIElement element, DragTargetFunc value)
        {
            element.SetValue(DragTargetProperty, value);
        }

        [TypeConverter(typeof (DragTargetConverter))]
        public static DragTargetFunc GetDragTarget(UIElement element)
        {
            return (DragTargetFunc) element.GetValue(DragTargetProperty);
        }

        #endregion

        public static readonly DependencyProperty DockModeProperty =
            DependencyProperty.RegisterAttached("DockMode", typeof (DockMode), typeof (FrameworkElement),
                                                new PropertyMetadata(DockMode.Default));

        /// <summary>
        /// Sets default dockmode for all children
        /// When child's dockmode is default parent's childrendockmode is taken
        /// </summary>
        public static readonly DependencyProperty ChildrenDockModeProperty =
            DependencyProperty.RegisterAttached("ChildrenDockMode", typeof (DockMode), typeof (DragPanel),
                                                new PropertyMetadata(DockMode.DockEverywhere));

        public static DockMode GetDockMode(DependencyObject obj)
        {
            var dockMode = (DockMode) obj.GetValue(DockModeProperty);
            if (dockMode == DockMode.Default)
                dockMode = GetChildrenDockMode((obj as FrameworkElement).GetNearestDragEnabledParent<DragPanel>());
            return dockMode;
        }

        public static void SetDockMode(DependencyObject obj, DockMode value)
        {
            obj.SetValue(DockModeProperty, value);
        }


        public static DockMode GetChildrenDockMode(DependencyObject obj)
        {
            return (DockMode) obj.GetValue(ChildrenDockModeProperty);
        }

        public static void SetChildrenDockMode(DependencyObject obj, DockMode value)
        {
            obj.SetValue(ChildrenDockModeProperty, value);
        }

        #endregion

        public DragPanel Sender;

        public DragManager(DragPanel sender)
        {
            Sender = sender;
        }

        #region IDragManager Members

        /// <summary>
        /// Builds fly object
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public virtual ObjectFly BuildDragObject(FrameworkElement target)
        {
            SimpleSubject<ObjectFly> subject = GetDragSubject(target);
            DockMode dockMode = GetDockMode(target);
            if (dockMode.Equals(DockMode.Default)) dockMode = GetChildrenDockMode(target.GetNearestDragParent());
            var fly = new ObjectFly(target) {DockMode = dockMode, PosSubject = subject};
            return fly;
        }

        /// <summary>
        /// initiates target dragging
        /// </summary>
        /// <param name="fly"></param>
        /// <param name="getPos"></param>
        /// <returns></returns>
        public virtual ObjectFly StartFlight(ObjectFly fly, Func<UIElement, Point> getPos)
        {
            FrameworkElement element = fly.Target;
            fly.Target = GetDragTarget(element).CleanExecute(element);
            DragCanvasFunc dragCanvas = GetDragCanvas(element);
            fly.Target.CaptureMouse();

            var flyFrom = fly.Target.GetNearestDragEnabledParent<DragPanel>();
            DragPanel flyTo = dragCanvas.CleanExecute(dragCanvas.Param0 ?? fly.Target);

            if (flyTo != null) 
                
                if(flyTo!=flyFrom)
                {
                    flyFrom.ChildToPanel(fly.Target, flyTo);
                } 
                else
                {
                    //var slot = fly.Target.GetSlot();
                    //flyTo.ReceiveChild(fly.Target,slot,slot);
                }
            
            if (flyTo == null) flyTo = Sender;

            fly.CurrentMouse = getPos(flyTo);
            fly.StartMouse = getPos(element);

            Rect bounds = flyTo.ExtractBounds(fly.Target);

            fly.PosSubject.DoOnError = i => flyTo.DragManager.ComeBack(fly, bounds);

            flyTo.Dragged.OnNextUnique(fly);
            return fly;
        }

        /// <summary>
        /// How element comes back
        /// </summary>
        /// <param name="fly"></param>
        /// <param name="place"></param>
        public virtual void ComeBack(ObjectFly fly, Rect place)
        {
            Rect bounds = SimplePanel.GetPlace(fly.Target);
            Sender.Shift(fly.Target, new Point(place.X - bounds.X, place.Y - bounds.Y));
            Sender.RunAterNextLayoutUpdate(i => Sender.DockChild(fly.SetDockMode(DockMode.DockEverywhere)));
        }

        /// <summary>
        /// Initiates dragging
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public virtual ObjectFly StartDrag(MouseButtonEventArgs e)
        {
            e.Handled = true;
            FrameworkElement element = (e.OriginalSource as FrameworkElement).GetNearestDragEnabledChild<DragPanel>();
            return StartFlight(BuildDragObject(element), e.GetPosition);
        }

        #endregion
    }
}