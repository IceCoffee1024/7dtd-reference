using System;

// Token: 0x02001274 RID: 4724
public interface ILaunchPref<out T> : ILaunchPref
{
	// Token: 0x17001205 RID: 4613
	// (get) Token: 0x060095F1 RID: 38385
	T Value { get; }
}
