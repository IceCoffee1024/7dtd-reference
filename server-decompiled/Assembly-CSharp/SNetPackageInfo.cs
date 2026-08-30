using System;

// Token: 0x02001433 RID: 5171
public struct SNetPackageInfo
{
	// Token: 0x0600A243 RID: 41539 RVA: 0x003CB031 File Offset: 0x003C9231
	public SNetPackageInfo(int _id, int _size)
	{
		this.Tick = GameTimer.Instance.ticks;
		this.Id = _id;
		this.Size = _size;
	}

	// Token: 0x0400796F RID: 31087
	public readonly ulong Tick;

	// Token: 0x04007970 RID: 31088
	public readonly int Id;

	// Token: 0x04007971 RID: 31089
	public readonly int Size;
}
