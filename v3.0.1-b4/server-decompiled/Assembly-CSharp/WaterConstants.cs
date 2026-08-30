using System;

// Token: 0x02000CC9 RID: 3273
public static class WaterConstants
{
	// Token: 0x06006459 RID: 25689 RVA: 0x002745C0 File Offset: 0x002727C0
	public static int GetStableMassBelow(int mass, int massBelow)
	{
		return Utils.FastMin(mass + massBelow, 19500);
	}

	// Token: 0x04004DD4 RID: 19924
	public const int MIN_MASS = 195;

	// Token: 0x04004DD5 RID: 19925
	public const int MAX_MASS = 19500;

	// Token: 0x04004DD6 RID: 19926
	public const int OVERFULL_MAX = 58500;

	// Token: 0x04004DD7 RID: 19927
	public const int MIN_FLOW = 195;

	// Token: 0x04004DD8 RID: 19928
	public const float FLOW_SPEED = 0.5f;

	// Token: 0x04004DD9 RID: 19929
	public const int MIN_MASS_SIDE_SPREAD = 4875;
}
