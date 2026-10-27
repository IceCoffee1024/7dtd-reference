using System;

// Token: 0x02001322 RID: 4898
public class CoroutineCancellationToken
{
	// Token: 0x06009AA8 RID: 39592 RVA: 0x003A9D9C File Offset: 0x003A7F9C
	public void Cancel()
	{
		this.cancelled = true;
	}

	// Token: 0x06009AA9 RID: 39593 RVA: 0x003A9DA5 File Offset: 0x003A7FA5
	public bool IsCancelled()
	{
		return this.cancelled;
	}

	// Token: 0x04007479 RID: 29817
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cancelled;
}
