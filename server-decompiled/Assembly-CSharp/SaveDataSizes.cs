using System;

// Token: 0x02000A06 RID: 2566
public readonly struct SaveDataSizes
{
	// Token: 0x06004C20 RID: 19488 RVA: 0x001D9C00 File Offset: 0x001D7E00
	public SaveDataSizes(long total, long remaining)
	{
		this.m_total = total;
		this.m_remaining = remaining;
	}

	// Token: 0x17000844 RID: 2116
	// (get) Token: 0x06004C21 RID: 19489 RVA: 0x001D9C10 File Offset: 0x001D7E10
	public long Total
	{
		get
		{
			return this.m_total;
		}
	}

	// Token: 0x17000845 RID: 2117
	// (get) Token: 0x06004C22 RID: 19490 RVA: 0x001D9C18 File Offset: 0x001D7E18
	public long Used
	{
		get
		{
			return this.m_total - this.m_remaining;
		}
	}

	// Token: 0x17000846 RID: 2118
	// (get) Token: 0x06004C23 RID: 19491 RVA: 0x001D9C27 File Offset: 0x001D7E27
	public long Remaining
	{
		get
		{
			return this.m_remaining;
		}
	}

	// Token: 0x04003BEF RID: 15343
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly long m_total;

	// Token: 0x04003BF0 RID: 15344
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly long m_remaining;
}
