using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Widgets.PanelAdorners
{
    public class WidgetAdorner : CanvasAdorner<UI.Widgets.WidgetGrid>
    {
        public static readonly DependencyProperty FreeSpaceBrushProperty =
            DependencyProperty.Register("FreeSpaceBrush", typeof (Brush), typeof (WidgetAdorner),
                                        new PropertyMetadata(new SolidColorBrush(Colors.Green) {Opacity = 0.5}));

        public static readonly DependencyProperty OccupiedSpaceBrushProperty =
            DependencyProperty.Register("OccupiedSpaceBrush", typeof (Brush), typeof (WidgetAdorner),
                                        new PropertyMetadata(new SolidColorBrush(Colors.Red) {Opacity = 0.5}));

        public Dictionary<ObjectFly, SmartCollection<Rectangle>> Rects =
            new Dictionary<ObjectFly, SmartCollection<Rectangle>>();

        public WidgetAdorner()
        {
            Rects = new Dictionary<ObjectFly, SmartCollection<Rectangle>>();
        }

        public Brush FreeSpaceBrush
        {
            get { return (Brush) GetValue(FreeSpaceBrushProperty); }
            set { SetValue(FreeSpaceBrushProperty, value); }
        }

        public Brush OccupiedSpaceBrush
        {
            get { return (Brush) GetValue(OccupiedSpaceBrushProperty); }
            set { SetValue(OccupiedSpaceBrushProperty, value); }
        }

        public override void Activate()
        {
            base.Activate();
            Panel.DockingEnter.Add(AddRects);
            Panel.UnderFlight.Add(OnMoveRects);
            Panel.DockingLeave.Add(RemoveRects);
        }

        protected void OnMoveRects(ObjectFly fly)
        {
            SmartCollection<Rectangle> rcs = Rects[fly];
            foreach (Rectangle rectangle in rcs) Panel.RemoveChild(rectangle);
            Rect bounds = fly.Target.GetRelativeRect(Panel);
            CellsRegion cells = Panel.MeasureCellsRegion(fly.Target, bounds);
            IEnumerable<FrameworkElement> elements =
                Panel.ChildrenInPlace<FrameworkElement>(bounds).Where(i => i != fly.Target);
            rcs.MakeNum(cells.Count);
            IEnumerator<Rectangle> e = rcs.GetEnumerator();
            e.Reset();

            for (int i = cells.Col; i < cells.RightCol; i++)
            {
                for (int j = cells.Row; j < cells.BottomRow; j++)
                {
                    e.MoveNext();
                    Rectangle rc = e.Current;
                    rc.Fill = elements.Where(el => el.CheckInCells(i, j)).ToCollection().Count > 0
                                  ? OccupiedSpaceBrush
                                  : FreeSpaceBrush;
                    rc.SetColumn(i).SetRow(j);
                }
            }
            foreach (Rectangle rectangle in rcs) Panel.AddChild(rectangle);
        }


        protected void AddRects(ObjectFly fly)
        {
            if (Rects.ContainsKey(fly)) return;
            Rects.Add(fly, new SmartCollection<Rectangle>());
        }

        protected void RemoveRects(ObjectFly fly)
        {
            if (!Rects.ContainsKey(fly)) return;
            foreach (Rectangle rect in Rects[fly])
            {
                Panel.RemoveChild(rect);
            }
            Rects.Remove(fly);
        }
    }
}