using System;

// Token: 0x020008A7 RID: 2215
public struct ServerDateTimeResult
{
	// Token: 0x170006CA RID: 1738
	// (get) Token: 0x06003FB5 RID: 16309 RVA: 0x001917DE File Offset: 0x0018F9DE
	// (set) Token: 0x06003FB6 RID: 16310 RVA: 0x001917E6 File Offset: 0x0018F9E6
	public bool RequestComplete
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006CB RID: 1739
	// (get) Token: 0x06003FB7 RID: 16311 RVA: 0x001917EF File Offset: 0x0018F9EF
	// (set) Token: 0x06003FB8 RID: 16312 RVA: 0x001917F7 File Offset: 0x0018F9F7
	public bool HasError
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006CC RID: 1740
	// (get) Token: 0x06003FB9 RID: 16313 RVA: 0x00191800 File Offset: 0x0018FA00
	// (set) Token: 0x06003FBA RID: 16314 RVA: 0x00191808 File Offset: 0x0018FA08
	public int SecondsOffset
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x06003FBB RID: 16315 RVA: 0x00191811 File Offset: 0x0018FA11
	public ServerDateTimeResult(bool _requestComplete, bool _hasError, int _secondsOffset)
	{
		this.RequestComplete = _requestComplete;
		this.HasError = _hasError;
		this.SecondsOffset = _secondsOffset;
	}
}
