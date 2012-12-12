using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Design.Data.ContentWall;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.UI.Classes.Utils;
using Smart.UI.Panels;
using Smart.TestExtensions;
using Smart.UI.Widgets;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Tests.TestBases
{
    [TestClass]
    public class  BasicWallTestBase: PanelTestBase<Wall>
    {
        [TestInitialize]
        public override void SetUp()
        {
            base.SetUp();
            this.Panel.RowDefinitions.PanelUpdateMode = PanelUpdateMode.None;
            this.Panel.ColumnDefinitions.PanelUpdateMode = PanelUpdateMode.None;
        }


        protected virtual void CheckItemPositions(double lineSize = 200, double otherLineSize = 200, double betweenLines = 10, double betweenOtherLines=10)
        {
            for (var i = 0; i < this.Panel.ItemLines.Count; i++)
            {
                var line = this.Panel.ItemLines[i];
                for (var j = 0; j < line.Count; j++)
                {
                    var item = line[j];
                   // this.Panel.UpdateLineNums();
                    var c = this.NumForLine(i);
                    var r = this.NumForLine(j);
                    var rs = this.ToSpan(this.Panel.Paginator.CellsIn(item));
                    var cs = this.ToSpan(1);
                    SetSmallLinesTest(item, c, r, cs, rs);
                    var b = item.GetBounds();
                    var mX = i * 2 * betweenLines + betweenLines;
                    var cX = i * lineSize;
                    var mY = j * 2 * betweenOtherLines + betweenOtherLines;
                    var cY = j * otherLineSize;
                    b.ShouldBeEqual(new Rect(mX + cX, mY + cY, lineSize, otherLineSize));
                }
            }
        }

        protected virtual void SetSmallLinesTest(FrameworkElement item, int c, int r, int cs, int rs)
        {
            item.GetColumn().ShouldBeEqual(c);
            item.GetRow().ShouldBeEqual(r);
            item.GetColumnSpan().ShouldBeEqual(cs);
            item.GetRowSpan().ShouldBeEqual(rs);
        }
       
        protected virtual ContentWallPanelItem MakeItem(ContentSize size = ContentSize.Medium)
        {
            var content = new Border { Background = new SolidColorBrush(ColorsDesign.GetRandomColor()) };
            var item = new ContentWallPanelItem(content, size, null);
            return item;
        }
       
        /// <summary>
        /// Find row/column num for line position taking into acount marinal rows/cols
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        protected virtual int NumForLine(int i)
        {
            return 3 * i + 1;
        }


        protected virtual int ToSpan(int cells)
        {
            return 1 + (cells - 1) * 3;
        }


       
    }
}
