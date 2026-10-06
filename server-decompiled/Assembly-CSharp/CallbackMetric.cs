using System;
using System.Text;

// Token: 0x02001549 RID: 5449
public class CallbackMetric : IMetric
{
	// Token: 0x1700140A RID: 5130
	// (get) Token: 0x0600AAC7 RID: 43719 RVA: 0x003FC078 File Offset: 0x003FA278
	// (set) Token: 0x0600AAC8 RID: 43720 RVA: 0x003FC080 File Offset: 0x003FA280
	public string Header { get; set; }

	// Token: 0x0600AAC9 RID: 43721 RVA: 0x003FC089 File Offset: 0x003FA289
	public void AppendLastValue(StringBuilder builder)
	{
		builder.Append(this.callback());
	}

	// Token: 0x0600AACA RID: 43722 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x04007ECF RID: 32463
	public CallbackMetric.GetLastValue callback;

	// Token: 0x0200154A RID: 5450
	// (Invoke) Token: 0x0600AACD RID: 43725
	public delegate string GetLastValue();
}
