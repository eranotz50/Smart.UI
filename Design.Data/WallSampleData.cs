using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Smart.UI.Panels;
using Smart.UI.Widgets;
using Smart.Classes.Collections;
using Smart.UI.Classes.Layout;
using Smart.UI.Classes.Utils;


namespace DesignData
{
  

   
        public class WallSampleData : SampleData
        {


            public override void Init(int count)
            {
                this.Text = "SampleText";
                this.Items = new SmartCollection<FrameworkElement>();
                for (var i = 0; i < 10; i++) Items.Add(Make(i));
            }

            public override FrameworkElement Make(int num)
            {
                return MakeWall(num);
            }

            
            public  Wall MakeWall(int num, int count = 10)
            {

                var item = new Wall
                {
                    Background = new SolidColorBrush(Colors.LightGray),
                    Items = new SmartCollection<FrameworkElement>(),
                    Orientation = Orientation.Vertical,
                    CellsInLine = 2,
                    LinesLength = new RelativeLength(1, 50),
                    OtherLinesLength = new RelativeLength(1, 50),
                    OutMode = OutMode.Clip
                };
                Wall.SetContentSize(item,ContentSize.Small);

                for (var j = 0; j < 10; j++) item.Items.Add(MakeBorder().SetContentSize(ContentSize.Small));
                return item;
            }

            protected Border MakeBorder()
            {
                return new Border { Background = new SolidColorBrush(ColorsDesign.GetRandomColor()), BorderBrush = new SolidColorBrush(Colors.White), BorderThickness = new Thickness(2), Opacity = 0.3 };
            }
        }

  
}
