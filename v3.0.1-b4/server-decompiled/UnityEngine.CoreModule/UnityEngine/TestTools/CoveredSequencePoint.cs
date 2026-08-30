using System;
using System.Reflection;
using UnityEngine.Bindings;

namespace UnityEngine.TestTools
{
	// Token: 0x020004B5 RID: 1205
	[NativeType(CodegenOptions.Custom, "ManagedCoveredSequencePoint", Header = "Runtime/Scripting/ScriptingCoverage.bindings.h")]
	public struct CoveredSequencePoint
	{
		// Token: 0x04000F96 RID: 3990
		public MethodBase method;

		// Token: 0x04000F97 RID: 3991
		public uint ilOffset;

		// Token: 0x04000F98 RID: 3992
		public uint hitCount;

		// Token: 0x04000F99 RID: 3993
		public string filename;

		// Token: 0x04000F9A RID: 3994
		public uint line;

		// Token: 0x04000F9B RID: 3995
		public uint column;
	}
}
