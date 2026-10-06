using System;
using System.Threading;

// Token: 0x020013EB RID: 5099
public sealed class AtomicCounter
{
	// Token: 0x17001311 RID: 4881
	// (get) Token: 0x06009FF6 RID: 40950 RVA: 0x003C5795 File Offset: 0x003C3995
	public int Value
	{
		get
		{
			return this.m_counter;
		}
	}

	// Token: 0x06009FF7 RID: 40951 RVA: 0x003C579D File Offset: 0x003C399D
	public int Increment()
	{
		return Interlocked.Increment(ref this.m_counter);
	}

	// Token: 0x06009FF8 RID: 40952 RVA: 0x003C57AA File Offset: 0x003C39AA
	public int Decrement()
	{
		return Interlocked.Decrement(ref this.m_counter);
	}

	// Token: 0x0400793D RID: 31037
	[PublicizedFrom(EAccessModifier.Private)]
	public int m_counter;
}
