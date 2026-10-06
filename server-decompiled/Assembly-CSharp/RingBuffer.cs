using System;

// Token: 0x020014B4 RID: 5300
public class RingBuffer<T>
{
	// Token: 0x0600A600 RID: 42496 RVA: 0x003DD28A File Offset: 0x003DB48A
	public RingBuffer(int _count)
	{
		this.data = new T[_count];
	}

	// Token: 0x0600A601 RID: 42497 RVA: 0x003DD2A0 File Offset: 0x003DB4A0
	public void Add(T _el)
	{
		T[] array = this.data;
		int num = this.idx;
		this.idx = num + 1;
		array[num] = _el;
		if (this.idx >= this.data.Length)
		{
			this.idx = 0;
		}
		this.count++;
		if (this.count > this.data.Length)
		{
			this.count = this.data.Length;
		}
	}

	// Token: 0x170013B0 RID: 5040
	// (get) Token: 0x0600A602 RID: 42498 RVA: 0x003DD30D File Offset: 0x003DB50D
	public int Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x0600A603 RID: 42499 RVA: 0x003DD315 File Offset: 0x003DB515
	public void Clear()
	{
		this.count = 0;
		this.idx = 0;
	}

	// Token: 0x0600A604 RID: 42500 RVA: 0x003DD325 File Offset: 0x003DB525
	public void SetToLast()
	{
		this.readIdx = this.idx - 1;
		if (this.readIdx < 0)
		{
			this.readIdx = this.data.Length - 1;
		}
	}

	// Token: 0x0600A605 RID: 42501 RVA: 0x003DD34E File Offset: 0x003DB54E
	public T Peek()
	{
		return this.data[this.readIdx];
	}

	// Token: 0x0600A606 RID: 42502 RVA: 0x003DD364 File Offset: 0x003DB564
	public T GetPrev()
	{
		T[] array = this.data;
		int num = this.readIdx;
		this.readIdx = num - 1;
		T t = array[num];
		if (this.readIdx < 0)
		{
			this.readIdx = this.data.Length - 1;
		}
		return t;
	}

	// Token: 0x0600A607 RID: 42503 RVA: 0x003DD3A8 File Offset: 0x003DB5A8
	public T GetNext()
	{
		T[] array = this.data;
		int num = this.readIdx;
		this.readIdx = num + 1;
		T t = array[num];
		if (this.readIdx >= this.data.Length)
		{
			this.readIdx = 0;
		}
		return t;
	}

	// Token: 0x04007BA2 RID: 31650
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] data;

	// Token: 0x04007BA3 RID: 31651
	[PublicizedFrom(EAccessModifier.Private)]
	public int idx;

	// Token: 0x04007BA4 RID: 31652
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04007BA5 RID: 31653
	[PublicizedFrom(EAccessModifier.Private)]
	public int readIdx;
}
