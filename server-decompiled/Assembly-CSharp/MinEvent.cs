using System;

// Token: 0x0200072D RID: 1837
public class MinEvent
{
	// Token: 0x04002D3F RID: 11583
	public static MinEventTypes[] Start = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionStart,
		MinEventTypes.onSelfSecondaryActionStart,
		MinEventTypes.onSelfAction2Start
	};

	// Token: 0x04002D40 RID: 11584
	public static MinEventTypes[] Update = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionUpdate,
		MinEventTypes.onSelfSecondaryActionUpdate,
		MinEventTypes.onSelfAction2Update
	};

	// Token: 0x04002D41 RID: 11585
	public static MinEventTypes[] End = new MinEventTypes[]
	{
		MinEventTypes.onSelfPrimaryActionEnd,
		MinEventTypes.onSelfSecondaryActionEnd,
		MinEventTypes.onSelfAction2End
	};
}
