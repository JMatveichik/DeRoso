using System;
using System.Globalization;
using System.Windows.Data;

namespace DeRoso.Core.Converters
{
    public enum TimeValueBase
    {
        Seconds = 0,
        Minutes = 1,
        Hours = 2,
        Days = 3
    }

    [ValueConversion(typeof(TimeSpan), typeof(double))]
    public class TimeSpanToDoubleConverter : IValueConverter
    {
        public TimeValueBase ConvertBase
        {
            get;
            set;
        } = TimeValueBase.Seconds;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TimeSpan ts = (TimeSpan)value;
            switch (ConvertBase)
            {
                case TimeValueBase.Seconds:
                    return ts.TotalSeconds;
                case TimeValueBase.Minutes:
                    return ts.TotalMinutes;
                case TimeValueBase.Hours:
                    return ts.TotalHours;
                case TimeValueBase.Days:
                    return ts.TotalDays;
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (ConvertBase)
            {
                case TimeValueBase.Seconds:
                    return TimeSpan.FromSeconds((double)value);
                case TimeValueBase.Minutes:
                    return TimeSpan.FromMinutes((double)value);
                case TimeValueBase.Hours:
                    return TimeSpan.FromHours((double)value);
                case TimeValueBase.Days:
                    return TimeSpan.FromDays((double)value);
            }
            return TimeSpan.FromSeconds(0);



        }
    }
}
