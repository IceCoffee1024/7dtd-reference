using System;

// Token: 0x0200089A RID: 2202
public struct ServerDateTimeResult
{
	// Token: 0x170006A5 RID: 1701
	// (get) Token: 0x06004004 RID: 16388 RVA: 0x00190946 File Offset: 0x0018EB46
	// (set) Token: 0x06004005 RID: 16389 RVA: 0x0019094E File Offset: 0x0018EB4E
	public bool RequestComplete
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006A6 RID: 1702
	// (get) Token: 0x06004006 RID: 16390 RVA: 0x00190957 File Offset: 0x0018EB57
	// (set) Token: 0x06004007 RID: 16391 RVA: 0x0019095F File Offset: 0x0018EB5F
	public bool HasError
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x170006A7 RID: 1703
	// (get) Token: 0x06004008 RID: 16392 RVA: 0x00190968 File Offset: 0x0018EB68
	// (set) Token: 0x06004009 RID: 16393 RVA: 0x00190970 File Offset: 0x0018EB70
	public int SecondsOffset
	{
		readonly get; [PublicizedFrom(EAccessModifier.Private)]
		set;
	}

	// Token: 0x0600400A RID: 16394 RVA: 0x00190979 File Offset: 0x0018EB79
	public ServerDateTimeResult(bool _requestComplete, bool _hasError, int _secondsOffset)
	{
		this.RequestComplete = _requestComplete;
		this.HasError = _hasError;
		this.SecondsOffset = _secondsOffset;
	}
}
