using System;

// Token: 0x02000195 RID: 405
public class StepSound
{
	// Token: 0x06000C53 RID: 3155 RVA: 0x0004C707 File Offset: 0x0004A907
	public StepSound(string _name)
	{
		this.name = _name;
	}

	// Token: 0x06000C54 RID: 3156 RVA: 0x0004C716 File Offset: 0x0004A916
	public static StepSound FromString(string _name)
	{
		return new StepSound(_name);
	}

	// Token: 0x04000B10 RID: 2832
	public static StepSound stone = new StepSound("stone");

	// Token: 0x04000B11 RID: 2833
	public string name;
}
