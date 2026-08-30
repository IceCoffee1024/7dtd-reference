using System;

// Token: 0x02001479 RID: 5241
public class RingBuffer<T>
{
	// Token: 0x0600A455 RID: 42069 RVA: 0x003D32BA File Offset: 0x003D14BA
	public RingBuffer(int _count)
	{
		this.data = new T[_count];
	}

	// Token: 0x0600A456 RID: 42070 RVA: 0x003D32D0 File Offset: 0x003D14D0
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

	// Token: 0x17001332 RID: 4914
	// (get) Token: 0x0600A457 RID: 42071 RVA: 0x003D333D File Offset: 0x003D153D
	public int Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x0600A458 RID: 42072 RVA: 0x003D3345 File Offset: 0x003D1545
	public void Clear()
	{
		this.count = 0;
		this.idx = 0;
	}

	// Token: 0x0600A459 RID: 42073 RVA: 0x003D3355 File Offset: 0x003D1555
	public void SetToLast()
	{
		this.readIdx = this.idx - 1;
		if (this.readIdx < 0)
		{
			this.readIdx = this.data.Length - 1;
		}
	}

	// Token: 0x0600A45A RID: 42074 RVA: 0x003D337E File Offset: 0x003D157E
	public T Peek()
	{
		return this.data[this.readIdx];
	}

	// Token: 0x0600A45B RID: 42075 RVA: 0x003D3394 File Offset: 0x003D1594
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

	// Token: 0x0600A45C RID: 42076 RVA: 0x003D33D8 File Offset: 0x003D15D8
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

	// Token: 0x04007A4E RID: 31310
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] data;

	// Token: 0x04007A4F RID: 31311
	[PublicizedFrom(EAccessModifier.Private)]
	public int idx;

	// Token: 0x04007A50 RID: 31312
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04007A51 RID: 31313
	[PublicizedFrom(EAccessModifier.Private)]
	public int readIdx;
}
