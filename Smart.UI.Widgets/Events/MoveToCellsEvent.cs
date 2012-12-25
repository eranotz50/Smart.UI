using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using Smart.UI.Classes.Events;
using Smart.UI.Panels;

namespace Smart.UI.Widgets.Events
{
    public class MoveToCellsEvent : MassiveAnimationEvent<Tuple<FrameworkElement, CellsRegion>>
    {
        public MoveToCellsEvent(string name, IEnumerable<Tuple<FrameworkElement, CellsRegion>> targets, TimeSpan howlong, IEasingFunction easing = null) : base(name, targets, howlong, easing)
        {
        }
    }
}
