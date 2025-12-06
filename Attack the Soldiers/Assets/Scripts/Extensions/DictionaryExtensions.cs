using System;
using System.Collections.Generic;

public static class DictionaryExtensions
{
    /// <summary>
    /// Inicializa un Dictionary a partir de una lista y una función para obtener la clave.
    /// </summary>
    public static void InitializeFromList<TKey, TValue>(
        this Dictionary<TKey, TValue> dictionary,
        List<TValue> list,
        Func<TValue, TKey> keySelector)
    {
        dictionary.Clear();
        foreach (var item in list)
        {
            var key = keySelector(item);
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, item);
            }
        }
    }
}
