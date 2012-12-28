using System.Windows;
using System.Windows.Data;
using Microsoft.Silverlight.Testing.UnitTesting.Metadata.VisualStudio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Bindables;
using Smart.UI.Classes;
using Smart.UI.Classes.Other;
using Smart.Classes.Reflectives;
using Smart.UI.Tests.PanelsTests;
using Smart.TestExtensions;
using Smart.UI.Tests.TestBases;
using Smart.UI.Widgets;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Tests.BindingTests
{


    [TestClass]
    public class BindingTest :PanelTestBase<WidgetGrid>
    {
        public Binding Binding;
        public DependencyClass DependencyObj;
        public MagicClass MagicObject;
        //   public 

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.Binding = new Binding();
            this.MagicObject = new MagicClass();
            this.DependencyObj = new DependencyClass();
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
            this.Binding = null;
            this.MagicObject = null;
            this.DependencyObj = null;
        }

        [TestMethod]
        public void RowsColsNumTest()
        {
            this.Panel.RowsNum.ShouldBeEqual(0);
            this.Panel.ColsNum.ShouldBeEqual(0);
            this.MagicObject.Rows.Value = 2;
            this.MagicObject.Cols = 4;
            this.Panel.SetBinding(WidgetGrid.ColsNumProperty,this.MagicObject.Bind2MyProp(b => b.Cols,BindingMode.TwoWay));                      
            this.Panel.SetBinding(WidgetGrid.RowsNumProperty,this.MagicObject.Bind2MyPath(b => b.GetProperty(i => i.Rows).GetChildProperty(i=>i.Value),BindingMode.TwoWay));
         
            this.Panel.RowsNum.ShouldBeEqual(2);
            this.Panel.ColsNum.ShouldBeEqual(4);
            //this.Panel.RowDefinitions.Count.ShouldBeEqual(2);
            //this.Panel.ColumnDefinitions.Count.ShouldBeEqual(4);
            this.MagicObject.Rows.Value = 10;
            this.MagicObject.Cols = 10;
            this.Panel.RowsNum.ShouldBeEqual(10);
            this.Panel.ColsNum.ShouldBeEqual(10);
       //     this.Panel.RowDefinitions.Count.ShouldBeEqual(10);
        //    this.Panel.ColumnDefinitions.Count.ShouldBeEqual(10);            
        }

     



        [TestMethod]
        public void SimpleBindingTest()
        {
            this.MagicObject.BindMe.ShouldBeEqual("");
            this.MagicObject.BindMe = "1";
            this.MagicObject.BindMe.ShouldBeEqual("1");
            this.Binding.Mode = BindingMode.TwoWay;            
            this.Binding.Source = this.MagicObject;
            this.Binding.Path = new PropertyPath(this.MagicObject.GetPropertyName(m=>m.BindMe));
            BindingOperations.SetBinding(this.DependencyObj, DependencyClass.StrProperty, this.Binding);
            this.DependencyObj.Str.ShouldBeEqual("1");
            this.MagicObject.BindMe = "2";
            this.DependencyObj.Str.ShouldBeEqual("2");
            this.DependencyObj.Str = "3d!";
            this.MagicObject.BindMe.ShouldBeEqual("3d!");
        }

        [TestMethod]
        public void Bind2MyPropTest()
        {
            this.MagicObject.BindMe.ShouldBeEqual("");
            this.MagicObject.BindMe = "1";
            this.MagicObject.BindMe.ShouldBeEqual("1");          
            BindingOperations.SetBinding(this.DependencyObj, DependencyClass.StrProperty, this.MagicObject.Bind2MyProp(p=>p.BindMe,BindingMode.TwoWay));
            this.DependencyObj.Str.ShouldBeEqual("1");
            this.MagicObject.BindMe = "2";
            this.DependencyObj.Str.ShouldBeEqual("2");
            this.DependencyObj.Str = "3d!";
            this.MagicObject.BindMe.ShouldBeEqual("3d!");
        }

        [TestMethod]
        public void Bind2MyPathTest()
        {
            this.MagicObject.BindMe.ShouldBeEqual("");
            this.MagicObject.BindMe = "1";
            this.MagicObject.BindMe.ShouldBeEqual("1");
            this.MagicObject.Child = new MagicClass();
            this.MagicObject.Child.BindMe = "1";
            BindingOperations.SetBinding(this.DependencyObj, DependencyClass.StrProperty, this.MagicObject.Bind2MyPath(p=>p.GetProperty(i=>i.Child).GetProperty(s=>s.BindMe),BindingMode.TwoWay));
            this.DependencyObj.Str.ShouldBeEqual("1");
            this.MagicObject.BindMe = "2";
            this.DependencyObj.Str.ShouldBeEqual("1");
            this.MagicObject.Child.BindMe = "2";
            this.DependencyObj.Str.ShouldBeEqual("2");            
            this.DependencyObj.Str = "3d!";
            this.MagicObject.BindMe.ShouldBeEqual("2");
            this.MagicObject.Child.BindMe.ShouldBeEqual("3d!");
        }

    }

    public class DependencyClass : DependencyObject
    {
        public static readonly DependencyProperty StrProperty =
            DependencyProperty<DependencyClass>.Register(d => d.Str, "default");

        public string Str
        {
            get { return (string) GetValue(StrProperty); }
            set { SetValue(StrProperty, value); }
        }
    }



    [Magic]
    public class MagicClass:Bindable
    {
        
        public MinMax<int> Rows { get; set; }
        public int Cols { get; set; }
        public string BindMe { get; set; }
        public MagicClass Child { get; set; }

        public MagicClass()
        {
            this.BindMe = "";
            this.Rows = new MinMax<int>(2,0,10);
        }
    }
}
