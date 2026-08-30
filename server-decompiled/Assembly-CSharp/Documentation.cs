using System;

// Token: 0x020013F7 RID: 5111
[AttributeUsage(AttributeTargets.Field)]
public class Documentation : Attribute
{
	// Token: 0x170012E4 RID: 4836
	// (get) Token: 0x0600A079 RID: 41081 RVA: 0x003C4BA9 File Offset: 0x003C2DA9
	public string Text { get; }

	// Token: 0x0600A07A RID: 41082 RVA: 0x003C4BB1 File Offset: 0x003C2DB1
	public Documentation(string value)
	{
		this.Text = value;
	}
}
