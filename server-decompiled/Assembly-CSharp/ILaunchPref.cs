using System;

// Token: 0x0200122D RID: 4653
public interface ILaunchPref
{
	// Token: 0x1700117A RID: 4474
	// (get) Token: 0x06009406 RID: 37894
	string Name { get; }

	// Token: 0x06009407 RID: 37895
	bool TrySet(string stringRepresentation);
}
