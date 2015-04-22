using System;
using System.Collections.Generic;
using System.Collections;

namespace EnCor.Configuration
{
    /// <devdoc>
    /// Represents a genereic enumerator for the NamedElementCollection.
    /// </devdoc>
    /// 
    internal class GenericEnumeratorWrapper<T> : IEnumerator<T>
    {
        private IEnumerator _wrappedEnumerator;

        internal GenericEnumeratorWrapper(IEnumerator wrappedEnumerator)
        {
            _wrappedEnumerator = wrappedEnumerator;
        }

        T IEnumerator<T>.Current
        {
            get
            {
                return (T)_wrappedEnumerator.Current;
            }
        }

        void IDisposable.Dispose()
        {
            _wrappedEnumerator = null;
        }

        object IEnumerator.Current
        {
            get
            {
                return _wrappedEnumerator.Current;
            }
        }

        bool IEnumerator.MoveNext()
        {
            return _wrappedEnumerator.MoveNext();
        }

        void IEnumerator.Reset()
        {
            _wrappedEnumerator.Reset();
        }
    }
}
