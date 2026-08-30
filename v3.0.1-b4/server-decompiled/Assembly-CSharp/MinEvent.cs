using System;

// Token: 0x02000719 RID: 1817
public class MinEvent
{
	// Token: 0x04002CB2 RID: 11442
	public static MinEventTypes[] Start = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionStart,
		MinEventTypes.onSelfSecondaryActionStart,
		MinEventTypes.onSelfAction2Start
	};

	// Token: 0x04002CB3 RID: 11443
	public static MinEventTypes[] Update = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionUpdate,
		MinEventTypes.onSelfSecondaryActionUpdate,
		MinEventTypes.onSelfAction2Update
	};

	// Token: 0x04002CB4 RID: 11444
	public static MinEventTypes[] End = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionEnd,
		MinEventTypes.onSelfSecondaryActionEnd,
		MinEventTypes.onSelfAction2End
	};
}
