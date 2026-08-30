using System;
using System.Threading;

// Token: 0x020013B0 RID: 5040
public sealed class AtomicCounter
{
	// Token: 0x17001293 RID: 4755
	// (get) Token: 0x06009E4E RID: 40526 RVA: 0x003BB8B5 File Offset: 0x003B9AB5
	public int Value
	{
		get
		{
			return this.m_counter;
		}
	}

	// Token: 0x06009E4F RID: 40527 RVA: 0x003BB8BD File Offset: 0x003B9ABD
	public int Increment()
	{
		return Interlocked.Increment(ref this.m_counter);
	}

	// Token: 0x06009E50 RID: 40528 RVA: 0x003BB8CA File Offset: 0x003B9ACA
	public int Decrement()
	{
		return Interlocked.Decrement(ref this.m_counter);
	}

	// Token: 0x040077E9 RID: 30697
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_counter;
}
