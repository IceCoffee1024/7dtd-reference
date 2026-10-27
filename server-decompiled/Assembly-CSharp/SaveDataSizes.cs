using System;

// Token: 0x02000A10 RID: 2576
public readonly struct SaveDataSizes
{
	// Token: 0x06004C44 RID: 19524 RVA: 0x001DACE8 File Offset: 0x001D8EE8
	public SaveDataSizes(long total, long remaining)
	{
		this.m_total = total;
		this.m_remaining = remaining;
	}

	// Token: 0x1700084A RID: 2122
	// (get) Token: 0x06004C45 RID: 19525 RVA: 0x001DACF8 File Offset: 0x001D8EF8
	public long Total
	{
		get
		{
			return this.m_total;
		}
	}

	// Token: 0x1700084B RID: 2123
	// (get) Token: 0x06004C46 RID: 19526 RVA: 0x001DAD00 File Offset: 0x001D8F00
	public long Used
	{
		get
		{
			return this.m_total - this.m_remaining;
		}
	}

	// Token: 0x1700084C RID: 2124
	// (get) Token: 0x06004C47 RID: 19527 RVA: 0x001DAD0F File Offset: 0x001D8F0F
	public long Remaining
	{
		get
		{
			return this.m_remaining;
		}
	}

	// Token: 0x04003C10 RID: 15376
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly long m_total;

	// Token: 0x04003C11 RID: 15377
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly long m_remaining;
}
