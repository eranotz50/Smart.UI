using System;
using System.Windows;
using Smart.UI.Panels;
using Smart.Classes.Subjects;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Relatives
{
    public static class Relative
    {
        #region DEFAULTS

        public static Boolean DefaultRelativeElement = false; //Временно отключен, так как недописан

        public static FrameworkElement GetDefaultRelativeElement<T>(this T element) where T : FrameworkElement
        {
            if (!DefaultRelativeElement) return null;
            var parent = element.Parent as SimplePanel;
            if (parent == null) return null;
            int i = parent.Children.IndexOf(element);
            return i > 0 ? parent.Children[i - 1] : null;
        }

        #endregion

        #region BINDINGS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element">element which position is binding to another "relative element"</param>        
        /// <param name="value">current value of relative element</param>
        /// <param name="pre">previous value of relative element</param>
        public static void UpdatePositionFunctionToElementBinding(this FrameworkElement element, FrameworkElement value,
                                                                  FrameworkElement pre)
        {
            ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement> pos = GetRelativePosition(element);
            if (pos == null) return;
            pos.Param1 = element;
            if (pre != null) pre.DeleteFromOnArrange(pos.Subscription);
            if (value == null) return;
            value.UpdateOnArrange(pos.Subscription);
            // if(pos.Param0!=null && pos.Param1!=null) pos.Execute();                  
            var p = element.GetParent<SimplePanel>();
            if (p != null) p.InvalidateMeasure();
        }

        /// <summary>
        /// Updates binding of the relativeActionWithArgs to relative element
        /// </summary>
        /// <param name="element">element</param>                
        /// <param name="relPos">relative position action withargs</param>
        /// <param name="pre">previous relativeposition action with args</param>
        public static void UpdateElementToPositionFunctionBinding(this FrameworkElement element,
                                                                  ActionWithArgs
                                                                      <Args<FrameworkElement, Rect, Size>,
                                                                      FrameworkElement> relPos,
                                                                  ActionWithArgs
                                                                      <Args<FrameworkElement, Rect, Size>,
                                                                      FrameworkElement> pre = null)
        {
            FrameworkElement rel = GetRelativeElement(element) ?? element.GetDefaultRelativeElement();
            if (rel == null) return;
            if (pre != null) rel.DeleteFromOnArrange(pre.Subscription);
            if (relPos != null) rel.UpdateOnArrange(relPos.Subscription);
        }

        #endregion

        #region DELAYED UPDATERS needed to take into account that an element can have no parent when properties are being attached to it

        public static Action<Args<FrameworkElement, Rect, Size>> UpdateToWithParent(this FrameworkElement element)
        {
            Action<Args<FrameworkElement, Rect, Size>> retval = i =>
                                                                    {
                                                                        var p = element.Parent as SimplePanel;
                                                                        if (p == null) return;
                                                                        p.WriteTo(element);
                                                                        p.InvalidateMeasure();
                                                                    };
            return retval;
        }

        #endregion

        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ToProperty =
            DependencyProperty.RegisterAttached("To", typeof (String), typeof (FrameworkElement),
                                                new PropertyMetadata(default(String), UpdateToCallBack));

        public static readonly DependencyProperty RelativeElementProperty =
            DependencyProperty.RegisterAttached("RelativeElement", typeof (FrameworkElement), typeof (FrameworkElement),
                                                new PropertyMetadata(default(FrameworkElement),
                                                                     UpdateRelativeElementCallBack));

        /// <summary>
        /// Settes all apropriate stuff to make relative positioning
        /// </summary>
        public static readonly DependencyProperty RelativePositionProperty =
            DependencyProperty.RegisterAttached("RelativePosition",
                                                typeof (
                                                    ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>
                                                    ), typeof (FrameworkElement),
                                                new PropertyMetadata(
                                                    default(
                                                        ActionWithArgs
                                                        <Args<FrameworkElement, Rect, Size>, FrameworkElement>),
                                                    UpdateRelativePositionCallBack));

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.RegisterAttached("OtherLineTriplets", typeof (String), typeof (FrameworkElement),
                                                new PropertyMetadata(UpdatePositionCallBack));

        public static readonly DependencyProperty DragGrowthModeProperty =
            DependencyProperty.RegisterAttached("DragGrowthMode", typeof (GrowthMode), typeof (FrameworkElement),
                                                new PropertyMetadata(default(GrowthMode), UpdateDragGrowth));

        public static void SetTo(FrameworkElement element, String value)
        {
            element.SetValue(ToProperty, value);
        }

        public static void WriteTo(this SimplePanel p, FrameworkElement element)
        {
            FrameworkElement el = GetRelativeElement(element);
            if (el != null) SetRelativeElement(element, null);
            p.CatchBySelector(e => e.Name.Equals(GetTo(element)), a => SetRelativeElement(element, a),
                              r => SetRelativeElement(element, null));
        }


        //Due to designtime ignorance of attachable deproperty setters I had to transform them into callbacks

        /// <summary>
        /// Callback than invalidates arrange of the element
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void UpdateToCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // if (e.OldValue!=default(String) || e.NewValue == default(String)) return;
            var element = d as FrameworkElement;
            if (element == null) return;
            //  var value = e.NewValue as String;
            DragPanel p = element.GetNearestDragParent();
            if (p == null)
            {
                element.RunOnFirstArrange(element.UpdateToWithParent());
            }
            else
                p.WriteTo(element);
        }

        public static String GetTo(FrameworkElement element)
        {
            return (String) element.GetValue(ToProperty);
        }


        public static void SetRelativeElement(FrameworkElement element, FrameworkElement value)
        {
            element.SetValue(RelativeElementProperty, value);
        }

        public static FrameworkElement GetRelativeElement(FrameworkElement element)
        {
            return (FrameworkElement) element.GetValue(RelativeElementProperty);
        }

        /// <summary>
        /// Callback that fires when we change the element in relation to which we carry out relative positioning
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void UpdateRelativeElementCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null) return;
            var pre = e.OldValue as FrameworkElement;
            var value = e.NewValue as FrameworkElement;
            var p = element.Parent as SimplePanel;
            if (p == null)
            {
                SimpleSubject<Args<FrameworkElement, Rect, Size>> sub = value.GetOnArrange() ??
                                                                        value.UpdateValue(
                                                                            BasicSmartPanel.OnArrangeProperty,
                                                                            new SimpleSubject
                                                                                <Args<FrameworkElement, Rect, Size>>());
                sub.At().DoOnNext += i => element.PutBeforeRelative(p);
            }
            else element.PutBeforeRelative(p);
            element.UpdatePositionFunctionToElementBinding(value, pre);
        }

        /// <summary>
        /// Function needed to ensure that all elements with relative positions are before elements they get position from
        /// </summary>
        /// <param name="element"></param>
        /// <param name="p"></param>
        public static void PutBeforeRelative<T>(this T element, SimplePanel p) where T : FrameworkElement
        {
            FrameworkElement value = GetRelativeElement(element);
            if (value == null) return;
            int r = p.Children.IndexOf(value);
            int i = p.Children.IndexOf(element);
            if (r > i)
            {
                p.Children.PutBefore(value, element);
                p.InvalidateMeasure();
            }
        }

        public static void SetRelativePosition(FrameworkElement element,
                                               ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>
                                                   value)
        {
            element.SetValue(RelativePositionProperty, value);
        }

        /// <summary>
        /// Update callback for relative position action with action args
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        public static void UpdateRelativePositionCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null) return;
            var pre = e.OldValue as ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>;
            var value = e.NewValue as ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>;
            element.UpdateElementToPositionFunctionBinding(value, pre);
        }


        public static ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement> GetRelativePosition(
            FrameworkElement element)
        {
            return
                (ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement>)
                element.GetValue(RelativePositionProperty);
        }

        public static void SetPosition(UIElement element, String value)
        {
            element.SetValue(PositionProperty, value);
        }

        public static void UpdatePositionCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null) return;
            var strVal = e.NewValue as string;
            if (strVal == null) return;
            Action<Args<FrameworkElement, Rect, Size>, FrameworkElement> value = strVal.ToRelativePositionAction();
            ActionWithArgs<Args<FrameworkElement, Rect, Size>, FrameworkElement> pos = GetRelativePosition(element) ??
                                                                                       element.UpdateValue(
                                                                                           RelativePositionProperty,
                                                                                           new ActionWithArgs
                                                                                               <
                                                                                               Args
                                                                                               <FrameworkElement, Rect,
                                                                                               Size>, FrameworkElement>(
                                                                                               value, null, element));
            pos.Action = value;
            element.UpdateElementToPositionFunctionBinding(pos);
        }

        public static String GetPosition(UIElement element)
        {
            return (String) element.GetValue(PositionProperty);
        }


        public static void SetDragGrowthMode(FrameworkElement element, GrowthMode value)
        {
            element.SetValue(DragGrowthModeProperty, value);
        }

        public static GrowthMode GetDragGrowthMode(FrameworkElement element)
        {
            return (GrowthMode) element.GetValue(DragGrowthModeProperty);
        }

        public static void UpdateDragGrowth(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            if (element == null) return;
            var old = (GrowthMode) e.OldValue;
            var val = (GrowthMode) e.NewValue;
            SimpleSubject<ObjectFly> subject = DragManager.GetDragSubject(element) ??
                                               element.UpdateValue(DragManager.DragSubjectProperty,
                                                                   new SimpleSubject<ObjectFly>());
            if (old != GrowthMode.None) subject.DoOnNext -= RelativeFunctions.ActionByGrowth(old);
            Action<ObjectFly> action = RelativeFunctions.ActionByGrowth(val);
            if (action != null) subject.DoOnNext += action;
        }

        #endregion
    }
}