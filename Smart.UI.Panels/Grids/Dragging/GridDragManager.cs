using System;
using System.Windows;
using Smart.UI.Panels;
using Smart.UI.Classes.Animations;

namespace Smart.UI.Panels
{
    public class GridDragManager : DragManager
    {
        public TimeSpan ComeBackTime = new TimeSpan(0, 0, 0, 1);

        public GridDragManager(FlexGrid sender) : base(sender)
        {
        }

        public static void SetRowMover(DependencyObject obj, LineMover value)
        {
            object sub = obj.GetValue(DragSubjectProperty);
            if (sub is GridLinesDragSubject)
                (sub as GridLinesDragSubject).RowMover = value;
            else if (sub is ColumnDragSubject)
                SetDragSubject(obj, new GridLinesDragSubject((sub as ColumnDragSubject).Mover, value));
            else if (sub is RowDragSubject)
                (sub as RowDragSubject).Mover = value;
            else
                obj.SetValue(DragSubjectProperty, new RowDragSubject {Mover = value});
            obj.SetValue(SimplePanel.ElementTypeProperty, ElementType.Adorner);
        }


        public static void SetColumnMover(DependencyObject obj, LineMover value)
        {
            object sub = obj.GetValue(DragSubjectProperty);
            if (sub is GridLinesDragSubject)
                (sub as GridLinesDragSubject).ColumnMover = value;
            else if (sub is RowDragSubject)
                SetDragSubject(obj, new GridLinesDragSubject(value, (sub as RowDragSubject).Mover));
            else if (sub is ColumnDragSubject)
                (sub as ColumnDragSubject).Mover = value;
            else
                obj.SetValue(DragSubjectProperty, new ColumnDragSubject {Mover = value});
            obj.SetValue(SimplePanel.ElementTypeProperty, ElementType.Adorner);
        }

        public override void ComeBack(ObjectFly fly, Rect place)
        {
            if (ComeBackTime == default(TimeSpan))
            {
                base.ComeBack(fly, place);
                return;
            }
            fly.Target.MoveTo(place, ComeBackTime, Easings.CubicEaseInOut).Go().DoOnCompleted += () =>
                                                                                                 Sender.
                                                                                                     RunAterNextLayoutUpdate
                                                                                                     (i =>
                                                                                                      Sender.DockChild(
                                                                                                          fly.
                                                                                                              SetDockMode
                                                                                                              (DockMode.
                                                                                                                   DockEverywhere)));
        }
    }
}