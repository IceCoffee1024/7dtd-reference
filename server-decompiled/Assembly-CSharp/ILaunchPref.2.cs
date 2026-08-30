using System;

// Token: 0x0200124A RID: 4682
public interface ILaunchPref<out T> : ILaunchPref
{
	// Token: 0x170011B6 RID: 4534
	// (get) Token: 0x0600950C RID: 38156
	T Value { get; }
}
