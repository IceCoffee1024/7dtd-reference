using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x0200178E RID: 6030
	public class Lookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable, ILookup<TKey, TElement>
	{
		// Token: 0x17001678 RID: 5752
		// (get) Token: 0x0600BB38 RID: 47928 RVA: 0x004593B3 File Offset: 0x004575B3
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

		// Token: 0x17001679 RID: 5753
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

		// Token: 0x0600BB3A RID: 47930 RVA: 0x0045941C File Offset: 0x0045761C
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

		// Token: 0x0600BB3B RID: 47931 RVA: 0x004594B8 File Offset: 0x004576B8
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

		// Token: 0x0600BB3C RID: 47932 RVA: 0x004594CF File Offset: 0x004576CF
		public bool Contains(TKey key)
		{
			if (key == null)
			{
				return this.nullGrouping != null;
			}
			return this.groups.ContainsKey(key);
		}

		// Token: 0x0600BB3D RID: 47933 RVA: 0x004594EF File Offset: 0x004576EF
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

		// Token: 0x0600BB3E RID: 47934 RVA: 0x004594FE File Offset: 0x004576FE
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04008C18 RID: 35864
		[PublicizedFrom(EAccessModifier.Private)]
		public IGrouping<TKey, TElement> nullGrouping;

		// Token: 0x04008C19 RID: 35865
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<TKey, IGrouping<TKey, TElement>> groups;
	}
}
