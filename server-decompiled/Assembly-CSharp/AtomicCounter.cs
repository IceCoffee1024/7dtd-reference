using System;
using System.Threading;

// Token: 0x020013FD RID: 5117
public sealed class AtomicCounter
{
	// Token: 0x1700131D RID: 4893
	// (get) Token: 0x0600A04E RID: 41038 RVA: 0x003C7B99 File Offset: 0x003C5D99
	public int Value
	{
		get
		{
			return this.m_counter;
		}
	}

	// Token: 0x0600A04F RID: 41039 RVA: 0x003C7BA1 File Offset: 0x003C5DA1
	public int Increment()
	{
		return Interlocked.Increment(ref this.m_counter);
	}

	// Token: 0x0600A050 RID: 41040 RVA: 0x003C7BAE File Offset: 0x003C5DAE
	public int Decrement()
	{
		return Interlocked.Decrement(ref this.m_counter);
	}

	// Token: 0x0400797E RID: 31102
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_counter;
}
