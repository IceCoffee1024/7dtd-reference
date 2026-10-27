using System;
using System.Text;

// Token: 0x0200155B RID: 5467
public class CallbackMetric : IMetric
{
	// Token: 0x17001416 RID: 5142
	// (get) Token: 0x0600AB1F RID: 43807 RVA: 0x003FE47C File Offset: 0x003FC67C
	// (set) Token: 0x0600AB20 RID: 43808 RVA: 0x003FE484 File Offset: 0x003FC684
	public string Header { get; set; }

	// Token: 0x0600AB21 RID: 43809 RVA: 0x003FE48D File Offset: 0x003FC68D
	public void AppendLastValue(StringBuilder builder)
	{
		builder.Append(this.callback());
	}

	// Token: 0x0600AB22 RID: 43810 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x04007F10 RID: 32528
	public CallbackMetric.GetLastValue callback;

	// Token: 0x0200155C RID: 5468
	// (Invoke) Token: 0x0600AB25 RID: 43813
	public delegate string GetLastValue();
}
