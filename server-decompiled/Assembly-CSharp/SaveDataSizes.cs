using System;

// Token: 0x020009FC RID: 2556
public readonly struct SaveDataSizes
{
	// Token: 0x06004C51 RID: 19537 RVA: 0x001D8950 File Offset: 0x001D6B50
	public SaveDataSizes(long total, long remaining)
	{
		this.m_total = total;
		this.m_remaining = remaining;
	}

	// Token: 0x17000824 RID: 2084
	// (get) Token: 0x06004C52 RID: 19538 RVA: 0x001D8960 File Offset: 0x001D6B60
	public long Total
	{
		get
		{
			return this.m_total;
		}
	}

	// Token: 0x17000825 RID: 2085
	// (get) Token: 0x06004C53 RID: 19539 RVA: 0x001D8968 File Offset: 0x001D6B68
	public long Used
	{
		get
		{
			return this.m_total - this.m_remaining;
		}
	}

	// Token: 0x17000826 RID: 2086
	// (get) Token: 0x06004C54 RID: 19540 RVA: 0x001D8977 File Offset: 0x001D6B77
	public long Remaining
	{
		get
		{
			return this.m_remaining;
		}
	}

	// Token: 0x04003BE2 RID: 15330
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly long m_total;

	// Token: 0x04003BE3 RID: 15331
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly long m_remaining;
}
