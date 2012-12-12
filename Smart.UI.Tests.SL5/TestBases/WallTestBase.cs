using System.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Graphs;
using Smart.Classes.Collections;
using Smart.TestExtensions;


namespace Smart.UI.Tests.TestBases
{
    public class WallTestBase:BasicWallTestBase
    {

        public SmartCollection<FrameworkElement> Items;
       // public WallCollection2D<FrameworkElement> Items2D;
        public int Count;
        public int ItemsInALine = 4;

        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.Items = new LinesCollection<FrameworkElement>();
         //   this.Items2D = new WallCollection2D<FrameworkElement>();
            ItemsInALine = 4;
            this.Count = 20;
            var c = 0;
            var r = 0;

            for (var i = 0; i < Count; i++)
            {
                var item = MakeItem();
                this.Items.Add(item);
         //       this.Items2D[r, c] = item;
                if (r == ItemsInALine - 1)
                {
                    r = 0;
                    c++;
                }
                else
                {
                    r++;
                }
            }
            this.Items.Count.ShouldBeEqual(20);
       //     this.Items2D.Count.ShouldBeEqual(this.ItemsInALine);
            //this.Items2D.Last().Count.ShouldBeEqual()
        }

 

        [TestCleanup]
        public override void CleanUp()
        {
            base.CleanUp();
            this.ItemsInALine = 0;
            this.Count = 0;
            //this.Items2D = null;
            this.Items = null;
            this.Panel = null;
        }
    }
}
