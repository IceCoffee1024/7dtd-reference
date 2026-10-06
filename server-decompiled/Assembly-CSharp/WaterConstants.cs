using System;

// Token: 0x02000CE9 RID: 3305
public static class WaterConstants
{
	// Token: 0x060064D7 RID: 25815 RVA: 0x0027A860 File Offset: 0x00278A60
	public static int GetStableMassBelow(int mass, int massBelow)
	{
		return Utils.FastMin(mass + massBelow, 19500);
	}

	// Token: 0x04004EA8 RID: 20136
	public const int MIN_MASS = 195;

	// Token: 0x04004EA9 RID: 20137
	public const int MAX_MASS = 19500;

	// Token: 0x04004EAA RID: 20138
	public const int OVERFULL_MAX = 58500;

	// Token: 0x04004EAB RID: 20139
	public const int MIN_FLOW = 195;

	// Token: 0x04004EAC RID: 20140
	public const float FLOW_SPEED = 0.5f;

	// Token: 0x04004EAD RID: 20141
	public const int MIN_MASS_SIDE_SPREAD = 4875;
}
