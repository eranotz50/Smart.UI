using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Smart.UI.Panels;
using Smart.UI.Widgets;

namespace Design.Data.ContentWall
{
    public class ContentWallPanelItem : UserControl, IPanelItem
    {
        #region Dependency properties

        public static readonly DependencyProperty IsFreasedProperty =
            DependencyProperty.Register("IsFreased", typeof (bool), typeof (ContentWallPanelItem),
                                        new PropertyMetadata(false));


        public static readonly DependencyProperty IsFadedProperty =
            DependencyProperty.Register("IsFaded", typeof (bool), typeof (ContentWallPanelItem),
                                        new PropertyMetadata(false));

        public static readonly DependencyProperty ToolbarVisibleProperty =
            DependencyProperty.Register("ToolbarVisible", typeof (Visibility), typeof (ContentWallPanelItem),
                                        new PropertyMetadata(Visibility.Visible));


        public static readonly DependencyProperty ToolbarAlwaysActiveProperty =
            DependencyProperty.Register("ToolbarAlwaysActive", typeof (bool), typeof (ContentWallPanelItem),
                                        new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register("HeaderText", typeof (string), typeof (ContentWallPanelItem),
                                        new PropertyMetadata(default(string)));

        public bool IsFreased
        {
            get { return (bool) GetValue(IsFreasedProperty); }
            set
            {
                SetValue(IsFreasedProperty, value);
                SetFreesing();
            }
        }

        public bool IsFaded
        {
            get { return (bool) GetValue(IsFadedProperty); }
            set
            {
                SetValue(IsFadedProperty, value);
                SetFaded();
            }
        }

        public Visibility ToolbarVisible
        {
            get { return (Visibility) GetValue(ToolbarVisibleProperty); }
            set { SetValue(ToolbarVisibleProperty, value); }
        }

        public bool ToolbarAlwaysActive
        {
            get { return (bool) GetValue(ToolbarAlwaysActiveProperty); }
            set { SetValue(ToolbarAlwaysActiveProperty, value); }
        }

        public string HeaderText
        {
            get { return (string) GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        #endregion

        #region Events

        private Action<ContentWallPanelItem, ContentWallPanelItemToolbarAction> onClick { get; set; }

        #endregion

        #region Properties

        public enum ContentWallPanelItemToolbarAction
        {
            None,
            Open,
            Close
        }

       // private ScaleOnMouseBehavior _scaleOnMouseOverBehaviour;
        private ContentWallPanelItemToolbar toolbar;

        public ContentSize contentWidthOrHeight { get; set; }

        public bool ItemInitialized { get; set; }

        public FrameworkElement ContentView { get; set; }

        public Func<double> CalculateLength { get; set; }
        public Func<double> CalculateWidthOrHeight { get; set; }
        public ContentSize ContentSize { get; set; }

        #endregion

        #region Local variables

        private Grid _headerControl;

        #endregion

        public ContentWallPanelItem()
        {
            Init(null, ContentSize.Medium, null);
        }

        public ContentWallPanelItem(FrameworkElement content, ContentSize contentSize,
                                    Action<ContentWallPanelItem, ContentWallPanelItemToolbarAction> onClick,
                                    Func<double> calculateLength = null,
                                    ContentSize contentWidthOrHeight = ContentSize.Auto,
                                    Func<double> calculateWidthOrHeight = null)
        {
            Init(content, contentSize, onClick, calculateLength, contentWidthOrHeight, calculateWidthOrHeight);
        }

        protected virtual void Init(FrameworkElement content, ContentSize contentSize,
                                    Action<ContentWallPanelItem, ContentWallPanelItemToolbarAction> onClick,
                                    Func<double> calculateLength = null,
                                    ContentSize contentWidthOrHeight = ContentSize.Auto,
                                    Func<double> calculateWidthOrHeight = null)
        {
            ContentView = content;
            ContentSize = contentSize;
            this.contentWidthOrHeight = contentWidthOrHeight;
            CalculateLength = calculateLength;
            CalculateWidthOrHeight = calculateWidthOrHeight;
            this.onClick = onClick;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            Loaded += (s, e) => InitControl();
        }

        public void ClearContent()
        {
            var g = Content as Grid;
            if (g != null)
            {
                g.Children.Clear();
            }
        }

        private void InitControl()
        {
            if (ItemInitialized) return;
            ItemInitialized = true;
            var g = new Grid
                        {
                            HorizontalAlignment = HorizontalAlignment.Stretch,
                            VerticalAlignment = VerticalAlignment.Stretch
                        };
            toolbar = new ContentWallPanelItemToolbar(ToolbarItemClicked)
                          {
                              VerticalAlignment = VerticalAlignment.Stretch,
                              MinHeight = 100,
                              IsFreased = IsFreased,
                              Visibility = ToolbarVisible
                          };
            g.Children.Add(ContentView);
            g.Children.Add(toolbar);

           // _scaleOnMouseOverBehaviour = new ScaleOnMouseBehavior{ScaleFactor = IsFreased ? 1.0 : 1.05, ChangeParentZIndex = true};
            //Interaction.GetBehaviors(g).Add(_scaleOnMouseOverBehaviour);

            #region  шапка контрола

            _headerControl = new Grid
                                 {
                                     Background = new SolidColorBrush(Colors.White),
                                     HorizontalAlignment = HorizontalAlignment.Left,
                                     VerticalAlignment = VerticalAlignment.Top
                                 };
            _headerControl.RowDefinitions.Add(new RowDefinition {Height = new GridLength(20)});
            _headerControl.RowDefinitions.Add(new RowDefinition {Height = GridLength.Auto});


            if (!string.IsNullOrWhiteSpace(HeaderText))
            {
                var headerText = new TextBlock
                                     {
                                         FontSize = 9,
                                         Foreground = new SolidColorBrush(Colors.Gray),
                                         HorizontalAlignment = HorizontalAlignment.Center,
                                         VerticalAlignment = VerticalAlignment.Center,
                                         Text = HeaderText,
                                         Margin = new Thickness(10, 1, 10, 1)
                                     };
                _headerControl.Children.Add(headerText);

                var rect = new Rectangle {Height = 3, Fill = new SolidColorBrush(Colors.Green)};
                rect.SetValue(Grid.RowProperty, 1);
                _headerControl.Children.Add(rect);
                _headerControl.Opacity = 0.0;

                g.Children.Add(_headerControl);
            }

            #endregion

            Content = g;


            ContentView.MouseLeftButtonDown += content_MouseLeftButtonDown;
            ContentView.MouseEnter += (s, e) =>
                                          {
                                              /*
                                              if (!IsFreased || ToolbarAlwaysActive)
                                                  ArtefactAnimator.AddEase(toolbar, OpacityProperty, 0.3, 0.3);
                                              if (_headerControl != null)
                                                  ArtefactAnimator.AddEase(_headerControl, OpacityProperty, 0.9, 0.3);
                                               */
                                          };
            ContentView.MouseLeave += (s, e) =>
                                          {
                                              /*
                                              ArtefactAnimator.AddEase(toolbar, OpacityProperty, 0.0, 0.3);
                                              if (_headerControl != null)
                                                  ArtefactAnimator.AddEase(_headerControl, OpacityProperty, 0.0, 0.3);
                                               */
                                          };
           // g.Loaded += (s, e) => _scaleOnMouseOverBehaviour.DoubleBlow();
        }

        private void SetFreesing()
        {
            /*
            if (_scaleOnMouseOverBehaviour != null)
                _scaleOnMouseOverBehaviour.ScaleFactor = IsFreased ? 1.0 : 1.05;
             */
            if (toolbar != null)
                toolbar.IsFreased = !ToolbarAlwaysActive && IsFreased;
                    // если тулбар всегда активен, то всегда, иначе, в зависимости от контрола-айтема
        }

        private void SetFaded()
        {
            Opacity = IsFaded ? 0.5 : 1.0;
        }

        /// <summary>
        /// публичный метод, чтобы можно было спокойно эмулировать команд тулбара из внутренних контролов, даже если на тулбаре соответствующей кнопки нет
        /// </summary>
        /// <param name="action"></param>
        public void ToolbarItemClicked(ContentWallPanelItemToolbarAction action)
        {
            if (onClick != null) onClick(this, action);
        }

        private void content_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (onClick != null && !IsFreased) onClick(this, ContentWallPanelItemToolbarAction.None);
        }

        public ContentWallPanelItemCoordinates GetCoordinates()
        {
            return new ContentWallPanelItemCoordinates
                       {
                           Column = this.GetColumn(),
                           ColumnSpan = this.GetColumnSpan(),
                           Row = this.GetRow(),
                           RowSpan = this.GetRowSpan()
                       };
        }
    }

    public class ContentWallPanelItemCoordinates
    {
        public int Column { get; set; }
        public int Row { get; set; }
        public int ColumnSpan { get; set; }
        public int RowSpan { get; set; }
    }
}