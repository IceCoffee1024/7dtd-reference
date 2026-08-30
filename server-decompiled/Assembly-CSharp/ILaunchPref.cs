using System;

// Token: 0x02001249 RID: 4681
public interface ILaunchPref
{
	// Token: 0x170011B5 RID: 4533
	// (get) Token: 0x0600950A RID: 38154
	string Name { get; }

	// Token: 0x0600950B RID: 38155
	bool TrySet(string stringRepresentation);
}
