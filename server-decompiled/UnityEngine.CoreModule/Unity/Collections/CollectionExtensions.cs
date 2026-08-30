using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine.Bindings;

namespace Unity.Collections
{
	// Token: 0x0200008C RID: 140
	[VisibleToOtherModules]
	internal static class CollectionExtensions
	{
		// Token: 0x060002A9 RID: 681 RVA: 0x00004DCC File Offset: 0x00002FCC
		internal static void AddSorted<T>([DisallowNull] this List<T> list, T item, IComparer<T> comparer = null)
		{
			bool flag = list == null;
			if (flag)
			{
				throw new ArgumentNullException("list must not be null.");
			}
			if (comparer == null)
			{
				comparer = Comparer<T>.Default;
			}
			bool flag2 = list.Count == 0;
			if (flag2)
			{
				list.Add(item);
			}
			else
			{
				bool flag3 = comparer.Compare(list[list.Count - 1], item) <= 0;
				if (flag3)
				{
					list.Add(item);
				}
				else
				{
					bool flag4 = comparer.Compare(list[0], item) >= 0;
					if (flag4)
					{
						list.Insert(0, item);
					}
					else
					{
						int num = list.BinarySearch(item, comparer);
						bool flag5 = num < 0;
						if (flag5)
						{
							num = ~num;
						}
						list.Insert(num, item);
					}
				}
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00004E80 File Offset: 0x00003080
		internal static bool ContainsByEquals<T>([DisallowNull] this IEnumerable<T> collection, T element)
		{
			bool flag = collection == null;
			if (flag)
			{
				throw new ArgumentNullException("collection must not be null.");
			}
			foreach (T t in collection)
			{
				bool flag2 = t.Equals(element);
				if (flag2)
				{
					return true;
				}
			}
			return false;
		}
	}
}
