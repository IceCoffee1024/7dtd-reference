using System;

// Token: 0x02001261 RID: 4705
public interface ILaunchPref
{
	// Token: 0x170011F9 RID: 4601
	// (get) Token: 0x06009598 RID: 38296
	string Name { get; }

	// Token: 0x06009599 RID: 38297
	bool TrySet(string stringRepresentation);
}
