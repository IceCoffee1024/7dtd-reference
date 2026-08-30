using System;

// Token: 0x020013DC RID: 5084
[AttributeUsage(AttributeTargets.Field)]
public class Documentation : Attribute
{
	// Token: 0x170012AB RID: 4779
	// (get) Token: 0x06009F7C RID: 40828 RVA: 0x003BF865 File Offset: 0x003BDA65
	public string Text { get; }

	// Token: 0x06009F7D RID: 40829 RVA: 0x003BF86D File Offset: 0x003BDA6D
	public Documentation(string value)
	{
		this.Text = value;
	}
}
