using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;

namespace Stud.Utils
{
    public static class Constants
    {
        public static Regex GROUPF_REGEX = new Regex(@"[A-Za-z][A-Za-z]-(\d\d|\d\d[m,M])-\d");
    }
    public class Refresher
    {
        public static void RefreshSelector(Selector el, IEnumerable<object> newItems, object selected)
        {
            el.ItemsSource = null;

            el.Items.Clear();

            el.SelectedIndex = -1;


            if(!ReferenceEquals(newItems, null))
            {
                foreach (var item in newItems) el.Items.Add(item);
            }
          

            el.SelectedItem = selected;


        }

    }
}
