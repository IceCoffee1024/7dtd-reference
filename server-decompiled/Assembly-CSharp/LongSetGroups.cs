using System;
using System.Collections.Generic;

// Token: 0x02001462 RID: 5218
public class LongSetGroups
{
	// Token: 0x1700135D RID: 4957
	// (get) Token: 0x0600A35E RID: 41822 RVA: 0x003D3B93 File Offset: 0x003D1D93
	public IReadOnlyCollection<LongSetGroups.Group> Groups
	{
		get
		{
			return this.groups;
		}
	}

	// Token: 0x1700135E RID: 4958
	// (get) Token: 0x0600A35F RID: 41823 RVA: 0x003D3B9B File Offset: 0x003D1D9B
	public int Count
	{
		get
		{
			return this.groups.Count;
		}
	}

	// Token: 0x1700135F RID: 4959
	// (get) Token: 0x0600A360 RID: 41824 RVA: 0x003D3BA8 File Offset: 0x003D1DA8
	public int GroupedLongsCount
	{
		get
		{
			return this.groupsByLongKey.Count;
		}
	}

	// Token: 0x0600A361 RID: 41825 RVA: 0x003D3BB8 File Offset: 0x003D1DB8
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

	// Token: 0x0600A362 RID: 41826 RVA: 0x003D3CF0 File Offset: 0x003D1EF0
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

	// Token: 0x0600A363 RID: 41827 RVA: 0x003D3E60 File Offset: 0x003D2060
	public bool TryGetGroup(long key, out LongSetGroups.Group group)
	{
		return this.groupsByLongKey.TryGetValue(key, out group);
	}

	// Token: 0x0600A364 RID: 41828 RVA: 0x003D3E6F File Offset: 0x003D206F
	public void Clear()
	{
		this.groupsByLongKey.Clear();
		this.groups.Clear();
	}

	// Token: 0x04007AAB RID: 31403
	[PublicizedFrom(EAccessModifier.Private)]
	public List<LongSetGroups.Group> groups = new List<LongSetGroups.Group>();

	// Token: 0x04007AAC RID: 31404
	[PublicizedFrom(EAccessModifier.Private)]
	public Dictionary<long, LongSetGroups.Group> groupsByLongKey = new Dictionary<long, LongSetGroups.Group>();

	// Token: 0x02001463 RID: 5219
	public class Group
	{
		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x0600A366 RID: 41830 RVA: 0x003D3EA5 File Offset: 0x003D20A5
		public IEnumerable<long> Keys
		{
			get
			{
				return this.keys;
			}
		}

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x0600A367 RID: 41831 RVA: 0x003D3EAD File Offset: 0x003D20AD
		public IEnumerable<HashSetLong> Sets
		{
			get
			{
				return this.sets;
			}
		}

		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x0600A368 RID: 41832 RVA: 0x003D3EB5 File Offset: 0x003D20B5
		public int Count
		{
			get
			{
				return this.keys.Count;
			}
		}

		// Token: 0x0600A369 RID: 41833 RVA: 0x003D3EC2 File Offset: 0x003D20C2
		public void AddSet(HashSetLong addSet)
		{
			this.sets.Add(addSet);
			this.keys.UnionWith(addSet);
		}

		// Token: 0x0600A36A RID: 41834 RVA: 0x003D3EDC File Offset: 0x003D20DC
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

		// Token: 0x0600A36B RID: 41835 RVA: 0x003D3FF4 File Offset: 0x003D21F4
		public bool Contains(long key)
		{
			return this.keys.Contains(key);
		}

		// Token: 0x0600A36C RID: 41836 RVA: 0x003D4004 File Offset: 0x003D2204
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

		// Token: 0x04007AAD RID: 31405
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly HashSetLong keys = new HashSetLong();

		// Token: 0x04007AAE RID: 31406
		[PublicizedFrom(EAccessModifier.Private)]
		public readonly List<HashSetLong> sets = new List<HashSetLong>();
	}
}
