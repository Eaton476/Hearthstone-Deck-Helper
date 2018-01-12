using System;
using System.Diagnostics;
using System.Globalization;
using HearthstoneDeckTracker.Enums;
using HearthstoneDeckTracker.Pages;

namespace HearthstoneDeckTracker.Utilities.Converters
{
    /// <summary>
    /// Converts the <see cref="PageNumber"/> to the actual page>
    /// </summary>
    public class PageValueConverter : BaseValueConverter<PageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((PageNumber)value)
            {
                case PageNumber.Home:
                    return new HomePage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
