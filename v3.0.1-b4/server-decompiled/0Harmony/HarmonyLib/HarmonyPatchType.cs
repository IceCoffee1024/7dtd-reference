using System;

namespace HarmonyLib
{
	// Token: 0x0200001C RID: 28
	public enum HarmonyPatchType
	{
		// Token: 0x0400003E RID: 62
		All,
		// Token: 0x0400003F RID: 63
		Prefix,
		// Token: 0x04000040 RID: 64
		Postfix,
		// Token: 0x04000041 RID: 65
		Transpiler,
		// Token: 0x04000042 RID: 66
		Finalizer,
		// Token: 0x04000043 RID: 67
		ReversePatch,
		// Token: 0x04000044 RID: 68
		ILManipulator
	}
}
