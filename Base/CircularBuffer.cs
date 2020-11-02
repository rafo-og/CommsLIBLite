using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CommsLIBLite.Base
{
    internal sealed class CircularBuffer<T> : IEnumerable<T>
    {
        int _size;
        T[] _values = null;
        int _valuesIndex = 0;

        public CircularBuffer(int size)
        {
            _size = Math.Max(size, 1);
            _values = new T[_size];
        }

        public void Add(T newValue)
        {
            if(Count < _size)
            {
                _values[_valuesIndex] = newValue;
                _valuesIndex++;
                _valuesIndex %= _size;
                Count++;
            }
        }



        #region Implementation of IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_values).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)_values).GetEnumerator();
        }
        #endregion

        public T Newest { get => _values[ (_valuesIndex - 1) < 0 ? _size-1 : (_valuesIndex - 1)]; }

        public T Oldest { get => _values[_valuesIndex]; }

        public int Count { get; private set; } = 0;
    }
}
