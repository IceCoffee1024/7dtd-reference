using System;

// Token: 0x02001273 RID: 4723
public interface ILaunchPref
{
	// Token: 0x17001204 RID: 4612
	// (get) Token: 0x060095EF RID: 38383
	string Name { get; }

	// Token: 0x060095F0 RID: 38384
	bool TrySet(string stringRepresentation);
}
