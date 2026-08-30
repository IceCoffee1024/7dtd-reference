using System;

// Token: 0x02001494 RID: 5268
public class RingBuffer<T>
{
	// Token: 0x0600A552 RID: 42322 RVA: 0x003D85FE File Offset: 0x003D67FE
	public RingBuffer(int _count)
	{
		this.data = new T[_count];
	}

	// Token: 0x0600A553 RID: 42323 RVA: 0x003D8614 File Offset: 0x003D6814
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

	// Token: 0x1700136B RID: 4971
	// (get) Token: 0x0600A554 RID: 42324 RVA: 0x003D8681 File Offset: 0x003D6881
	public int Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x0600A555 RID: 42325 RVA: 0x003D8689 File Offset: 0x003D6889
	public void Clear()
	{
		this.count = 0;
		this.idx = 0;
	}

	// Token: 0x0600A556 RID: 42326 RVA: 0x003D8699 File Offset: 0x003D6899
	public void SetToLast()
	{
		this.readIdx = this.idx - 1;
		if (this.readIdx < 0)
		{
			this.readIdx = this.data.Length - 1;
		}
	}

	// Token: 0x0600A557 RID: 42327 RVA: 0x003D86C2 File Offset: 0x003D68C2
	public T Peek()
	{
		return this.data[this.readIdx];
	}

	// Token: 0x0600A558 RID: 42328 RVA: 0x003D86D8 File Offset: 0x003D68D8
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

	// Token: 0x0600A559 RID: 42329 RVA: 0x003D871C File Offset: 0x003D691C
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

	// Token: 0x04007B27 RID: 31527
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] data;

	// Token: 0x04007B28 RID: 31528
	[PublicizedFrom(EAccessModifier.Private)]
	public int idx;

	// Token: 0x04007B29 RID: 31529
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04007B2A RID: 31530
	[PublicizedFrom(EAccessModifier.Private)]
	public int readIdx;
}
