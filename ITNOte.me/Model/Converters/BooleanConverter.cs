using System.Globalization;
using System.Windows.Data;
using ITNOte.me.Model.Notes;

namespace ITNOte.me.Model.Converters

{
    public class BooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Folder;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
