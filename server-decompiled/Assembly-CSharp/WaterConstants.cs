using System;

// Token: 0x02000CF4 RID: 3316
public static class WaterConstants
{
	// Token: 0x060064FC RID: 25852 RVA: 0x0027BD88 File Offset: 0x00279F88
	public static int GetStableMassBelow(int mass, int massBelow)
	{
		return Utils.FastMin(mass + massBelow, 19500);
	}

	// Token: 0x04004ECD RID: 20173
	public const int MIN_MASS = 195;

	// Token: 0x04004ECE RID: 20174
	public const int MAX_MASS = 19500;

	// Token: 0x04004ECF RID: 20175
	public const int OVERFULL_MAX = 58500;

	// Token: 0x04004ED0 RID: 20176
	public const int MIN_FLOW = 195;

	// Token: 0x04004ED1 RID: 20177
	public const float FLOW_SPEED = 0.5f;

	// Token: 0x04004ED2 RID: 20178
	public const int MIN_MASS_SIDE_SPREAD = 4875;
}
