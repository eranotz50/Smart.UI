using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Smart.UI.Panels;

namespace Smart.UI.Widgets.PanelAdorners
{
    public class RectangleAndEllipseBoundary : Boundary<Rectangle, Ellipse>
    {
        #region PROPERTIES

        private Size _cornerSize = new Size(15, 15);
        private Brush _cornersFill = new SolidColorBrush(Colors.Blue);


        private Brush _linesFill = new SolidColorBrush(Colors.Green);

        /// <summary>
        /// Размеры уголков
        /// </summary>
        public Size CornerSize
        {
            get { return _cornerSize; }
            set
            {
                _cornerSize = value;
                UpdateCornersSizes();
            }
        }

        /// <summary>
        /// Заливка линии ресайза
        /// </summary>
        public Brush LinesFill
        {
            get { return _linesFill; }
            set
            {
                if (value == _linesFill) return;
                _linesFill = value;
                if (Sides != null) PaintSides();
            }
        }

        public Brush CornersFill
        {
            get { return _cornersFill; }
            set
            {
                if (value == CornersFill) return;
                _cornersFill = value;
                if (Corners != null) PaintCorners();
            }
        }

        #endregion

        public override void Activate(FlexCanvas host, FrameworkElement target = null)
        {
            base.Activate(host, target);
            PaintSides();
            UpdateThickness();
            PaintCorners();
            UpdateCornersSizes();
        }

        public override void PaintSides()
        {
            foreach (Rectangle side in Sides)
            {
                side.Fill = LinesFill;
                side.Opacity = LinesOpacity;
            }
        }

        public void PaintCorners()
        {
            foreach (Ellipse corner in Corners)
            {
                corner.Fill = CornersFill;
            }
        }


        /// <summary>
        /// Устанавливаем размеры уголков
        /// </summary>
        protected void UpdateCornersSizes()
        {
            foreach (Ellipse corner in Corners)
            {
                corner.Width = CornerSize.Width;
                corner.Height = CornerSize.Height;
            }
        }
    }
}