using System;
using System.Windows;
using System.Windows.Media;
using Smart.Classes.Arguments;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;

namespace Smart.UI.Panels
{
    public enum CanvasUpdateMode
    {
        CanvasWithPanel,
        CanvasSeparate,
        KeepCanvas,
        AutoGrowCanvas,
    }

    /// <summary>
    /// Class devoted to all operations with panel's canvas
    /// including zooming
    /// </summary>
    public class CanvasPlace
    {
        // public ScrollMode ScrollMode;       
        /// <summary>
        /// Last arrange size
        /// </summary>
        public Size ArrangeSize;

        public Rect Canvas;
        public Rect Elements;

        /// <summary>
        /// las measure size
        /// </summary>
        public Size MeasureSize;

        public Rect Panel;

        /// <summary>
        /// UpdateMode of the canvas defines how it behaves on Place and savechanges
        /// </summary>
        public CanvasUpdateMode UpdateMode = CanvasUpdateMode.CanvasWithPanel;

        public CanvasPlace()
        {
            PanelChange = new ActionWithArgs<CanvasPlace, Rect, Rect>((a, b, v) => Panel = v, this, Panel, Panel);
            CanvasChange = new ActionWithArgs<CanvasPlace, Rect, Rect>((a, b, v) => Canvas = v, this, Canvas, Canvas);
        }

        #region ZOOM AND SCALE

        public void Scale(double q)
        {
            CanvasChange.FireChange(new Rect(Canvas.X, Canvas.Y, Canvas.Width*q,
                                             Canvas.Height*q));
        }

        public void GrowCenteredBy(Point delta)
        {
            CanvasChange.FireChange(new Rect(Canvas.X, Canvas.Y, Canvas.Width + delta.X, Canvas.Height + delta.Y));
            PanelChange.FireChange(new Rect(Panel.X + delta.X/2, Panel.Y + delta.Y/2,
                                            Panel.Width, Panel.Height));
        }


        /// <summary>
        /// Zoom Canvas centered
        /// </summary>
        /// <param name="q"></param>
        public void Zoom(double q)
        {
            Size size = Canvas.Size();
            var delta = new Point(size.Width*(q - 1), size.Height*(q - 1));
            ZoomByDelta(delta);
        }

        protected void ZoomByDelta(Point delta)
        {
            CanvasChange.FireChange(new Rect(Canvas.X, Canvas.Y, Canvas.Width + delta.X,
                                             Canvas.Height + delta.Y));
            PanelChange.FireChange(new Rect(Panel.X + delta.X/2, Panel.Y + delta.Y/2, Panel.Width,
                                            Panel.Height));
        }

        /// <summary>
        /// Zooms at particular rectangle
        /// </summary>
        /// <param name="bounds"></param>
        public void ZoomAt(Rect bounds)
        {
            var factor = new Size(Panel.Width/bounds.Width, Panel.Height/bounds.Height);
            CanvasChange.FireChange(new Rect(Canvas.X, Canvas.Y, Canvas.Width*factor.Width,
                                             Canvas.Height*factor.Height));
            PanelChange.FireChange(new Rect((Panel.X + bounds.X)*factor.Width, (Panel.Y + bounds.Y)*factor.Height,
                                            Panel.Width,
                                            Panel.Height));
        }

        public void ZoomAt(Point zoom, Rect bounds)
        {
            Size size = Canvas.Size();
            var factor = new Size(Panel.Width/bounds.Width, Panel.Height/bounds.Height);
            Size newCanvasSize = size.MultiplyBy(factor);
            Point delta = newCanvasSize.Substract(size);
            var q = new Point(zoom.X/factor.Width, zoom.Y/factor.Height);
            var f = new Point(q.X*(factor.Width - 1) + 1, q.Y*(factor.Height - 1) + 1);
            var endShift = new Point(factor.Width*bounds.X, factor.Height*bounds.Y);
            PanelChange.FireChange(new Rect(Panel.X*f.X + endShift.X*q.X, Panel.Y*f.Y + endShift.Y*q.Y,
                                            Panel.Width, Panel.Height));
            CanvasChange.FireChange(new Rect(Canvas.X, Canvas.Y, size.Width + delta.X*q.X,
                                             size.Height + delta.Y*q.Y));
        }


        public Point ToRelativePoint(Point some)
        {
            return new Point(some.X/Panel.Width, some.Y/Panel.Height);
        }

        #endregion

        #region EVENTS

        public ActionWithArgs<CanvasPlace, Rect, Rect> CanvasChange;
        public ActionWithArgs<CanvasPlace, Rect, Rect> PanelChange;


        /*
                /// <summary>
                /// Fires PanelChange
                /// </summary>
                public CanvasPlace FirePanelChange(Rect newVal)
                {
                    if(this._panelChange==null) return this;
                    _panelChange.FireChange()
                    return this;
                }
                 /// <summary>
                /// Fires PanelChange
                /// </summary>
                public CanvasPlace  FireCanvasChange()
                {
                    if(this._canvasChange==null || _canvasChange.Param2.Equals(this.Canvas)) return this;
                    this.CanvasChange.Execute(this, this.CanvasChange.Param2, this.Canvas);
                    return this;
                }
             */

        #endregion

        #region GROW CANVAS

