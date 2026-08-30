using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000257 RID: 599
	[VisibleToOtherModules]
	[NativeHeader("Runtime/Export/Scripting/ScriptingRuntime.h")]
	internal class ScriptingRuntime
	{
		// Token: 0x06001973 RID: 6515
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern string[] GetAllUserAssemblies();
	}
}
