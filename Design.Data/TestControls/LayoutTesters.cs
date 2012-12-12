using System;
using System.Windows;
using System.Windows.Controls;
using Smart.Classes.Subjects;
using Smart.UI.Panels;

namespace Design.Data.TestControls
{
    public interface ILayoutTesterMixin<T> where T : FrameworkElement
    {
        int ArrangeCount { get; set; }
        int MeasureCount { get; set; }
        String MyName { get; }
    }

    public class TestButton : Button, ILayoutTesterMixin<Button>
    {
        #region ILayoutTesterMixin<Button> Members

        public int ArrangeCount { get; set; }
        public int MeasureCount { get; set; }

        public string MyName
        {
            get { return Name; }
        }

        #endregion

        protected override Size MeasureOverride(Size availableSize)
        {
            MeasureCount++;
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            ArrangeCount++;
            return base.ArrangeOverride(finalSize);
        }
    }

    public class TestFlexCanvas : FlexCanvas, ILayoutTesterMixin<FlexCanvas>
    {
        public TestFlexCanvas()
        {
            BeforeLayoutUpdated = new SimpleSubject<Size>();
            AfterLayoutUpdated = new SimpleSubject<Size>();
            LayoutUpdated += LayoutUpdatedHandler;
        }

        public int LayoutCount { get; set; }

        #region ILayoutTesterMixin<FlexCanvas> Members

        public int ArrangeCount { get; set; }
        public int MeasureCount { get; set; }


        public string MyName
        {
            get { return Name; }
        }

        #endregion

        public void ResetAllCounts()
        {
            ArrangeCount = MeasureCount = LayoutCount = 0;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            MeasureCount++;
            return base.MeasureOverride(availableSize);
        }

        public override void LayoutUpdatedHandler(object sender, EventArgs e)
        {
            LayoutCount++;
            base.LayoutUpdatedHandler(sender, e);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            ArrangeCount++;
            return base.ArrangeOverride(finalSize);
        }
    }

    public class TestSmartGrid : SmartGrid, ILayoutTesterMixin<SmartGrid>
    {
        public TestSmartGrid()
        {
            BeforeLayoutUpdated = new SimpleSubject<Size>();
            AfterLayoutUpdated = new SimpleSubject<Size>();
            LayoutUpdated += LayoutUpdatedHandler;
        }

        public int LayoutCount { get; set; }

        #region ILayoutTesterMixin<SmartGrid> Members

        public int ArrangeCount { get; set; }
        public int MeasureCount { get; set; }


        public string MyName
        {
            get { return Name; }
        }

        #endregion

        public void ResetAllCounts()
        {
            ArrangeCount = MeasureCount = LayoutCount = 0;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            MeasureCount++;
            return base.MeasureOverride(availableSize);
        }

        public override void LayoutUpdatedHandler(object sender, EventArgs e)
        {
            LayoutCount++;
            base.LayoutUpdatedHandler(sender, e);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            ArrangeCount++;
            return base.ArrangeOverride(finalSize);
        }
    }
}