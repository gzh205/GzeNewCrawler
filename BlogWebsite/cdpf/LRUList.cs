using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogWebsite.cdpf
{
    public class LRUList<T> : LinkedList<T>
    {
        public T this[int index]
        {
            get
            {
                if (index >= Count)
                    return default;
                Enumerator e = GetEnumerator();
                for (int i = 0; i <= index; i++)
                {
                    e.MoveNext();
                }
                return e.Current;
            }
        }
    }
}