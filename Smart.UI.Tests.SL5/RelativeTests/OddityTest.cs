
using System;
using System.Windows;
using Design.Data.TestControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.UI.Relatives;
using Smart.Classes.Arguments;
using Smart.TestExtensions;
using Smart.UI.Tests.PanelsTests;

namespace Smart.UI.Tests.RelativeTests
{
   
   [TestClass]
   public class OddityTest:GridTestBase<TestSmartGrid>
   {
       public int Q 
       { get; set; }
       public Action<Args<FrameworkElement, Rect, Size>> Left;
       public Action<Args<FrameworkElement, Rect, Size>> Right;


       public override void SetUp()
       {
           base.SetUp();
           RelativeConverter.OddActions = true;
           RelativeConverter.OddActions.ShouldBeTrue();
           this.Left = ToOdityAction("left");
           this.Right = ToOdityAction("right");
           this.Grids.ResetAllCounts();
       }

       public override void CleanUp()
       {
           base.CleanUp();
           Q = 0;
       }
        
       public Action<Args<FrameworkElement, Rect, Size>> ToOdityAction(String source)
       {
           var action = RelativeConverter.Instance.ConvertFrom(source);
           var cap = false;
           Action<Args<FrameworkElement, Rect, Size>> retval =
               delegate(Args<FrameworkElement, Rect, Size> args)
               {
                   cap = !cap;
                   if (cap == false) return;
                   action(args, this.Cells[0,0]);
                   Q++;
               };
           return retval;
       }

        
       public Action<Args<FrameworkElement, Rect, Size>> EmptyOdityAction()
       {
           var cap = false;

           Action<Args<FrameworkElement, Rect, Size>,FrameworkElement> action = delegate { Q++; };
           Action<Args<FrameworkElement, Rect, Size>> retval =
               delegate(Args<FrameworkElement, Rect, Size> args)
               {
                   cap = !cap;
                   if (cap==false) return;
                   action(args, this.Cells[0, 0]);
                 //  Q++;
               };
           return retval;
       }

       [TestMethod]
       public void OdityCheckTest()
       {
           Q.ShouldBeEqual(0);
           Left(new Args<FrameworkElement, Rect, Size>(Cell, new Rect(100, 100, 100, 100), new Size(1000, 1000)));
           Q.ShouldBeEqual(1);
           Left(new Args<FrameworkElement, Rect, Size>(Cell, new Rect(100, 100, 100, 100), new Size(1000, 1000)));
           Q.ShouldBeEqual(1);
           Left(new Args<FrameworkElement, Rect, Size>(Cell, new Rect(100, 100, 100, 100), new Size(1000, 1000)));
           Q.ShouldBeEqual(2);
           Left(new Args<FrameworkElement, Rect, Size>(Cell, new Rect(100, 100, 100, 100), new Size(1000, 1000)));
           Q.ShouldBeEqual(2);

       }

       [TestMethod]
       public void OdityCheck2FuncTest()
       {
           Q.ShouldBeEqual(0);
           Left(new Args<FrameworkElement, Rect, Size>(Cell, new Rect(100, 100, 100, 100), new Size(1000, 1000)));
           Q.ShouldBeEqual(1);
           Right(new Args<FrameworkElement, Rect, Size>(Cell, new Rect(100, 100, 100, 100), new Size(1000, 1000)));
           Q.ShouldBeEqual(2);            
           Left(new Args<FrameworkElement, Rect, Size>(Cell, new Rect(100, 100, 100, 100), new Size(1000, 1000)));
           Q.ShouldBeEqual(2);
           Right(new Args<FrameworkElement, Rect, Size>(Cell, new Rect(100, 100, 100, 100), new Size(1000, 1000)));
           Q.ShouldBeEqual(2);
           Left(new Args<FrameworkElement, Rect, Size>(Cell, new Rect(100, 100, 100, 100), new Size(1000, 1000)));
           Q.ShouldBeEqual(3);
           Right(new Args<FrameworkElement, Rect, Size>(Cell, new Rect(100, 100, 100, 100), new Size(1000, 1000)));
           Q.ShouldBeEqual(4);

       }

       
       [TestMethod]
       public void OdityCheckArrangeOverride()
       {                     
           Cell.InvalidateMeasure();
           Grids.InvalidateMeasure();
           Cells[0,0].InvalidateMeasure();
           TestPanel.UpdateLayout();
           this.Grids.LayoutCount.ShouldBeEqual(1);
           this.Grids.MeasureCount.ShouldBeEqual(1);
           this.Grids.ArrangeCount.ShouldBeEqual(1);

           this.Grids.ResetAllCounts();

           this.Grids.LayoutCount.ShouldBeEqual(0);
           this.Grids.MeasureCount.ShouldBeEqual(0);
           this.Grids.ArrangeCount.ShouldBeEqual(0);

           BasicSmartPanel.SetOnArrange(this.Cell,EmptyOdityAction());
           this.Q.ShouldBeEqual(0);
           Grids.InvalidateMeasure();
           this.Q.ShouldBeEqual(0);            
           TestPanel.UpdateLayout();
           this.Grids.LayoutCount.ShouldBeEqual(1);
           this.Grids.MeasureCount.ShouldBeEqual(1);
           this.Grids.ArrangeCount.ShouldBeEqual(1);

           this.Q.ShouldBeEqual(1);
           Grids.InvalidateMeasure();            
           TestPanel.UpdateLayout();
           this.Q.ShouldBeEqual(1);
           this.Grids.LayoutCount.ShouldBeEqual(2);
           this.Grids.MeasureCount.ShouldBeEqual(2);
           this.Grids.ArrangeCount.ShouldBeEqual(2);

       }

   }
}


