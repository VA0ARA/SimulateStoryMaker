using System.Collections.Generic;

namespace Timeline.Helper
{
    public interface IReadOnlyMap<T1, T2> 
    {
        IReadOnlyDictionary<T1, T2> Forwards { get; }
        IReadOnlyDictionary<T2, T1> Backwards { get; }


        bool ContainsKey(T2 key);

        bool TryGetValue(T2 key, out T1 value);

         T1 this[T2 index] { get; set; }

         bool ContainsKey(T1 key);

         bool TryGetValue(T1 key, out T2 value);

        T2 this[T1 index] { get; set; }

        int Count { get; }

        bool Contains(T1 item);
        bool Contains(T2 item);

        IEnumerator<KeyValuePair<T2, T1>> GetForwardEnumerator();

        IEnumerator<KeyValuePair<T1, T2>> GetBackwardEnumerator();
    }
}