using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenmCloud.Core.Manager.Impl
{
    public class ObjectPool<T> where T : class
    {
        private struct Element
        {
            internal T Value;
        }

        public delegate T Factory();

        private T _firstItem;

        private readonly Element[] _items;

        private readonly Factory _factory;

        public ObjectPool(Factory factory) : this(factory, Environment.ProcessorCount * 2)
        { }

        public ObjectPool(Factory factory, int size)
        {
            Debug.Assert(size >= 1);
            _factory = factory;
            _items = new Element[size - 1];
        }

        private T CreateInstance()
        {
            T inst = _factory();
            return inst;
        }


        public T Allocate()
        {
            T inst = _firstItem;
            if (inst == null || inst != Interlocked.CompareExchange(ref _firstItem, null, inst))
            {
                inst = AllocateSlow();
            }

            return inst;
        }

        private T AllocateSlow()
        {
            Element[] items = _items;
            for (int i = 0; i < items.Length; i++)
            {
                T inst = items[i].Value;
                if (inst != null)
                {
                    if (inst == Interlocked.CompareExchange(ref items[i].Value, null, inst))
                    {
                        return inst;
                    }
                }
            }
            return CreateInstance();
        }


        public void Free(T obj)
        {
            Validate(obj);

            if (_firstItem == null)
            {
                // Intentionally not using interlocked here. In a worst case scenario two objects may be stored into same
                // slot. It is very unlikely to happen and will only mean that one of the objects will get collected.
                _firstItem = obj;
            }
            else
            {
                FreeSlow(obj);
            }
        }

        private void FreeSlow(T obj)
        {
            Element[] items = _items;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Value == null)
                {
                    // Intentionally not using interlocked here. In a worst case scenario two objects may be stored into
                    // same slot. It is very unlikely to happen and will only mean that one of the objects will get collected.
                    items[i].Value = obj;
                    break;
                }
            }
        }

        private void Validate(object obj)
        {
            Debug.Assert(obj != null, "freeing null?");

            Debug.Assert(_firstItem != obj, "freeing twice?");

            var items = _items;
            for (int i = 0; i < items.Length; i++)
            {
                var value = items[i].Value;
                if (value == null)
                {
                    return;
                }

                Debug.Assert(value != obj, "freeing twice?");
            }
        }
    }
}
