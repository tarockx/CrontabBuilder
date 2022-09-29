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
        public static readonly DependencyProperty ShowCurrentCrontabProperty = DependencyProperty.Register("ShowCurrentCrontab", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));

        public bool ShowCurrentCrontabDescription
        {
            get { return (bool)GetValue(ShowCurrentCrontabDescriptionProperty); }
            set { SetValue(ShowCurrentCrontabDescriptionProperty, value); }
        }
        public static readonly DependencyProperty ShowCurrentCrontabDescriptionProperty = DependencyProperty.Register("ShowCurrentCrontabDescription", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));

        public bool ShowInfoOutsideMainScroller
        {
            get { return (bool)GetValue(ShowInfoOutsideMainScrollerProperty); }
            set { SetValue(ShowInfoOutsideMainScrollerProperty, value); }
        }
        public static readonly DependencyProperty ShowInfoOutsideMainScrollerProperty = DependencyProperty.Register("ShowInfoOutsideMainScroller", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(false, ShowInfoOutsideMainScroller_Changed));

        public bool ShowSecondsMode
        {
            get { return (bool)GetValue(ShowSecondsModeProperty); }
            set { SetValue(ShowSecondsModeProperty, value); }
        }
        public static readonly DependencyProperty ShowSecondsModeProperty = DependencyProperty.Register("ShowSecondsMode", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));

        public bool ShowMinutesMode
        {
            get { return (bool)GetValue(ShowMinutesModeProperty); }
            set { SetValue(ShowMinutesModeProperty, value); }
        }
        public static readonly DependencyProperty ShowMinutesModeProperty = DependencyProperty.Register("ShowMinutesMode", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));

        public bool ShowHoursMode
        {
            get { return (bool)GetValue(ShowHoursModeProperty); }
            set { SetValue(ShowHoursModeProperty, value); }
        }
        public static readonly DependencyProperty ShowHoursModeProperty = DependencyProperty.Register("ShowHoursMode", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));

        public bool ShowDayMode
        {
            get { return (bool)GetValue(ShowDayModeProperty); }
            set { SetValue(ShowDayModeProperty, value); }
        }
        public static readonly DependencyProperty ShowDayModeProperty = DependencyProperty.Register("ShowDayMode", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));

        public bool ShowWeekMode
        {
            get { return (bool)GetValue(ShowWeekModeProperty); }
            set { SetValue(ShowWeekModeProperty, value); }
        }
        public static readonly DependencyProperty ShowWeekModeProperty = DependencyProperty.Register("ShowWeekMode", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));

        public bool ShowNthDayOfWeekMode
        {
            get { return (bool)GetValue(ShowNthDayOfWeekModeProperty); }
            set { SetValue(ShowNthDayOfWeekModeProperty, value); }
        }
        public static readonly DependencyProperty ShowNthDayOfWeekModeProperty = DependencyProperty.Register("ShowNthDayOfWeekMode", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));

        public bool ShowMonthMode
        {
            get { return (bool)GetValue(ShowMonthModeProperty); }
            set { SetValue(ShowMonthModeProperty, value); }
        }
        public static readonly DependencyProperty ShowMonthModeProperty = DependencyProperty.Register("ShowMonthMode", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));

        public bool ShowLastDayOfMonthMode
        {
            get { return (bool)GetValue(ShowLastDayOfMonthModeProperty); }
            set { SetValue(ShowLastDayOfMonthModeProperty, value); }
        }
        public static readonly DependencyProperty ShowLastDayOfMonthModeProperty = DependencyProperty.Register("ShowLastDayOfMonthMode", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));

        public bool ShowCustomExpressionMode
        {
            get { return (bool)GetValue(ShowCustomExpressionModeProperty); }
            set { SetValue(ShowCustomExpressionModeProperty, value); }
        }
        public static readonly DependencyProperty ShowCustomExpressionModeProperty = DependencyProperty.Register("ShowCustomExpressionMode", typeof(bool), typeof(CrontabEditorControl), new PropertyMetadata(true));


        private static void ShowInfoOutsideMainScroller_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ce = (d as CrontabEditorControl);
            var item = ce.gbDescriptions;

            if ((bool)e.NewValue)
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
            if (CrontabExpression != null)
            {
                CrontabExpression.PropertyChanged -= CrontabExpression_PropertyChanged;
            }
            CrontabExpression = crontab == null ? new CrontabExpressionModel() { CustomMode = customMode } : CrontabHelper.Parse(crontab, customMode);
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
                    //Locale = "en"
                });
                txtDescriptor.Text = description;
                txtCrontab.Text = CrontabExpression.OutputCrontabString;
            }
            catch (Exception)
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
