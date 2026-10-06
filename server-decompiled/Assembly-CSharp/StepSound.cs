using System;

// Token: 0x02000198 RID: 408
public class StepSound
{
	// Token: 0x06000C55 RID: 3157 RVA: 0x0004CB23 File Offset: 0x0004AD23
	public StepSound(string _name)
	{
		this.name = _name;
	}

	// Token: 0x06000C56 RID: 3158 RVA: 0x0004CB32 File Offset: 0x0004AD32
	public static StepSound FromString(string _name)
	{
		return new StepSound(_name);
	}

	// Token: 0x04000B16 RID: 2838
	public static StepSound stone = new StepSound("stone");

	// Token: 0x04000B17 RID: 2839
	public string name;
}
