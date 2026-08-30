using System;
using System.Collections.Generic;

// Token: 0x020013AD RID: 5037
public class BucketHashSetList
{
	// Token: 0x06009EC3 RID: 40643 RVA: 0x003BE5BF File Offset: 0x003BC7BF
	public BucketHashSetList()
	{
		this.buckets = new OptimizedList<HashSetLong>(4);
	}

	// Token: 0x06009EC4 RID: 40644 RVA: 0x003BE5EC File Offset: 0x003BC7EC
	public BucketHashSetList(int _noBuckets)
	{
		this.buckets = new OptimizedList<HashSetLong>(_noBuckets);
		for (int i = 0; i < _noBuckets; i++)
		{
			this.buckets.Add(new HashSetLong());
		}
	}

	// Token: 0x06009EC5 RID: 40645 RVA: 0x003BE63D File Offset: 0x003BC83D
	public void Add(int _bucketIdx, long _value)
	{
		this.buckets.array[_bucketIdx].Add(_value);
	}

	// Token: 0x06009EC6 RID: 40646 RVA: 0x003BE653 File Offset: 0x003BC853
	public void Add(int _bucketIdx, HashSetLong _otherBucket)
	{
		this.buckets.array[_bucketIdx].UnionWithHashSetLong(_otherBucket);
	}

	// Token: 0x06009EC7 RID: 40647 RVA: 0x003BE668 File Offset: 0x003BC868
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

	// Token: 0x06009EC8 RID: 40648 RVA: 0x003BE6B0 File Offset: 0x003BC8B0
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

	// Token: 0x06009EC9 RID: 40649 RVA: 0x003BE70C File Offset: 0x003BC90C
	public void Clear()
	{
		for (int i = 0; i < this.buckets.Count; i++)
		{
			this.buckets.array[i].Clear();
		}
		this.list.Clear();
		this.IsRecalc = false;
	}

	// Token: 0x06009ECA RID: 40650 RVA: 0x003BE754 File Offset: 0x003BC954
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

	// Token: 0x06009ECB RID: 40651 RVA: 0x003BE794 File Offset: 0x003BC994
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

	// Token: 0x06009ECC RID: 40652 RVA: 0x003BE834 File Offset: 0x003BCA34
	public BucketHashSetList Clone()
	{
		BucketHashSetList bucketHashSetList = new BucketHashSetList(this.buckets.Count);
		for (int i = 0; i < this.buckets.Count; i++)
		{
			bucketHashSetList.buckets.array[i].UnionWithHashSetLong(this.buckets.array[i]);
		}
		return bucketHashSetList;
	}

	// Token: 0x04007848 RID: 30792
	public List<long> list = new List<long>();

	// Token: 0x04007849 RID: 30793
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<long> elementsInList = new HashSet<long>();

	// Token: 0x0400784A RID: 30794
	public OptimizedList<HashSetLong> buckets;

	// Token: 0x0400784B RID: 30795
	public bool IsRecalc;
}
