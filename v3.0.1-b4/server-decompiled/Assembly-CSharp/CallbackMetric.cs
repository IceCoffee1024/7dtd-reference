using System;
using System.Text;

// Token: 0x0200150C RID: 5388
public class CallbackMetric : IMetric
{
	// Token: 0x1700138C RID: 5004
	// (get) Token: 0x0600A917 RID: 43287 RVA: 0x003F1EE4 File Offset: 0x003F00E4
	// (set) Token: 0x0600A918 RID: 43288 RVA: 0x003F1EEC File Offset: 0x003F00EC
	public string Header { get; set; }

	// Token: 0x0600A919 RID: 43289 RVA: 0x003F1EF5 File Offset: 0x003F00F5
	public void AppendLastValue(StringBuilder builder)
	{
		builder.Append(this.callback());
	}

	// Token: 0x0600A91A RID: 43290 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x04007D73 RID: 32115
	public CallbackMetric.GetLastValue callback;

	// Token: 0x0200150D RID: 5389
	// (Invoke) Token: 0x0600A91D RID: 43293
	public delegate string GetLastValue();
}
