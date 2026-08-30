using System;
using System.Text;

// Token: 0x02001525 RID: 5413
public class ConstantValueMetric : IMetric
{
	// Token: 0x170013C3 RID: 5059
	// (get) Token: 0x0600AA0A RID: 43530 RVA: 0x003F71D8 File Offset: 0x003F53D8
	// (set) Token: 0x0600AA0B RID: 43531 RVA: 0x003F71E0 File Offset: 0x003F53E0
	public string Header { get; set; }

	// Token: 0x0600AA0C RID: 43532 RVA: 0x003F71E9 File Offset: 0x003F53E9
	public void AppendLastValue(StringBuilder builder)
	{
		builder.Append(this.value);
	}

	// Token: 0x0600AA0D RID: 43533 RVA: 0x000027FC File Offset: 0x000009FC
	public void Cleanup()
	{
	}

	// Token: 0x04007E49 RID: 32329
	public int value;
}
