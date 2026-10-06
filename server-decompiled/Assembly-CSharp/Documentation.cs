using System;

// Token: 0x02001417 RID: 5143
[AttributeUsage(AttributeTargets.Field)]
public class Documentation : Attribute
{
	// Token: 0x17001329 RID: 4905
	// (get) Token: 0x0600A124 RID: 41252 RVA: 0x003C9745 File Offset: 0x003C7945
	public string Text { get; }

	// Token: 0x0600A125 RID: 41253 RVA: 0x003C974D File Offset: 0x003C794D
	public Documentation(string value)
	{
		this.Text = value;
	}
}
