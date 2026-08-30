using System;

// Token: 0x0200144E RID: 5198
public struct SNetPackageInfo
{
	// Token: 0x0600A340 RID: 41792 RVA: 0x003D0375 File Offset: 0x003CE575
	public SNetPackageInfo(int _id, int _size)
	{
		this.Tick = GameTimer.Instance.ticks;
		this.Id = _id;
		this.Size = _size;
	}

	// Token: 0x04007A48 RID: 31304
	public readonly ulong Tick;

	// Token: 0x04007A49 RID: 31305
	public readonly int Id;

	// Token: 0x04007A4A RID: 31306
	public readonly int Size;
}
