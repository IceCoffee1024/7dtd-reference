using System;

// Token: 0x02000CDB RID: 3291
public static class WaterConstants
{
	// Token: 0x060064E5 RID: 25829 RVA: 0x002774BC File Offset: 0x002756BC
	public static int GetStableMassBelow(int mass, int massBelow)
	{
		return Utils.FastMin(mass + massBelow, 19500);
	}

	// Token: 0x04004E72 RID: 20082
	public const int MIN_MASS = 195;

	// Token: 0x04004E73 RID: 20083
	public const int MAX_MASS = 19500;

	// Token: 0x04004E74 RID: 20084
	public const int OVERFULL_MAX = 58500;

	// Token: 0x04004E75 RID: 20085
	public const int MIN_FLOW = 195;

	// Token: 0x04004E76 RID: 20086
	public const float FLOW_SPEED = 0.5f;

	// Token: 0x04004E77 RID: 20087
	public const int MIN_MASS_SIDE_SPREAD = 4875;
}
