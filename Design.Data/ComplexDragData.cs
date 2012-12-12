using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using DesignData;
using Smart.UI.Panels;
using Smart.UI.Widgets;
using Smart.Classes.Collections;
using Smart.UI.Classes.Utils;
using Smart.UI.Widgets.PanelAdorners;


namespace Design.Data
{
    public class ComplexDragData :SampleData 
    {
        public override void Init(int count)
        {
            this.Text = "SampleText";
            this.Items = new SmartCollection<FrameworkElement>
                             {
                                 this.MakeWidget().SetContentSize(ContentSize.Large),
                                 this.MakeWidget().SetContentSize(ContentSize.Large)
                             };
            //var r = 0;
            //var contentSize = ContentSize.Medium;


            for (var i = 0; i < count; i++)
            {
                //if (r == 0 || r == 3 || r == 5) contentSize = ContentSize.Small;
                //if (r == 1 || r == 4 || r==2) contentSize = ContentSize.Medium;
                //if (r == 5) r = 0;else r++;
                Items.Add(this.Make(i).SetContentSize(ContentSize.Small));
            }
        }

        public override FrameworkElement Make(int num)
        {
            return new Border { Background = new SolidColorBrush(ColorsDesign.GetRandomColor()), BorderBrush = new SolidColorBrush(Colors.White), BorderThickness = new Thickness(2), Opacity = 0.3 };
        }

        /// <summary>
        /// Makes a widget grid
        /// </summary>
        protected WidgetGrid MakeWidget()
        {
            var grid = new WidgetGrid {ColsNum = 5, RowsNum = 10};

            grid.AddChild(this.Make(1).SetRow(1).SetColumn(1).SetRowSpan(2).SetColumnSpan(2));
            grid.AddChild(this.Make(2).SetRow(5).SetColumn(3).SetRowSpan(2).SetColumnSpan(2));
            grid.AddChild(this.Make(3).SetRow(4).SetColumn(2).SetRowSpan(1).SetColumnSpan(2));
            var adorners = Adorners.GetSmartGridAdorners(grid);
            adorners.Add(new GridLines());            
            return grid;
        }
    }
}
