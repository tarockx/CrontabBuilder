using CronExpressionDescriptor;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace MasterT.WPF.CrontabBuilder
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class CrontabEditorControl : UserControl, INotifyPropertyChanged
    {
        private CrontabExpressionModel _crontabExpression;
        public CrontabExpressionModel CrontabExpression
        {
            get => _crontabExpression;
            set
            {
                _crontabExpression = value;
                OnPropertyChanged(nameof(CrontabExpression));
            }
        }

        private bool pauseCrontabStringMonitoring;

        public string CrontabString
        {
            get { return (string)GetValue(CrontabStringProperty); }
            set { SetValue(CrontabStringProperty, value); }
        }

        public static readonly DependencyProperty CrontabStringProperty = DependencyProperty.Register("CrontabString", typeof(string), typeof(CrontabEditorControl), new PropertyMetadata(null, CrontabString_Changed));



        public bool ShowCurrentCrontab
        {
            get { return (bool)GetValue(ShowCurrentCrontabProperty); }
            set { SetValue(ShowCurrentCrontabProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowCurrentCrontab.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCurrentCrontabProperty = DependencyProperty.Register("ShowCurrentCrontab", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));



        public bool ShowCurrentCrontabDescription
        {
            get { return (bool)GetValue(ShowCurrentCrontabDescriptionProperty); }
            set { SetValue(ShowCurrentCrontabDescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowCurrentCrontabDescription.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowCurrentCrontabDescriptionProperty = DependencyProperty.Register("ShowCurrentCrontabDescription", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));




        public bool ShowInfoOutsideMainScroller
        {
            get { return (bool)GetValue(ShowInfoOutsideMainScrollerProperty); }
            set { SetValue(ShowInfoOutsideMainScrollerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowInfoOutsideMainScroller.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowInfoOutsideMainScrollerProperty = DependencyProperty.Register("ShowInfoOutsideMainScroller", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(false, ShowInfoOutsideMainScroller_Changed));

        private static void ShowInfoOutsideMainScroller_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ce = (d as CrontabEditorControl);
            var item = ce.gbDescriptions;
            
            if((bool)e.NewValue)
            {
                ce.stackGroups.Children.Remove(item);
                item.SetValue(Grid.RowProperty, 2);
                ce.mainGrid.Children.Add(item);
            }
            else
            {
                ce.mainGrid.Children.Remove(item);
                ce.stackGroups.Children.Add(item);
            }
        }

        public CrontabEditorControl() : this(null)
        {
        }

        public event EventHandler<string> CrontabStringChanged;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public CrontabEditorControl(string crontab)
        {
            InitializeComponent();

            CrontabString = crontab;
        }

        private void CrontabExpression_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(CrontabExpression.OutputCrontabString))
            {
                CrontabHelper.RecalculateCrontab(CrontabExpression);
                ComputeDescription();

                pauseCrontabStringMonitoring = true;
                CrontabString = CrontabExpression.OutputCrontabString;
                pauseCrontabStringMonitoring = false;


                CrontabStringChanged?.Invoke(this, CrontabString);
            }
        }

        private static void CrontabString_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CrontabEditorControl owner = d as CrontabEditorControl;
            if (!owner.pauseCrontabStringMonitoring)
            {
                owner.UpdateExpression(e.NewValue as string, owner.CrontabExpression?.CustomMode ?? false);            

            }
        }

        private void UpdateExpression(string crontab, bool customMode)
        {            
            if(CrontabExpression != null)
            {
                CrontabExpression.PropertyChanged -= CrontabExpression_PropertyChanged;
            }
            CrontabExpression = crontab == null ? new CrontabExpressionModel() { CustomMode = customMode} : CrontabHelper.Parse(crontab, customMode);
            CrontabExpression.PropertyChanged += CrontabExpression_PropertyChanged;
            ComputeDescription();
        }

        private void ComputeDescription()
        {
            try
            {
                string description = ExpressionDescriptor.GetDescription(CrontabExpression.OutputCrontabString, new Options()
                {
                    ThrowExceptionOnParseError = true,
                    DayOfWeekStartIndexZero = false,
                    Use24HourTimeFormat = true,
                    Locale = "en"
                });
                txtDescriptor.Text = description;
                txtCrontab.Text = CrontabExpression.OutputCrontabString;
            }
            catch (Exception ex)
            {
                txtDescriptor.Text = "Invalid expression";
                txtCrontab.Text = "N/A";
            }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/crontrigger.html");
        }
    }
}
