using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Stud.Utils
{
    public static class FilterUtils
    {
        public static string RESET_FILTER_VALUE = String.Empty;
        public static IEnumerable<T> GetFiltered<T>(IEnumerable<T> source, string filter, Func<T, string, bool> isMatchesSearch)
            where T : IComparable<T>
        {
            var search = filter?.Trim() ?? "" ;

            if (search.Length == 0)
            {
                return source;
            }

            var filtered = new List<T>();

            foreach (var item in source)
            {
                if (isMatchesSearch(item, search))
                {
                    filtered.Add(item);
                }
            }

            return filtered;
        }
        public static bool ContainsIgnoreCase(string source, string toCheck)
        {
            return source?.IndexOf(toCheck, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
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

            if (!ReferenceEquals(newItems, null))
            {
                foreach (var item in newItems) el.Items.Add(item);
            }

            el.SelectedItem = selected;
        }

        public static void HighlightSearch(ListBox el, String search)
        {
            //el.Items.Filter()
            // el.find
        }

    }
}
