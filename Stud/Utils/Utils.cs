using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Text.RegularExpressions;
using System.Globalization;
using UnivirsityModels;

namespace Stud.Utils
{
    public static class FilterUtils
    {
        public static string RESET_FILTER_VALUE = String.Empty;
        public static IEnumerable<T> GetFiltered<T>(IEnumerable<T> source, string filter, Func<T, string, bool> isMatchesSearch)
            where T : IComparable<T>
        {
            var search = filter?.Trim() ?? "";

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
    }

    public static class Constants
    {
        public static int MAX_COURCE_NUMBER = 6;
    }
    public static class StubData
    {
        public static DoubleLinkedList<NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>> CreateFacultyList()
        {
            var facultyList = new DoubleLinkedList<NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>>();

            var defaultFaculty = new NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>("FFEKS");
            var f2 = new NamedDoubleLinkedList<NamedDoubleLinkedList<Student>>("FACULTY");

            var defaultGroup = new NamedDoubleLinkedList<Student>("ks-16-1");
            var g2 = new NamedDoubleLinkedList<Student>("ks-18-1");

            var g3 = new NamedDoubleLinkedList<Student>("mc-18-1");

            g3.Push(new Student("a", "a", "a", 1994, 20));
            g3.Push(new Student("b", "b", "b", 1994, 35.3f));

            g2.Push(new Student("S", "N", "P", 1996, 20));
            g2.Push(new Student("M", "N", "P", 1996, 35.3f));


            defaultGroup.Push(new Student("Surname", "Name", "Patronimic", 1996, 20));
            defaultGroup.Push(new Student("Surname1", "Name1", "Patronimic1", 1997, 35.3f));

            defaultFaculty.Push(defaultGroup);

            f2.Push(g3);
            f2.Push(g2);
            facultyList.Push(defaultFaculty);
            facultyList.Push(f2);

            facultyList.MoveCurrentToHead();
            facultyList.Current().MoveCurrentToHead();
            facultyList.Current().Current().MoveCurrentToHead();

            facultyList.Sort();

            return facultyList;
        }
    }
}
