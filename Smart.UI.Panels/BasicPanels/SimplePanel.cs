using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Smart.Classes.Events;
using Smart.Classes.Extensions;
using Smart.Classes.Interfaces;
using Smart.Classes.Subjects;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Classes.Layout;

namespace Smart.UI.Panels
{
    public enum OutMode
    {
        Free,
        Watch,
        Clip,
        ClipExceptAdorners,
        Cut
    }

    public enum ElementType
    {
        Normal,
        Adorner,
        DecorationAdorner
    }

    /// <summary>
    /// Класс простой панельки, из фукнционала - пару удобностей и простое икс-игрик позиционирование,
    /// от неё наследуют драгпанельки, от которых в свою очередь - канвас и грид
    /// </summary>
    public class SimplePanel : BasicSmartPanel, IChildElement, IEventHub, IEventsHolder
    {
        #region DEPENDENCY PROPS Депенденси проперти, а также гетеры и сетеры к ним

        public static readonly DependencyProperty PosProperty =
            DependencyProperty.RegisterAttached("Pos", typeof (Pos3D), typeof (FrameworkElement),
                                                new PropertyMetadata(default(Pos3D), UpdatePosCallback));

        public static readonly DependencyProperty PlaceProperty =
            DependencyProperty.RegisterAttached("Place", typeof (Rect), typeof (FrameworkElement),
                                                new PropertyMetadata(Rect.Empty, InvalidateParentMeasureCallback));

        /// <summary>
        /// Служебная деппроперти. 
        /// В случае если мы рассчитали все необходимое еще на этапе мэжура, мы можем записать в нее прямоугольник для аррейнджа.
        /// Именно поэтому у этой проперти нет колбека
        /// </summary>
        public static readonly DependencyProperty SlotProperty =
            DependencyProperty.RegisterAttached("Slot", typeof (Rect), typeof (FrameworkElement),
                                                new PropertyMetadata(Rect.Empty));

        public static readonly DependencyProperty OutModeProperty =
            DependencyProperty.Register("OutMode", typeof (OutMode), typeof (SimplePanel),
                                        new PropertyMetadata(OutMode.Free, OutModeCallback));

        public static readonly DependencyProperty ElementTypeProperty =
            DependencyProperty.RegisterAttached("ElementType", typeof (ElementType), typeof (FrameworkElement),
                                                new PropertyMetadata(ElementType.Normal));

        public static readonly DependencyProperty VirtualizedProperty =
            DependencyProperty.RegisterAttached("Virtualized", typeof (Boolean), typeof (FrameworkElement),
                                                new PropertyMetadata(false, VirtualizedCallback));

        /// <summary>
        /// Canvas mode determines the behaviour of the canvas when elements go out of it
        /// </summary>
        public static readonly DependencyProperty CanvasUpdateModeProperty =
            DependencyProperty.Register("CanvasUpdateMode", typeof (CanvasUpdateMode), typeof (SimplePanel),
                                        new PropertyMetadata(CanvasUpdateMode.CanvasWithPanel, CanvasUpdateModeCallBack));

        public static readonly DependencyProperty AllowSelfGrowProperty =
            DependencyProperty.Register("AllowSelfGrow", typeof (Boolean), typeof (SimplePanel),
                                        new PropertyMetadata(true));

        public OutMode OutMode
        {
            get { return (OutMode) GetValue(OutModeProperty); }
            set { SetValue(OutModeProperty, value); }
        }

        public CanvasUpdateMode CanvasUpdateMode
        {
            get { return (CanvasUpdateMode) GetValue(CanvasUpdateModeProperty); }
            set { SetValue(CanvasUpdateModeProperty, value); }
        }

        public Boolean AllowSelfGrow
        {
            get { return (Boolean) GetValue(AllowSelfGrowProperty); }
            set { SetValue(AllowSelfGrowProperty, value); }
        }

        [TypeConverter(typeof (Pos3DConverter))]
        public static void SetPos(FrameworkElement element, Pos3D value)
        {
            element.SetValue(PosProperty, value);
            if (value.HasZ) Canvas.SetZIndex(element, Convert.ToInt32(value.Z*10));
        }

        [TypeConverter(typeof (Pos3DConverter))]
        public static Pos3D GetPos(FrameworkElement element)
        {
            return (Pos3D) element.GetValue(PosProperty);
        }

        public static void UpdatePosCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null) return;

