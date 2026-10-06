using System;
using System.Text;

// Token: 0x02001547 RID: 5447
public class ConstantValueMetric : IMetric
{
	// Token: 0x17001408 RID: 5128
	// (get) Token: 0x0600AABD RID: 43709 RVA: 0x003FC02C File Offset: 0x003FA22C
	// (set) Token: 0x0600AABE RID: 43710 RVA: 0x003FC034 File Offset: 0x003FA234
	public string Header { get; set; }

	// Token: 0x0600AABF RID: 43711 RVA: 0x003FC03D File Offset: 0x003FA23D
	public void AppendLastValue(StringBuilder builder)
	{
		builder.Append(this.value);
	}

	// Token: 0x0600AAC0 RID: 43712 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x04007ECC RID: 32460
	public int value;
}
