using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Design.Data.ContentWall
{
    public class ContentWallPanelItemToolbar : UserControl
    {
        public static readonly DependencyProperty IsFreasedProperty =
            DependencyProperty.Register("IsFreased", typeof (bool), typeof (ContentWallPanelItemToolbar),
                                        new PropertyMetadata(false));

        private readonly Button _expandButton;
        private readonly Grid grid;


        private readonly Action<ContentWallPanelItem.ContentWallPanelItemToolbarAction> onClick;
        private readonly StackPanel stack;
        private Border panel;
        private Image playButton;

        public ContentWallPanelItemToolbar(Action<ContentWallPanelItem.ContentWallPanelItemToolbarAction> OnClick)
        {
            onClick = OnClick;
            HorizontalAlignment = HorizontalAlignment.Right;
            VerticalAlignment = VerticalAlignment.Stretch;
            Opacity = 0.0;

            _expandButton = new Button
                                {
                                    Height = 30,
                                    Width = 30,
                                    Style = (Style) Application.Current.Resources["7MetroExpandButtonBlack"]
                                };
            _expandButton.SetValue(ToolTipService.ToolTipProperty, "Развернуть");

            _expandButton.Click += (s, e) =>
                                       {
                                           if (onClick != null)
                                               onClick(ContentWallPanelItem.ContentWallPanelItemToolbarAction.Open);
                                       };

            grid = new Grid
                       {
                           VerticalAlignment = VerticalAlignment.Top,
                           HorizontalAlignment = HorizontalAlignment.Right
                       };

            //panel = new Border
            //            {
            //                Background = new SolidColorBrush(Colors.Black),                            
            //                CornerRadius = new CornerRadius(3),
            //                VerticalAlignment =  VerticalAlignment.Stretch,
            //                HorizontalAlignment= HorizontalAlignment.Stretch
            //            };

            stack = new StackPanel
                        {
                            Margin = new Thickness {Bottom = 3, Left = 3, Top = 25, Right = 3},
                            VerticalAlignment = VerticalAlignment.Center
                        };

            //grid.Children.Add(panel);
            grid.Children.Add(stack);

       

            //stack.Children.Add(playButton);
            stack.Children.Add(_expandButton);

            Background = new SolidColorBrush(Colors.Transparent);
            Content = grid;

//            MouseEnter += (s, e) => { if (!IsFreased) ArtefactAnimator.AddEase(this, OpacityProperty, 1.0, 0.3); };
  //          MouseLeave += (s, e) => ArtefactAnimator.AddEase(this, OpacityProperty, 0.0, 0.3);
        }

        public bool IsFreased
        {
            get { return (bool) GetValue(IsFreasedProperty); }
            set { SetValue(IsFreasedProperty, value); }
        }
    }
}