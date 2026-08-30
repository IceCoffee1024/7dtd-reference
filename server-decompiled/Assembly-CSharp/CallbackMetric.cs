using System;
using System.Text;

// Token: 0x02001527 RID: 5415
public class CallbackMetric : IMetric
{
	// Token: 0x170013C5 RID: 5061
	// (get) Token: 0x0600AA14 RID: 43540 RVA: 0x003F7224 File Offset: 0x003F5424
	// (set) Token: 0x0600AA15 RID: 43541 RVA: 0x003F722C File Offset: 0x003F542C
	public string Header { get; set; }

	// Token: 0x0600AA16 RID: 43542 RVA: 0x003F7235 File Offset: 0x003F5435
	public void AppendLastValue(StringBuilder builder)
	{
		builder.Append(this.callback());
	}

	// Token: 0x0600AA17 RID: 43543 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x04007E4C RID: 32332
	public CallbackMetric.GetLastValue callback;

	// Token: 0x02001528 RID: 5416
	// (Invoke) Token: 0x0600AA1A RID: 43546
	public delegate string GetLastValue();
}
