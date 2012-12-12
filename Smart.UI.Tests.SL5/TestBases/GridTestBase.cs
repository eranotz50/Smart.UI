using System.Windows;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Smart.Classes.Extensions;
using Smart.UI.Panels;
using Smart.Classes.Collections;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Animations;
using Smart.UI.Classes.Extensions;
using Smart.UI.Classes.Extensions.StructExtensions;
using Smart.UI.Tests.TestBases;


namespace Smart.UI.Tests.PanelsTests
{
    /// <summary>
    /// Basic test class needed for different tests of flexgrid and its siblings
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GridTestBase<T> : SilverlightTest,IGetterUp where T : FlexGrid, new()    
    {
        public T Grids;
        public SmartCollection2D<T> Cells;
        public T Cell;
        public Rect Bounds;

        [TestInitialize]
        public virtual void SetUp()
        {
            Grids = new T(){Name = "Grids"};
            Cells = new SmartCollection2D<T>().Fill(10, 10);
            this.TestPanel.Children.Add(Grids);
            Grids.Width = 1000;
            Grids.Height = 1000;
            for (var i = 0; i < 10; i++)
            {
                Grids.RowDefinitions.Add(new LineDefinition() {Stars = 1});
                Grids.ColumnDefinitions.Add(new LineDefinition() {Stars = 1});
            }

            for (var i = 0; i < Grids.ColumnDefinitions.Count; i++)
            {
                //Cells[i] = new SmartCollection<T>();
                for (var j = 0; j < Grids.RowDefinitions.Count; j++)
                {
                    Cells[i,j].SetColumn(i).SetRow(j);                    
                    //Cells[i,j] = new T().SetColumn(i).SetRow(j);
                    Cells[i,j].Name = "cell_" + i.ToString() + "_" + j.ToString();
                    Grids.AddChild(Cells[i,j]);
                }
            }
            Cell = Cells[4,4];
            //   Grids.AddChild(Cell);

            TestPanel.UpdateLayout();
            Bounds = new Rect();
            Animator.TestInit();
        }

        public void UpdateLayout()
        {
            this.TestPanel.UpdateLayout();
        }


        public ObjectFly GetUp(FrameworkElement element)
        {
            return this.Grids.DragManager.StartFlight(this.Grids.DragManager.BuildDragObject(element), e => element.GetBounds().TopLeft());
        }

        [TestCleanup]
        public virtual void CleanUp()
        {
            Bounds = default(Rect);
            TestPanel.Children.Remove(Grids);
            Grids.RemoveChild(Cell);
            Grids = null;
            Cell = null;
            Animator.EachFrame = null;
        }
    }
}
