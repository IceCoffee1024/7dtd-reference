using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017E0 RID: 6112
	public class Lookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable, ILookup<TKey, TElement>
	{
		// Token: 0x1700178C RID: 6028
		// (get) Token: 0x0600BE12 RID: 48658 RVA: 0x0046364B File Offset: 0x0046184B
		public int Count
		{
			get
			{
				if (this.nullGrouping != null)
				{
					return this.groups.Count + 1;
				}
				return this.groups.Count;
			}
		}

		// Token: 0x1700178D RID: 6029
		public IEnumerable<TElement> this[TKey key]
		{
			get
			{
				if (key == null && this.nullGrouping != null)
				{
					return this.nullGrouping;
				}
				IGrouping<TKey, TElement> grouping;
				if (key != null && this.groups.TryGetValue(key, out grouping))
				{
					return grouping;
				}
				return new TElement[0];
			}
		}

		// Token: 0x0600BE14 RID: 48660 RVA: 0x004636B4 File Offset: 0x004618B4
		[PublicizedFrom(EAccessModifier.Internal)]
		public Lookup(Dictionary<TKey, List<TElement>> lookup, IEnumerable<TElement> nullKeyElements)
		{
			this.groups = new Dictionary<TKey, IGrouping<TKey, TElement>>(lookup.Comparer);
			foreach (KeyValuePair<TKey, List<TElement>> keyValuePair in lookup)
			{
				this.groups.Add(keyValuePair.Key, new Grouping<TKey, TElement>(keyValuePair.Key, keyValuePair.Value));
			}
			if (nullKeyElements != null)
			{
				this.nullGrouping = new Grouping<TKey, TElement>(default(TKey), nullKeyElements);
			}
		}

		// Token: 0x0600BE15 RID: 48661 RVA: 0x00463750 File Offset: 0x00461950
		public IEnumerable<TResult> ApplyResultSelector<TResult>(Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
		{
			if (this.nullGrouping != null)
			{
				yield return resultSelector(this.nullGrouping.Key, this.nullGrouping);
			}
			foreach (KeyValuePair<TKey, IGrouping<TKey, TElement>> keyValuePair in this.groups)
			{
				yield return resultSelector(keyValuePair.Value.Key, keyValuePair.Value);
			}
			Dictionary<TKey, IGrouping<TKey, TElement>>.Enumerator enumerator = default(Dictionary<TKey, IGrouping<TKey, TElement>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600BE16 RID: 48662 RVA: 0x00463767 File Offset: 0x00461967
		public bool Contains(TKey key)
		{
			if (key == null)
			{
				return this.nullGrouping != null;
			}
			return this.groups.ContainsKey(key);
		}

		// Token: 0x0600BE17 RID: 48663 RVA: 0x00463787 File Offset: 0x00461987
		public IEnumerator<IGrouping<TKey, TElement>> GetEnumerator()
		{
			if (this.nullGrouping != null)
			{
				yield return this.nullGrouping;
			}
			foreach (KeyValuePair<TKey, IGrouping<TKey, TElement>> keyValuePair in this.groups)
			{
				yield return keyValuePair.Value;
			}
			Dictionary<TKey, IGrouping<TKey, TElement>>.Enumerator enumerator = default(Dictionary<TKey, IGrouping<TKey, TElement>>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x0600BE18 RID: 48664 RVA: 0x00463796 File Offset: 0x00461996
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04008DF9 RID: 36345
		[PublicizedFrom(EAccessModifier.Private)]
		public IGrouping<TKey, TElement> nullGrouping;

		// Token: 0x04008DFA RID: 36346
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<TKey, IGrouping<TKey, TElement>> groups;
	}
}