            var old = (Pos3D) e.OldValue;
            var pos = (Pos3D) e.NewValue;

            #if SILVERLIGHT
                if (pos.HasZ && !pos.Z.Equals(0.0)) fe.MakeProjection(pos.Z);            
                else fe.Projection = default(Projection);
            #endif
            var p = fe.Parent as BasicSmartPanel;
            if (p == null) return;
            if (p.Dirty != Dirtiness.Measure)
                SlotByPos(fe, old, pos, p);
            else
                p.BeforeArrange.At().Add(i => SlotByPos(fe, old, pos, p));
        }

        protected static void SlotByPos(FrameworkElement fe, Pos3D old, Pos3D pos, BasicSmartPanel p)
        {
            Rect slot = GetSlot(fe);
            if ( /*!old.NotEmpty || */!pos.NotEmpty || slot.IsEmpty)
            {
                p.InvalidateMeasure();
                p.Dirty = Dirtiness.Measure;
                return;
            }
            SetSlot(fe, pos.ToRect(slot.Size()));
            p.Dirty = Dirtiness.Arrange;
            p.InvalidateArrange();
        }


        public static Rect GetPlace(FrameworkElement element)
        {
            return (Rect) element.GetValue(PlaceProperty);
        }

        public static void SetPlace(FrameworkElement element, Rect value)
        {
            element.SetValue(PlaceProperty, value);
        }

        public static Rect GetSlot(FrameworkElement element)
        {
            return (Rect) element.GetValue(SlotProperty);
        }

        public static void SetSlot(FrameworkElement element, Rect value)
        {
            element.SetValue(SlotProperty, value);
        }

        public static void OutModeCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = d as FrameworkElement;
            if (p == null) return;
            var o = (OutMode) e.OldValue;
            // var n = (UpdateMode) e.NewValue;
            if (o == OutMode.Clip)
            {
                p.Clip = default(Geometry);
            }
            p.InvalidateArrange();
        }

       

        public static ElementType GetElementType(DependencyObject obj)
        {
            return (ElementType) obj.GetValue(ElementTypeProperty);
        }
       
        public static void SetElementType(DependencyObject obj, ElementType value)
        {
            obj.SetValue(ElementTypeProperty, value);
        }
        
        public static Boolean GetVirtualized(DependencyObject obj)
        {
            return (bool) obj.GetValue(VirtualizedProperty);
        }

        public static void SetVirtualized(DependencyObject obj, bool value)
        {
            obj.SetValue(VirtualizedProperty, value);
        }

        public static void VirtualizedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = d as FrameworkElement;
            if (fe == null) return;
            var val = (bool) e.NewValue;
            fe.SetValue(VisibilityProperty, val ? Visibility.Collapsed : Visibility.Visible);
            var p = fe.Parent as SimplePanel;
            if (p == null) return;
            if (!val && p.OnShowItem != null) p.OnShowItem(fe);
            if (val && p.OnHideItem != null) p.OnHideItem(fe);
        }


        /// <summary>
        /// TO WORKOUT 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void CanvasUpdateModeCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var p = d as SimplePanel;
            if (p == null) return;
            p.Space.UpdateMode = (CanvasUpdateMode) e.NewValue;
            p.InvalidateMeasure();
        }

        #endregion

        public SimpleSubject<EventPattern<FrameworkElement, MouseEventArgs>> ChildMouseEnterSubject;
        public SimpleSubject<EventPattern<FrameworkElement, MouseEventArgs>> ChildMouseLeaveSubject;
        public SimpleSubject<EventPattern<MouseButtonEventArgs>> MouseDownSubject;
        public SimpleSubject<EventPattern<MouseEventArgs>> MouseEnterSubject;
        public SimpleSubject<EventPattern<MouseEventArgs>> MouseLeaveSubject;
        public SimpleSubject<EventPattern<MouseEventArgs>> MouseMoveSubject;
        public SimpleSubject<EventPattern<MouseButtonEventArgs>> MouseUpSubject;

        public SimpleSubject<EventPattern<MouseWheelEventArgs>> MouseWheelSubject;


        public SimplePanel()
        {
            MouseDownSubject = this.FromEventPattern<MouseButtonEventArgs>("MouseLeftButtonDown");
            MouseMoveSubject = this.FromEventPattern<MouseEventArgs>("MouseMove");
            MouseUpSubject = this.FromEventPattern<MouseButtonEventArgs>("MouseLeftButtonUp");

            MouseEnterSubject = this.FromEventPattern<MouseEventArgs>("MouseEnter").AddAction(MouseEnterHandler);
            MouseLeaveSubject = this.FromEventPattern<MouseEventArgs>("MouseLeave").AddAction(MouseLeaveHandler);
            MouseWheelSubject = this.FromEventPattern<MouseWheelEventArgs>("MouseWheel");
            //.AddAction(MouseWheelHandler);

            ChildMouseEnterSubject = new SimpleSubject<EventPattern<FrameworkElement, MouseEventArgs>>();
            ChildMouseLeaveSubject = new SimpleSubject<EventPattern<FrameworkElement, MouseEventArgs>>();

            Out = new SimpleSubject<FrameworkElement>();
            OutPartial = new SimpleSubject<FrameworkElement>();
            OnAddedToStage = new SimpleSubject<SimplePanel>();
            OnRemovedFromStage = new SimpleSubject<SimplePanel>();
            ChildAdded += OnChildAdded;
            ChildRemoved += OnChildRemoved;

            OnShowItem += ShowItemHandler;
            OnHideItem += HideItemHandler;

            SmartEventManager = new SmartEventManager();
            ;
        }

        #region CHILD ADDED AND CHILD REMOVED HANDLERS

        protected virtual void OnChildAdded(FrameworkElement child)
        {
            child.MouseEnter += ChildMouseEnterHandler;
            child.MouseLeave += ChildMouseLeaveHandler;
            if (child is IChildElement == false) return;
            (child as IChildElement).OnAddedToStage.OnNext(this);
        }

        protected virtual void OnChildRemoved(FrameworkElement child)
        {
            child.MouseEnter -= ChildMouseEnterHandler;
            child.MouseLeave -= ChildMouseLeaveHandler;
            if (child is IChildElement == false) return;
            (child as IChildElement).OnRemovedFromStage.OnNext(this);
        }

        #endregion

        #region SHOW AND HIDE HANDLERS

        /// <summary>
        /// Fires on showing by virtualization
        /// </summary>
        public Action<FrameworkElement> OnHideItem;

        /// <summary>
        /// Fires on showing by devirtualiazation
        /// </summary>
        public Action<FrameworkElement> OnShowItem;

        public virtual void ShowItemHandler(FrameworkElement item)
        {
            item.Visibility = Visibility.Visible;
            //this.InvalidateMeasure();
        }

        public virtual void HideItemHandler(FrameworkElement item)
        {
            item.Visibility = Visibility.Collapsed;
            //this.InvalidateMeasure();
        }

        #endregion

        #region MOUSEENTER HANDLERS

        public FrameworkElement LastEnteredChild { get; set; }

        public virtual void ChildMouseEnterHandler(object sender, MouseEventArgs e)
        {
            object f = sender;
            //e.OriginalSource as FrameworkElement;/*  var p = f.GetParent<SimplePanel>();             if (p == null) return;           */           
            MouseEnterSubject.OnNext(new EventPattern<MouseEventArgs>(f, e));
            LastEnteredChild = f as FrameworkElement;
        }


        public virtual void ChildMouseLeaveHandler(object sender, MouseEventArgs e)
        {
            object f = sender;
            //e.OriginalSource as FrameworkElement; /*  var p = f.GetParent<SimplePanel>();             if (p == null) return;           */
            LastEnteredChild = null;
            MouseLeaveSubject.OnNext(new EventPattern<MouseEventArgs>(f, e));
        }

        #endregion

        #region ROUTING for MOUSEWHEEL, MOUSEENTER AND MOUSELEAVE ПОКА ПОЧТИ НЕ ПАШУТ

        public virtual void MouseEnterHandler(EventPattern<MouseEventArgs> e)
        {
            RouteMouseEnter(e);
        }


        public virtual void MouseLeaveHandler(EventPattern<MouseEventArgs> e)
        {
            RouteMouseLeave(e);
        }

        protected void RouteMouseEnter(EventPattern<MouseEventArgs> e)
        {
            var p = this.GetParent<SimplePanel>();
            if (p == null) return;
            if (p.LastEnteredChild != this)
                p.ChildMouseEnterSubject.OnNext(new EventPattern<FrameworkElement, MouseEventArgs>(this, e.EventArgs));
        }

        protected void RouteMouseLeave(EventPattern<MouseEventArgs> e)
        {
            var p = this.GetParent<SimplePanel>();
            if (p == null) return;
            if (p.LastEnteredChild == this)
                p.ChildMouseLeaveSubject.OnNext(new EventPattern<FrameworkElement, MouseEventArgs>(this, e.EventArgs));
        }

        #endregion

        #region MOVEMENTS BETWEEN PANELS

        public SimpleSubject<FrameworkElement> Out;
        public SimpleSubject<FrameworkElement> OutPartial;


        public virtual void ChildToPanel<T>(FrameworkElement child, T destination) where T : SimplePanel
        {
            Contract.Requires(child!=null && destination!=null && destination!=this);          
            GeneralTransform transform = TransformToVisual(destination);
            Rect bounds = child.GetBounds();
            Children.Move(child, destination.Children);
            destination.ReceiveChild(child, bounds, transform.TransformBounds(bounds));
        }

        /// <summary>
        /// Child parking after drag and docking
        /// </summary>
        /// <param name="child"></param>
        public virtual void ParkChild(FrameworkElement child)
        {
        }

        /// <summary>
        /// receives dragelement
        /// </summary>
        /// <param name="child"></param>
        /// <param name="oldPlace"></param>
        /// <param name="newPlace"></param>
        public virtual void ReceiveChild(FrameworkElement child, Rect oldPlace, Rect newPlace)
        {
            Pos3D pos = GetPos(child);
            if (pos.NotEmpty && pos.IsValid())
            {
                child.Width = newPlace.Width;
                child.Height = newPlace.Height;
                //SetPos(child, new Pos3D(newPlace.X, newPlace.Y,pos.Z,pos.Centered));                
                SetPos(child, new Pos3D(newPlace.X, newPlace.Y, pos.Z, pos.Centered));
            }
            else child.SetPlace(newPlace);
        }

        /// <summary>
        /// Shifts drag element
        /// </summary>
        /// <param name="child"></param>
        /// <param name="shift"></param>
        /// <returns></returns>
        public virtual bool Shift(FrameworkElement child, Point shift)
        {
            Pos3D pos = GetPos(child);
            if (pos.NotEmpty)
            {
                SetPos(child, pos + shift);
                return true;
            }
            Rect place = GetPlace(child);
            if (place.IsEmpty) return false;
            SetPlace(child, new Rect(place.X + shift.X, place.Y + shift.Y, place.Width, place.Height));
            return true;
        }

        #endregion

        #region ARRANGE & MEASURE Непосредственное расположение дочерних элементов

        /// <summary>
        /// Сюда записывается расположения панельки, если она является дочкой другой драгпанельки
        /// </summary>
        public Boolean HasPos(FrameworkElement child)
        {
            return GetPos(child).NotEmpty;
        }

        /// <summary>
        /// Овверрайдинг измерялки сколько хочет этот контрол места
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns>Возвращает желаемый размер</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Dirty = Dirtiness.Measure;
            Size size = this.ExtractSize(availableSize);
            Space.UpdateSizes(size);
            Size constrains = Space.Canvas.Size();
            // this.Dirty = Dirtiness.Arrange;
            BeforeMeasurement.OnNext(constrains);
            foreach (FrameworkElement child in Children) MeasureChild(child, constrains);
            AfterMeasurement.OnNext(constrains);
            return size;
        }


        /// <summary>
        /// Мэжурим дочерний элемент
        /// </summary>
        /// <param name="child"></param>
        /// <param name="constrains"></param>
        /// <returns></returns>
        public override bool MeasureChild(FrameworkElement child, Size constrains)
        {
            Rect slot = WriteSlot(child, constrains);
            if (!slot.IsEmpty) SetSlot(child, slot);
            return true;
        }

        /// <summary>
        /// Cleans children position
        /// </summary>
        /// <param name="child"></param>
        public virtual void ClearPosition(FrameworkElement child)
        {
            SetPos(child,default(Pos3D));
            SetPlace(child, Rect.Empty);
        }

        /// <summary>
        /// Populate with canvas info
        /// </summary>
        /// <param name="child"></param>
        /// <param name="constrains"></param>
        protected virtual Rect WriteSlot(FrameworkElement child, Size constrains)
        {
            Rect place = GetPlace(child);
            if (!place.IsEmpty)
            {
                Size size = place.Size();
                child.Measure(size.Min(constrains));
                size = child.DesiredSize(size);
                return new Rect(place.X, place.Y, size.Width, size.Height);
            }
            Pos3D pos = GetPos(child);
            if (pos.NotEmpty)
            {
                child.Measure(constrains);
                Size size = child.SizeForRender(constrains);
                return pos.ToRect(size);
            }
            return Rect.Empty;
        }

        /// <summary>
        /// Do all appropriate stuff before arranging
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected virtual Size PrepareArrange(Size finalSize)
        {
            Dirty = Dirtiness.Arrange;
            Space.Elements = default(Rect);
            //this.Space.Reset(this.ExtractSize(finalSize));
            Size constrains = Space.Panel.Size();
            base.BeforeArrange.OnNext(constrains);
            Dirty = Dirtiness.Clean;
            if (OutMode == OutMode.Clip)
            {
                RectangleGeometry g = Clip as RectangleGeometry ?? new RectangleGeometry();
                g.Rect = new Rect(0, 0, constrains.Width, constrains.Height);
                Clip = g;
            }
            ;
            return constrains;
        }


        /// <summary>
        /// А вот в этой функции расставляем все контролзы
        /// </summary>
        /// <param name="finalSize"></param>
        /// <returns></returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Size constrains = PrepareArrange(finalSize);
            foreach (FrameworkElement child in Children) ArrangeChild(child, constrains);
            return FinishArrange(constrains);
        }

        /// <summary>
        /// Do all appropriate stuff after arranging
        /// </summary>
        /// <param name="constrains"></param>
        /// <returns></returns>
        protected Size FinishArrange(Size constrains)
        {
            AfterArrange.OnNext(constrains);
            var p = Parent as SimplePanel;
            if (p == null)
            {
                var rect = new Rect(0, 0, constrains.Width, constrains.Height);
                SetSlot(this, rect);
                DoOnArrange(this, constrains, rect);
            }
            AfterArrange.OnNext(constrains);
            Space.SaveChanges();
            return constrains;
        }


        /// <summary>
        /// Располагаем один чаилд
        /// </summary>
        /// <param name="child"></param>
        /// <param name="constrains"></param>
        /// <returns>Возвращает тру если удалось расположить по параметрам</returns>      
        public override Boolean ArrangeChild(FrameworkElement child, Size constrains)
        {
            // if (ArrangeChildByPos(child, constrains)) return true;
            Rect slot = GetSlot(child);
            if (slot.IsValid() == false) return false;
            Rect rect = Space.Place(slot);
            if (rect.IsEmpty) return true;
            WorkOut(child, constrains, rect);
            child.Arrange(rect);
            DoOnArrange(child, constrains, rect);
            return true;
        }


        protected void DoOnArrange(FrameworkElement child, Size constrains, Rect rect)
        {
            SimpleSubject<Args<FrameworkElement, Rect, Size>> onArrange = GetOnArrange(child);
            if (onArrange != null)
                onArrange.OnNext(new Args<FrameworkElement, Rect, Size>(child, rect, constrains)); //);
        }

        /// <summary>
        /// Proccessing element going out of the panel
        /// </summary>
        /// <param name="child"></param>
        /// <param name="constrains"></param>
        /// <param name="rect"></param>
        protected void WorkOut(FrameworkElement child, Size constrains, Rect rect)
        {
            if (OutMode == OutMode.Free) return;
            var place = new Rect(new Point(), constrains); // получаем квадрат, где контролы видимы
            Rect bounds = place.GetIntersectionRect(rect);
            // получаем квадрат пересечения канваса с местом расположения контрола
            // ElementType type = GetElementType(child); // тип контрола
            Intersection inter = bounds.IsEmpty
                                     ? Intersection.None
                                     : (bounds.RoundEquals(rect) ? Intersection.Full : Intersection.Partial);
            // определяем тип пересечения - полный (контрол внутри канваса), частичный - на границе, и отсутствие пересечения - за пределами канваса
            if (inter == Intersection.None) Out.OnNext(child);
            if (inter == Intersection.Partial) OutPartial.OnNext(child);
            switch (OutMode)
            {
                case OutMode.ClipExceptAdorners:
                    if (this.AllowClip(child))
                    {
                        child.Clip = inter == Intersection.Partial
                                         ? new RectangleGeometry
                                               {
                                                   Rect =
                                                       new Rect(bounds.X - rect.X, bounds.Y - rect.Y, bounds.Width,
                                                                bounds.Height)
                                               }
                                         : default(Geometry);

                        SetVirtualized(child, inter == Intersection.None);
                    }
                    break;
                case OutMode.Clip:
                    SetVirtualized(child, inter == Intersection.None);
                    break;
                case OutMode.Cut:
                    SetVirtualized(child, inter == Intersection.None || inter == Intersection.Partial);
                    break;
            }
        }

        /// <summary>
        /// NAPILNIK
        /// </summary>
        /// <param name="child"></param>
        /// <returns></returns>
        private Boolean AllowClip(FrameworkElement child)
        {
            return GetElementType(child) != ElementType.DecorationAdorner;
        }


        #endregion

       

        #region IChildElement Members

        public SimpleSubject<SimplePanel> OnAddedToStage { get; set; }
        public SimpleSubject<SimplePanel> OnRemovedFromStage { get; set; }

        #endregion

        #region GROW REGION

        /// <summary>
        /// Grows child according to settings
        /// </summary>
        /// <param name="child">child to grow</param>
        /// <param name="delta">delta</param>
        /// <param name="hor">horizontal alignment</param>
        /// <param name="ver">vertical alignament</param>
        public override T GrowChild<T>(T child, Point delta, AlignmentX hor = AlignmentX.Center,
                                       AlignmentY ver = AlignmentY.Center)
        {
            Rect place = GetPlace(child);
            return place.IsEmpty ? child.AddSize(delta) : child.SetPlace(place.GrowRect(delta, hor, ver));
        }

        public override T PlaceChild<T>(T child, Rect rect)
        {
            return GetPlace(child).IsEmpty
                       ? child.SetSize(rect.Width, rect.Height).SetPos(rect.X, rect.Y)
                       : child.SetPlace(rect);
        }


        protected virtual void GrowSelf(Point delta, AlignmentX hor, AlignmentY ver,
                                        PanelUpdateMode updateMode = PanelUpdateMode.Canvas)
        {
            if (delta.Equals(default(Point)) || updateMode == PanelUpdateMode.None) return;
            if (updateMode == PanelUpdateMode.Panel)
            {
                Space.GrowPanel(delta, hor, ver);
            }
            else
                Space.GrowCanvas(delta, hor, ver);
            Space.SyncCanvasToPanel();
            InvalidateMeasure();
        }

        /// <summary>
        /// Growth canvas or self in accordance to PanelUpdateMode
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="hor"></param>
        /// <param name="mode"> </param>
        protected void GrowSelfHorizontal(double delta, AlignmentX hor, PanelUpdateMode mode)
        {
            GrowSelf(new Point(delta, 0), hor, AlignmentY.Bottom, mode);
        }

        protected void GrowSelfVertical(double delta, AlignmentY ver, PanelUpdateMode mode)
        {
            GrowSelf(new Point(0, delta), AlignmentX.Right, ver, mode);
        }


        protected override void OnPanelChange(CanvasPlace place, Rect oldVal, Rect newVal)
        {
            GrowPanel(new Point(newVal.Width - oldVal.Width, newVal.Height - oldVal.Height));
            InvalidateMeasure();
        }


        protected void GrowPanel(Point delta, AlignmentX hor = AlignmentX.Right, AlignmentY ver = AlignmentY.Bottom)
        {
            var p = Parent as SimplePanel;
            if (p == null)
            {
                if (AllowSelfGrow) this.AddSize(delta);
                if (Dirty == Dirtiness.Measure)
                {
                    Space.Canvas.Width += delta.X;
                    Space.Canvas.Height += delta.Y;
                    Space.SyncCanvasToPanel();
                }
            }
            else
            {
                if (AllowSelfGrow) p.GrowChild(this, delta, hor, ver);
            }
        }

        #endregion

        #region Implementation of IEventHub

        public SimpleSubject<TEventArgs> RunEvent<TEventArgs>(TEventArgs eventArgs) where TEventArgs : INamed
        {
            return SmartEventManager.RunEvent(eventArgs);
        }

        public SimpleSubject<TEventArgs> RunEvent<TEventArgs>(string name, TEventArgs eventArgs)
        {
            return SmartEventManager.RunEvent(name, eventArgs);
        }

        public bool HasEvent<TEvenrArgs>(string name)
        {
            return SmartEventManager.HasEvent<TEvenrArgs>(name);
        }

        #endregion

        #region IEventsHolder Members

        public SmartEventManager SmartEventManager { get; set; }

        #endregion
    }
}