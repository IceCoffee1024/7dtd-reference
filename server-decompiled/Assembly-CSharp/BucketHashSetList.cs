using System;
using System.Collections.Generic;

// Token: 0x02001392 RID: 5010
public class BucketHashSetList
{
	// Token: 0x06009DC6 RID: 40390 RVA: 0x003B927B File Offset: 0x003B747B
	public BucketHashSetList()
	{
		this.buckets = new OptimizedList<HashSetLong>(4);
	}

	// Token: 0x06009DC7 RID: 40391 RVA: 0x003B92A8 File Offset: 0x003B74A8
	public BucketHashSetList(int _noBuckets)
	{
		this.buckets = new OptimizedList<HashSetLong>(_noBuckets);
		for (int i = 0; i < _noBuckets; i++)
		{
			this.buckets.Add(new HashSetLong());
		}
	}

	// Token: 0x06009DC8 RID: 40392 RVA: 0x003B92F9 File Offset: 0x003B74F9
	public void Add(int _bucketIdx, long _value)
	{
		this.buckets.array[_bucketIdx].Add(_value);
	}

	// Token: 0x06009DC9 RID: 40393 RVA: 0x003B930F File Offset: 0x003B750F
	public void Add(int _bucketIdx, HashSetLong _otherBucket)
	{
		this.buckets.array[_bucketIdx].UnionWithHashSetLong(_otherBucket);
	}

	// Token: 0x06009DCA RID: 40394 RVA: 0x003B9324 File Offset: 0x003B7524
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

	// Token: 0x06009DCB RID: 40395 RVA: 0x003B936C File Offset: 0x003B756C
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

	// Token: 0x06009DCC RID: 40396 RVA: 0x003B93C8 File Offset: 0x003B75C8
	public void Clear()
	{
		for (int i = 0; i < this.buckets.Count; i++)
		{
			this.buckets.array[i].Clear();
		}
		this.list.Clear();
		this.IsRecalc = false;
	}

	// Token: 0x06009DCD RID: 40397 RVA: 0x003B9410 File Offset: 0x003B7610
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

	// Token: 0x06009DCE RID: 40398 RVA: 0x003B9450 File Offset: 0x003B7650
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

	// Token: 0x06009DCF RID: 40399 RVA: 0x003B94F0 File Offset: 0x003B76F0
	public BucketHashSetList Clone()
	{
		BucketHashSetList bucketHashSetList = new BucketHashSetList(this.buckets.Count);
		for (int i = 0; i < this.buckets.Count; i++)
		{
			bucketHashSetList.buckets.array[i].UnionWithHashSetLong(this.buckets.array[i]);
		}
		return bucketHashSetList;
	}

	// Token: 0x0400776F RID: 30575
	public List<long> list = new List<long>();

	// Token: 0x04007770 RID: 30576
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<long> elementsInList = new HashSet<long>();

	// Token: 0x04007771 RID: 30577
	public OptimizedList<HashSetLong> buckets;

	// Token: 0x04007772 RID: 30578
	public bool IsRecalc;
}
