using System;

namespace HarmonyLib
{
	// Token: 0x0200001A RID: 26
	public enum MethodType
	{
		// Token: 0x04000031 RID: 49
		Normal,
		// Token: 0x04000032 RID: 50
		Getter,
		// Token: 0x04000033 RID: 51
		Setter,
		// Token: 0x04000034 RID: 52
		Constructor,
		// Token: 0x04000035 RID: 53
		StaticConstructor,
		// Token: 0x04000036 RID: 54
		Enumerator,
		// Token: 0x04000037 RID: 55
		Async
	}
}
