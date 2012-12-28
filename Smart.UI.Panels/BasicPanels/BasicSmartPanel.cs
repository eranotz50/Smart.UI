using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using Smart.Classes.Collections;
using Smart.Classes.Interfaces;
using Smart.Classes.Subjects;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Panels
{

    #region ENUMS

    public enum Dirtiness
    {
        Clean,
        Measure,
        Arrange
    }

    #endregion

    /// <summary>
    /// Basic panel of Smart.UI.Panels library.
    /// It has its own children propertie that is based on SmartCollection and has some advantages over default Children
    /// Like monitoring addition and removel of childrens, different subscriptions
    /// </summary>
    [ContentProperty("Children")]
    public abstract class BasicSmartPanel : Panel, IBasicSmartPanel, INamed, IChildrenHolder<FrameworkElement>,IParentHolder<DependencyObject>

    {
        #region CALLBACKS

        #region INVALIDATE CALLBACKS

        /// <summary>
        /// Callback than invalidates arrange of the element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void InvalidateArrangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe != null) fe.InvalidateArrange();
        }

        /// <summary>
        /// Callback than invalidates measure of the element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void InvalidateMeasureCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe != null) fe.InvalidateMeasure();
        }

        public static void InvalidateParentArrangeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null) return;
            var p = fe.Parent as BasicSmartPanel;
            if (p == null) return;
            if (p.Dirty == Dirtiness.Measure) return;
            p.Dirty = Dirtiness.Arrange;
            p.InvalidateArrange();
        }

        public static void InvalidateParentMeasureCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null) return;
            var p = fe.Parent as BasicSmartPanel;
            if (p == null) return;
            p.Dirty = Dirtiness.Measure;
            p.InvalidateMeasure();
        }

        #endregion

        #endregion

        #region DEPENDENCY PROPS Депенденси проперти, а также гетеры и сетеры к ним

        /// <summary>
        /// Children property, an alternative for the native Children property of the raiser.
        /// It has some advantages like an ability to fire events on child removal and addition
        /// </summary>
        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register("Children",
                                                                                                 typeof (
                                                                                                     SmartCollection
                                                                                                     <FrameworkElement>),
                                                                                                 typeof (
                                                                                                     BasicSmartPanel),
                                                                                                 new PropertyMetadata(
                                                                                                     null,
                                                                                                     InvalidateMeasureCallback));

        public new SmartCollection<FrameworkElement> Children
        {
            get { return (SmartCollection<FrameworkElement>) GetValue(ChildrenProperty); }
            set
            {                
                SmartCollection<FrameworkElement> old = Children;
                if (old != null)
                {
                    value.ItemsAdded = old.ItemsAdded;
                    value.ItemsRemoved = old.ItemsRemoved;
                }
                SetValue(ChildrenProperty, value); 
            }
        }

        #region ATTACHABLE DEPENDENCY PROPERTIES

        /// <summary>
        /// Fires when element is being arranged
        /// </summary>
        public static readonly DependencyProperty OnArrangeProperty =
            DependencyProperty.RegisterAttached("OnArrange", typeof (SimpleSubject<Args<FrameworkElement, Rect, Size>>),
                                                typeof (FrameworkElement),
                                                new PropertyMetadata(null /*,InvalidateParentArrangeCallback*/));

        public static readonly DependencyProperty DirtyProperty =
            DependencyProperty.Register("Dirty", typeof (Dirtiness), typeof (BasicSmartPanel),
                                        new PropertyMetadata(Dirtiness.Measure));

        public Dirtiness Dirty
        {
            get { return (Dirtiness) GetValue(DirtyProperty); }
            set { SetValue(DirtyProperty, value); }
        }

        #region CANVAS WIDTH AND HEIGHT FOR TESTING PURPOSES

        public static readonly DependencyProperty CanvasWidthProperty =
            DependencyProperty.Register("CanvasWidth", typeof (double), typeof (BasicSmartPanel),
                                        new PropertyMetadata(CanvasWidthCallback));

        public static readonly DependencyProperty CanvasHeightProperty =
            DependencyProperty.Register("CanvasHeight", typeof (double), typeof (BasicSmartPanel),
                                        new PropertyMetadata(CanvasHeightCallback));

        public double CanvasWidth
        {
            get { return (double) GetValue(CanvasWidthProperty); }
            set { SetValue(CanvasWidthProperty, value); }
        }

        public double CanvasHeight
        {
            get { return (double) GetValue(CanvasHeightProperty); }
            set { SetValue(CanvasHeightProperty, value); }
        }

        public static void CanvasWidthCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = d as BasicSmartPanel;
            if (p == null) return;
            p.Space.CanvasChange.FireChange(p.Space.Canvas.SetSize((double) e.NewValue, p.Space.Canvas.Height));
            p.InvalidateMeasure();
        }

        public static void CanvasHeightCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = d as BasicSmartPanel;
            if (p == null) return;
            p.Space.CanvasChange.FireChange(p.Space.Canvas.SetSize(p.Space.Canvas.Width, (double) e.NewValue));
            p.InvalidateMeasure();
        }

        #endregion

        public static SimpleSubject<Args<FrameworkElement, Rect, Size>> GetOnArrange(FrameworkElement element)
        {
            return (SimpleSubject<Args<FrameworkElement, Rect, Size>>) element.GetValue(OnArrangeProperty);
        }

        public static void SetOnArrange(FrameworkElement element,
                                        SimpleSubject<Args<FrameworkElement, Rect, Size>> value)
        {
            element.SetValue(OnArrangeProperty, value);
            var p = element.GetParent<SimplePanel>();
            if (p != null) p.InvalidateArrange();
            // element.UpdateArrange();
        }

        public static void SetOnArrange(FrameworkElement element, Action<Args<FrameworkElement, Rect, Size>> value)
        {
            element.SetValue(OnArrangeProperty, new SimpleSubject<Args<FrameworkElement, Rect, Size>>(value));
            var p = element.GetParent<SimplePanel>();
            if (p != null) p.InvalidateArrange();

            // element.UpdateArrange();
        }

        #endregion

        #endregion

        public SimpleSubject<Size> AfterArrange = new SimpleSubject<Size>();
        public SimpleSubject<Size> AfterMeasurement = new SimpleSubject<Size>();
        public SimpleSubject<Size> BeforeArrange = new SimpleSubject<Size>();
        public SimpleSubject<Size> BeforeMeasurement = new SimpleSubject<Size>();


        /// <summary>
        /// Реактивная панелька, в конструкторе костыль
        /// </summary>
        protected BasicSmartPanel()
        {
            CacheMode = new BitmapCache();
            Children = new SmartCollection<FrameworkElement>();
            #if SILVERLIGHT
            Children.LeadByType(base.Children);
            #elif WPF
            SpecificCollectionsExtensions.LeadByType(this.Children, base.Children);
            //this.Children.LeadByType(base.Children);            
            #endif
            Children.ItemsAdded.Add(i => { if (ChildAdded != null) ChildAdded(i); }); //NAPILNIK
            Children.ItemsRemoved.Add(i => { if (ChildRemoved != null) ChildRemoved(i); });

            InitScroll();
        }

        #region IBasicSmartPanel Members

        /// <summary>
        /// Отрабатывает под добавлению чаилда
        /// </summary>
        public event Action<FrameworkElement> ChildAdded;

        /// <summary>
        /// Отрабатывает по удалению чаилда
        /// </summary>
        public event Action<FrameworkElement> ChildRemoved;

        #endregion

        /// <summary>
        /// Function for easy child addition. It is shorter than this.Children.Add(child) 
        /// and automaticaly removes child from its previous parent if it has it
        /// </summary>
        /// <param name="child"></param>
        public void AddChild(FrameworkElement child)
        {
            DependencyObject parent = child.Parent;
            if (parent == this) /*if(this.Children.Contains(child))*/ return;
            if (parent is Panel) (parent as Panel).Children.Remove(child);
            Children.Add(child);
        }

        /// <summary>
        /// Removes specified children
        /// </summary>
        /// <param name="removeSelector"></param>
        public void RemoveChildren(Func<FrameworkElement,bool> removeSelector)
        {
            this.Children.Where(removeSelector).ToList().ForEach(i=>this.Children.Remove(i));
        }

        public void RemoveChild(FrameworkElement child)
        {
            if (child != null && child.Parent == this) Children.Remove(child);
        }

        public abstract bool MeasureChild(FrameworkElement child, Size constrains);
        public abstract Boolean ArrangeChild(FrameworkElement child, Size constrains);

        #region LAYOUT HANDLERS RELATED

        public SimpleSubject<Size> BeforeLayoutUpdated
        {
            get
            {
                InitLayoutSubjects();
                return _beforeLayoutUpdated;
            }
            set { _beforeLayoutUpdated = value; }
        }


        public SimpleSubject<Size> AfterLayoutUpdated
        {
            get
            {
                InitLayoutSubjects();
                return _afterLayoutUpdated;
            }
            set { _afterLayoutUpdated = value; }
        }

        private void InitLayoutSubjects()
        {
            if (_beforeLayoutUpdated != null || _afterLayoutUpdated != null) return;
            _beforeLayoutUpdated = new SimpleSubject<Size>();
            _afterLayoutUpdated = new SimpleSubject<Size>();
            SubscribeLayout();
        }


        protected void SubscribeLayout()
        {
            LayoutUpdated += LayoutUpdatedHandler;
        }


        /// <summary>
        /// This is a part of layout optimisaion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public virtual void LayoutUpdatedHandler(object sender, EventArgs e)
        {
            BeforeLayoutUpdated.OnNext(Space.ArrangeSize);
            AfterLayoutUpdated.OnNext(Space.ArrangeSize);
        }

        public void RunAterNextLayoutUpdate(Action<Size> action)
        {
            AfterLayoutUpdated.At().Add(action);
        }

        public void RunAterNextLayoutUpdate(Action action)
        {
            AfterLayoutUpdated.At().Add(i => action());
        }

        #endregion

        #region SCROLL

        /// <summary>
        /// former CanvasPlace, inlude Canvas and Panel rectangles and some scaling and zooming functionality
        /// </summary>
        public CanvasPlace Space;

        private SimpleSubject<Size> _afterLayoutUpdated;
        private SimpleSubject<Size> _beforeLayoutUpdated;


        protected virtual void InitScroll()
        {
            Space = new CanvasPlace();
            Space.CanvasChange.Action += OnCanvasChange;
            Space.PanelChange.Action += OnPanelChange;
        }


        protected virtual void OnCanvasChange(CanvasPlace place, Rect oldVal, Rect newVal)
        {
            InvalidateMeasure();
        }

        protected virtual void OnPanelChange(CanvasPlace place, Rect oldVal, Rect newVal)
        {
            InvalidateMeasure();
        }

        #endregion

        #region BY NAME CHILD GETTERS

        /// <summary>
        /// Gets child by its name
        /// </summary>
        /// <param name="name">Name of a child that we want to obtain</param>
        /// <returns></returns>
        public FrameworkElement ChildByName(string name)
        {
            return Children.FirstOrDefault(child => child.Name.Equals(name));
        }

        public T ChildByName<T>(string name) where T : FrameworkElement
        {
            return Children.OfType<T>().FirstOrDefault(child => child.Name.Equals(name));
        }

        /// <summary>
        /// Executes action on a child wicth specified name
        /// If a child was not found it adds hanler to Children that will fire when a child with such name will be added
        /// </summary>
        /// <param name="name">Child's name</param>
        /// <param name="action">Action to be executed on the child</param>
        public void CatchByName(string name, Action<FrameworkElement> action)
        {
            FrameworkElement child = ChildByName(name);
            if (child != null) action(child);
            else Children.ItemsAdded.OnceIf(i => i.Name.Equals(name), action);
        }

        /// <summary>
        /// Executes action on a child wicth specified name
        /// If a child was not found it adds hanler to Children that will fire when a child with such name will be added
        /// </summary>
        /// <param name="name">Child's name</param>
        /// <param name="action">Action to be executed on the child</param>
        /// <param name="onRemove">Action executed on removal </param>
        public void CatchByName(string name, Action<FrameworkElement> action, Action<FrameworkElement> onRemove)
        {
            Action<FrameworkElement> act = i =>
                                               {
                                                   action(i);
                                                   Children.ItemsRemoved.OnceIf(c => c.Equals(i), onRemove);
                                               };
            CatchByName(name, act);
        }

        public void CatchBySelector(Func<FrameworkElement, Boolean> selector, Action<FrameworkElement> action,
                                    Action<FrameworkElement> onRemove)
        {
            Children.ItemsRemoved.OnceIf(selector, onRemove);
            FrameworkElement child = Children.FirstOrDefault(selector);
            if (child != null) action(child);
            else Children.ItemsAdded.OnceIf(selector, action);
        }

        #endregion

        #region GROW CHILD

        public abstract T GrowChild<T>(T child, Point delta, AlignmentX hor = AlignmentX.Center,
                                       AlignmentY ver = AlignmentY.Center) where T : FrameworkElement;

        public abstract T PlaceChild<T>(T child, Rect rect) where T : FrameworkElement;

        /// <summary>
        /// Grows child horizontally
        /// </summary>
        /// <typeparam name="T">child's type</typeparam>
        /// <param name="child">child to grow</param>
        /// <param name="delta">delta</param>
        /// <param name="hor">horizontal alignment</param>
        /// <returns>Child</returns>
        public T GrowChildHorizontal<T>(T child, double delta, AlignmentX hor) where T : FrameworkElement
        {
            return GrowChild(child, new Point(delta, 0), hor);
        }

        public T GrowChildVertical<T>(T child, double delta, AlignmentY ver) where T : FrameworkElement
        {
            return GrowChild(child, new Point(0, delta), AlignmentX.Center, ver);
        }

        #endregion

    }
}