using System;

// Token: 0x020012D6 RID: 4822
public class CoroutineCancellationToken
{
	// Token: 0x060098A9 RID: 39081 RVA: 0x0039DB98 File Offset: 0x0039BD98
	public void Cancel()
	{
		this.cancelled = true;
	}

	// Token: 0x060098AA RID: 39082 RVA: 0x0039DBA1 File Offset: 0x0039BDA1
	public bool IsCancelled()
	{
		return this.cancelled;
	}

	// Token: 0x040072E6 RID: 29414
	[PublicizedFrom(EAccessModifier.Private)]
	public bool cancelled;
}
