using System;
using System.Collections.Generic;

// Token: 0x02001450 RID: 5200
public class LongSetGroups
{
	// Token: 0x17001351 RID: 4945
	// (get) Token: 0x0600A306 RID: 41734 RVA: 0x003D178F File Offset: 0x003CF98F
	public IReadOnlyCollection<LongSetGroups.Group> Groups
	{
		get
		{
			return this.groups;
		}
	}

	// Token: 0x17001352 RID: 4946
	// (get) Token: 0x0600A307 RID: 41735 RVA: 0x003D1797 File Offset: 0x003CF997
	public int Count
	{
		get
		{
			return this.groups.Count;
		}
	}

	// Token: 0x17001353 RID: 4947
	// (get) Token: 0x0600A308 RID: 41736 RVA: 0x003D17A4 File Offset: 0x003CF9A4
	public int GroupedLongsCount
	{
		get
		{
			return this.groupsByLongKey.Count;
		}
	}

	// Token: 0x0600A309 RID: 41737 RVA: 0x003D17B4 File Offset: 0x003CF9B4
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

	// Token: 0x0600A30A RID: 41738 RVA: 0x003D18EC File Offset: 0x003CFAEC
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

	// Token: 0x0600A30B RID: 41739 RVA: 0x003D1A5C File Offset: 0x003CFC5C
	public bool TryGetGroup(long key, out LongSetGroups.Group group)
	{
		return this.groupsByLongKey.TryGetValue(key, out group);
	}

	// Token: 0x0600A30C RID: 41740 RVA: 0x003D1A6B File Offset: 0x003CFC6B
	public void Clear()
	{
		this.groupsByLongKey.Clear();
		this.groups.Clear();
	}

	// Token: 0x04007A6A RID: 31338
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LongSetGroups.Group> groups = new List<LongSetGroups.Group>();

	// Token: 0x04007A6B RID: 31339
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, LongSetGroups.Group> groupsByLongKey = new Dictionary<long, LongSetGroups.Group>();

	// Token: 0x02001451 RID: 5201
	public class Group
	{
		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x0600A30E RID: 41742 RVA: 0x003D1AA1 File Offset: 0x003CFCA1
		public IEnumerable<long> Keys
		{
			get
			{
				return this.keys;
			}
		}

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x0600A30F RID: 41743 RVA: 0x003D1AA9 File Offset: 0x003CFCA9
		public IEnumerable<HashSetLong> Sets
		{
			get
			{
				return this.sets;
			}
		}

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x0600A310 RID: 41744 RVA: 0x003D1AB1 File Offset: 0x003CFCB1
		public int Count
		{
			get
			{
				return this.keys.Count;
			}
		}

		// Token: 0x0600A311 RID: 41745 RVA: 0x003D1ABE File Offset: 0x003CFCBE
		public void AddSet(HashSetLong addSet)
		{
			this.sets.Add(addSet);
			this.keys.UnionWith(addSet);
		}

		// Token: 0x0600A312 RID: 41746 RVA: 0x003D1AD8 File Offset: 0x003CFCD8
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

		// Token: 0x0600A313 RID: 41747 RVA: 0x003D1BF0 File Offset: 0x003CFDF0
		public bool Contains(long key)
		{
			return this.keys.Contains(key);
		}

		// Token: 0x0600A314 RID: 41748 RVA: 0x003D1C00 File Offset: 0x003CFE00
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

		// Token: 0x04007A6C RID: 31340
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSetLong keys = new HashSetLong();

		// Token: 0x04007A6D RID: 31341
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<HashSetLong> sets = new List<HashSetLong>();
	}
}
