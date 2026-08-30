using System;
using System.Text;

// Token: 0x0200150A RID: 5386
public class ConstantValueMetric : IMetric
{
	// Token: 0x1700138A RID: 5002
	// (get) Token: 0x0600A90D RID: 43277 RVA: 0x003F1E98 File Offset: 0x003F0098
	// (set) Token: 0x0600A90E RID: 43278 RVA: 0x003F1EA0 File Offset: 0x003F00A0
	public string Header { get; set; }

	// Token: 0x0600A90F RID: 43279 RVA: 0x003F1EA9 File Offset: 0x003F00A9
	public void AppendLastValue(StringBuilder builder)
	{
		builder.Append(this.value);
	}

	// Token: 0x0600A910 RID: 43280 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x04007D70 RID: 32112
	public int value;
}
