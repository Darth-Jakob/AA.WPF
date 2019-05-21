using System.Windows;
using System.Windows.Controls;

namespace AA.WPF.Controls
{
    [TemplatePart(Name="PART_ltButton", Type= typeof(Button))]
    [TemplatePart(Name="PART_RtButton", Type= typeof(Button))]
    public class SpinNumericTextBox : Control
    {
        static SpinNumericTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpinNumericTextBox), new FrameworkPropertyMetadata(typeof(SpinNumericTextBox)));

        }

        #region Dependency property
        
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(SpinNumericTextBox), new PropertyMetadata(0));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(int?), typeof(SpinNumericTextBox));
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(int?), typeof(SpinNumericTextBox));

        #endregion

        #region property
        
        
        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        public int? MaxValue
        {
            get
            {
                return (int?)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        public int? MinValue
        {
            get
            {
                return (int?)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        #endregion

        #region Override

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Button ltBtn = this.GetTemplateChild("PART_ltButton") as Button;
            if (ltBtn != null)
            {
                ltBtn.Click += ltBtn_Click;
            }
            Button RtBtn = this.GetTemplateChild("PART_RtButton") as Button;
            if (RtBtn != null)
            {
                RtBtn.Click += RtBtn_Click;
            }
        }

        #endregion

        #region eventclick

        void RtBtn_Click(object sender, RoutedEventArgs e)
        {
                if (MaxValue != null && MaxValue <= Value)
                {
                    Value = (int)MaxValue;
                }
                else
                {
                    Value++;
            }
        }

        void ltBtn_Click(object sender, RoutedEventArgs e)
        {
                if (MinValue != null && MinValue >=  Value)
                {
                    Value = (int)MinValue;
                }
                else
                {
                    Value--;
                }
            }

        #endregion
    }
}
