using System;

// Token: 0x02000723 RID: 1827
public class MinEvent
{
	// Token: 0x04002D2A RID: 11562
	public static MinEventTypes[] Start = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionStart,
		MinEventTypes.onSelfSecondaryActionStart,
		MinEventTypes.onSelfAction2Start
	};

	// Token: 0x04002D2B RID: 11563
	public static MinEventTypes[] Update = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionUpdate,
		MinEventTypes.onSelfSecondaryActionUpdate,
		MinEventTypes.onSelfAction2Update
	};

	// Token: 0x04002D2C RID: 11564
	public static MinEventTypes[] End = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionEnd,
		MinEventTypes.onSelfSecondaryActionEnd,
		MinEventTypes.onSelfAction2End
	};
}
