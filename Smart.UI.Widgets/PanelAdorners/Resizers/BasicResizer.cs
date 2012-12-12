using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Smart.Classes.Collections;
using Smart.Classes.Extensions;

namespace Smart.UI.Widgets.PanelAdorners
{
    public class BasicResizer : CanvasAdorner //<T> where T:FlexCanvas
    {
        #region DEPENDENCY PROPERTIES

        /// <summary>
        /// Размеры уголков
        /// </summary>
        public static readonly DependencyProperty CornerSizeProperty =
            DependencyProperty.Register("CornerSize", typeof (Size), typeof (FrameworkElement),
                                        new PropertyMetadata(new Size(15, 15)));

        /// <summary>
        /// дефолтный канвас зетиндекс
        /// </summary>
        public static int DefZindex = 1000000;

        /// <summary>
        /// Заливка линии ресайза
        /// </summary>
        public static readonly DependencyProperty LinesFillProperty =
            DependencyProperty.Register("LinesFill", typeof (Brush), typeof (Resizer),
                                        new PropertyMetadata(new SolidColorBrush(Colors.LightGray)));

        public static readonly DependencyProperty CornersFillProperty =
            DependencyProperty.Register("CornersFill", typeof (Brush), typeof (Resizer),
                                        new PropertyMetadata(new SolidColorBrush(Colors.Purple)));

        /// <summary>
        /// Прозрачность линии ресайза
        /// </summary>
        public static readonly DependencyProperty LinesOpacityProperty =
            DependencyProperty.Register("LinesOpacity", typeof (double), typeof (Resizer), new PropertyMetadata(0.4));

        /// <summary>
        /// Толщина линий ресайза
        /// </summary>
        public static readonly DependencyProperty LinesThicknessProperty =
            DependencyProperty.Register("LinesThickness", typeof (Thickness), typeof (FrameworkElement),
                                        new PropertyMetadata(new Thickness(5, 5, 5, 5)));


        public static readonly DependencyProperty TargetNameProperty =
            DependencyProperty.Register("TargetName", typeof (String), typeof (Resizer),
                                        new PropertyMetadata(default(String)));

        public Size CornerSize
        {
            get { return (Size) GetValue(CornerSizeProperty); }
            set
            {
                SetValue(CornerSizeProperty, value);
                foreach (RectangleAndEllipseBoundary boundary in Boundaries) boundary.CornerSize = value;
            }
        }

        public Brush LinesFill
        {
            get { return (Brush) GetValue(LinesFillProperty); }
            set
            {
                if (value == LinesFill) return;
                SetValue(LinesFillProperty, value);
                foreach (RectangleAndEllipseBoundary boundary in Boundaries) boundary.LinesFill = value;
            }
        }

        public Brush CornersFill
        {
            get { return (Brush) GetValue(CornersFillProperty); }
            set
            {
                if (value == CornersFill) return;
                SetValue(CornersFillProperty, value);
                foreach (RectangleAndEllipseBoundary boundary in Boundaries) boundary.CornersFill = value;
            }
        }

        public double LinesOpacity
        {
            get { return (double) GetValue(LinesOpacityProperty); }
            set
            {
                if (value.Equals(LinesOpacity)) return;
                SetValue(LinesOpacityProperty, value);
                foreach (RectangleAndEllipseBoundary boundary in Boundaries) boundary.LinesOpacity = value;
            }
        }

        public Thickness LinesThickness
        {
            get { return (Thickness) GetValue(LinesThicknessProperty); }
            set
            {
                if (LinesThickness.Equals(value)) return;
                SetValue(LinesThicknessProperty, value);
                foreach (RectangleAndEllipseBoundary boundary in Boundaries) boundary.LinesThickness = value;
            }
        }

        public String TargetName
        {
            get { return (String) GetValue(TargetNameProperty); }
            set
            {
                SetValue(TargetNameProperty, value);
                if (Host == null) return;
            }
        }


        protected FrameworkElement UpdateTargetByName()
        {
            Target = Host.Children.FirstOrDefault(child => child.Name == TargetName) ?? Host;
            return Target;
        }

        #endregion

        public SmartCollection<RectangleAndEllipseBoundary> Boundaries;
        protected FrameworkElement _target;


        public BasicResizer()
        {
            Boundaries = new SmartCollection<RectangleAndEllipseBoundary> {new RectangleAndEllipseBoundary()};
        }

        public virtual FrameworkElement Target
        {
            get { return _target ?? Host; }
            set
            {
                if (value == Target) return;
                _target = value;
                // _target.UpdateArrange();
                if (Host != null) Host.InvalidateArrange();
            }
        }

        public RectangleAndEllipseBoundary AddBoundary(FrameworkElement target)
        {
            RectangleAndEllipseBoundary boundary = Boundaries.FirstOrDefault(b => b.Activated == false) ??
                                                   Boundaries.AddElement(new RectangleAndEllipseBoundary());
            FillBoundary(boundary).Activate(Host, target);
            return boundary;
        }


        public RectangleAndEllipseBoundary BoundaryByTarget(FrameworkElement target)
        {
            return Boundaries.FirstOrDefault(b => b.Target.Equals(target));
        }

        public void DeactivateBoundaryByTarget(FrameworkElement target)
        {
            foreach (RectangleAndEllipseBoundary boundary in Boundaries.Where(b => b.Target == target).ToArray())
                boundary.Deactivate();
        }


        public void RemoveBoundaryByTarget(FrameworkElement target)
        {
            foreach (RectangleAndEllipseBoundary boundary in Boundaries.Where(b => b.Target == target).ToArray())
            {
                boundary.Deactivate();
                Boundaries.Remove(boundary);
            }
        }


        protected RectangleAndEllipseBoundary FillBoundary(RectangleAndEllipseBoundary boundary)
        {
            boundary.Host = Host;
            boundary.LinesThickness = LinesThickness;
            boundary.LinesFill = LinesFill;
            boundary.CornerSize = CornerSize;
            boundary.CornersFill = CornersFill;
            boundary.LinesOpacity = LinesOpacity;
            return boundary;
        }


        /// <summary>
        /// По добавлению адорнера на панельку
        /// </summary>
        public override void Activate()
        {
            if (_target == null && TargetName != "") UpdateTargetByName();
            Host.DragEnabled = true;
            foreach (RectangleAndEllipseBoundary boundary in Boundaries)
            {
                FillBoundary(boundary).Activate(Host, Target);
                boundary.AddChildren();
            }
            base.Activate();
        }

        /*
        protected override void OnBeforeArrange(Size constrains)
        {
            base.OnBeforeArrange(constrains);
            foreach (var boundary in Boundaries) boundary.Pos(constrains);
        }
        */


        /// <summary>
        /// По удалению адорнера с панельки
        /// </summary>
        public override void Deactivate()
        {
            foreach (RectangleAndEllipseBoundary boundary in Boundaries) boundary.RemoveChildren();
            base.Deactivate();
        }
    }
}