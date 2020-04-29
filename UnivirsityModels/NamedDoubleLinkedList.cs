using System;
using System.Collections.Generic;
using System.Text;

namespace UnivirsityModels
{
    public class NamedDoubleLinkedList<T> : DoubleLinkedList<T>, IComparable<NamedDoubleLinkedList<T>>
    where T : IComparable<T>
    {
        public string Name { get; set; }
        public NamedDoubleLinkedList(string name)
        {
            Name = name;
        }

        public NamedDoubleLinkedList<T> FromList(NamedDoubleLinkedList<T> source)
        {
            var list = (NamedDoubleLinkedList<T>)base.Clone();

            list.Name = source.Name;

            return list;
        }

        public override object Clone()
        {
            return FromList(this);
        }

        public int CompareTo(NamedDoubleLinkedList<T> other)
        {
            if (ReferenceEquals(null, other)) return 1;

            return ReferenceEquals(this, other) ? 0 : string.CompareOrdinal(Name, other.Name);
        }

        public static int Compare(NamedDoubleLinkedList<T> list1, NamedDoubleLinkedList<T> list2)
        {
            return string.CompareOrdinal(list1?.Name, list2?.Name);
        }

        public override bool Equals(object obj)
        {
            return obj is NamedDoubleLinkedList<T> list &&
                   base.Equals(obj) &&
                   Name == list.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), Name);
        }

        public static bool operator ==(NamedDoubleLinkedList<T> list1, NamedDoubleLinkedList<T> list2)
        {
            return Compare(list1, list2) == 0;
        }

        public static bool operator !=(NamedDoubleLinkedList<T> list1, NamedDoubleLinkedList<T> list2)
        {
            return Compare(list1, list2) != 0;
        }

        public static bool operator >(NamedDoubleLinkedList<T> list1, NamedDoubleLinkedList<T> list2)
        {
            return Compare(list1, list2) < 0;
        }

        public static bool operator <(NamedDoubleLinkedList<T> list1, NamedDoubleLinkedList<T> list2)
        {
            return Compare(list1, list2) > 0;
        }

        public static bool operator >=(NamedDoubleLinkedList<T> list1, NamedDoubleLinkedList<T> list2)
        {
            return Compare(list1, list2) <= 0;
        }

        public static bool operator <=(NamedDoubleLinkedList<T> list1, NamedDoubleLinkedList<T> list2)
        {
            return Compare(list1, list2) >= 0;
        }
    }
}
