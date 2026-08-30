using System;
using System.Collections.Generic;
using System.Linq;

namespace HarmonyLib
{
	// Token: 0x02000051 RID: 81
	public static class CollectionExtensions
	{
		// Token: 0x06000386 RID: 902 RVA: 0x0000E6D0 File Offset: 0x0000C8D0
		public static void Do<T>(this IEnumerable<T> sequence, Action<T> action)
		{
			if (sequence == null)
			{
				return;
			}
			foreach (T t in sequence)
			{
				action(t);
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x0000E6FE File Offset: 0x0000C8FE
		public static void DoIf<T>(this IEnumerable<T> sequence, Func<T, bool> condition, Action<T> action)
		{
			sequence.Where<T>(condition).Do<T>(action);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x0000E70D File Offset: 0x0000C90D
		public static IEnumerable<T> AddItem<T>(this IEnumerable<T> sequence, T item)
		{
			return (sequence ?? new T[0]).Concat<T>(new T[] { item });
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0000E72D File Offset: 0x0000C92D
		public static T[] AddToArray<T>(this T[] sequence, T item)
		{
			return sequence.AddItem(item).ToArray<T>();
		}

		// Token: 0x0600038A RID: 906 RVA: 0x0000E73C File Offset: 0x0000C93C
		public static T[] AddRangeToArray<T>(this T[] sequence, T[] items)
		{
			return (sequence ?? Enumerable.Empty<T>()).Concat<T>(items).ToArray<T>();
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0000E760 File Offset: 0x0000C960
		internal static Dictionary<K, V> Merge<K, V>(this IEnumerable<KeyValuePair<K, V>> firstDict, params IEnumerable<KeyValuePair<K, V>>[] otherDicts)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			foreach (KeyValuePair<K, V> keyValuePair in firstDict)
			{
				dictionary[keyValuePair.Key] = keyValuePair.Value;
			}
			foreach (IEnumerable<KeyValuePair<K, V>> enumerable in otherDicts)
			{
				foreach (KeyValuePair<K, V> keyValuePair2 in enumerable)
				{
					dictionary[keyValuePair2.Key] = keyValuePair2.Value;
				}
			}
			return dictionary;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x0000E824 File Offset: 0x0000CA24
		internal static Dictionary<K, V> TransformKeys<K, V>(this Dictionary<K, V> origDict, Func<K, K> transform)
		{
			Dictionary<K, V> dictionary = new Dictionary<K, V>();
			foreach (KeyValuePair<K, V> keyValuePair in origDict)
			{
				dictionary.Add(transform(keyValuePair.Key), keyValuePair.Value);
			}
			return dictionary;
		}
	}
}
