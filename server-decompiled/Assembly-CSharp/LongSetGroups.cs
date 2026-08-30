using System;
using System.Collections.Generic;

// Token: 0x02001430 RID: 5168
public class LongSetGroups
{
	// Token: 0x1700130C RID: 4876
	// (get) Token: 0x0600A25B RID: 41563 RVA: 0x003CCBF3 File Offset: 0x003CADF3
	public IReadOnlyCollection<LongSetGroups.Group> Groups
	{
		get
		{
			return this.groups;
		}
	}

	// Token: 0x1700130D RID: 4877
	// (get) Token: 0x0600A25C RID: 41564 RVA: 0x003CCBFB File Offset: 0x003CADFB
	public int Count
	{
		get
		{
			return this.groups.Count;
		}
	}

	// Token: 0x1700130E RID: 4878
	// (get) Token: 0x0600A25D RID: 41565 RVA: 0x003CCC08 File Offset: 0x003CAE08
	public int GroupedLongsCount
	{
		get
		{
			return this.groupsByLongKey.Count;
		}
	}

	// Token: 0x0600A25E RID: 41566 RVA: 0x003CCC18 File Offset: 0x003CAE18
	public LongSetGroups.Group MergeOrCreateGroup(HashSetLong longsToGroup)
	{
		LongSetGroups.Group group = null;
		foreach (long num in longsToGroup)
		{
			LongSetGroups.Group group2;
			if (this.groupsByLongKey.TryGetValue(num, out group2))
			{
				if (group == null)
				{
					group = group2;
				}
				else if (group != group2)
				{
					group.MergeFrom(group2);
					this.groups.Remove(group2);
					foreach (long num2 in group2.Keys)
					{
						this.groupsByLongKey[num2] = group;
					}
				}
			}
		}
		if (group == null)
		{
			group = new LongSetGroups.Group();
			group.AddSet(new HashSetLong(longsToGroup));
			this.groups.Add(group);
		}
		else
		{
			group.AddSet(new HashSetLong(longsToGroup));
		}
		foreach (long num3 in longsToGroup)
		{
			this.groupsByLongKey[num3] = group;
		}
		return group;
	}

	// Token: 0x0600A25F RID: 41567 RVA: 0x003CCD50 File Offset: 0x003CAF50
	public void RemoveGroupedKeys(HashSetLong keys)
	{
		List<HashSetLong> list = new List<HashSetLong>();
		for (int i = this.groups.Count - 1; i >= 0; i--)
		{
			LongSetGroups.Group group = this.groups[i];
			if (group.RemoveSet(keys, list))
			{
				foreach (long num in keys)
				{
					if (!group.Contains(num))
					{
						this.groupsByLongKey.Remove(num);
					}
				}
				foreach (HashSetLong hashSetLong in list)
				{
					foreach (long num2 in hashSetLong)
					{
						this.groupsByLongKey.Remove(num2);
					}
				}
				if (group.Count == 0)
				{
					this.groups.RemoveAt(i);
				}
			}
		}
		foreach (HashSetLong hashSetLong2 in list)
		{
			this.MergeOrCreateGroup(hashSetLong2);
		}
	}

	// Token: 0x0600A260 RID: 41568 RVA: 0x003CCEC0 File Offset: 0x003CB0C0
	public bool TryGetGroup(long key, out LongSetGroups.Group group)
	{
		return this.groupsByLongKey.TryGetValue(key, out group);
	}

	// Token: 0x0600A261 RID: 41569 RVA: 0x003CCECF File Offset: 0x003CB0CF
	public void Clear()
	{
		this.groupsByLongKey.Clear();
		this.groups.Clear();
	}

	// Token: 0x040079EF RID: 31215
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LongSetGroups.Group> groups = new List<LongSetGroups.Group>();

	// Token: 0x040079F0 RID: 31216
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, LongSetGroups.Group> groupsByLongKey = new Dictionary<long, LongSetGroups.Group>();

	// Token: 0x02001431 RID: 5169
	public class Group
	{
		// Token: 0x1700130F RID: 4879
		// (get) Token: 0x0600A263 RID: 41571 RVA: 0x003CCF05 File Offset: 0x003CB105
		public IEnumerable<long> Keys
		{
			get
			{
				return this.keys;
			}
		}

		// Token: 0x17001310 RID: 4880
		// (get) Token: 0x0600A264 RID: 41572 RVA: 0x003CCF0D File Offset: 0x003CB10D
		public IEnumerable<HashSetLong> Sets
		{
			get
			{
				return this.sets;
			}
		}

		// Token: 0x17001311 RID: 4881
		// (get) Token: 0x0600A265 RID: 41573 RVA: 0x003CCF15 File Offset: 0x003CB115
		public int Count
		{
			get
			{
				return this.keys.Count;
			}
		}

		// Token: 0x0600A266 RID: 41574 RVA: 0x003CCF22 File Offset: 0x003CB122
		public void AddSet(HashSetLong addSet)
		{
			this.sets.Add(addSet);
			this.keys.UnionWith(addSet);
		}

		// Token: 0x0600A267 RID: 41575 RVA: 0x003CCF3C File Offset: 0x003CB13C
		public bool RemoveSet(HashSetLong removeSet, List<HashSetLong> orphanedSets)
		{
			bool flag = false;
			for (int i = this.sets.Count - 1; i >= 0; i--)
			{
				if (this.sets[i].SetEquals(removeSet))
				{
					this.sets.RemoveAt(i);
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (this.sets.Count > 1)
				{
					HashSetLong hashSetLong = this.sets[this.sets.Count - 1];
					for (int j = this.sets.Count - 2; j >= 0; j--)
					{
						HashSetLong hashSetLong2 = this.sets[j];
						if (!hashSetLong.Overlaps(hashSetLong2))
						{
							orphanedSets.Add(hashSetLong2);
							this.sets.RemoveAt(j);
						}
					}
				}
				this.keys.Clear();
				foreach (HashSetLong hashSetLong3 in this.sets)
				{
					this.keys.UnionWith(hashSetLong3);
				}
			}
			return flag;
		}

		// Token: 0x0600A268 RID: 41576 RVA: 0x003CD054 File Offset: 0x003CB254
		public bool Contains(long key)
		{
			return this.keys.Contains(key);
		}

		// Token: 0x0600A269 RID: 41577 RVA: 0x003CD064 File Offset: 0x003CB264
		public void MergeFrom(LongSetGroups.Group other)
		{
			foreach (HashSetLong hashSetLong in other.sets)
			{
				bool flag = false;
				using (List<HashSetLong>.Enumerator enumerator2 = this.sets.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.SetEquals(hashSetLong))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					this.sets.Add(hashSetLong);
					this.keys.UnionWith(hashSetLong);
				}
			}
		}

		// Token: 0x040079F1 RID: 31217
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSetLong keys = new HashSetLong();

		// Token: 0x040079F2 RID: 31218
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<HashSetLong> sets = new List<HashSetLong>();
	}
}
