using AA.WPF.MVVM;
using AA.WPF.MVVM.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AA.WPF.Solution
{
  
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= MainWindow_Loaded;
            this.DataContext = new MainVIewModel();

            ServiceLocator.Instance.RegisterService<TestService>(new TestService());
            ServiceLocator.Instance.RegisterService<TestService2>(new TestService2());
        }

        private void ServiceLocator_Button_Click(object sender, RoutedEventArgs e)
        {
            TestService service = ServiceLocator.Instance.GetService<TestService>();
            if (service != null)
            {
                //MessageBox.Show("Get TestService");
            }
        }

        private void ServiceLocator2_Button_Click(object sender, RoutedEventArgs e)
        {
            TestService2 service = ServiceLocator.Instance.GetService<TestService2>();
            if (service != null)
            {
                MessageBox.Show("Get TestService2");
            }
        }

      

        
        


       


    }


    public class TEST1Message : IMessage
    {
        public string Value { get; set; }
    }

    public class TEST2Message : IMessage
    {
        public string Value { get; set; }
    }
}
