using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Smart.UI.Classes.Extensions;
using Smart.UI.Panels;

namespace Design.Data.TestControls
{
    public class TestDragPanel : DragPanel
    {
        protected override void InitDragAndDrop()
        {
            TopParentChange += OnTopParentChange;
            DragPanels.ItemsAdded.Subscribe(p =>
                                                {
                                                    var testDragPanel = p as TestDragPanel;
                                                    if (testDragPanel != null)
                                                        testDragPanel.TopDragPanel = TopDragPanel;
                                                });
            DragPanels.ItemsRemoved.Subscribe(p =>
                                                  {
                                                      var testDragPanel = p as TestDragPanel;
                                                      if (testDragPanel != null)
                                                          testDragPanel.TopDragPanel = p;
                                                  });
            base.InitDragAndDrop();
            InitTestDock();
        }

        public void InitTestDock()
        {
            DockingEnter.Add(fly =>
                                 {
                                     FrameworkElement el = fly.Target;
                                     if (el is Border) (el as Border).Background = Background;
                                 });
            DockingLeave.Add(fly =>
                                 {
                                     FrameworkElement el = fly.Target;
                                     if ((el is Border) && (el as Border).Child == null)
                                         (el as Border).Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                                 });
        }

        #region FIND TOP PANEL нахождение верхней панельки

        private DragPanel _topDragPanel;

        /// <summary>
        /// Наиболее высокая драгабельная панелька
        /// </summary>
        public DragPanel TopDragPanel
        {
            get { return _topDragPanel ?? this; }
            set
            {
                if (value == _topDragPanel) return;
                DragPanel old = TopDragPanel;
                _topDragPanel = value;
                TopParentChange(old, value); //потом поменяю на реактивный                                              
            }
        }


        public Boolean IsOnTop
        {
            get { return TopDragPanel == this; }
        }

        public event Action<DragPanel, DragPanel> TopParentChange;

        protected virtual void OnTopParentChange(DragPanel oldValue, DragPanel newValue)
        {
            foreach (DragPanel dragPanel in DragPanels) (dragPanel as TestDragPanel).TopDragPanel = newValue;
        }

        public Rect GetBoundsTop()
        {
            return this.GetRelativeRect(TopDragPanel);
        }


        public Rect GetTopParentBounds(FrameworkElement element)
        {
            return element.GetRelativeRect(TopDragPanel);
        }

        #endregion
    }
}