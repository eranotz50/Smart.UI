using System;
using System.Windows;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.Classes.Subjects;

namespace Smart.UI.Panels
{
    public interface IBasicSmartPanel : IChildrenHolder
    {
        String Name { get; }
        DependencyObject Parent { get; }
        event Action<FrameworkElement> ChildAdded;
        event Action<FrameworkElement> ChildRemoved;
    }

    public interface IChildrenHolder
    {
        SmartCollection<FrameworkElement> Children { get; set; }
    }

    public interface IDocker : IBasicSmartPanel
    {
        SimpleSubject<ObjectFly> DockingEnter { get; }
        SimpleSubject<ObjectFly> DockingLeave { get; }
        DragPanel FindIntersection(FrameworkElement el);
        DragPanel FindIntersection(Rect place, FrameworkElement el = null);
        DragPanel FindDock(Rect place);
        DragPanel FindDock(FrameworkElement element);
    }


    public interface IDragPanel : IDocker
    {
        MemoryPipe<ObjectFly> Dragged { get; set; }
        void DockChild(ObjectFly fly);
        /*
        DragPanel TopDragPanel { get; set; }       
        DragPanel TopDragPanel { get; set; }        
        event Action<DragPanel, DragPanel> TopParentChange;        
        IDragPanel TopDragPanel { get; set; }        
        event Action<IDragPanel, IDragPanel> TopParentChange;        
         */
    }
}