using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x02001811 RID: 6161
	public class Lookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable, ILookup<TKey, TElement>
	{
		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x0600BF26 RID: 48934 RVA: 0x00466FF7 File Offset: 0x004651F7
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

		// Token: 0x170017D5 RID: 6101
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

		// Token: 0x0600BF28 RID: 48936 RVA: 0x00467060 File Offset: 0x00465260
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

		// Token: 0x0600BF29 RID: 48937 RVA: 0x004670FC File Offset: 0x004652FC
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

		// Token: 0x0600BF2A RID: 48938 RVA: 0x00467113 File Offset: 0x00465313
		public bool Contains(TKey key)
		{
			if (key == null)
			{
				return this.nullGrouping != null;
			}
			return this.groups.ContainsKey(key);
		}

		// Token: 0x0600BF2B RID: 48939 RVA: 0x00467133 File Offset: 0x00465333
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

		// Token: 0x0600BF2C RID: 48940 RVA: 0x00467142 File Offset: 0x00465342
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04008EE2 RID: 36578
		[PublicizedFrom(EAccessModifier.Private)]
		public IGrouping<TKey, TElement> nullGrouping;

		// Token: 0x04008EE3 RID: 36579
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<TKey, IGrouping<TKey, TElement>> groups;
	}
}
