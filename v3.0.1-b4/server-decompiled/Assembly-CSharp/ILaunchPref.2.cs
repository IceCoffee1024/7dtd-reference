using System;

// Token: 0x0200122E RID: 4654
public interface ILaunchPref<out T> : ILaunchPref
{
	// Token: 0x1700117B RID: 4475
	// (get) Token: 0x06009408 RID: 37896
	T Value { get; }
}
