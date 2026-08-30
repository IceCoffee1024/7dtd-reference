using System;

// Token: 0x0200088E RID: 2190
public struct ServerDateTimeResult
{
	// Token: 0x17000699 RID: 1689
	// (get) Token: 0x06003F9F RID: 16287 RVA: 0x0018E876 File Offset: 0x0018CA76
	// (set) Token: 0x06003FA0 RID: 16288 RVA: 0x0018E87E File Offset: 0x0018CA7E
	public bool RequestComplete
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700069A RID: 1690
	// (get) Token: 0x06003FA1 RID: 16289 RVA: 0x0018E887 File Offset: 0x0018CA87
	// (set) Token: 0x06003FA2 RID: 16290 RVA: 0x0018E88F File Offset: 0x0018CA8F
	public bool HasError
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x1700069B RID: 1691
	// (get) Token: 0x06003FA3 RID: 16291 RVA: 0x0018E898 File Offset: 0x0018CA98
	// (set) Token: 0x06003FA4 RID: 16292 RVA: 0x0018E8A0 File Offset: 0x0018CAA0
	public int SecondsOffset
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003FA5 RID: 16293 RVA: 0x0018E8A9 File Offset: 0x0018CAA9
	public ServerDateTimeResult(bool _requestComplete, bool _hasError, int _secondsOffset)
	{
		this.RequestComplete = _requestComplete;
		this.HasError = _hasError;
		this.SecondsOffset = _secondsOffset;
	}
}
