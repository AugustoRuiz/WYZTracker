using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace WYZTracker.Wpf
{
    internal class Utils
    {
        public static T Clone<T>(T source)
        {
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source));
        }

        public static T FindParent<T>(DependencyObject element) where T :DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            if(parent == null || parent is T)
            {
                return parent as T;
            }
            else
            {
                return Utils.FindParent<T>(parent);
            }
        }
    }
}
