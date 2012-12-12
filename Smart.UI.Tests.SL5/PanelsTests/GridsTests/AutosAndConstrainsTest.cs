using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.TestExtensions;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Tests.TestBases;

namespace Smart.UI.Tests.PanelsTests.GridsTests
{
    
    [TestClass]
    public class AutosAndConstrainsTest : PanelTestBase<SmartGrid>
    {
        public Border Border;
        public LineDefinitions Lds;
        public SmartCollection<LineDefinition> Defs;
        public int U;
        
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.Border = new Border().SetWidth(500).SetHeight(500).SetColumn(0).SetRow(0);
           // this.Lds = Panel.ColumnDefinitions;
            this.Defs = new SmartCollection<LineDefinition>
                {
                    new LineDefinition(10, 20) {IsAuto = true},
                    new LineDefinition(10, 20),
                    new LineDefinition(100),
                    new LineDefinition(-0.1, 100),
                    new LineDefinition() {IsAuto = true},
                    new LineDefinition(40, 50)
                };
            this.U = 0;
        }

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp(); 

        }

        [TestMethod]
        public void SortingTest()
        {
            this.Lds = new LineDefinitions(Defs);
            this.Lds.Autos.Count.ShouldBeEqual(2);
            this.Lds.Stars.Count.ShouldBeEqual(2);
            this.Lds.Values.Count.ShouldBeEqual(4);            
        }

        [TestMethod]
        public void UpdateTest()
        {
            this.Lds = new LineDefinitions(Defs);
            Lds.Update += () => U++;
            U.ShouldBeEqual(0);
            this.Defs[0].AbsoluteValue += 10;
            U.ShouldBeEqual(1);
            this.Defs[1].IsAuto = true;
            U.ShouldBeEqual(2);
            this.Defs[0].IsAuto = false;
            U.ShouldBeEqual(3);
            this.Defs[3].Stars = 10;
            U.ShouldBeEqual(4);
            this.Defs[3].Stars = 7;
            U.ShouldBeEqual(5);
            this.Defs[3].Stars = 7;
            U.ShouldBeEqual(5);            
            Lds.BlockPanelUpdate = true;
            this.Defs[3].Stars = 70;
            U.ShouldBeEqual(5);
            this.Defs[1].IsAuto = false;
            U.ShouldBeEqual(5);
            Lds.BlockPanelUpdate = false;
            this.Defs[3].Stars = 71;
            U.ShouldBeEqual(6);
            this.Defs[0].AbsoluteValue += 10;
            U.ShouldBeEqual(7);
        }

        [Ignore]
        [TestMethod]
        public void ApplyConstrainsTest()
        {
            this.Lds=this.Panel.ColumnDefinitions;
            this.Lds.Clear();
            for (int i = 0; i < 5; i++)
            {
                this.Lds.Add(new LineDefinition(1,0.0));
                this.Lds.Add(new LineDefinition(100));
            }
            this.Lds[2].MinLength = 200;

            Panel.Width.ShouldBeEqual(1000.0);
            this.UpdateLayout();
            Panel.Width.ShouldBeEqual(1000.0);
            this.Panel.ColumnDefinitions[1].Value.ShouldBeEqual(100);
            this.Panel.ColumnDefinitions[3].Value.ShouldBeEqual(100);

            this.Panel.ColumnDefinitions[0].Stars.ShouldBeEqual(1);
        }


        [TestMethod]
        public void AutoTest()
        {
            this.Lds = this.Panel.ColumnDefinitions;
            this.Panel.RowDefinitions.Add(new LineDefinition(1.0, 0.0));
            this.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Height.ShouldBeEqual(1000);
            this.Panel.AddChild(this.Border);
            this.Lds.Add(new LineDefinition(10.0){IsAuto = true});
            this.Lds.Add(new LineDefinition(1.0, 0.0));
            this.Lds.Add(new LineDefinition(200));

        
            this.UpdateLayout();

        

            this.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Height.ShouldBeEqual(1000);            
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Size().Width.ShouldBeEqual(1000);
          
            this.Lds[0].Value.ShouldBeEqual(500);
            this.Lds[0].IsAuto.ShouldBeTrue();
            this.Lds[1].Value.ShouldBeEqual(300);
            this.Lds[1].IsStar.ShouldBeTrue();
            this.Lds[1].IsAbsolute.ShouldBeFalse();            
            this.Lds[2].IsAbsolute.ShouldBeTrue();
            this.Lds[2].Value.ShouldBeEqual(200);

            this.Border.SetWidth(100).SetHeight(100);
            this.UpdateLayout();
            
            
            this.Lds[0].Value.ShouldBeEqual(100);
            this.Lds[0].IsAuto.ShouldBeTrue();
            this.Lds[1].Value.ShouldBeEqual(700);
            this.Lds[1].IsStar.ShouldBeTrue();
            this.Lds[1].IsAbsolute.ShouldBeFalse();
            this.Lds[2].IsAbsolute.ShouldBeTrue();
            this.Lds[2].Value.ShouldBeEqual(200);
            


        }

    
        [TestMethod]
        public void AutoGrowTest()
        {            
            this.UpdateLayout();
            this.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Height.ShouldBeEqual(1000);

            this.Panel.RowDefinitions.Add(new LineDefinition(1.0, 0.0));            
            this.Lds = this.Panel.ColumnDefinitions;
            /*
            this.Panel.RowDefinitions.GrowPanel = false;
            this.Panel.ColumnDefinitions.GrowCanvas = false;
             */
            this.Lds.GrowAuto = true;
            
        
            var aline = new LineDefinition(10.0) {IsAuto = true};
            this.Lds.Add(aline);
            this.Lds.Add(new LineDefinition(1.0, 0.0));
            this.Lds.Add(new LineDefinition(200));
            this.Panel.AddChild(this.Border);
            this.Panel.ColumnDefinitions.Autos.ShouldContain(aline);
            this.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Height.ShouldBeEqual(1000);            
            this.Panel.Space.Panel.Width.ShouldBeEqual(1000);
            this.Panel.Space.Panel.Height.ShouldBeEqual(1000);
            

            this.UpdateLayout();
           
            
        //    this.Panel.Width.ShouldBeEqual(1500);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1500);
            this.Panel.Space.Canvas.Size().Width.ShouldBeEqual(1500);
            
            
            this.Panel.Height.ShouldBeEqual(1000);
            
            this.Lds[0].Value.ShouldBeEqual(500);
            this.Lds[0].IsAuto.ShouldBeTrue();
            this.Lds[1].Value.ShouldBeEqual(800);
            this.Lds[1].IsStar.ShouldBeTrue();
            this.Lds[1].IsAbsolute.ShouldBeFalse();
            this.Lds[2].IsAbsolute.ShouldBeTrue();
            this.Lds[2].Value.ShouldBeEqual(200);
            this.Lds.AllAutos.ShouldBeEqual(500);
            this.Lds.GrowAuto = false;
            Lds.DeltaForce.ShouldBeEqual(-500.0);
            this.UpdateLayout();

          //  this.Panel.Width.ShouldBeEqual(1000);            
            this.Panel.Height.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1000);
            this.Panel.Space.Canvas.Size().Width.ShouldBeEqual(1000);

            this.Lds[0].Value.ShouldBeEqual(500);
            this.Lds[0].IsAuto.ShouldBeTrue();
            this.Lds[1].Value.ShouldBeEqual(300);
            this.Lds[1].IsStar.ShouldBeTrue();
            this.Lds[1].IsAbsolute.ShouldBeFalse();
            this.Lds[2].IsAbsolute.ShouldBeTrue();
            this.Lds[2].Value.ShouldBeEqual(200);
            this.Lds.GrowAuto = true;
            Lds.DeltaForce.ShouldBeEqual(500.0);
            
            this.UpdateLayout();
            
            //this.Panel.Width.ShouldBeEqual(1500);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1500);
            this.Panel.Height.ShouldBeEqual(1000);
            this.Lds[0].Value.ShouldBeEqual(500);
            this.Lds[0].IsAuto.ShouldBeTrue();
            this.Lds[1].Value.ShouldBeEqual(800);
            this.Lds[1].IsStar.ShouldBeTrue();
            this.Lds[1].IsAbsolute.ShouldBeFalse();
            this.Lds[2].IsAbsolute.ShouldBeTrue();
            this.Lds[2].Value.ShouldBeEqual(200);

            this.Border.SetWidth(100).SetHeight(100);
            this.UpdateLayout();

            //this.Panel.Width.ShouldBeEqual(1100);
            this.Panel.Space.Canvas.Width.ShouldBeEqual(1100);            
            this.Panel.Height.ShouldBeEqual(1000);
            this.Lds[0].Value.ShouldBeEqual(100);
            this.Lds[0].IsAuto.ShouldBeTrue();
            this.Lds[1].Value.ShouldBeEqual(800);
            this.Lds[1].IsStar.ShouldBeTrue();
            this.Lds[1].IsAbsolute.ShouldBeFalse();
            this.Lds[2].IsAbsolute.ShouldBeTrue();
            this.Lds[2].Value.ShouldBeEqual(200);



            this.Lds.GrowAuto = false;
            this.UpdateLayout();

            this.Lds[0].Value.ShouldBeEqual(100);
            this.Lds[0].IsAuto.ShouldBeTrue();
            this.Lds[1].Value.ShouldBeEqual(700);
            this.Lds[1].IsStar.ShouldBeTrue();
            this.Lds[1].IsAbsolute.ShouldBeFalse();
            this.Lds[2].IsAbsolute.ShouldBeTrue();
            this.Lds[2].Value.ShouldBeEqual(200);

        }
       
    }
}
