using System;
using System.Collections.Generic;
using System.Text;

namespace UnivirsityModels
{
    class LinkedListMergeSort<T> where T : IComparable<T>
    {
        public static DoubleLinkedList<T>.Node MergeSort(DoubleLinkedList<T>.Node head, Func<T, T, bool> isFirstBefore)
        {
            int blockSize = 1, blockCount;
            do
            {
                DoubleLinkedList<T>.Node left = head, right = head, tail = null;
                head = null;
                blockCount = 0;

                while (!ReferenceEquals(left, null))
                {
                    blockCount++;

                    int leftSize = 0, rightSize = blockSize;

                    for (; leftSize < blockSize && !ReferenceEquals(right, null); ++leftSize)
                        right = right.Next;

                    bool leftEmpty = leftSize == 0,
                        rightEmpty = rightSize == 0 || ReferenceEquals(right, null);

                    while (!leftEmpty || !rightEmpty)
                    {
                        DoubleLinkedList<T>.Node smaller;

                        if (rightEmpty || (!leftEmpty && isFirstBefore(left.Value, right.Value)))
                        {
                            smaller = left; left = left.Next; --leftSize;
                            leftEmpty = leftSize == 0;
                        }
                        else
                        {
                            smaller = right; right = right.Next; --rightSize;
                            rightEmpty = rightSize == 0 || ReferenceEquals(right, null);
                        }

                        if (!ReferenceEquals(tail, null))
                            tail.Next = smaller;
                        else
                            head = smaller;
                        tail = smaller;
                    }

                    left = right;
                }

                if (!ReferenceEquals(tail, null))
                    tail.Next = null;

                blockSize *= 2;

            } while (blockCount > 1);

            return head;
        }
    }

}
