using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace HearthstoneDeckTracker.Utilities.Converters
{
    /// <summary>
    /// A base value converter that allows direct XAML usage
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter where T: class, new()
    {
        private static T _mConverter;

        /// <summary>
        /// Provides a static instance of the value converter.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override Object ProvideValue(IServiceProvider serviceProvider)
        {
            return _mConverter ?? (_mConverter = new T());
        }

        #region Value Converter Methods

        /// <summary>
        /// The method to convert one type to another.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// The method to convert a type back to its previous version.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion
    }
}
