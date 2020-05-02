using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Stud.Utils
{
    public class Refresher
    {
        public static void RefreshSelector(Selector el, IEnumerable<object> newItems)
        {
            el.ItemsSource = null;

            el.Items.Clear();

            if(!ReferenceEquals(newItems, null))
            {
                // todo: selected item

                foreach (var item in newItems) el.Items.Add(item);
            }
        }

    }
}
