namespace MasterT.WPF.CrontabBuilder
{
    public class CrontabExpressionModel : BaseNotifyPropertyChangedModel
    {
        string monthlyModeTime;
        string monthlyModeDay;
        string minuteModeMinutes;
        string hourlyModeHours;
        string dailyModeTime;
        string weeklyModeTime;
        string lastDayOfMonthModeTime;

        bool isEveryMinuteMode;
        bool isEveryHourMode;
        bool isEveryDayMode;
        bool isLastDayOfMonthMode;
        bool isLastWeekDayOfMonthMode;
        bool isDayOfMonthMode;
        bool isWeekDayOfMonthMode;
        bool isDayOfWeekMode;
        bool isCustomMode;

        bool monday;
        bool tuesday;
        bool wednesday;
        bool thursday;
        bool saturday;
        bool friday;
        bool sunday;

        string outputCrontabString;
        string customCrontabString;

        /// <summary>
        /// Configuration parameters
        /// </summary>
        public string DailyModeTime
        {
            get
            {
                return dailyModeTime;
            }
            set
            {
                SetField(ref dailyModeTime, value);
            }
        }

        public string WeeklyModeTime
        {
            get
            {
                return weeklyModeTime;
            }
            set
            {
                SetField(ref weeklyModeTime, value);
            }
        }

        public string MonthlyModeTime
        {
            get
            {
                return monthlyModeTime;
            }
            set
            {
                SetField(ref monthlyModeTime, value);
            }
        }

        public string MonthlyModeDay
        {
            get
            {
                return monthlyModeDay;
            }
            set
            {
                SetField(ref monthlyModeDay, value);
            }
        }

        public string LastDayOfMonthModeTime
        {
            get
            {
                return lastDayOfMonthModeTime;
            }

            set
            {
                SetField(ref lastDayOfMonthModeTime, value);
            }
        }

        public string MinuteModeMinutes
        {
            get
            {
                return minuteModeMinutes;
            }

            set
            {
                SetField(ref minuteModeMinutes, value);
            }
        }

        public string HourlyModeHours
        {
            get
            {
                return hourlyModeHours;
            }

            set
            {
                SetField(ref hourlyModeHours, value);
            }
        }

        /// <summary>
        /// Mode
        /// </summary>
        public bool MinuteMode
        {
            get
            {
                return isEveryMinuteMode;
            }
            set
            {
                SetField(ref isEveryMinuteMode, value);
            }
        }

        public bool HourlyMode
        {
            get
            {
                return isEveryHourMode;
            }
            set
            {
                SetField(ref isEveryHourMode, value);
            }
        }

        public bool DailyMode
        {
            get
            {
                return isEveryDayMode;
            }
            set
            {
                SetField(ref isEveryDayMode, value);
            }
        }

        public bool LastDayOfMonthMode
        {
            get
            {
                return isLastDayOfMonthMode;
            }
            set
            {
                SetField(ref isLastDayOfMonthMode, value);
            }
        }

        public bool LastWeekDayOfMonthMode
        {
            get
            {
                return isLastWeekDayOfMonthMode;
            }
            set
            {
                SetField(ref isLastWeekDayOfMonthMode, value);
            }
        }

        public bool MonthlyMode
        {
            get
            {
                return isDayOfMonthMode;
            }
            set
            {
                SetField(ref isDayOfMonthMode, value);
            }
        }

        public bool MonthlyWeekdayMode
        {
            get
            {
                return isWeekDayOfMonthMode;
            }
            set
            {
                SetField(ref isWeekDayOfMonthMode, value);
            }
        }

        public bool WeeklyMode
        {
            get
            {
                return isDayOfWeekMode;
            }
            set
            {
                SetField(ref isDayOfWeekMode, value);
            }
        }

        public bool Monday
        {
            get
            {
                return monday;
            }
            set
            {
                SetField(ref monday, value);
            }
        }

        public bool Tuesday
        {
            get
            {
                return tuesday;
            }
            set
            {
                SetField(ref tuesday, value);
            }
        }

        public bool Wednesday
        {
            get
            {
                return wednesday;
            }
            set
            {
                SetField(ref wednesday, value);
            }
        }

        public bool Thursday
        {
            get
            {
                return thursday;
            }
            set
            {
                SetField(ref thursday, value);
            }
        }

        public bool Friday
        {
            get
            {
                return friday;
            }
            set
            {
                SetField(ref friday, value);
            }
        }

        public bool Saturday
        {
            get
            {
                return saturday;
            }
            set
            {
                SetField(ref saturday, value);
            }
        }

        public bool Sunday
        {
            get
            {
                return sunday;
            }
            set
            {
                SetField(ref sunday, value);
            }
        }

        public bool CustomMode
        {
            get
            {
                return isCustomMode;
            }
            set
            {
                SetField(ref isCustomMode, value);
            }
        }

        public string CustomCrontabString
        {
            get
            {
                return customCrontabString;
            }

            set
            {
                SetField(ref customCrontabString, value);
            }
        }


        /// <summary>
        /// Result
        /// </summary>
        public string OutputCrontabString
        {
            get
            {
                return CustomMode ? customCrontabString : outputCrontabString;
            }

            set
            {
                if (CustomMode)
                {
                    SetField(ref customCrontabString, value);
                }
                else
                {
                    SetField(ref outputCrontabString, value);
                }
            }
        }



    }
}
