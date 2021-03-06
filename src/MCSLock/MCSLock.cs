﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace System.Threading
{
    public class MCSLock
    {
        public class Node
        {
            public int Index { get; internal set; }
            public Node Next { get; internal set; }
            public bool IsLocked { get; internal set; }
            internal string LockKey { get; set; }
        }

        public MCSLock()
        {
            _key = Guid.NewGuid().ToString();
        }

        Node _queue;

        int _index;

        string _key;

        public int DelayMs { get; set; }

        public Node GetNode()
        {
            Interlocked.Increment(ref _index);
            return new Node()
            {
                Index = _index,
                LockKey = _key
            };
        }

        public Node Lock()
        {
            Node node = GetNode();
            Lock(node);
            return node;
        }

        public void Lock(Node node)
        {
            Node predecessor = Interlocked.Exchange(ref _queue, node);
            if (predecessor != null)
            {
                node.IsLocked = true;
                predecessor.Next = node;
                while (node.IsLocked)
                {
                    //TODO : notice
                    if (DelayMs > 0)
                    {
                        Thread.Sleep(DelayMs);
                    }
                }
            }
        }

        public void Unlock(Node node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (!_key.Equals(node.LockKey))
            {
                throw new ArgumentException();
            }
            if (node.Next == null)
            {
                if (Interlocked.CompareExchange(ref _queue, null, node) == node)
                {
                    return;
                }
                else
                {
                    while (node.Next == null)
                    {
                        //TODO : notice
                        if (DelayMs > 0)
                        {
                            Thread.Sleep(DelayMs);
                        }
                    };
                }
            }
            node.Next.IsLocked = false;
        }

    }
}
