using System;

// Token: 0x02001262 RID: 4706
public interface ILaunchPref<out T> : ILaunchPref
{
	// Token: 0x170011FA RID: 4602
	// (get) Token: 0x0600959A RID: 38298
	T Value { get; }
}
