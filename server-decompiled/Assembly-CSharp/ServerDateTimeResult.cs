using System;

// Token: 0x0200089D RID: 2205
public struct ServerDateTimeResult
{
	// Token: 0x170006C4 RID: 1732
	// (get) Token: 0x06003F92 RID: 16274 RVA: 0x00190736 File Offset: 0x0018E936
	// (set) Token: 0x06003F93 RID: 16275 RVA: 0x0019073E File Offset: 0x0018E93E
	public bool RequestComplete
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006C5 RID: 1733
	// (get) Token: 0x06003F94 RID: 16276 RVA: 0x00190747 File Offset: 0x0018E947
	// (set) Token: 0x06003F95 RID: 16277 RVA: 0x0019074F File Offset: 0x0018E94F
	public bool HasError
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006C6 RID: 1734
	// (get) Token: 0x06003F96 RID: 16278 RVA: 0x00190758 File Offset: 0x0018E958
	// (set) Token: 0x06003F97 RID: 16279 RVA: 0x00190760 File Offset: 0x0018E960
	public int SecondsOffset
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003F98 RID: 16280 RVA: 0x00190769 File Offset: 0x0018E969
	public ServerDateTimeResult(bool _requestComplete, bool _hasError, int _secondsOffset)
	{
		this.RequestComplete = _requestComplete;
		this.HasError = _hasError;
		this.SecondsOffset = _secondsOffset;
	}
}