        public CanvasPlace GrowCanvas(Point delta, AlignmentX hor = AlignmentX.Right, AlignmentY ver = AlignmentY.Bottom)
        {
            CanvasChange.FireChange(Canvas.GrowRect(delta, hor, ver));
            return this;
        }

        public CanvasPlace GrowPanel(Point delta, AlignmentX hor = AlignmentX.Right, AlignmentY ver = AlignmentY.Bottom)
        {
            PanelChange.FireChange(Panel.GrowRect(delta, hor, ver));
            return this;
        }

        public CanvasPlace UpdatePanel(Rect rect)
        {
            PanelChange.FireChange(rect);
            return this;
        }

        #endregion

        /// <summary>
        /// Getter that tels if scrollerbars is needed for the panel
        /// </summary>
        public bool Scrollable
        {
            get { return Panel.Width < Canvas.Width || Panel.Height < Canvas.Height; }
        }

        public bool ScrollableVertically
        {
            get { return Panel.Height < Canvas.Height; }
        }

        public bool ScrollableHorizontally
        {
            get { return Panel.Width < Canvas.Width; }
        }

        public double MaxWidth
        {
            get { return Math.Max(Panel.Width, Canvas.Width); }
        }

        public double MaxHeight
        {
            get { return Math.Max(Panel.Height, Canvas.Height); }
        }


        /// <summary>
        /// Getter and setter to set up a canvasshift in relative values to canvassize
        /// Makes thinkgs easier for some tasks, just a syntax sugar
        /// </summary>
        public Point RelativeShift
        {
            get { return new Point(Panel.X/Canvas.Width, Panel.Y/Canvas.Height); }
            set
            {
                Panel.X = value.X*Canvas.Width;
                Panel.Y = value.Y*Canvas.Height;
            }
        }


        /// <summary>
        /// What place the panel holds in relation to canvas (in points)
        /// </summary>
        public Rect RelativeRect
        {
            get
            {
                return new Rect(Panel.X/Canvas.Width, Panel.Y/Canvas.Height, Panel.Width/Canvas.Width,
                                Panel.Height/Canvas.Height);
            }
            set
            {
                PanelChange.FireChange(new Rect(value.X*Canvas.Width, value.Y*Canvas.Height, value.Width*Canvas.Width,
                                                value.Height*Canvas.Height));
            }
        }


        /// <summary>
        /// Update canvas size when measuring
        /// </summary>
        /// <param name="size"></param>
        public CanvasPlace UpdateSizes(Size size)
        {
            Point delta = size.Substract(MeasureSize);
            if (UpdateMode == CanvasUpdateMode.KeepCanvas && Canvas.IsMoreThanNull())
                Panel = Panel.ShiftToPlace(Canvas);
            if (UpdateMode == CanvasUpdateMode.CanvasWithPanel && !Panel.Size().IsNear(size))
            {
                Canvas.Width += delta.X;
                Canvas.Height += delta.Y;
            }
            Panel.Width = size.Width;
            Panel.Height = size.Height;
            Elements = default(Rect);
            SyncCanvasToPanel();

            PanelChange.Refresh(Panel);
            CanvasChange.Refresh(Canvas);
            MeasureSize = size;
            return this;
        }

        public CanvasPlace SyncCanvasToPanel()
        {
            Canvas.Union(Panel); //(new Rect(0,0,Panel.Right+Panel.X,Panel.Bottom+Panel.Y));
            return this;
        }


        /// <summary>
        /// Places a slot rectangle on canvas
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Rect Place(Rect rect)
        {
            if (UpdateMode == CanvasUpdateMode.KeepCanvas) rect = rect.ShiftToPlace(Canvas);
            rect.X -= Panel.X;
            rect.Y -= Panel.Y;
            Elements.Union(rect);
            return rect;
        }

        /// <summary>
        /// Moves visible panel space to coordinates on Canvas
        /// </summary>
        /// <param name="coords"></param>
        /// <returns></returns>
        public CanvasPlace MoveTo(Point coords)
        {
            if (coords == default(Point)) return this;
            Panel.X = coords.X;
            Panel.Y = coords.Y;
            PanelChange.FireChange(new Rect(coords.X, coords.Y, Panel.Width, Panel.Height));
            return this;
        }

        /// <summary>
        /// Shifts visible space to coordinates
        /// </summary>
        /// <param name="shift"></param>
        public void Shift(Point shift)
        {
            if (shift == default(Point)) return;
            PanelChange.FireChange(Panel.Add(shift));
        }

        /// <summary>
        /// Saves changes to panels and canvas positions and sizes
        /// </summary>
        /// <returns></returns>
        public CanvasPlace SaveChanges()
        {
            ArrangeSize = Panel.Size();
            if (UpdateMode != CanvasUpdateMode.AutoGrowCanvas) return this;
            Elements.Union(Panel);
            // var rect = this.Canvas.GetIntersectionRect(this.Elements);
            if (Elements.Equals(Canvas) || !Elements.IsMoreThanNull()) return this;
            Canvas = Elements;
            //this.Canvas.Union(this.Elements);            
            CanvasChange.FireChange(new Rect(Canvas.X, Canvas.Y, Canvas.Width - Canvas.X, Canvas.Height - Canvas.Y));
            PanelChange.FireChange(new Rect(Panel.X - Canvas.X, Panel.Y - Canvas.Y, Panel.Width, Panel.Height));
            return this;
        }
    }
}