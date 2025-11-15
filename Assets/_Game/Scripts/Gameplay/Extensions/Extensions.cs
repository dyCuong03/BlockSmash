namespace BlockSmash.Extensions
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public static class Extensions
    {
        public static T DequeueOrDefault<T>(this Queue<T> queue, Func<T> valueFactory)
        {
            return queue.Count > 0 ? queue.Dequeue() : valueFactory();
        }

        public static T GetComponentOrThrow<T>(this GameObject gameObject)
        {
            return gameObject.TryGetComponent(out T result) ? result : throw new MissingComponentException($"Component {typeof(T).Name} not found in {gameObject.name}");
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            dictionary.TryAdd(key, valueFactory);
            return dictionary[key];
        }

        public static bool TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            if (dictionary.ContainsKey(key)) return false;
            dictionary.Add(key, valueFactory());
            return true;
        }

        public static int RemoveWhere<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            Func<TKey, TValue, bool>       predicate
        )
        {
            var toRemove = new List<TKey>();

            foreach (var kv in dictionary)
            {
                if (predicate(kv.Key, kv.Value))
                {
                    toRemove.Add(kv.Key);
                }
            }

            foreach (var key in toRemove)
            {
                dictionary.Remove(key);
            }

            return toRemove.Count;
        }

        public static GameObject DontDestroyOnLoad(this GameObject go)
        {
            Object.DontDestroyOnLoad(go);
            return go;
        }
    }
}