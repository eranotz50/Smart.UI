using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Smart.UI.Classes.Extensions;
using Smart.Classes.Collections;
using Smart.Classes.Extensions;
using Smart.UI.Classes.Other;
using Smart.UI.Panels;
using Smart.UI.Widgets;

namespace Design.Data.ContentWall
{
    public class FlexContentWallPanel : UserControl, IOrientable
    {
        #region DEPENDENCY PROPERTIES

        public static readonly DependencyProperty ContentItemLengthProperty =
            DependencyProperty.Register("ContentItemLength", typeof (double), typeof (FlexContentWallPanel),
                                        new FrameworkPropertyMetadata((double) 300,
                                                                      FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty RowsForContentProperty =
            DependencyProperty.Register("RowsForContent", typeof (int), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(3, BasicSmartPanel.InvalidateArrangeCallback));

        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof (Orientation), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(Orientation.Horizontal,
                                                             BasicSmartPanel.InvalidateArrangeCallback));


        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register("ItemsSource", typeof (SmartCollection<ContentWallPanelItem>),
                                        typeof (FlexContentWallPanel),
                                        new PropertyMetadata(new SmartCollection<ContentWallPanelItem>(),
                                                             BasicSmartPanel.InvalidateArrangeCallback));

        public static readonly DependencyProperty DataMarginProperty =
            DependencyProperty.Register("DataMargin", typeof (Thickness), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(
                                            new Thickness {Left = 20, Top = 50, Right = 20, Bottom = 50},
                                            BasicSmartPanel.InvalidateArrangeCallback));


        public static readonly DependencyProperty ItemsMarginProperty =
            DependencyProperty.Register("ItemsMargin", typeof (Thickness), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(new Thickness(10, 3, 5, 3)));

        public static readonly DependencyProperty HorizontalScrollOffsetProperty =
            DependencyProperty.Register("HorizontalScrollOffset", typeof (double), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(default(double)));


        public static readonly DependencyProperty ItemsOnPageProperty =
            DependencyProperty.Register("ItemsOnPage", typeof (int), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(20));


        /// <summary>
        /// отступ левого края от 0 до 1.
        /// </summary>
        public static readonly DependencyProperty LeftMarginRatioProperty =
            DependencyProperty.Register("LeftMarginRatio", typeof (double), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(1.0));

        public static readonly DependencyProperty ScrollFrictionProperty =
            DependencyProperty.Register("ScrollFriction", typeof (double), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(0.1));

        /// <summary>
        /// использовать гравитацию по центру. Типа после окончания скроллинга контрол, что ближе к центру, отцентрируется
        /// </summary>
        public static readonly DependencyProperty UseCenteredGravityProperty =
            DependencyProperty.Register("UseCenteredGravity", typeof (bool), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(false));

        /// <summary>
        /// использовать гравитацию по центру. Типа после окончания скроллинга контрол, что ближе к центру, отцентрируется
        /// </summary>
        public static readonly DependencyProperty UseLeftGravityProperty =
            DependencyProperty.Register("UseLeftGravity", typeof (bool), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(false));

        /// <summary>
        /// сколько шагов надо сделать при гладком скроллировании элемента. По умолчанию 5
        /// </summary>
        public static readonly DependencyProperty SmoothScrollingStepsProperty =
            DependencyProperty.Register("SmoothScrollingSteps", typeof (int), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(5));

        /// <summary>
        /// авто растягивать айтемы, чтобы заполнить слоты, если айтему не достает до конца слотов минимального размера (1/n от кол-ва строк)
        /// </summary>
        public static readonly DependencyProperty AutoStretchItemsToFillSlotsProperty =
            DependencyProperty.Register("AutoStretchItemsToFillSlots", typeof (bool), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(false));


        /// <summary>
        /// при создании айтема смотрится, видим ли у него всего тулбар (то есть в режиме заморозки айтема и в обычном режиме) или только в обычном режиме
        /// </summary>
        public static readonly DependencyProperty ItemToolbarAlwaysActiveProperty =
            DependencyProperty.Register("ItemToolbarAlwaysActive", typeof (bool), typeof (FlexContentWallPanel),
                                        new PropertyMetadata(default(bool)));

        public double ContentItemLength
        {
            get { return (double) GetValue(ContentItemLengthProperty); }
            set
            {
                SetValue(ContentItemLengthProperty, value);
                ShowItems();
            }
        }

        public int RowsForContent
        {
            get { return (int) GetValue(RowsForContentProperty); }
            set
            {
                SetValue(RowsForContentProperty, value);
                ShowItems();
            }
        }

        public SmartCollection<ContentWallPanelItem> ItemsSource
        {
            get { return (SmartCollection<ContentWallPanelItem>) GetValue(ItemsSourceProperty); }
            set
            {
                ClearLayout();
                SetValue(ItemsSourceProperty, value);
                ShowItems();
            }
        }

        public Thickness DataMargin
        {
            get { return (Thickness) GetValue(DataMarginProperty); }
            set
            {
                SetValue(DataMarginProperty, value);
                ShowItems();
            }
        }

        public Thickness ItemsMargin
        {
            get { return (Thickness) GetValue(ItemsMarginProperty); }
            set { SetValue(ItemsMarginProperty, value); }
        }

        public double HorizontalScrollOffset
        {
            get { return (double) GetValue(HorizontalScrollOffsetProperty); }
            set
            {
                if (value != HorizontalScrollOffset) SetValue(HorizontalScrollOffsetProperty, value);
                CheckForUpdates();
            }
        }

        public int ItemsOnPage
        {
            get { return (int) GetValue(ItemsOnPageProperty); }
            set { SetValue(ItemsOnPageProperty, value); }
        }

        public double LeftMarginRatio
        {
            get { return (double) GetValue(LeftMarginRatioProperty); }
            set { SetValue(LeftMarginRatioProperty, value); }
        }

        public double ScrollFriction
        {
            get { return (double) GetValue(ScrollFrictionProperty); }
            set
            {
                SetValue(ScrollFrictionProperty, value);
                //if (_scrollAdapter != null) _scrollAdapter.Friction = value;
            }
        }

        public bool UseCenteredGravity
        {
            get { return (bool) GetValue(UseCenteredGravityProperty); }
            set
            {
                if (value) // если установлена, то отключаем гравитацию по левому краю
                    UseLeftGravity = false;

                SetValue(UseCenteredGravityProperty, value);
                //if (_scrollAdapter != null)_scrollAdapter.ScrollStopDelta = 20.0;
            }
        }

        public bool UseLeftGravity
        {
            get { return (bool) GetValue(UseLeftGravityProperty); }
            set
            {
                if (value) // если установлена, то отключаем гравитацию по центру
                    UseCenteredGravity = false;
                SetValue(UseLeftGravityProperty, value);
                //if (_scrollAdapter != null) _scrollAdapter.ScrollStopDelta = 20.0;
            }
        }

        public int SmoothScrollingSteps
        {
            get { return (int) GetValue(SmoothScrollingStepsProperty); }
            set { SetValue(SmoothScrollingStepsProperty, value); }
        }

        public bool AutoStretchItemsToFillSlots
        {
            get { return (bool) GetValue(AutoStretchItemsToFillSlotsProperty); }
            set { SetValue(AutoStretchItemsToFillSlotsProperty, value); }
        }

        public bool ItemToolbarAlwaysActive
        {
            get { return (bool) GetValue(ItemToolbarAlwaysActiveProperty); }
            set { SetValue(ItemToolbarAlwaysActiveProperty, value); }
        }

        public Orientation Orientation
        {
            get { return (Orientation) GetValue(OrientationProperty); }
            set
            {
                SetValue(OrientationProperty, value);
                ShowItems();
            }
        }

        #endregion

        #region Layout controls

        private readonly SmartGrid _flexGrid = new SmartGrid(); //FlexGrid();

        private readonly HyperlinkButton _nextButton = new HyperlinkButton
                                                           {
                                                               Content = "еще посты",
                                                               FontSize = 24,
                                                               Style =
                                                                   (Style)
                                                                   Application.Current.Resources["PageHyperlinkButtonStyle"
                                                                       ]
                                                           };

        private readonly ScrollViewer _scrollViewer = new ScrollViewer();

        #endregion

        #region Local variables

      //  private readonly DragScrollViewerAdaptor _scrollAdapter;
        private int _indexOfLastPost;
        private int _lastFreeColumn = 1;
        private int _lastFreeRow;

        // эти параметры нужны для корректировки предыдущего элемента, если он меньше на минимальный размер контрола в панели, а следующий уходит на следующую колонку. Тогда этот предыдущий мы слегка растянем
        private FrameworkElement _previousElement;
        private int _previousSpan;
        // шаг скроллинга при движении колесом мыши
        private int _scrollAmount = 3;
        private bool isLoaded;
        private double previousHorizontalScrollOffset;

        private double ContentRowHeight
        {
            get
            {
                return
                    ((Orientation == Orientation.Horizontal ? _scrollViewer.ActualHeight : _scrollViewer.ActualWidth) -
                     (Orientation == Orientation.Horizontal
                          ? (DataMargin.Top + DataMargin.Bottom)
                          : (DataMargin.Left + DataMargin.Right)))
                    /RowsForContent;
            }
        }

        private bool IsHorizontal
        {
            get { return Orientation == Orientation.Horizontal; }
        }

        #endregion

        #region Events

        public RoutedEventHandler OnItemClicked = null; // клик на контроле
        public RoutedEventHandler OnItemLongClicked = null; // длинный клик на контроле
        public RoutedEventHandler OnItemLongHold = null; // удерживание контрола

        public RoutedEventHandler OnNeedNewData = null;
                                  // если мы приблизились к концу локального списка, райзим событие, что нужны еще данные, а уж тот, кто перехватит событие, пусть думает, что делать дальше

        #endregion

        #region Layout methods

        private void ClearLayout()
        {
            HorizontalScrollOffset = 0;
            _scrollViewer.ScrollToHorizontalOffset(0);
            //if (ItemsSource != null  && ItemsSource.Count > 0)
            {
                // очистим стенку
                while (_flexGrid.ColumnDefinitions.Count > 0)
                    DeleteColumn(0);
                if (IsHorizontal) AddColumn(true);
                else AddRow(true); // отступ
                return;
            }
        }

        private void InitializeLayout()
        {
            _flexGrid.ColumnDefinitions.Clear();
            _flexGrid.RowDefinitions.Clear();
            _flexGrid.Width = 0;
            _flexGrid.Height = 0;
            _lastFreeColumn = 1;
            _lastFreeRow = 0;

            if (IsHorizontal) AddColumn(true);
            else AddRow(true); // отступ
            for (int i = 0; i < RowsForContent; i++)
                if (IsHorizontal) AddRow();
                else AddColumn();

            if (ItemsSource == null || ItemsSource.Count == 0) return;

            if (IsHorizontal)
            {
                _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                _scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                _scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
            // _scrollViewer.LayoutUpdated += (s, e) => ScrollViewerStopedScrolling();

            #region Скроллинг мышкой

            var mouseWeelSupport = new ThrottledMouseWheelEvent(_scrollViewer);
            mouseWeelSupport.ThrottledMouseWheel += mouseWeelSupport_ThrottledMouseWheel;

            #endregion
        }

        private void mouseWeelSupport_ThrottledMouseWheel(object sender, ThrottledMouseWheelEventArgs e)
        {
            int delta = e.Delta;

            delta *= _scrollAmount;

            double newOffset = IsHorizontal
                                   ? _scrollViewer.HorizontalOffset - delta
                                   : _scrollViewer.VerticalOffset - delta;

            double length = (IsHorizontal
                                 ? _scrollViewer.ScrollableWidth
                                 : _scrollViewer.ScrollableHeight);
            if (newOffset > length)
                newOffset = length;
            else if (newOffset < 0)
                newOffset = 0;

            if (IsHorizontal)
                _scrollViewer.HorizontalSmoothScrollTo(newOffset, 5, ScrollViewerStopedScrolling);
            else
                _scrollViewer.ScrollToVerticalOffset(newOffset);

            //e.Handled = true;
        }

        private void ShowItems()
        {
            if (!isLoaded) return;
            InitializeLayout();
            // контролы
            _indexOfLastPost = 0;
            ShowNextPage();
            // отдельно вызываем виртуализацию, так как бывают ситуации, когда после переключений экранов и восстановления нового экрана не возникает событие скроллинга, на котором висит виртуализация
            VirtualizeItems();
        }

        /// <summary>
        /// Вывод на стенку новую страницу, то есть ItemsOnPage айтемов или вполовину меньше
        /// </summary>
        /// <param name="largeStep">полная страница или вполовину меньше</param>
        private void ShowNextPage(bool largeStep = true)
        {
            int cnt = 0;
            if (ItemsSource == null || _indexOfLastPost >= ItemsSource.Count)
            {
                if (OnNeedNewData != null)
                    OnNeedNewData(this, null); // если мы подошли к концу списка - райзим событие о подкачке данных
                return;
            }
            for (int i = _indexOfLastPost; i < ItemsSource.Count; i++)
            {
                ContentWallPanelItem element = ItemsSource[i];
                if (cnt >= (largeStep ? ItemsOnPage : ItemsOnPage/2)) break;
                PrepareSlotsForControl(element);
                element.Margin = ItemsMargin;
                element.ToolbarAlwaysActive = ItemToolbarAlwaysActive;
                _flexGrid.AddChild(element);
                _indexOfLastPost++;
                cnt++;
            }

            if (IsHorizontal) AddColumn();
            else AddRow(); // отступ
            if (_indexOfLastPost >= ItemsSource.Count && OnNeedNewData != null)
                OnNeedNewData(this, null); // если мы подошли к концу списка - райзим событие о подкачке данных
        }


        private void CheckForUpdates()
        {
            if (HorizontalScrollOffset > _scrollViewer.ScrollableWidth - (_scrollViewer.ScrollableWidth*0.2))
                ShowNextPage(false);
            //CheckItemsToHide();
            if (previousHorizontalScrollOffset != HorizontalScrollOffset)
            {
                VirtualizeItems();
            }
            previousHorizontalScrollOffset = HorizontalScrollOffset;
        }

        /// <summary>
        /// виртуализировать данные (спрятать те, которые не показываются)
        /// </summary>
        public void VirtualizeItems()
        {
            if (ItemsSource == null) return;
            Size containerSize = Application.Current.RootVisual.RenderSize;
            foreach (ContentWallPanelItem item in ItemsSource)
            {
                if (item.Content == null) continue;
                Rect pos = SimplePanel.GetSlot(item);


                if (
                    (pos.X >= HorizontalScrollOffset
                     && pos.X < containerSize.Width + HorizontalScrollOffset
                    ) ||
                    (
                        pos.X + pos.Width > HorizontalScrollOffset
                        && pos.X + pos.Width < containerSize.Width + HorizontalScrollOffset
                    )
                    )
                    item.Visibility = Visibility.Visible;
                else
                    item.Visibility = Visibility.Collapsed;
            }
        }

        private void PrepareSlotsForControl(FrameworkElement element, int Span = 0)
        {
            int span = Span > 0 ? Span : getSpanBySize((ContentWallPanelItem) element);
            if ((IsHorizontal ? (_lastFreeRow + span) : (_lastFreeColumn + span)) >
                (IsHorizontal ? _flexGrid.RowDefinitions.Count : _flexGrid.ColumnDefinitions.Count))
            {
                // если мы уходим на новую колонку, проверим, как размещен предыдущий контрол. Если до конца колонки не хватает чуть-чуть (максимум - минимальное значение контрола в гриде - Minimum), то мы его растянем
                if (_previousElement != null && AutoStretchItemsToFillSlots)
                    if (IsHorizontal && ((RowsForContent - _lastFreeRow) <= RowsForContent/3))
                    {
                        _previousElement.SetRowSpan(_previousSpan + (RowsForContent - _lastFreeRow));
                        _previousSpan += (RowsForContent - _lastFreeRow);
                    }
                    else if (!IsHorizontal && ((RowsForContent - _lastFreeColumn) <= RowsForContent/3))
                    {
                        _previousElement.SetColumnSpan(_previousSpan + (RowsForContent - _lastFreeColumn));
                        _previousSpan += (RowsForContent - _lastFreeColumn);
                    }

                _lastFreeRow = IsHorizontal ? 0 : _lastFreeRow + 1;
                _lastFreeColumn = IsHorizontal ? _lastFreeColumn + 1 : 0;
                if (IsHorizontal)
                {
                    var cwpi = (ContentWallPanelItem) element;
                    if (cwpi != null)
                    {
                        if (cwpi.contentWidthOrHeight == ContentSize.Custom)
                            AddColumn(false, cwpi.Width);
                        else if (cwpi.contentWidthOrHeight == ContentSize.Auto && cwpi.CalculateWidthOrHeight != null)
                            AddColumn(false, cwpi.CalculateWidthOrHeight());
                        else
                            AddColumn();
                    }
                    else
                        AddColumn();
                }
                else
                {
                    var cwpi = (ContentWallPanelItem) element;
                    if (cwpi != null)
                    {
                        if (cwpi.contentWidthOrHeight == ContentSize.Custom)
                            AddRow(false, cwpi.Height);
                        else if (cwpi.contentWidthOrHeight == ContentSize.Auto && cwpi.CalculateWidthOrHeight != null)
                            AddRow(false, cwpi.CalculateWidthOrHeight());
                        else
                            AddRow();
                    }
                    else
                        AddRow();
                }
            }
            element.SetColumn(_lastFreeColumn).SetRow(_lastFreeRow);
            if (IsHorizontal)
                element.SetRowSpan(span);
            else
                element.SetColumnSpan(span);


            if (IsHorizontal)
                _lastFreeRow = _lastFreeRow + span;
            else
                _lastFreeColumn = _lastFreeColumn + span;
            _previousElement = element;
            _previousSpan = span;
            if (element != _nextButton) return;
            _lastFreeRow = IsHorizontal ? _lastFreeRow - 1 : _lastFreeRow;
            _lastFreeRow = IsHorizontal && _lastFreeRow < 0 ? 0 : _lastFreeRow;
            _lastFreeColumn = !IsHorizontal ? _lastFreeColumn - 1 : _lastFreeColumn;
            _lastFreeColumn = !IsHorizontal && _lastFreeColumn < 0 ? 0 : _lastFreeColumn;
        }

        private void AddColumn(bool isFirstColumn = false, double customSize = 0.0)
        {
            if (ContentItemLength < 0) return;
            double len = customSize > 0.0
                             ? customSize
                             : ContentItemLength*(isFirstColumn ? LeftMarginRatio : 1.0);
            _flexGrid.ColumnDefinitions.Add(new LineDefinition(len));
            _flexGrid.Width += len;
        }

        private void AddRow(bool isFirstRow = false, double customSize = 0.0)
        {
            if (ContentRowHeight < 0) return;
            double len = customSize > 0
                             ? customSize
                             : ContentRowHeight*(isFirstRow ? LeftMarginRatio : 1.0);
            _flexGrid.RowDefinitions.Add(new LineDefinition(len));
            _flexGrid.Height += len;
        }


        /// <summary>
        /// добавление поста в конце ленты
        /// </summary>
        /// <param name="element"></param>
        public void AddOldPost(ContentWallPanelItem element)
        {
            PrepareSlotsForControl(element);
            element.Margin = ItemsMargin; // new Thickness(10, 3, 5, 3);
            _flexGrid.Children.Add(element);
            ItemsSource.Add(element);
            _indexOfLastPost++;
            //ShowNextPage();
        }

        /// <summary>
        ///  вставка нового поста в начало ленты
        /// </summary>
        /// <param name="element"></param>
        public void InsertNewPost(ContentWallPanelItem element, int Span = 0)
        {
            if (ItemsSource == null) return;
            if (_flexGrid.ColumnDefinitions.Count == 0 || _flexGrid.RowDefinitions.Count == 0)
                InitializeLayout();
            int span = Span > 0 ? Span : getSpanBySize(element);
            // надйем свободное место в нулевой колоннке
            // айтемы на нулевой колонке
            IEnumerable<ContentWallPanelItem> itemOnFirstPosition =
                (from c in ItemsSource where c.GetCoordinates().Column == 1 select c);
            // вычислим занятые слоты
            int availableSpan =
                itemOnFirstPosition.Sum(
                    post => IsHorizontal ? post.GetCoordinates().RowSpan : post.GetCoordinates().ColumnSpan);
                // свободные слоты
            // свободные слоты
            availableSpan = RowsForContent - availableSpan;
            // если свободнх слотов не хватает, вставляем новую колонку
            if (availableSpan < span)
                PrepareForInsertion(0, 1.0);
            else
            {
                // сдвинем посты по колонке вниз (или по строке вправо), чтобы вверху вставить наш пост
                foreach (ContentWallPanelItem post in itemOnFirstPosition)
                {
                    ContentWallPanelItemCoordinates coord = post.GetCoordinates();
                    post.SetValue(IsHorizontal ? FlexGrid.RowProperty : FlexGrid.ColumnProperty,
                                  (IsHorizontal ? coord.Row : coord.Column) + span);
                }
            }
            // всунуть в грид
            PutControlAndPostInsertionAction(0, element, span);
        }

        public void InsertColumn(int columnPosition, ContentWallPanelItem element, double lengthScaleFactor = 1.5)
        {
            if (element == null) return;
            double otherLength = 0.0;
            if (element.contentWidthOrHeight == ContentSize.Custom)
                otherLength = element.Width;
            else if (element.contentWidthOrHeight == ContentSize.Auto && element.CalculateWidthOrHeight != null)
                otherLength = element.CalculateWidthOrHeight();

            double calculatedColumnSize = PrepareForInsertion(columnPosition, lengthScaleFactor, false, otherLength);
            PutControlAndPostInsertionAction(columnPosition, element, RowsForContent, calculatedColumnSize, false);
        }

        /// <summary>
        /// положить контрол на грид, скорректировать все списки, позиции и проскроллинговаться к посту
        /// </summary>
        /// <param name="columnPosition"></param>
        /// <param name="element"></param>
        private void PutControlAndPostInsertionAction(int columnPosition, ContentWallPanelItem element, int span,
                                                      double calculatedColumnSize = 0, bool doAnimatedResize = false)
        {
            element.SetColumn(columnPosition + 1).SetRow(0).SetRowSpan(span);
            element.Margin = ItemsMargin; // new Thickness(10, 3, 5, 3);
            _flexGrid.Children.Add(element);
            ItemsSource.Insert(columnPosition, element);
            _indexOfLastPost++;
            ScrollToElement(element);
            if (doAnimatedResize)
                _flexGrid.ResizeColumnTo(columnPosition, calculatedColumnSize);
        }

        public void ScrollToElement(ContentWallPanelItem element, ScrollToPosition to = ScrollToPosition.Center)
        {
            _flexGrid.InvalidateMeasure();
            _flexGrid.RunOnFirstArrange(i =>
                                            {
                                                if (!ItemsSource.Contains(element)) return;
                                                Rect pos = element.GetSlot();
                                                double newPosition =
                                                    to == ScrollToPosition.Center
                                                        ? pos.X - (_scrollViewer.ActualWidth/2 - pos.Width/2)
                                                        : pos.X;
                                                _scrollViewer.HorizontalSmoothScrollTo(newPosition,
                                                                                       SmoothScrollingSteps);
                                            });
        }

        public void ScrollToElementFast(ContentWallPanelItem element, ScrollToPosition to = ScrollToPosition.Center)
        {
            _flexGrid.InvalidateMeasure();
            _flexGrid.RunOnFirstArrange(i =>
                                            {
                                                if (!ItemsSource.Contains(element)) return;
                                                Rect pos = element.GetSlot();
                                                double newPosition =
                                                    to == ScrollToPosition.Center
                                                        ? pos.X - (_scrollViewer.ActualWidth/2 - pos.Width/2)
                                                        : 0.0;
                                                _scrollViewer.ScrollToHorizontalOffset(newPosition);
                                                _scrollViewer.UpdateLayout();
                                            });
        }

        public Rect GetElementRect(ContentWallPanelItem element)
        {
            Rect rect = element.GetSlot();
            rect.X = rect.X - HorizontalScrollOffset;
            return rect;
        }

        /// <summary>
        /// реальная вставка колонки и раздвижка грида. Для гризонтального скроллинга, по вертикали тут упустил.
        /// </summary>
        /// <param name="columnPosition"></param>
        /// <param name="lengthScaleFactor"></param>
        /// <param name="initialZeroSize">если true, то размер колонки изначально будет равен нулю</param>
        private double PrepareForInsertion(int columnPosition, double lengthScaleFactor, bool initialZeroSize = false,
                                           double fixedOtherLength = 0.0)
        {
            double insertionLength = fixedOtherLength > 0.0 ? fixedOtherLength : ContentItemLength*lengthScaleFactor;
            var ld = new LineDefinition(initialZeroSize ? 0.0 : insertionLength);
            _flexGrid.ColumnDefinitions.Insert(columnPosition + 1, ld);
            _flexGrid.Width += insertionLength;
            foreach (ContentWallPanelItem contentWallPanelItem in ItemsSource)
            {
                ContentWallPanelItemCoordinates coord = contentWallPanelItem.GetCoordinates();
                if (coord.Column > columnPosition)
                    contentWallPanelItem.SetColumn(coord.Column + 1);
            }
            if (IsHorizontal) _lastFreeColumn++;
            else _lastFreeRow++;

            return insertionLength;
        }

        /// <summary>
        /// удалить контрол и если он был единственный в колонке - колонку
        /// </summary>
        /// <param name="whereItemExists"></param>
        public void DeleteControl(ContentWallPanelItem item)
        {
            int column = item.GetCoordinates().Column;
            if (ItemsSource.Count(c => c.GetCoordinates().Column == column) == 1)
                // на этой колонке контрол был один, значит просто удаляем колонку
            {
                DeleteColumn(column);
            }
            else // иначе удаляем контрол, но не удаляем колонку
            {
                _flexGrid.RemoveChild(item);
                ItemsSource.Remove(item);
                _indexOfLastPost--;
            }
        }

        public void DeleteColumn(ContentWallPanelItem whereItemExists)
        {
            if (!ItemsSource.Contains(whereItemExists)) return;
            DeleteColumn(whereItemExists.GetCoordinates().Column, true);
        }

        public void DeleteColumn(int columnPosition, bool animate = false)
        {
            if (ItemsSource != null && ItemsSource.Count > 0)
            {
                // удаляем наши привязанные контролы к определенной колонке
                while ((ItemsSource.Where(c => c.GetCoordinates().Column == columnPosition)).Any())
                {
                    ContentWallPanelItem itemToRemove =
                        ItemsSource.FirstOrDefault(c => c.GetCoordinates().Column == columnPosition);
                    if (itemToRemove == null) continue;
                    _flexGrid.RemoveChild(itemToRemove);
                    ItemsSource.Remove(itemToRemove);
                    _indexOfLastPost--;
                }
            }
            // теперь сделаем то же самое для контролов не из ItemsSource
            // удаляем привязанные контролы к определенной колонке
            while ((_flexGrid.Children.Where(c => (c.GetColumn()) == columnPosition)).Any())
            {
                FrameworkElement itemToRemove = _flexGrid.Children.FirstOrDefault(c => (c.GetColumn()) == columnPosition);
                if (itemToRemove != null) _flexGrid.RemoveChild(itemToRemove);
            }

            // сдвигаем контролы справа, налево
            if (ItemsSource != null)
            {
                foreach (ContentWallPanelItem contentWallPanelItem in ItemsSource)
                {
                    ContentWallPanelItemCoordinates coord = contentWallPanelItem.GetCoordinates();
                    if (coord.Column > columnPosition) contentWallPanelItem.SetColumn(coord.Column - 1);
                }
            }

            LineDefinition ld = _flexGrid.ColumnDefinitions[columnPosition];
            _flexGrid.ColumnDefinitions.Remove(ld);
            _flexGrid.Width -= _flexGrid.Width > ld.AbsoluteValue ? ld.AbsoluteValue : _flexGrid.Width;
            //ld.Length.Value ? ld.Length.Value : _flexGrid.Width;// здесь так сделано из-за того, что из-за мелочи после запятой может вывалиться
            if (IsHorizontal) _lastFreeColumn--;
            else _lastFreeRow--;
            _flexGrid.RunOnFirstArrange(i => VirtualizeItems());
            _flexGrid.InvalidateMeasure();
        }

        private int getSpanBySize(ContentWallPanelItem element)
        {
            int span = 0;
            switch (element.ContentSize)
            {
                case ContentSize.Small:
                    span = RowsForContent/3;
                    break;
                case ContentSize.Medium:
                    span = RowsForContent/2;
                    break;
                case ContentSize.Large:
                    span = RowsForContent;
                    break;
                case ContentSize.Auto:
                    double length = IsHorizontal ? element.GetRealHeight() : element.GetRealWidth();
                    if (!length.IsValid() || element.CalculateLength != null)
                    {
                        if (element.CalculateLength != null)
                            span = getSpanByLength(element.CalculateLength());
                        else
                            span = RowsForContent/2;
                    }
                    else
                        span = getSpanByLength(length);
                    break;
            }
            return span;
        }


        private int getSpanByLength(double length)
        {
            var _span = (int) (RowsForContent/(RowsForContent*ContentRowHeight/length));
            _span = (_span < 1) ? 1 : _span;
            _span = (_span > RowsForContent) ? RowsForContent : _span;
            return _span;
        }

        /// <summary>
        /// щелкнули внутри скроллинга
        /// </summary>
        /// <param name="point"></param>
        private void WallPanelItemClicked(Point point)
        {
            if (OnItemClicked != null)
            {
                ContentWallPanelItem item = FindControlByPoint(point);
                if (item != null) OnItemClicked(this, new WallPanelItemEvent {Item = item});
            }
        }

        /// <summary>
        /// длинный щелчок внутри скроллинга
        /// </summary>
        /// <param name="point"></param>
        private void WallPanelItemLongClicked(Point point)
        {
            if (OnItemLongClicked != null)
            {
                ContentWallPanelItem item = FindControlByPoint(point);
                if (item != null) OnItemLongClicked(this, new WallPanelItemEvent {Item = item});
            }
        }

        /// <summary>
        /// схватили что-то внутри скроллинга
        /// </summary>
        /// <param name="point"></param>
        private void WallPanelItemOnHold(Point point)
        {
            if (OnItemLongHold != null)
            {
                ContentWallPanelItem item = FindControlByPoint(point);
                if (item != null) OnItemLongHold(this, new WallPanelItemEvent {Item = item});
            }
        }

        /// <summary>
        /// найти контрол на стенке по указанной координате
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private ContentWallPanelItem FindControlByPoint(Point point)
        {
            if (ItemsSource == null) return null;
            {
                foreach (ContentWallPanelItem item in ItemsSource)
                    try
                    {
                        if (item.Content == null) continue;
                        GeneralTransform gt = item.TransformToVisual(_scrollViewer); //Application.Current.RootVisual);
                        Point pos = gt.Transform(new Point(0, 0));
                        var rectangleSize = new Rect(pos.X, pos.Y, item.ActualWidth, item.ActualHeight);
                        var cursorPoint = new Rect(point.X, point.Y, 1, 1);
                        cursorPoint.Intersect(rectangleSize);
                        if (!cursorPoint.IsEmpty) return item;
                    }
                    catch
                    {
                    }
            }
            return null;
        }

        /// <summary>
        /// Произошла остановка скроллинга
        /// </summary>
        private void ScrollViewerStopedScrolling()
        {
            if (UseCenteredGravity)
                MakeCenterGravity();
            else if (UseLeftGravity)
                MakeLeftGravity();
        }

        /// <summary>
        /// применить гравитацию по центру
        /// </summary>
        private void MakeCenterGravity()
        {
            Size containerSize = Application.Current.RootVisual.RenderSize;
            double centerX = HorizontalScrollOffset + containerSize.Width/2; // центр стены
            double minDiff = centerX; // минимальное расстояние сркли контролов до центра экрана
            ContentWallPanelItem bestItem = null; // контрол с наиболее подходящим расстоянием
            foreach (ContentWallPanelItem item in ItemsSource.Where(_ => _.Visibility == Visibility.Visible))
            {
                if (item.Content == null) continue;
                Rect pos = SimplePanel.GetSlot(item);
                double itemCenter = pos.X + pos.Width/2;
                double diff = Math.Abs(centerX - itemCenter);
                if (diff < minDiff)
                {
                    minDiff = diff;
                    bestItem = item;
                }
            }
            if (bestItem != null) ScrollToElement(bestItem);
        }

        /// <summary>
        /// применить гравитацию по левому краю
        /// </summary>
        private void MakeLeftGravity()
        {
            double leftX = HorizontalScrollOffset; // левый край стены
            double minDiff = leftX; // минимальное расстояние сркли контролов до центра экрана
            ContentWallPanelItem bestItem = null; // контрол с наиболее подходящим расстоянием
            foreach (ContentWallPanelItem item in ItemsSource.Where(_ => _.Visibility == Visibility.Visible))
            {
                if (item.Content == null) continue;
                Rect pos = SimplePanel.GetSlot(item);
                double itemLeft = pos.X;
                double diff = Math.Abs(leftX - itemLeft);
                if (diff <= minDiff)
                {
                    minDiff = diff;
                    bestItem = item;
                }
            }
            if (bestItem != null) ScrollToElement(bestItem, ScrollToPosition.LeftCorner);
        }

        #endregion

        #region ScrollToPosition enum

        public enum ScrollToPosition
        {
            LeftCorner,
            Center
        }

        #endregion

        public FlexContentWallPanel()
        {
            _scrollViewer.BorderBrush =
                _scrollViewer.Background = _flexGrid.Background = new SolidColorBrush(Colors.Transparent);
            _flexGrid.DragEnabled = false;
            Content = _scrollViewer;
            _scrollViewer.Content = _flexGrid;
            /*
            _scrollAdapter = new DragScrollViewerAdaptor(_scrollViewer) {Friction = ScrollFriction};
            // клик где-то внутри скроллера
            _scrollAdapter.ScrollViewerClicked +=
                (scroller, args) => WallPanelItemClicked(((DragScrollViewerAdaptorMouseClick) args).ClickPoint);
            // длинный щелчок внутри скроллинга
            _scrollAdapter.ScrollViewerLongClicked +=
                (scroller, args) => WallPanelItemLongClicked(((DragScrollViewerAdaptorMouseClick) args).ClickPoint);
            // схватили и держим что-то внутри скроллера
            _scrollAdapter.ScrollViewerLongHold +=
                (scroller, args) => WallPanelItemOnHold(((DragScrollViewerAdaptorMouseClick) args).ClickPoint);
            // произошла остановка скроллинга
            _scrollAdapter.ScrollViewerStopedScrolling +=
                (scroller, args) => ScrollViewerStopedScrolling();
             */
            Loaded += (s, e) =>
                          {
                              isLoaded = true;
                              ShowItems();
                          };

            _scrollViewer.LayoutUpdated += (s, e) => { HorizontalScrollOffset = _scrollViewer.HorizontalOffset; };
        }
    }


    public class WallPanelItemEvent : RoutedEventArgs
    {
        public ContentWallPanelItem Item { get; set; }
    }
}