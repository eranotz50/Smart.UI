using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using Smart.UI.Classes.Extensions;

namespace Smart.UI.Controls
{
    [ContentProperty("Child")]
    public class SmartBorder : FrameworkElement
    {
        public static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register("BorderBrush", typeof (Brush), typeof (SmartBorder),
                                        new FrameworkPropertyMetadata(default(Brush),
                                                                      FrameworkPropertyMetadataOptions.
                                                                          AffectsParentArrange));

        public static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register("BorderThickness", typeof (Thickness), typeof (SmartBorder),
                                        new FrameworkPropertyMetadata(default(Thickness),
                                                                      FrameworkPropertyMetadataOptions.
                                                                          AffectsParentArrange));

        public static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register("BorderBrush",
                                                                                                   typeof (Brush),
                                                                                                   typeof (SmartBorder
                                                                                                       ),
                                                                                                   new FrameworkPropertyMetadata
                                                                                                       (default(Brush),
                                                                                                        FrameworkPropertyMetadataOptions
                                                                                                            .
                                                                                                            AffectsParentArrange));

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius",
                                                                                                     typeof (
                                                                                                         CornerRadius),
                                                                                                     typeof (
                                                                                                         SmartBorder),
                                                                                                     new FrameworkPropertyMetadata
                                                                                                         (default(
                                                                                                              CornerRadius
                                                                                                              ),
                                                                                                          FrameworkPropertyMetadataOptions
                                                                                                              .
                                                                                                              AffectsParentArrange));

        public static readonly DependencyProperty PaddingProperty = DependencyProperty.Register("Padding",
                                                                                                typeof (Thickness),
                                                                                                typeof (SmartBorder),
                                                                                                new FrameworkPropertyMetadata
                                                                                                    (default(Thickness),
                                                                                                     FrameworkPropertyMetadataOptions
                                                                                                         .
                                                                                                         AffectsParentArrange));

        private static readonly DependencyProperty ChildProperty = DependencyProperty.Register("Child",
                                                                                               typeof (FrameworkElement),
                                                                                               typeof (SmartBorder),
                                                                                               new FrameworkPropertyMetadata
                                                                                                   (null,
                                                                                                    FrameworkPropertyMetadataOptions
                                                                                                        .
                                                                                                        AffectsParentArrange));

        public Brush BorderBrush
        {
            get { return (Brush) GetValue(BorderBrushProperty); }
            set { SetValue(BorderBrushProperty, value); }
        }

        public Thickness BorderThickness
        {
            get { return (Thickness) GetValue(BorderThicknessProperty); }
            set { SetValue(BorderThicknessProperty, value); }
        }

        public Brush Background
        {
            get { return (Brush) GetValue(BackgroundProperty); }
            set { SetValue(BackgroundProperty, value); }
        }

        public CornerRadius CornerRadius
        {
            get { return (CornerRadius) GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public Thickness Padding
        {
            get { return (Thickness) GetValue(PaddingProperty); }
            set { SetValue(PaddingProperty, value); }
        }

        public UIElement Child
        {
            get { return (UIElement) GetValue(ChildProperty); }
            set { SetValue(ChildProperty, value); }
        }

        public Size SizeWithPadding
        {
            get
            {
                return Child != null
                           ? new Size(Padding.Left + Padding.Right + Child.DesiredSize.Width,
                                      Padding.Top + Padding.Bottom + Child.DesiredSize.Height)
                           : new Size(Padding.Left + Padding.Right, Padding.Top + Padding.Bottom);
            }
        }

        public Rect ChildBounds
        {
            get
            {
                return Child != null
                           ? new Rect(Padding.Left, Padding.Top, Child.DesiredSize.Width + Padding.Right,
                                      Child.DesiredSize.Height + Padding.Bottom)
                           : new Rect(Padding.Left, Padding.Top, Padding.Right, Padding.Bottom);
            }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            Size constrains = this.GetSize(availableSize);
            Child.Measure(constrains);
            return SizeWithPadding;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            Size constrains = this.GetSize(finalSize);
            Child.Arrange(ChildBounds);
            return SizeWithPadding;
        }
    }
}