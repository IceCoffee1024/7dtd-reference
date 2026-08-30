using System;

// Token: 0x02000197 RID: 407
public class StepSound
{
	// Token: 0x06000C5E RID: 3166 RVA: 0x0004CB1F File Offset: 0x0004AD1F
	public StepSound(string _name)
	{
		this.name = _name;
	}

	// Token: 0x06000C5F RID: 3167 RVA: 0x0004CB2E File Offset: 0x0004AD2E
	public static StepSound FromString(string _name)
	{
		return new StepSound(_name);
	}

	// Token: 0x04000B1D RID: 2845
	public static StepSound stone = new StepSound("stone");

	// Token: 0x04000B1E RID: 2846
	public string name;
}
