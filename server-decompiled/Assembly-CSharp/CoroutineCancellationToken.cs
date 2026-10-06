using System;

// Token: 0x02001310 RID: 4880
public class CoroutineCancellationToken
{
	// Token: 0x06009A50 RID: 39504 RVA: 0x003A7998 File Offset: 0x003A5B98
	public void Cancel()
	{
		this.cancelled = true;
	}

	// Token: 0x06009A51 RID: 39505 RVA: 0x003A79A1 File Offset: 0x003A5BA1
	public bool IsCancelled()
	{
		return this.cancelled;
	}

	// Token: 0x04007438 RID: 29752
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cancelled;
}
