using System;
using System.Threading;

// Token: 0x020013CB RID: 5067
public sealed class AtomicCounter
{
	// Token: 0x170012CC RID: 4812
	// (get) Token: 0x06009F4B RID: 40779 RVA: 0x003C0BF9 File Offset: 0x003BEDF9
	public int Value
	{
		get
		{
			return this.m_counter;
		}
	}

	// Token: 0x06009F4C RID: 40780 RVA: 0x003C0C01 File Offset: 0x003BEE01
	public int Increment()
	{
		return Interlocked.Increment(ref this.m_counter);
	}

	// Token: 0x06009F4D RID: 40781 RVA: 0x003C0C0E File Offset: 0x003BEE0E
	public int Decrement()
	{
		return Interlocked.Decrement(ref this.m_counter);
	}

	// Token: 0x040078C2 RID: 30914
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_counter;
}
