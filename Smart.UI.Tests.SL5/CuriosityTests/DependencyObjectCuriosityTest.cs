/*
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Graphs;
using Smart.UI.Graphs.Traversals;
using Smart.UI.Graphs.VisualNodes;
using Smart.TestExtensions;


namespace Smart.UI.Tests.CuriosityTests
{
    public class TestVisualEdge : VisualEdge
    {
        private SearchInfo _searchInfo = default(SearchInfo);
        public SearchInfo SearchInfo
        {
            get { return _searchInfo; }
            set { _searchInfo = value; }
        }

    }

    [TestClass]
    public class DependencyObjectCuriosityTest:CuriosityTestBase
    {
        public List<FrameworkElement> Elements;
        public List<TestVisualEdge> Edges;
        public int Count = 1000;
        

        public static readonly DependencyProperty SearchInfoProperty =
            DependencyProperty.RegisterAttached("SearchInfo", typeof (SearchInfo), typeof (FrameworkElement), new PropertyMetadata(default(SearchInfo)));

        public static SearchInfo GetSearchInfo(FrameworkElement element)
        { return (SearchInfo)element.GetValue(SearchInfoProperty); }

        public static void SetSearchInfo(FrameworkElement element, SearchInfo value)
        { element.SetValue(SearchInfoProperty, value); }

        [TestInitialize]
        public virtual void SetUp()
        {
            this.Elements = new List<FrameworkElement>();
            this.Edges = new List<TestVisualEdge>();
            for (int i = 0; i < Count; i++)
            {
                this.Edges.Add(new TestVisualEdge());
                this.Elements.Add(new Button());
            }
        }

        [TestCleanup]
        public virtual void CleanUp()
        {
            this.Edges = null;
            this.Elements = null;
        }

        [Ignore]
        [TestMethod]
        public void TestWriteAndRead()
        {

            var t0 = this.Measure(() => { foreach (var e in Edges) e.SearchInfo = new SearchInfo() { Discovered = 1 }; }, "Property");           
            var t1 = this.Measure(() => { foreach (var e in Edges) e.SetData(new SearchInfo() {Discovered = 1}); },"SetData");
            var t2 = this.Measure(() => { foreach (var e in Elements) SetSearchInfo(e,new SearchInfo() { Discovered = 1 }); }, "Set DP");
            (t1.TotalMilliseconds < t2.TotalMilliseconds).ShouldBeTrue();
            MessageBox.Show("SetData: " + t1.ToString() + " | Set DP: " + t2.ToString()+" SetData is "+t2.TotalMilliseconds/t1.TotalMilliseconds+" times faster");
            MessageBox.Show("Original property is "+t2.TotalMilliseconds/t0.TotalMilliseconds+" times faster than DP");
            var t3 = this.Measure(() => { foreach (var e in Edges) { var tmp = e.SearchInfo; } }, "Property");                       
            var t4 = this.Measure(() => { foreach (var e in Edges) {var tmp = e.GetById<SearchInfo>(e.Id);} }, "GetData");
            var t5 = this.Measure(() => { foreach (var e in Elements) { var tmp = GetSearchInfo(e); } }, "Get DP");            
         //   (t3.TotalMilliseconds < t4.TotalMilliseconds).ShouldBeFalse();
            MessageBox.Show("GetData: " + t4.ToString() + " | Get DP: " + t5.ToString() + " GetData is " + t5.TotalMilliseconds / t4.TotalMilliseconds + " times faster");
            MessageBox.Show("Original property is " + t5.TotalMilliseconds / t3.TotalMilliseconds + " faster than DP and"+
                t4.TotalMilliseconds / t3.TotalMilliseconds + " faster than Get Data"
                );
            
            
        }
    }
}
*/