using System;

// Token: 0x02000725 RID: 1829
public class MinEvent
{
	// Token: 0x04002D23 RID: 11555
	public static MinEventTypes[] Start = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionStart,
		MinEventTypes.onSelfSecondaryActionStart,
		MinEventTypes.onSelfAction2Start
	};

	// Token: 0x04002D24 RID: 11556
	public static MinEventTypes[] Update = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionUpdate,
		MinEventTypes.onSelfSecondaryActionUpdate,
		MinEventTypes.onSelfAction2Update
	};

	// Token: 0x04002D25 RID: 11557
	public static MinEventTypes[] End = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionEnd,
		MinEventTypes.onSelfSecondaryActionEnd,
		MinEventTypes.onSelfAction2End
	};
}
