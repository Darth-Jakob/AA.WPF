using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AA.WPF.Controls
{
    public class OutlineTextBlock : FrameworkElement
    {
        private Geometry _textGeometry;

        #region Depdendency Properties
        public static readonly DependencyProperty TextProperty =
                        DependencyProperty.Register("Text", typeof(string), typeof(OutlineTextBlock),
                            new FrameworkPropertyMetadata(string.Empty,
                                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                   OnPropertyChanged));

        public static readonly DependencyProperty OriginPointProperty =
                        DependencyProperty.Register("Origin", typeof(Point), typeof(OutlineTextBlock),
                            new FrameworkPropertyMetadata(new Point(0, 0),
                                FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontFamilyProperty = TextElement.FontFamilyProperty.AddOwner(typeof(OutlineTextBlock),
                               new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontSizeProperty = TextElement.FontSizeProperty.AddOwner(typeof(OutlineTextBlock),
                                new FrameworkPropertyMetadata(SystemFonts.MessageFontSize,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontStretchProperty = TextElement.FontStretchProperty.AddOwner(typeof(OutlineTextBlock),
                               new FrameworkPropertyMetadata(TextElement.FontStretchProperty.DefaultMetadata.DefaultValue,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontStyleProperty = TextElement.FontStyleProperty.AddOwner(typeof(OutlineTextBlock),
                               new FrameworkPropertyMetadata(SystemFonts.MessageFontStyle,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty FontWeightProperty = TextElement.FontWeightProperty.AddOwner(typeof(OutlineTextBlock),
                               new FrameworkPropertyMetadata(SystemFonts.MessageFontWeight,
                                   FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                   OnPropertyChanged));

        public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register("Stroke", typeof(Brush), typeof(OutlineTextBlock),
                                new FrameworkPropertyMetadata(new SolidColorBrush(Colors.Black),
                                    FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                    OnPropertyChanged));
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(ushort), typeof(OutlineTextBlock),
                                new FrameworkPropertyMetadata((ushort)0, FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                OnPropertyChanged));

        public static readonly DependencyProperty FillProperty = DependencyProperty.Register("Fill", typeof(Brush), typeof(OutlineTextBlock),
                                new FrameworkPropertyMetadata(new SolidColorBrush(Colors.LightSteelBlue), FrameworkPropertyMetadataOptions.AffectsRender | FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.Inherits,
                                OnPropertyChanged));

        #endregion

        #region Property Accessors

        public Brush Fill
        {
            get { return (Brush)GetValue(FillProperty); }
            set { SetValue(FillProperty, value); }
        }

        public ushort StrokeThickness
        {
            get { return (ushort)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }

        public Brush Stroke
        {
            get { return (Brush)GetValue(StrokeProperty); }
            set { SetValue(StrokeProperty, value); }
        }

        public Point Origin
        {
            get { return (Point)GetValue(OriginPointProperty); }
            set { SetValue(OriginPointProperty, value); }
        }

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public FontStretch FontStretch
        {
            get { return (FontStretch)GetValue(FontStretchProperty); }
            set { SetValue(FontStretchProperty, value); }
        }

        public FontStyle FontStyle
        {
            get { return (FontStyle)GetValue(FontStyleProperty); }
            set { SetValue(FontStyleProperty, value); }
        }

        public FontWeight FontWeight
        {
            get { return (FontWeight)GetValue(FontWeightProperty); }
            set { SetValue(FontWeightProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((OutlineTextBlock)d).CreateTextGeometry();
        }

        private void CreateTextGeometry()
        {
            if (Text != null)
            {
                _textGeometry = null;
                var formattedText = new FormattedText(Text, Thread.CurrentThread.CurrentUICulture, FlowDirection.LeftToRight, new Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, Brushes.Black);
                _textGeometry = formattedText.BuildGeometry(Origin);
                _textGeometry.Freeze();
                this.MinWidth = formattedText.Width;
                this.MinHeight = formattedText.Height;
            }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            CreateTextGeometry();
            drawingContext.DrawGeometry(Fill, new Pen(Stroke, StrokeThickness), _textGeometry);
        }
    }
}