using System;

// Token: 0x02001480 RID: 5248
public struct SNetPackageInfo
{
	// Token: 0x0600A444 RID: 42052 RVA: 0x003D7389 File Offset: 0x003D5589
	public SNetPackageInfo(int _id, int _size)
	{
		this.Tick = GameTimer.Instance.ticks;
		this.Id = _id;
		this.Size = _size;
	}

	// Token: 0x04007B04 RID: 31492
	public readonly ulong Tick;

	// Token: 0x04007B05 RID: 31493
	public readonly int Id;

	// Token: 0x04007B06 RID: 31494
	public readonly int Size;
}
