using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Smart.UI.Widgets;
using Smart.Classes.Collections;
using Smart.UI.Classes.Utils;


namespace DesignData
{
    public class WallSimpleData : SampleData
    {

        public override void Init(int count)
        {
            this.Text = "SampleText";
            this.Items = new SmartCollection<FrameworkElement>();
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
    }
}
