using System;
using System.Collections.Generic;

// Token: 0x02001415 RID: 5141
public class LongSetGroups
{
	// Token: 0x170012D3 RID: 4819
	// (get) Token: 0x0600A15E RID: 41310 RVA: 0x003C78AF File Offset: 0x003C5AAF
	public IReadOnlyCollection<LongSetGroups.Group> Groups
	{
		get
		{
			return this.groups;
		}
	}

	// Token: 0x170012D4 RID: 4820
	// (get) Token: 0x0600A15F RID: 41311 RVA: 0x003C78B7 File Offset: 0x003C5AB7
	public int Count
	{
		get
		{
			return this.groups.Count;
		}
	}

	// Token: 0x170012D5 RID: 4821
	// (get) Token: 0x0600A160 RID: 41312 RVA: 0x003C78C4 File Offset: 0x003C5AC4
	public int GroupedLongsCount
	{
		get
		{
			return this.groupsByLongKey.Count;
		}
	}

	// Token: 0x0600A161 RID: 41313 RVA: 0x003C78D4 File Offset: 0x003C5AD4
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

	// Token: 0x0600A162 RID: 41314 RVA: 0x003C7A0C File Offset: 0x003C5C0C
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

	// Token: 0x0600A163 RID: 41315 RVA: 0x003C7B7C File Offset: 0x003C5D7C
	public bool TryGetGroup(long key, out LongSetGroups.Group group)
	{
		return this.groupsByLongKey.TryGetValue(key, out group);
	}

	// Token: 0x0600A164 RID: 41316 RVA: 0x003C7B8B File Offset: 0x003C5D8B
	public void Clear()
	{
		this.groupsByLongKey.Clear();
		this.groups.Clear();
	}

	// Token: 0x04007916 RID: 30998
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LongSetGroups.Group> groups = new List<LongSetGroups.Group>();

	// Token: 0x04007917 RID: 30999
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, LongSetGroups.Group> groupsByLongKey = new Dictionary<long, LongSetGroups.Group>();

	// Token: 0x02001416 RID: 5142
	public class Group
	{
		// Token: 0x170012D6 RID: 4822
		// (get) Token: 0x0600A166 RID: 41318 RVA: 0x003C7BC1 File Offset: 0x003C5DC1
		public IEnumerable<long> Keys
		{
			get
			{
				return this.keys;
			}
		}

		// Token: 0x170012D7 RID: 4823
		// (get) Token: 0x0600A167 RID: 41319 RVA: 0x003C7BC9 File Offset: 0x003C5DC9
		public IEnumerable<HashSetLong> Sets
		{
			get
			{
				return this.sets;
			}
		}

		// Token: 0x170012D8 RID: 4824
		// (get) Token: 0x0600A168 RID: 41320 RVA: 0x003C7BD1 File Offset: 0x003C5DD1
		public int Count
		{
			get
			{
				return this.keys.Count;
			}
		}

		// Token: 0x0600A169 RID: 41321 RVA: 0x003C7BDE File Offset: 0x003C5DDE
		public void AddSet(HashSetLong addSet)
		{
			this.sets.Add(addSet);
			this.keys.UnionWith(addSet);
		}

		// Token: 0x0600A16A RID: 41322 RVA: 0x003C7BF8 File Offset: 0x003C5DF8
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

		// Token: 0x0600A16B RID: 41323 RVA: 0x003C7D10 File Offset: 0x003C5F10
		public bool Contains(long key)
		{
			return this.keys.Contains(key);
		}

		// Token: 0x0600A16C RID: 41324 RVA: 0x003C7D20 File Offset: 0x003C5F20
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

		// Token: 0x04007918 RID: 31000
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSetLong keys = new HashSetLong();

		// Token: 0x04007919 RID: 31001
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<HashSetLong> sets = new List<HashSetLong>();
	}
}
