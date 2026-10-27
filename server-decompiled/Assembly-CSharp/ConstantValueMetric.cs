using System;
using System.Text;

// Token: 0x02001559 RID: 5465
public class ConstantValueMetric : IMetric
{
	// Token: 0x17001414 RID: 5140
	// (get) Token: 0x0600AB15 RID: 43797 RVA: 0x003FE430 File Offset: 0x003FC630
	// (set) Token: 0x0600AB16 RID: 43798 RVA: 0x003FE438 File Offset: 0x003FC638
	public string Header { get; set; }

	// Token: 0x0600AB17 RID: 43799 RVA: 0x003FE441 File Offset: 0x003FC641
	public void AppendLastValue(StringBuilder builder)
	{
		builder.Append(this.value);
	}

	// Token: 0x0600AB18 RID: 43800 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x04007F0D RID: 32525
	public int value;
}
