using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Extensions;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.UI.Classes.Extensions;
using Smart.TestExtensions;

namespace Smart.UI.Tests.CollectionsTest
{
    [TestClass]
    public class NumedCollectionTest
    {
        public LineDefinitions Lines;
        public SmartCollection<LineDefinition> Data;
        public int Count = 10;

        [TestInitialize]
        public void SetUp()
        {
            this.Lines = new LineDefinitions();
            this.Data = new SmartCollection<LineDefinition>();
            for (int i = 0; i < Count; i++)
            {
               this.Data.Add(new LineDefinition());
            }
            

        }

        [TestCleanup]
        public void CleanUp()
        {
            this.Lines = null;
        }


        [TestMethod]
        public void NumChangesTest()
        {
            this.Data[4].Num.ShouldBeEqual(0);
            this.Lines.Add(Data);
            this.Data[4].Num.ShouldBeEqual(4);
            
            this.Data[4].Num = 0;
            this.Data[4].Num.ShouldBeEqual(0);
            var item = this.Lines[4];
            this.Data[4].Num.ShouldBeEqual(4);
            
            this.Data[4].Num = 0;
            this.Data[4].Num.ShouldBeEqual(0);
            this.Lines.UpdateNums();
            this.Data[4].Num.ShouldBeEqual(4);
            
            this.Data.Remove(item);
            item.Num.ShouldBeEqual(4);
            this.Data.Add(item);
            this.Data[4].Num.ShouldBeEqual(5);            
            item.Num.ShouldBeEqual(4);
            this.Lines.Remove(item);
            this.Lines[4].Num.ShouldBeEqual(4);
            this.Data[4].Num.ShouldBeEqual(4);
            item.Num.ShouldBeEqual(4);
            this.Lines.Add(item);
            item.Num.ShouldBeEqual(9);
            this.Data[5].Num.ShouldBeEqual(5);
            this.Lines[5].Num.ShouldBeEqual(5);
            this.Data[5].Num.ShouldBeEqual(5);
            this.Lines.UpdateNums(6);
            this.Data[7].Num.ShouldBeEqual(7);
        }
    }
}
