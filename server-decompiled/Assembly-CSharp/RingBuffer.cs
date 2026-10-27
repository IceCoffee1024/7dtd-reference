using System;

// Token: 0x020014C6 RID: 5318
public class RingBuffer<T>
{
	// Token: 0x0600A658 RID: 42584 RVA: 0x003DF68E File Offset: 0x003DD88E
	public RingBuffer(int _count)
	{
		this.data = new T[_count];
	}

	// Token: 0x0600A659 RID: 42585 RVA: 0x003DF6A4 File Offset: 0x003DD8A4
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

	// Token: 0x170013BC RID: 5052
	// (get) Token: 0x0600A65A RID: 42586 RVA: 0x003DF711 File Offset: 0x003DD911
	public int Count
	{
		get
		{
			return this.count;
		}
	}

	// Token: 0x0600A65B RID: 42587 RVA: 0x003DF719 File Offset: 0x003DD919
	public void Clear()
	{
		this.count = 0;
		this.idx = 0;
	}

	// Token: 0x0600A65C RID: 42588 RVA: 0x003DF729 File Offset: 0x003DD929
	public void SetToLast()
	{
		this.readIdx = this.idx - 1;
		if (this.readIdx < 0)
		{
			this.readIdx = this.data.Length - 1;
		}
	}

	// Token: 0x0600A65D RID: 42589 RVA: 0x003DF752 File Offset: 0x003DD952
	public T Peek()
	{
		return this.data[this.readIdx];
	}

	// Token: 0x0600A65E RID: 42590 RVA: 0x003DF768 File Offset: 0x003DD968
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

	// Token: 0x0600A65F RID: 42591 RVA: 0x003DF7AC File Offset: 0x003DD9AC
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

	// Token: 0x04007BE3 RID: 31715
	[PublicizedFrom(EAccessModifier.Private)]
	public T[] data;

	// Token: 0x04007BE4 RID: 31716
	[PublicizedFrom(EAccessModifier.Private)]
	public int idx;

	// Token: 0x04007BE5 RID: 31717
	[PublicizedFrom(EAccessModifier.Private)]
	public int count;

	// Token: 0x04007BE6 RID: 31718
	[PublicizedFrom(EAccessModifier.Private)]
	public int readIdx;
}
