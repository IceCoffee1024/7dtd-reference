using System;

// Token: 0x02001429 RID: 5161
[AttributeUsage(AttributeTargets.Field)]
public class Documentation : Attribute
{
	// Token: 0x17001335 RID: 4917
	// (get) Token: 0x0600A17C RID: 41340 RVA: 0x003CBB49 File Offset: 0x003C9D49
	public string Text { get; }

	// Token: 0x0600A17D RID: 41341 RVA: 0x003CBB51 File Offset: 0x003C9D51
	public Documentation(string value)
	{
		this.Text = value;
	}
}
