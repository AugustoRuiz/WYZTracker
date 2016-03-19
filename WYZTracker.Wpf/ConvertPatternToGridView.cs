using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using WYZTracker.Wpf.ViewModels;

namespace WYZTracker.Wpf
{
    public class ConvertPatternToGridView : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Pattern p = value as Pattern;
            if(p!= null)
            {
                List<ExpandoObject> result = new List<ExpandoObject>();
                foreach(ChannelLine l in p.Lines)
                {
                    ExpandoObject line = new ExpandoObject();
                    IDictionary<string, object> lineObj = line;
                    for (int i = 0; i < p.Channels; i++)
                    {
                        lineObj.Add(string.Format("Channel{0}", i), l.Notes[i]);
                    }
                    lineObj.Add("FX", l.Fx);
                    lineObj.Add("TempoModifier", l.TempoModifier);
                    result.Add(line);
                }

                return result;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
