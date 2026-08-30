using System;
using Unity.IL2CPP.CompilerServices;

namespace UnityEngineInternal
{
	// Token: 0x0200000D RID: 13
	[Il2CppEagerStaticClassConstruction]
	public struct MathfInternal
	{
		// Token: 0x0400001C RID: 28
		public static volatile float FloatMinNormal = 1.1754944E-38f;

		// Token: 0x0400001D RID: 29
		public static volatile float FloatMinDenormal = float.Epsilon;

		// Token: 0x0400001E RID: 30
		public static bool IsFlushToZeroEnabled = MathfInternal.FloatMinDenormal == 0f;
	}
}
