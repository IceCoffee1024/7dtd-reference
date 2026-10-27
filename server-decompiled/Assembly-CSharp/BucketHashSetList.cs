using System;
using System.Collections.Generic;

// Token: 0x020013DF RID: 5087
public class BucketHashSetList
{
	// Token: 0x06009FC6 RID: 40902 RVA: 0x003C555F File Offset: 0x003C375F
	public BucketHashSetList()
	{
		this.buckets = new OptimizedList<HashSetLong>(4);
	}

	// Token: 0x06009FC7 RID: 40903 RVA: 0x003C558C File Offset: 0x003C378C
	public BucketHashSetList(int _noBuckets)
	{
		this.buckets = new OptimizedList<HashSetLong>(_noBuckets);
		for (int i = 0; i < _noBuckets; i++)
		{
			this.buckets.Add(new HashSetLong());
		}
	}

	// Token: 0x06009FC8 RID: 40904 RVA: 0x003C55DD File Offset: 0x003C37DD
	public void Add(int _bucketIdx, long _value)
	{
		this.buckets.array[_bucketIdx].Add(_value);
	}

	// Token: 0x06009FC9 RID: 40905 RVA: 0x003C55F3 File Offset: 0x003C37F3
	public void Add(int _bucketIdx, HashSetLong _otherBucket)
	{
		this.buckets.array[_bucketIdx].UnionWithHashSetLong(_otherBucket);
	}

	// Token: 0x06009FCA RID: 40906 RVA: 0x003C5608 File Offset: 0x003C3808
	public bool Contains(long _value)
	{
		if (!this.IsRecalc)
		{
			return false;
		}
		for (int i = 0; i < this.buckets.Count; i++)
		{
			if (this.buckets.array[i].Contains(_value))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06009FCB RID: 40907 RVA: 0x003C5650 File Offset: 0x003C3850
	public void Remove(long _value)
	{
		this.list.Remove(_value);
		for (int i = 0; i < this.buckets.Count; i++)
		{
			if (this.buckets.array[i].Contains(_value))
			{
				this.buckets.array[i].Remove(_value);
				return;
			}
		}
	}

	// Token: 0x06009FCC RID: 40908 RVA: 0x003C56AC File Offset: 0x003C38AC
	public void Clear()
	{
		for (int i = 0; i < this.buckets.Count; i++)
		{
			this.buckets.array[i].Clear();
		}
		this.list.Clear();
		this.IsRecalc = false;
	}

	// Token: 0x06009FCD RID: 40909 RVA: 0x003C56F4 File Offset: 0x003C38F4
	public void ExceptTarget(HashSetLong hash)
	{
		if (!this.IsRecalc)
		{
			return;
		}
		for (int i = 0; i < this.buckets.Count; i++)
		{
			hash.ExceptWithHashSetLong(this.buckets.array[i]);
		}
	}

	// Token: 0x06009FCE RID: 40910 RVA: 0x003C5734 File Offset: 0x003C3934
	public void RecalcHashSetList()
	{
		this.list.Clear();
		this.elementsInList.Clear();
		this.IsRecalc = true;
		for (int i = 0; i < this.buckets.Count; i++)
		{
			foreach (long num in this.buckets.array[i])
			{
				if (this.elementsInList.Add(num))
				{
					this.list.Add(num);
				}
			}
		}
	}

	// Token: 0x06009FCF RID: 40911 RVA: 0x003C57D4 File Offset: 0x003C39D4
	public BucketHashSetList Clone()
	{
		BucketHashSetList bucketHashSetList = new BucketHashSetList(this.buckets.Count);
		for (int i = 0; i < this.buckets.Count; i++)
		{
			bucketHashSetList.buckets.array[i].UnionWithHashSetLong(this.buckets.array[i]);
		}
		return bucketHashSetList;
	}

	// Token: 0x04007904 RID: 30980
	public List<long> list = new List<long>();

	// Token: 0x04007905 RID: 30981
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<long> elementsInList = new HashSet<long>();

	// Token: 0x04007906 RID: 30982
	public OptimizedList<HashSetLong> buckets;

	// Token: 0x04007907 RID: 30983
	public bool IsRecalc;
}
