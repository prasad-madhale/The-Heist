using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<Node> where Node : IComparable<Node> {
   
    private List<Node> seq;

    public PriorityQueue()
        {
            seq = new List<Node>();
        }

        public int Count()
        {
            return seq.Count;
        }

        public void Add(Node tobeadded)
        {
            seq.Add(tobeadded); // adds to the end of the list
            Shiftup();
        }

        void Shiftup()
        {
            int child = seq.Count - 1;
            while (child > 0)
            {
                int parent = (child - 1) / 2;

                if (seq[child].CompareTo(seq[parent]) >= 0)
                {
                    break;   // heap condition satisfies  
                }

                swap(child, parent);
                child = parent;
            }
        }


        public Node Poll()
        {
            Node firstitem;
            if (seq.Count == 0)
                return default(Node);
            else if (seq.Count == 1)
            {
                firstitem = seq[0];
                seq.RemoveAt(0);
                return firstitem;
            }

            int lastindex = seq.Count - 1;

            firstitem = seq[0];
            seq[0] = seq[lastindex];
            seq.RemoveAt(lastindex);
            
            Shiftdown();
            return firstitem;
        }

        void Shiftdown()
        {
            if (seq.Count == 0)
                return;

            int lastindex = seq.Count - 1;
            int parentindex = 0;

            while (true)
            {
                int leftchild = (parentindex * 2) + 1;

                if (leftchild > lastindex) break;

                int rightchild = leftchild + 1;

                if (rightchild <= seq.Count-1 && seq[rightchild].CompareTo(seq[leftchild]) < 0)
                    leftchild = rightchild;

                if (seq[parentindex].CompareTo(seq[leftchild]) <= 0)
                    break; // heap condition satisfied

                Node parentitem = seq[parentindex];

                swap(parentindex, leftchild);
                parentindex = leftchild;
            }
        }


        public int GetIndex(Node item)
        {
            return seq.IndexOf(item);
        }

        public void Print()
        {
            foreach (Node item in this.seq)
            {
                Debug.Log(item);
            }
        }

        public bool Is_heap()
        {
            if (seq.Count == 0) return true;

            int lastindex = seq.Count - 1;

            for (int parentindex = 0; parentindex < seq.Count; parentindex++)
            {
                int leftchild = 2 * parentindex + 1;
                int rightchild = leftchild + 1;

                if (leftchild <= lastindex && seq[parentindex].CompareTo(seq[leftchild]) > 0)
                {
                    return false;
                }
                if (rightchild <= lastindex && seq[parentindex].CompareTo(seq[rightchild]) > 0)
                {
                    return false;
                }

            }

            return true;
        }

        public void swap(int index1, int index2)
        {
            Node temp = seq[index1];
            seq[index1] = seq[index2];
            seq[index2] = temp;
        }

        public bool IsEmpty()
        {
            if (seq.Count == 0)
                return true;
            else
                return false;
        }

        public bool Contains(Node item)
        {
            return seq.Contains(item);
        }

    }

