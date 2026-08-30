using System;
using System.Collections;
using System.Collections.Generic;

namespace UniLinq
{
	// Token: 0x020017B4 RID: 6068
	public class Lookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable, ILookup<TKey, TElement>
	{
		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x0600BCBB RID: 48315 RVA: 0x0045F12B File Offset: 0x0045D32B
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

		// Token: 0x170016EB RID: 5867
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

		// Token: 0x0600BCBD RID: 48317 RVA: 0x0045F194 File Offset: 0x0045D394
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

		// Token: 0x0600BCBE RID: 48318 RVA: 0x0045F230 File Offset: 0x0045D430
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

		// Token: 0x0600BCBF RID: 48319 RVA: 0x0045F247 File Offset: 0x0045D447
		public bool Contains(TKey key)
		{
			if (key == null)
			{
				return this.nullGrouping != null;
			}
			return this.groups.ContainsKey(key);
		}

		// Token: 0x0600BCC0 RID: 48320 RVA: 0x0045F267 File Offset: 0x0045D467
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

		// Token: 0x0600BCC1 RID: 48321 RVA: 0x0045F276 File Offset: 0x0045D476
		[PublicizedFrom(EAccessModifier.Private)]
		public IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		// Token: 0x04008D34 RID: 36148
		[PublicizedFrom(EAccessModifier.Private)]
		public IGrouping<TKey, TElement> nullGrouping;

		// Token: 0x04008D35 RID: 36149
		[PublicizedFrom(EAccessModifier.Private)]
		public Dictionary<TKey, IGrouping<TKey, TElement>> groups;
	}
}
