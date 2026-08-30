using System;

// Token: 0x020009EB RID: 2539
public readonly struct SaveDataSizes
{
	// Token: 0x06004BCB RID: 19403 RVA: 0x001D6168 File Offset: 0x001D4368
	public SaveDataSizes(long total, long remaining)
	{
		this.m_total = total;
		this.m_remaining = remaining;
	}

	// Token: 0x17000812 RID: 2066
	// (get) Token: 0x06004BCC RID: 19404 RVA: 0x001D6178 File Offset: 0x001D4378
	public long Total
	{
		get
		{
			return this.m_total;
		}
	}

	// Token: 0x17000813 RID: 2067
	// (get) Token: 0x06004BCD RID: 19405 RVA: 0x001D6180 File Offset: 0x001D4380
	public long Used
	{
		get
		{
			return this.m_total - this.m_remaining;
		}
	}

	// Token: 0x17000814 RID: 2068
	// (get) Token: 0x06004BCE RID: 19406 RVA: 0x001D618F File Offset: 0x001D438F
	public long Remaining
	{
		get
		{
			return this.m_remaining;
		}
	}

	// Token: 0x04003B51 RID: 15185
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly long m_total;

	// Token: 0x04003B52 RID: 15186
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly long m_remaining;
}
