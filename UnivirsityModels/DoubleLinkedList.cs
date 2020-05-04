using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.Globalization;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UnivirsityModels
{
    public class DoubleLinkedList<T> : ICloneable, IEnumerable<T>, IComparable<DoubleLinkedList<T>> where T : IComparable<T>
    {
        public DoubleLinkedList() { }
        protected Node Head { get; set; }
        protected Node CurrentNode { get; set; }
        public int Length { get; private set; } = 0;

        public static DoubleLinkedList<T> Of(IEnumerable<T> source)
        {
            if (ReferenceEquals(source, null)) throw new ArgumentNullException("Can not clone list from 'null'");

            var list = new DoubleLinkedList<T>();

            if (source is DoubleLinkedList<T>) return FromList((DoubleLinkedList<T>)source);

            foreach (T element in source)
            {
                var elem = Utils.Clone(element);

                if (!ReferenceEquals(elem, null)) list.Push((T)elem);
            }

            return list;
        }

        private static DoubleLinkedList<T> FromList(DoubleLinkedList<T> source)
        {
            if (ReferenceEquals(source, null)) throw new ArgumentNullException("Can not clone list from 'null'");

            var list = new DoubleLinkedList<T>();
            var node = source.Head;
            int? currPos = null;

            for (int i = 0; i < source.Length; i++)
            {
                var val = Utils.Clone(node.Value);
                list.Push((T)val);

                if (ReferenceEquals(node, source.CurrentNode) && !ReferenceEquals(source.CurrentNode, null))
                {
                    currPos = i;
                }

                node = node.Next;
            }

            if (!ReferenceEquals(currPos, null))
            {
                list.MoveCurrentToHead();

                for (int i = 0; i < currPos; i++) list++;
            }

            return list;
        }

        public static DoubleLinkedList<T> Of(params T[] source)
        {
            return Of((IEnumerable<T>)source);
        }

        public virtual object Clone()
        {
            return FromList(this);
        }

        public void Push(T value)
        {
            AddAfter(value, Head?.Prev);
        }

        public void Unshift(T value)
        {
            Push(value);

            Head = Head.Prev;
        }

        public void PutAt(T data, uint index)
        {
            var nodeToAddAfter = GetNode(index - 1);

            AddAfter(data, nodeToAddAfter);
        }

        public T Get(uint index)
        {
            return GetNode(index).Value;
        }

        public void SetCurrent(uint index)
        {
            CurrentNode = GetNode(index);
        }


        public void SetCurrentByReference(T item)
        {
            CurrentNode = ReferenceEquals(item, null) ? null : FindNode((el1) => ReferenceEquals(el1, item));
        }

        public void UnsetCurrent()
        {
            CurrentNode = null;
        }

        private Node GetNode(uint index)
        {
            if (index >= Length)
            {
                throw new IndexOutOfRangeException("Argument 'index' can not be greater than length of list");
            }

            Node node = Head;

            bool isIndexNearestToTail = Length / 2 - index < 0;

            if (isIndexNearestToTail)
            {
                for (var i = 0; i < Length - index; i++) node = node.Prev;
            }
            else
            {
                for (var i = 0; i < index; i++) node = node.Next;
            }

            return node;
        }

        public T Current()
        {
            if (ReferenceEquals(CurrentNode, null))
            {
                return default;
            }

            return CurrentNode.Value;
        }
        public bool Includes(T data)
        {
            return Includes(elem => elem.CompareTo(data) == 0);
        }

        public bool Includes(Func<T, bool> predicate)
        {
            Node node = FindNode(predicate);

            return !(ReferenceEquals(node, null));
        }

        public T Find(Func<T, bool> predicate)
        {
            Node node = FindNode(predicate);

            return ReferenceEquals(node, null) ? default : node.Value;
        }



        private Node FindNode(T data)
        {
            return FindNode(elem => elem.CompareTo(data) == 0);
        }

        private Node FindNode(Func<T, bool> predicate)
        {
            Node temp = Head;

            for (int i = 0; i < Length; i++)
            {
                if (predicate(temp.Value)) return temp;
                temp = temp.Next;
            }

            return null;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = Head;
            for (var i = 0; i < Length; i++)
            {
                yield return node.Value;
                node = node.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool Remove(T data, bool allEntries = false)
        {
            return Remove(val => val.CompareTo(data) == 0, allEntries);
        }

        public bool Remove(Func<T, bool> predicate, bool allEntries = false)
        {
            if (ReferenceEquals(Head, null))
            {
                return false;
            }

            bool isDeleted = false;
            Node temp = Head.Prev;

            do
            {
                if (predicate(temp.Next.Value))
                {
                    DeleteNode(temp.Next);
                    isDeleted = true;

                    if (!allEntries) break;
                }
                temp = temp.Next;
            } while (temp.Next != Head);

            return isDeleted;
        }

        private void DeleteNode(Node target)
        {
            Length--;
            if (Length == 0)
            {
                Head = null;
                return;
            }

            target.Prev.Next = target.Next;
            target.Next.Prev = target.Prev;

            if (ReferenceEquals(target, Head))
            {
                Head = target.Next;
            }
        }

        public void Clear()
        {
            Head = CurrentNode = null;
            Length = 0;
        }

        public void Sort(Func<T, T, bool> isFirstNodeBeforeSecond)
        {
            if (Length < 2) return;

            Head.Prev.Next = null;
            Head.Prev = null;
            Head = LinkedListMergeSort<T>.MergeSort(Head, isFirstNodeBeforeSecond);

            var tail = Head;
            while (!ReferenceEquals(tail.Next, null))
            {
                tail.Next.Prev = tail;
                tail = tail.Next;
            }

            Head.Prev = tail;
            tail.Next = Head;
        }

        public void Sort(bool istempNodesc)
        {
            var isFirstNodeBeforeSecond = istempNodesc ? (Func<T, T, bool>)((el1, el2) => el1.CompareTo(el2) == -1)
                 : ((el1, el2) => el1.CompareTo(el2) == 1);

            Sort(isFirstNodeBeforeSecond);
        }


        public void Sort()
        {
            Sort(true);
        }

        private void AddFirst(T data)
        {
            Head = new Node(data);

            Head.Next = Head;
            Head.Prev = Head;

            Length++;
        }

        private void AddAfter(T data, Node target)
        {
            if (ReferenceEquals(data, null))
            {
                throw new ArgumentNullException("Can not add null value to list");
            }

            if (ReferenceEquals(Head, null))
            {
                AddFirst(data);
                return;
            }

            Node newNode = new Node(data);

            newNode.Next = target.Next;
            target.Next.Prev = newNode;

            target.Next = newNode;
            newNode.Prev = target;

            Length++;
        }

        public static int Compare(DoubleLinkedList<T> first, DoubleLinkedList<T> second)
        {
            if (ReferenceEquals(first, second)) return 0;

            if (ReferenceEquals(first, null)) return -1;

            if (ReferenceEquals(second, null)) return 1;

            return first.Length > second.Length ? 1
                : first.Length < second.Length ? -1 : 0;
        }


        public int CompareTo(DoubleLinkedList<T> obj)
        {
            return Compare(this, obj);
        }

        public override bool Equals(object obj)
        {
            if (GetType() != obj?.GetType())
            {
                return false;
            }

            return Compare(this, (DoubleLinkedList<T>)obj) == 0;
        }

        #region overloaded operators

        public static bool operator ==(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return Compare(left, right) == 0;
        }

        public static bool operator !=(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return Compare(left, right) != 0;
        }

        public static bool operator <(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return Compare(left, right) == -1;
        }

        public static bool operator <=(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return Compare(left, right) <= 0;
        }

        public static bool operator >(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return Compare(left, right) == 1;
        }

        public static bool operator >=(DoubleLinkedList<T> left, DoubleLinkedList<T> right)
        {
            return Compare(left, right) >= 0;
        }

        public static bool operator !(DoubleLinkedList<T> list)
        {
            return ReferenceEquals(list, null) || list.Length == 0;
        }

        public static DoubleLinkedList<T> operator ++(DoubleLinkedList<T> list)
        {
            if (!ReferenceEquals(list?.CurrentNode, null))
            {
                list.CurrentNode = list.CurrentNode.Next;
            }

            return list;
        }

        public static DoubleLinkedList<T> operator --(DoubleLinkedList<T> list)
        {
            if (!ReferenceEquals(list?.CurrentNode, null))
            {
                list.CurrentNode = list.CurrentNode.Prev;
            }

            return list;
        }

        #endregion

        #region CurrentNode hanlers
        public void DeleteCurrent()
        {
            if (!ReferenceEquals(CurrentNode, null)) DeleteNode(CurrentNode);
        }

        public void MoveCurrentToHead()
        {
            CurrentNode = Head;
        }

        public void MoveCurrentToTail()
        {
            CurrentNode = Head?.Prev;
        }

        // Assuming that all elements already sorted except current by tech task
        public bool SortCurrent(Func<T, T, bool> isFirtBeforeSecond)
        {
            if (Length > 1 && !(ReferenceEquals(CurrentNode, null)))
            {
                var tempNode = Head;

                bool isAsc = isFirtBeforeSecond(CurrentNode.Value, CurrentNode.Next.Value);

                if (!isAsc) tempNode = tempNode.Prev;

                for (int i = 0; i < Length; i++)
                {
                    if (isFirtBeforeSecond(tempNode.Value, CurrentNode.Value))
                    {
                        Swap(tempNode, CurrentNode);

                        return true;
                    }
                    tempNode = isAsc ? tempNode.Next : tempNode.Prev;
                }
            }
            return false;
        }

        public bool SortCurrent()
        {
            return SortCurrent((val, curVal) => val.CompareTo(curVal) == -1);
        }

        #endregion

        private static void Swap(Node A, Node B)
        {
            var p = A;
            A = B;
            B = p;

            B.Next = A.Next;
            B.Prev = A.Prev;

            A.Next = p.Next;
            A.Prev = p.Prev;
        }


        public class Node
        {
            public T Value { get; set; }
            public Node Prev { get; set; }
            public Node Next { get; set; }

            public Node(T data)
            {
                Value = data;
            }

            public override int GetHashCode()
            {
                if (!typeof(T).IsValueType)
                {
                    return ((object)Value).GetHashCode();
                }

                var byteArray = Value.GetBytesGeneric();

                int hash = 5381;
                Parallel.ForEach(byteArray, (element) =>
                {
                    hash += Convert.ToInt32((hash << 5) + hash + Convert.ToInt32(element));
                });

                return hash;
            }
        }
    }
}
