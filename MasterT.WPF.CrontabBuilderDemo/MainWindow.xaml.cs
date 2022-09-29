using System.Windows;

namespace MasterT.WPF.CrontabBuilderDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string CrontabString
        {
            get { return (string)GetValue(CrontabStringProperty); }
            set { SetValue(CrontabStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CrontabString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CrontabStringProperty = DependencyProperty.Register("CrontabString", typeof(string), typeof(MainWindow), new PropertyMetadata(""));



        public MainWindow()
        {
            InitializeComponent();
            CrontabString = "0 0 9 ? * THU#4 *";

        }
    }
}
