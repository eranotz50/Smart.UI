using System.Reactive.Concurrency;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Collections;


namespace Smart.UI.Tests.PanelsTests
{
    /*
    [Ignore]
    [TestClass]
    public class LinesPanelTest:SilverlightTest
    {
        protected double[] CommonWidth;
        protected double[] CommonHeight;
        protected Size Constraints;
        public SmartCollection<FrameworkElement> Btns;
        public SmartCollection<FrameworkElement> BtnsV;
        public LinesPanel WrapPanel;
        public LinesPanel WrapPanelVertical;

        public int Count = 10;        
        public SmartCollection<SmartCollection<FrameworkElement>> Lines;
        public SmartCollection<SmartCollection<FrameworkElement>> LinesVertical;

        [TestInitialize]        
        public void SetUp()
        {
            Count = 10;
            Btns = new SmartCollection<FrameworkElement>();
            for (int i = 0; i < Count; i++)
            {
                Btns.Add(new Button());
                Btns[i].Width = 10*i + 100;
                Btns[i].Height = 10*i + 100;
            }
 
            BtnsV = new SmartCollection<FrameworkElement>();
            for (int i = 0; i < Count; i++)
            {
                BtnsV.Add(new Button());
                BtnsV[i].Width = 10 * i + 101;
                BtnsV[i].Height = 10 * i + 101;
            }

            Constraints = new Size(500, 500);

            WrapPanel = new LinesPanel { Width = Constraints.Width, Height = Constraints.Height };
            //foreach (var button in Btns) WrapPanel.ItemsSource.Add(button);
           

            this.TestPanel.Children.Add(WrapPanel);
            TestPanel.UpdateLayout();
            WrapPanel.ItemsSource = Btns;

            WrapPanel.Splitter.SplitToLines(Constraints, Scheduler.CurrentThread, false, WrapPanel.ItemsSource);
            Setup2Phase(); 


            // теперь разобьем по-вертикали

            WrapPanelVertical = new LinesPanel { Width = Constraints.Width, Height = Constraints.Height };
            //foreach (var button in BtnsV) WrapPanelVertical.Children.Add(button);
           
            this.TestPanel.Children.Add(WrapPanelVertical);
            TestPanel.UpdateLayout();
            WrapPanelVertical.ItemsSource = BtnsV;          
            WrapPanelVertical.Splitter.SplitToLines(Constraints, Scheduler.CurrentThread, true, WrapPanelVertical.ItemsSource);
            Setup3Phase();
           

         

        }

        void Setup2Phase()
        {
            Lines = WrapPanel.Splitter.TotalLines;
            // общая изначальная длина цепочки контролов для каждой строки
            CommonWidth = new double[Lines.Count];
            for (int i = 0; i < Lines.Count; i++)
            {
                var w = 0.0;
                foreach (var frameworkElement in Lines[i])
                {
                    w += frameworkElement.Margin.Left;
                    w += frameworkElement.Width;
                    w += frameworkElement.Margin.Right;
                }
                CommonWidth[i] = w;
            }
        }

         void Setup3Phase()
         {
             LinesVertical = WrapPanelVertical.Splitter.TotalLines;
             // общая изначальная длина цепочки контролов для каждой колонки
             CommonHeight = new double[LinesVertical.Count];
             for (int i = 0; i < LinesVertical.Count; i++)
             {
                 var w = 0.0;
                 foreach (var frameworkElement in LinesVertical[i])
                 {
                     w += frameworkElement.Margin.Top;
                     w += frameworkElement.Height;
                     w += frameworkElement.Margin.Bottom;
                 }
                 CommonHeight[i] = w;
             }
         }

        [TestCleanup]
        public void CleanUp()
        {
            TestPanel.Children.Remove(WrapPanel);
            TestPanel.Children.Remove(WrapPanelVertical);
            WrapPanel = null;        
            Btns.Clear();
            Btns = null;
            Lines = null;
            CommonWidth = null;

            WrapPanelVertical = null; 
            BtnsV.Clear();
            BtnsV = null;
            LinesVertical = null;
            CommonHeight = null;

            Constraints = default(Size);
           
        }

        /// <summary>
        /// Тестируем правильность разбиения на строки
        /// </summary>
        //[Ignore]
        [TestMethod]
        public void SplitterTest()
        {
            
            Lines = WrapPanel.Splitter.TotalLines;
            Lines.Count.ShouldBeEqual(4);
            Lines[0].Count.ShouldBeEqual(4);
            Lines[1].Count.ShouldBeEqual(3);
            Lines[2].Count.ShouldBeEqual(2);
            Lines[3].Count.ShouldBeEqual(1);
        }

        [TestMethod]
        public void HorizontalLeftAlignTest()
        {
            double offset = 0.0;
            for (int i = 0; i < Lines.Count; i++)
            {
                var Al = new LinesPanelAlignment(offset, Lines[i], Constraints, Orientation.Horizontal,
                                                 HorizontalAlignmentPosition.Left);
                var rect = Al.Align();
                // проверяем правильность алайна
                // длина цепочки контролов, расположенных друг за другом должна соответствовать оригинальной длине всех контролов                                
                rect.Width.ShouldBeEqual(CommonWidth[i]);
                // отступ слева должен быть равен нулю
                rect.X.ShouldBeEqual(0.0);
                // расстояние от правого края канваса НЕ должно равняться нулю, то есть должен быть отступ 
                //(возможно случайное совпадение длин контролов и ширины канваса, так что может и вывалиться здесь, но для проверки пойдет)
                (Constraints.Width - rect.X - rect.Width).ShouldNotBeEqual(0.0);
                // сдвигаемся вниз на высоту строки
                offset += rect.Height;
            }
        }
        
        [TestMethod]
        public void HorizontalRightAlignTest()
        {

            double offset = 0.0;
            for (int i = 0; i < Lines.Count; i++)
            {
                var Al = new LinesPanelAlignment(offset, Lines[i], Constraints, Orientation.Horizontal,
                                                 HorizontalAlignmentPosition.Right);
                var rect = Al.Align();
                // проверяем правильность алайна
                // длина цепочки контролов, расположенных друг за другом должна соответствовать оригинальной длине всех контролов                                
                rect.Width.ShouldBeEqual(CommonWidth[i]);
                // расстояние от правого края канваса должно равняться нулю, то есть не должно быль отступа
                (Constraints.Width - rect.X - rect.Width).ShouldBeEqual(0.0);
                // левый край должен быть правильным
                rect.X.ShouldBeEqual(Constraints.Width - CommonWidth[i]);
                // сдвигаемся вниз на высоту строки
                offset += rect.Height;
            }
        }

        [TestMethod]
        public void HorizontalCenterAlignTest()
        {

            double offset = 0.0;
            for (int i = 0; i < Lines.Count; i++)
            {
                var Al = new LinesPanelAlignment(offset, Lines[i], Constraints, Orientation.Horizontal,
                                                 HorizontalAlignmentPosition.Middle);
                var rect = Al.Align();
                // проверяем правильность алайна
                // длина цепочки контролов, расположенных друг за другом должна соответствовать оригинальной длине всех контролов                                
                rect.Width.ShouldBeEqual(CommonWidth[i]);
                // расстояние от правого края канваса НЕ должно равняться нулю
                (Constraints.Width - rect.X - rect.Width).ShouldNotBeEqual(0.0);
                // отступ слева не должен равен быть нулю
                rect.X.ShouldNotBeEqual(0.0);
                // отступ с левого края и с правого должны быть одинаковыми
                rect.X.ShouldBeEqual(Constraints.Width - rect.X - rect.Width);
                // сдвигаемся вниз на высоту строки
                offset += rect.Height;
            }

        }

        [TestMethod]
        public void HorizontalJustifiedAlignTest()
        {


            double offset = 0.0;
            for (int i = 0; i < Lines.Count; i++)
            {
                var Al = new LinesPanelAlignment(offset, Lines[i], Constraints, Orientation.Horizontal,
                                                 HorizontalAlignmentPosition.Justified);
                var rect = Al.Align();
                // проверяем правильность алайна
                // длина цепочки контролов, расположенных друг за другом НЕ должна соответствовать оригинальной длине всех контролов                                
                if (Lines[i].Count > 1)
                rect.Width.ShouldNotBeEqual(CommonWidth[i]);
                // расстояние от правого края канваса должно равняться нулю, если контролов больше 1
                if (Lines[i].Count > 1)
                    (Constraints.Width - rect.X - rect.Width).ShouldBeEqual(0.0);
                // отступ слева должен равен быть нулю
                rect.X.ShouldBeEqual(0.0);
                // отступ с левого края и с правого должны быть одинаковыми, если контролов больше 1
                if (Lines[i].Count > 1)
                    rect.X.ShouldBeEqual(Constraints.Width - rect.X - rect.Width);
                // сдвигаемся вниз на высоту строки
                offset += rect.Height;
            }

           
        }
     

        [TestMethod]
        public void VerticalAlignTopTest()
        {

            double offset = 0.0;
            for (int i = 0; i < LinesVertical.Count; i++)
            {
                var Al = new LinesPanelAlignment(offset, LinesVertical[i], Constraints, Orientation.Vertical,
                                                 HorizontalAlignmentPosition.Left, VerticalAlignmentPosition.Top);
                var rect = Al.Align();
                // проверяем правильность алайна
                // длина цепочки контролов, расположенных друг за другом должна соответствовать оригинальной длине всех контролов                                
                rect.Height.ShouldBeEqual(CommonHeight[i]);
                //отступ сверху должен быть нулевой
                rect.Y.ShouldBeEqual(0.0);
                // сдвигаемся вправо на ширину колонки
                offset += rect.Width;
            }
        }

        [TestMethod]
        public void VerticalAlignBottomTest()
        {

            double offset = 0.0;
            for (int i = 0; i < LinesVertical.Count; i++)
            {
                var Al = new LinesPanelAlignment(offset, LinesVertical[i], Constraints, Orientation.Vertical,
                                                 HorizontalAlignmentPosition.Left,VerticalAlignmentPosition.Bottom);
                var rect = Al.Align();
                // проверяем правильность алайна
                // длина цепочки контролов, расположенных друг за другом должна соответствовать оригинальной длине всех контролов                                
                rect.Height.ShouldBeEqual(CommonHeight[i]);
                // расстояние от нижнего края канваса должно равняться нулю, то есть не должно быль отступа
                (Constraints.Height - rect.Y - rect.Height).ShouldBeEqual(0.0);
                // верхний край должен быть правильным
                rect.Y.ShouldBeEqual(Constraints.Height - CommonHeight[i]);
                // сдвигаемся вправо на ширину колонки
                offset += rect.Width;
            }

        }

        [TestMethod]
        public void VerticalAlignCenterTest()
        {
            double offset = 0.0;
            for (int i = 0; i < LinesVertical.Count; i++)
            {
                var Al = new LinesPanelAlignment(offset, LinesVertical[i], Constraints, Orientation.Vertical,
                                                 HorizontalAlignmentPosition.Left, VerticalAlignmentPosition.Middle);
                var rect = Al.Align();
                // проверяем правильность алайна
                // длина цепочки контролов, расположенных друг за другом должна соответствовать оригинальной длине всех контролов                                
                rect.Height.ShouldBeEqual(CommonHeight[i]);
                // расстояние от нижнего края канваса НЕ должно равняться нулю
                (Constraints.Height - rect.Y - rect.Height).ShouldNotBeEqual(0.0);
                // отступ сверху не должен равен быть нулю
                rect.Y.ShouldNotBeEqual(0.0);
                // отступ с верхнего края и с нижнего должны быть одинаковыми
                rect.Y.ShouldBeEqual(Constraints.Height - rect.Y - rect.Height);
                // сдвигаемся вправо на ширину колонки
                offset += rect.Width;
            }
        }

    

        [TestMethod]
        public void VerticalAlignJustifiedTest()
        {

            double offset = 0.0;
            for (int i = 0; i < LinesVertical.Count; i++)
            {
                var Al = new LinesPanelAlignment(offset, LinesVertical[i], Constraints, Orientation.Vertical,
                                                 HorizontalAlignmentPosition.Left,VerticalAlignmentPosition.Justified);
                var rect = Al.Align();
                // проверяем правильность алайна
                // длина цепочки контролов, расположенных друг за другом НЕ должна соответствовать оригинальной длине всех контролов                                
                if (LinesVertical[i].Count > 1)
                    rect.Height.ShouldNotBeEqual(CommonHeight[i]);
                // расстояние от нижнего края канваса должно равняться нулю, если контролов больше 1
                if (LinesVertical[i].Count > 1)
                    (Constraints.Height - rect.Y - rect.Height).ShouldBeEqual(0.0);
                // отступ сверху должен равен быть нулю
                rect.Y.ShouldBeEqual(0.0);
                // отступ с верхнего края и с нижнего должны быть одинаковыми, если контролов больше 1
                if (LinesVertical[i].Count > 1)
                    rect.Y.ShouldBeEqual(Constraints.Height - rect.Y - rect.Height);
                // сдвигаемся вправо на ширину колонки
                offset += rect.Width;
            }

        }


    }
     */
}
