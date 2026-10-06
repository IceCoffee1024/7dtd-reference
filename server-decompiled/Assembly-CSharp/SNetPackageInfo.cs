using System;

// Token: 0x0200146E RID: 5230
public struct SNetPackageInfo
{
	// Token: 0x0600A3EC RID: 41964 RVA: 0x003D4F85 File Offset: 0x003D3185
	public SNetPackageInfo(int _id, int _size)
	{
		this.Tick = GameTimer.Instance.ticks;
		this.Id = _id;
		this.Size = _size;
	}

	// Token: 0x04007AC3 RID: 31427
	public readonly ulong Tick;

	// Token: 0x04007AC4 RID: 31428
	public readonly int Id;

	// Token: 0x04007AC5 RID: 31429
	public readonly int Size;
}
