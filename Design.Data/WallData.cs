using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SilverExperimental.DesignData;
using Smart.UI.Panels;
using Smart.UI.Widgets;
using Smart.Classes.Collections;
using Smart.UI.Classes.Events;
using Smart.UI.Classes.Layout;
using Smart.UI.Classes.Utils;


namespace DesignData
{
    public class WallData: SampleData
    {
        public WallData() :base(20)
        { 
            
        }

        protected Random R = new Random();
        public override void Init(int count)
        {
            this.Text = "SampleText";
            this.Items = new SmartCollection<FrameworkElement>();
            for (var i = 0; i < count; i++) Items.Add(Make(i));
        }

        public override FrameworkElement Make(int num)
        {
            ContentSize contentSize;
            var r = this.R.Next(4);
            switch (r)
            {
                case 0:
                case 1:
                    contentSize = ContentSize.Small;
                    break;
                case 2:
                    contentSize = ContentSize.Medium;
                    break;
                default:
                    contentSize = ContentSize.Large;
                    break;
            }
            var item = contentSize == ContentSize.Small ? (FrameworkElement)MakePost() : MakeWall();
            return item.SetContentSize(contentSize);

        }

        public Post MakePost()
        {
            var r = this.R.Next(3);
            var str = "";
            switch (r)
            {
                case 0:str = "<h1>Hi, how are you doing?</h1>"; break;
                case 1:str = "Great, thanks, what about you?"; break;
                case 2:str = "<b>Even do not mention</b>"; break;
                
            }
            var p = new Post {htmlTextBlock = {FontSize = 18}};
            p.htmlTextBlock.Load(str);

            return p;
      
        }

        public Wall MakeWall()
            {

                var item = new Wall
                {
                    Background = new SolidColorBrush(ColorsDesign.GetRandomColor()),
                    Items = new SmartCollection<FrameworkElement>(),
                    Orientation = Orientation.Vertical,
                    LinesLength = new RelativeLength(200),
                    OtherLinesLength = new RelativeLength(200),
                    BetweenLines = new RelativeLength(5),
                    BetweenOtherLines = new RelativeLength(5),
                    CellsInLine = 2,
                    OutMode = OutMode.Clip,
                    CanvasUpdateMode = CanvasUpdateMode.KeepCanvas
                };

                for (var j = 0; j < 10; j++) item.Items.Add(MakePost().SetContentSize(ContentSize.Small));
                item.MouseDownSubject.DoOnNext +=i=> item.RaiseEvent(new ZoomAtEvent(item)); //ZoomAtEvent
                return item;
            }

            protected Border MakeBorder()
            {
                return new Border { Background = new SolidColorBrush(ColorsDesign.GetRandomColor()), BorderBrush = new SolidColorBrush(Colors.White), BorderThickness = new Thickness(2),Opacity = 0.3};
            }          
    }
}
