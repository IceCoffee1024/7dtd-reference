using System;
using System.Collections.Generic;

// Token: 0x020013CD RID: 5069
public class BucketHashSetList
{
	// Token: 0x06009F6E RID: 40814 RVA: 0x003C315B File Offset: 0x003C135B
	public BucketHashSetList()
	{
		this.buckets = new OptimizedList<HashSetLong>(4);
	}

	// Token: 0x06009F6F RID: 40815 RVA: 0x003C3188 File Offset: 0x003C1388
	public BucketHashSetList(int _noBuckets)
	{
		this.buckets = new OptimizedList<HashSetLong>(_noBuckets);
		for (int i = 0; i < _noBuckets; i++)
		{
			this.buckets.Add(new HashSetLong());
		}
	}

	// Token: 0x06009F70 RID: 40816 RVA: 0x003C31D9 File Offset: 0x003C13D9
	public void Add(int _bucketIdx, long _value)
	{
		this.buckets.array[_bucketIdx].Add(_value);
	}

	// Token: 0x06009F71 RID: 40817 RVA: 0x003C31EF File Offset: 0x003C13EF
	public void Add(int _bucketIdx, HashSetLong _otherBucket)
	{
		this.buckets.array[_bucketIdx].UnionWithHashSetLong(_otherBucket);
	}

	// Token: 0x06009F72 RID: 40818 RVA: 0x003C3204 File Offset: 0x003C1404
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

	// Token: 0x06009F73 RID: 40819 RVA: 0x003C324C File Offset: 0x003C144C
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

	// Token: 0x06009F74 RID: 40820 RVA: 0x003C32A8 File Offset: 0x003C14A8
	public void Clear()
	{
		for (int i = 0; i < this.buckets.Count; i++)
		{
			this.buckets.array[i].Clear();
		}
		this.list.Clear();
		this.IsRecalc = false;
	}

	// Token: 0x06009F75 RID: 40821 RVA: 0x003C32F0 File Offset: 0x003C14F0
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

	// Token: 0x06009F76 RID: 40822 RVA: 0x003C3330 File Offset: 0x003C1530
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

	// Token: 0x06009F77 RID: 40823 RVA: 0x003C33D0 File Offset: 0x003C15D0
	public BucketHashSetList Clone()
	{
		BucketHashSetList bucketHashSetList = new BucketHashSetList(this.buckets.Count);
		for (int i = 0; i < this.buckets.Count; i++)
		{
			bucketHashSetList.buckets.array[i].UnionWithHashSetLong(this.buckets.array[i]);
		}
		return bucketHashSetList;
	}

	// Token: 0x040078C3 RID: 30915
	public List<long> list = new List<long>();

	// Token: 0x040078C4 RID: 30916
	[PublicizedFrom(EAccessModifier.Private)]
	public HashSet<long> elementsInList = new HashSet<long>();

	// Token: 0x040078C5 RID: 30917
	public OptimizedList<HashSetLong> buckets;

	// Token: 0x040078C6 RID: 30918
	public bool IsRecalc;
}
