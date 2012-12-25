using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using Smart.UI.Classes.Events;

namespace Smart.UI.Widgets.Events
{
    public class MoveInCellsEvent:MassiveAnimationEvent<Tuple<FrameworkElement,Rect>>
    {
        public MoveInCellsEvent(string name, IEnumerable<Tuple<FrameworkElement, Rect>> targets, TimeSpan howlong, IEasingFunction easing = null) : base(name, targets, howlong, easing)
        {
        }

    }
}
