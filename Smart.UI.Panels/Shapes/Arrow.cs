using System;
using System.Windows;
using System.Windows.Media;

//#if SILVERLIGHT
namespace Smart.UI.Panels
{
    /// <summary>
    /// Put arrow functionality to another classs for ease of usage
    /// </summary>
    public class Arrow : LinesPath
    {
        #region DEPENDENCY PROPERTIES

        /// <summary>
        /// высота стрелки
        /// </summary>
        public static readonly DependencyProperty HeadHeightProperty =
            DependencyProperty.Register("HeadHeight", typeof (double), typeof (LinesPath), new PropertyMetadata(3.0));

        /// <summary>
        /// длина стрелки
        /// </summary>
        public static readonly DependencyProperty HeadWidthProperty =
            DependencyProperty.Register("HeadWidth", typeof (double), typeof (LinesPath), new PropertyMetadata(7.0));


        /// <summary>
        /// начальная точка стрелки. Если не указана, то будет использоваться левый верхний угол канваса (или того, где может прорисоваться стелка)
        /// </summary>
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof (Point), typeof (LinesPath),
                                        new PropertyMetadata(default(Point)));

        /// <summary>
        /// конечная точка стрелки. Если не указана, то будет показывать на правый нижный угол канваса
        /// </summary>
        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof (Point), typeof (LinesPath),
                                        new PropertyMetadata(default(Point)));

        /// <summary>
        /// показывать стартовую стрелку
        /// </summary>
        public static readonly DependencyProperty ShowStartCapProperty =
            DependencyProperty.Register("ShowStartCap", typeof (bool), typeof (LinesPath), new PropertyMetadata(true));


        /// <summary>
        /// показывать конечную стрелку
        /// </summary>
        public static readonly DependencyProperty ShowEndCapProperty =
            DependencyProperty.Register("ShowEndCap", typeof (bool), typeof (LinesPath), new PropertyMetadata(true));

        public double HeadHeight
        {
            get { return (double) GetValue(HeadHeightProperty); }
            set { SetValue(HeadHeightProperty, value); }
        }

        public double HeadWidth
        {
            get { return (double) GetValue(HeadWidthProperty); }
            set { SetValue(HeadWidthProperty, value); }
        }

        public Point StartPoint
        {
            get { return (Point) GetValue(StartPointProperty); }
            set
            {
                SetValue(StartPointProperty, value);
                InvalidateArrange();
            }
        }

        public Point EndPoint
        {
            get { return (Point) GetValue(EndPointProperty); }
            set
            {
                SetValue(EndPointProperty, value);
                InvalidateArrange();
            }
        }

        public bool ShowStartCap
        {
            get { return (bool) GetValue(ShowStartCapProperty); }
            set
            {
                SetValue(ShowStartCapProperty, value);
                Geometry.Children.Clear();
                InvalidateArrange();
            }
        }

        public bool ShowEndCap
        {
            get { return (bool) GetValue(ShowEndCapProperty); }
            set
            {
                SetValue(ShowEndCapProperty, value);
                Geometry.Children.Clear();
                InvalidateArrange();
            }
        }

        #endregion

        public Arrow()
        {
            Stroke = new SolidColorBrush(Color.FromArgb(255, 73, 97, 141));
            //StrokeThickness = 1;            
            CreateFigure(new Point(0, 0), new Point(0, 0));
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            CreateFigure(StartPoint,
                         (EndPoint.X.Equals(0.0) & EndPoint.Y.Equals(0.0))
                             ? new Point(finalSize.Width, finalSize.Height)
                             : EndPoint);
            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// Не могу понять зачем ты столько линий делаешь, когда можно одной фигурой?
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public void CreateFigure(Point start, Point end)
        {
            // рисуем линию
            MakeLine(start, end, 0);
            // расчеты для стрелки
            double theta = (ShowStartCap || ShowEndCap) ? Math.Atan2(start.Y - end.Y, start.X - end.X) : 0;
            double sint = (ShowStartCap || ShowEndCap) ? Math.Sin(theta) : 0;
            double cost = (ShowStartCap || ShowEndCap) ? Math.Cos(theta) : 0;


            int lineIndex = 1;

            if (ShowEndCap) // если указано, рисуем конечную стрелку
            {
                var pt3 = new Point(
                    end.X + (HeadWidth*cost - HeadHeight*sint),
                    end.Y + (HeadWidth*sint + HeadHeight*cost));

                var pt4 = new Point(
                    end.X + (HeadWidth*cost + HeadHeight*sint),
                    end.Y - (HeadHeight*cost - HeadWidth*sint));
                MakeLine(end, pt3, lineIndex++);
                MakeLine(end, pt4, lineIndex++);
            }
            if (!ShowStartCap) return;
            var pt5 = new Point(
                start.X - (HeadWidth*cost - HeadHeight*sint),
                start.Y - (HeadWidth*sint + HeadHeight*cost)
                );

            var pt6 = new Point(
                start.X - (HeadWidth*cost + HeadHeight*sint),
                start.Y + (HeadHeight*cost - HeadWidth*sint));
            MakeLine(start, pt5, lineIndex++);
            MakeLine(start, pt6, lineIndex++);
        }
    }
}
//#endif