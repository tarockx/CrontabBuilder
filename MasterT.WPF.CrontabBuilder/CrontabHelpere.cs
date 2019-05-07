using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MasterT.WPF.CrontabBuilder
{
    /// <summary>
    /// This helps us working with crontab expressions and the UI parsing.
    /// </summary>
    public static class CrontabHelper
    {
        public static CrontabExpressionModel Parse(string crontab, bool customMode)
        {
            CrontabExpressionModel model = new CrontabExpressionModel();

            if (string.IsNullOrEmpty(crontab))
            {
                model.CustomMode = customMode;
                return model;
            }

            if (customMode)
            {
                model.CustomMode = true;
                model.CustomCrontabString = crontab;
                return model;
            }
            else
            {
                model.OutputCrontabString = crontab;

                var re = new Regex(@"0 0/([0-9]|[0-5][0-9]) \* 1/1 \* \?");
                if (re.IsMatch(crontab))
                {
                    model.MinuteMode = true;
                    model.MinuteModeMinutes = re.Match(crontab).Groups[1].Value;
                    return model;
                }

                re = new Regex(@"0 0 0/([0-9]|[0-2][0-9]) 1/1 \* \?");
                if (re.IsMatch(crontab))
                {
                    model.HourlyMode = true;
                    model.HourlyModeHours = re.Match(crontab).Groups[1].Value;
                    return model;
                }

                re = new Regex(@"0 ([0-9]|[0-5][0-9]) ([0-9]|[0-2][0-9]) \* \* \?");
                if (re.IsMatch(crontab))
                {
                    model.DailyMode = true;

                    string minutes = Pad(re.Match(crontab).Groups[1].Value);
                    string hours = re.Match(crontab).Groups[2].Value;
                    model.DailyModeTime = $"{hours}:{minutes}";
                    return model;
                }

                re = new Regex(@"0 ([0-9]|[0-5][0-9]) ([0-9]|[0-2][0-9]) ([1-9]|0[1-9]|[1-2][0-9]|3[0-1]) \* \?");
                if (re.IsMatch(crontab))
                {
                    model.MonthlyMode = true;

                    string minutes = Pad(re.Match(crontab).Groups[1].Value);
                    string hours = re.Match(crontab).Groups[2].Value;
                    string day = re.Match(crontab).Groups[3].Value;

                    model.MonthlyModeTime = $"{hours}:{minutes}";
                    model.MonthlyModeDay = day;
                    return model;
                }

                re = new Regex(@"0 ([0-9]|[0-5][0-9]) ([0-9]|[0-2][0-9]) L \* \?");
                if (re.IsMatch(crontab))
                {
                    model.LastDayOfMonthMode = true;

                    string minutes = Pad(re.Match(crontab).Groups[1].Value);
                    string hours = re.Match(crontab).Groups[2].Value;

                    model.LastDayOfMonthModeTime = $"{hours}:{minutes}";
                    return model;
                }

                re = new Regex(@"0 ([0-9]|[0-5][0-9]) ([0-9]|[0-2][0-9]) \? \* (MON,?)?(TUE,?)?(WED,?)?(THU,?)?(FRI,?)?(SAT,?)?(SUN,?)?");
                if (re.IsMatch(crontab))
                {
                    model.WeeklyMode = true;

                    string minutes = Pad(re.Match(crontab).Groups[1].Value);
                    string hours = re.Match(crontab).Groups[2].Value;
                    model.WeeklyModeTime = $"{hours}:{minutes}";

                    model.Monday = crontab.Contains("MON");
                    model.Tuesday = crontab.Contains("TUE");
                    model.Wednesday = crontab.Contains("WED");
                    model.Thursday = crontab.Contains("THU");
                    model.Friday = crontab.Contains("FRI");
                    model.Saturday = crontab.Contains("SAT");
                    model.Sunday = crontab.Contains("SUN");
                    return model;
                }

                //No match -> assume custom mode
                model.CustomCrontabString = crontab;
                model.CustomMode = true;
                return model;
            }
        }

        private static void ComputeEveryMinuteCrontab(CrontabExpressionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.MinuteModeMinutes)) model.MinuteModeMinutes = "1";
            if (model.MinuteMode && checkIfIsValidMinute(model.MinuteModeMinutes))
            {
                var minutes = int.Parse(model.MinuteModeMinutes);
                model.OutputCrontabString = $@"0 0/{minutes} * 1/1 * ?";
            }
        }

        private static void ComputeEveryHourCrontab(CrontabExpressionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.HourlyModeHours)) model.HourlyModeHours = "1";
            if (model.HourlyMode && checkIfIsValidHour(model.HourlyModeHours))
            {
                var hours = int.Parse(model.HourlyModeHours);
                model.OutputCrontabString = $@"0 0 0/{hours} 1/1 * ?";
            }
        }

        private static void ComputeNDayInMonthCrontab(CrontabExpressionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.MonthlyModeDay)) model.MonthlyModeDay = "1";
            if (string.IsNullOrWhiteSpace(model.MonthlyModeTime)) model.MonthlyModeTime = "12:00";

            if (model.MonthlyMode && checkIfIsValidTime(model.MonthlyModeTime) && checkIfIsValidMonthDay(model.MonthlyModeDay))
            {
                var minutes = int.Parse(model.MonthlyModeTime.Split(':')[1]);
                var hours = int.Parse(model.MonthlyModeTime.Split(':')[0]);
                var dayInMonth = int.Parse(model.MonthlyModeDay);
                model.OutputCrontabString = $"0 {minutes} {hours} {dayInMonth} * ?";
            }
        }

        private static void ComputeEveryDayCrontab(CrontabExpressionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.DailyModeTime)) model.DailyModeTime = "12:00";

            if (model.DailyMode && checkIfIsValidTime(model.DailyModeTime))
            {
                var minutes = int.Parse(model.DailyModeTime.Split(':')[1]);
                var hours = int.Parse(model.DailyModeTime.Split(':')[0]);
                model.OutputCrontabString = $"0 {minutes} {hours} * * ?";
            }
        }

        private static void ComputeEveryWeekDayCrontab(CrontabExpressionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.WeeklyModeTime)) model.WeeklyModeTime = "12:00";

            if (!(model.WeeklyMode && checkIfIsValidTime(model.WeeklyModeTime))) return;

            var daysArray = new Dictionary<string, bool>();
            daysArray.Add("MON", model.Monday);
            daysArray.Add("TUE", model.Tuesday);
            daysArray.Add("WED", model.Wednesday);
            daysArray.Add("THU", model.Thursday);
            daysArray.Add("FRI", model.Friday);
            daysArray.Add("SAT", model.Saturday);
            daysArray.Add("SUN", model.Sunday);

            var days = "";
            var checkedDays = daysArray.Where(r => r.Value);
            var builder = new StringBuilder();
            builder.Append(days);
            foreach (var cd in checkedDays)
            {
                builder.Append(cd.Key.ToUpper() + ",");
            }
            days = builder.ToString().Trim(',');
            if (string.IsNullOrWhiteSpace(days)) days = "*";
            var minutes = int.Parse(model.WeeklyModeTime.Split(':')[1]);
            var hours = int.Parse(model.WeeklyModeTime.Split(':')[0]);
            model.OutputCrontabString = $"0 {minutes} {hours} ? * {days}";
        }

        private static void ComputeLastMonthDay(CrontabExpressionModel model)
        {
            if (string.IsNullOrWhiteSpace(model.LastDayOfMonthModeTime)) model.LastDayOfMonthModeTime = "12:00";

            if (model.LastDayOfMonthMode && checkIfIsValidTime(model.LastDayOfMonthModeTime))
            {
                var minutes = int.Parse(model.LastDayOfMonthModeTime.Split(':')[1]);
                var hours = int.Parse(model.LastDayOfMonthModeTime.Split(':')[0]);
                model.OutputCrontabString = $"0 {minutes} {hours} L * ?";
            }
        }

        /// <summary>
        /// It verifies if the provided expression is a valid crontab expression (it uses regular expressions)
        /// </summary>
        /// <param name="crontab"></param>
        /// <returns></returns>
        public static bool CheckCronTabExpression(string crontab)
        {
            var rePattern = @"(((([0-9]|[0-5][0-9]),)*([0-9]|[0-5][0-9]))|(([\*]|[0-9]|[0-5][0-9])(/|-)([0-9]|[0-5][0-9]))|([\?])|([\*]))[\s](((([0-9]|[0-5][0-9]),)*([0-9]|[0-5][0-9]))|(([\*]|[0-9]|[0-5][0-9])(/|-)([0-9]|[0-5][0-9]))|([\?])|([\*]))[\s](((([0-9]|[0-1][0-9]|[2][0-3]),)*([0-9]|[0-1][0-9]|[2][0-3]))|(([\*]|[0-9]|[0-1][0-9]|[2][0-3])(/|-)([0-9]|[0-1][0-9]|[2][0-3]))|([\?])|([\*]))[\s](((([1-9]|[0][1-9]|[1-2][0-9]|[3][0-1]),)*([1-9]|[0][1-9]|[1-2][0-9]|[3][0-1])(C)?)|(([1-9]|[0][1-9]|[1-2][0-9]|[3][0-1])(/|-)([1-9]|[0][1-9]|[1-2][0-9]|[3][0-1])(C)?)|(L(-[0-9])?)|(L(-[1-2][0-9])?)|(L(-[3][0-1])?)|(LW)|([1-9]W)|([1-3][0-9]W)|([\?])|([\*]))[\s](((([1-9]|0[1-9]|1[0-2]),)*([1-9]|0[1-9]|1[0-2]))|(([1-9]|0[1-9]|1[0-2])(/|-)([1-9]|0[1-9]|1[0-2]))|(((JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC),)*(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))|((JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC)(-|/)(JAN|FEB|MAR|APR|MAY|JUN|JUL|AUG|SEP|OCT|NOV|DEC))|([\?])|([\*]))[\s]((([1-7],)*([1-7]))|([1-7](/|-)([1-7]))|(((MON|TUE|WED|THU|FRI|SAT|SUN),)*(MON|TUE|WED|THU|FRI|SAT|SUN)(C)?)|((MON|TUE|WED|THU|FRI|SAT|SUN)(-|/)(MON|TUE|WED|THU|FRI|SAT|SUN)(C)?)|(([1-7]|(MON|TUE|WED|THU|FRI|SAT|SUN))?(L|LW)?)|(([1-7]|MON|TUE|WED|THU|FRI|SAT|SUN)#([1-7])?)|([\?])|([\*]))([\s]?(([\*])?|(19[7-9][0-9])|(20[0-9][0-9]))?| (((19[7-9][0-9])|(20[0-9][0-9]))(-|/)((19[7-9][0-9])|(20[0-9][0-9])))?| ((((19[7-9][0-9])|(20[0-9][0-9])),)*((19[7-9][0-9])|(20[0-9][0-9])))?)";
            var cronTabRegEx = new Regex(rePattern, RegexOptions.IgnoreCase);
            var match = cronTabRegEx.Match(crontab);
            if (match.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// It verifies if the provided string is a valid time value (it uses regular expressions)
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static bool checkIfIsValidTime(string time)
        {
            if (string.IsNullOrWhiteSpace(time)) return false;
            var re = new Regex(@"([0-2][0-9]):([0-5][0-9])");
            var match = re.Match(time);
            if (match.Success) return true;
            return false;
        }

        /// <summary>
        /// Check if the provided string is a valid month number value.
        /// </summary>
        /// <param name="monthDay"></param>
        /// <returns></returns>
        private static bool checkIfIsValidMonthDay(string monthDay)
        {
            if (string.IsNullOrWhiteSpace(monthDay)) return false;
            var md = 0;
            if (!int.TryParse(monthDay, out md)) return false;
            if (md > 31 || md < 1) return false;
            return true;

        }

        /// <summary>
        /// Check if the value is a valid minute number
        /// </summary>
        /// <param name="minute"></param>
        /// <returns></returns>
        private static bool checkIfIsValidMinute(string minute)
        {
            if (string.IsNullOrWhiteSpace(minute)) return false;
            var md = 0;
            if (!int.TryParse(minute, out md)) return false;
            if (md > 59 || md < 1) return false;
            return true;

        }

        /// <summary>
        /// Check if the value is a valid hour number
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        private static bool checkIfIsValidHour(string hour)
        {
            if (string.IsNullOrWhiteSpace(hour)) return false;
            var md = 0;
            if (!int.TryParse(hour, out md)) return false;
            if (md > 23 || md < 1) return false;
            return true;

        }

        private static string Pad(string input)
        {
            return input.Length == 1 ? $"0{input}" : input;
        }

        /// <summary>
        /// This is used to react to Crontab UI radio button form event. It read the UI model and call the right method to produce the crontab expression.
        /// </summary>
        /// <param name="model"></param>

        public static void RecalculateCrontab(CrontabExpressionModel model)
        {
            //model.OutputCrontabString = null;

            try
            {
                if (model.CustomMode)
                {
                    return;
                }

                if (model.MinuteMode)
                {
                    ComputeEveryMinuteCrontab(model);
                    return;
                }

                if (model.HourlyMode)
                {
                    ComputeEveryHourCrontab(model);
                    return;
                }

                if (model.DailyMode)
                {
                    ComputeEveryDayCrontab(model);
                    return;
                }

                if (model.WeeklyMode)
                {
                    ComputeEveryWeekDayCrontab(model);
                    return;
                }

                if (model.LastDayOfMonthMode)
                {
                    ComputeLastMonthDay(model);
                    return;
                }

                if (model.MonthlyMode)
                {

                    ComputeNDayInMonthCrontab(model);
                    return;
                }

            }
            catch (Exception ex)
            {

            }
        }

    }
}
