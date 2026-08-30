using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine.Experimental.Rendering
{
	// Token: 0x020004D3 RID: 1235
	[NativeHeader("Runtime/Camera/ReflectionProbes.h")]
	internal class BuiltinRuntimeReflectionSystem : IScriptableRuntimeReflectionSystem, IDisposable
	{
		// Token: 0x06002B20 RID: 11040 RVA: 0x000490AC File Offset: 0x000472AC
		public bool TickRealtimeProbes()
		{
			return BuiltinRuntimeReflectionSystem.BuiltinUpdate();
		}

		// Token: 0x06002B21 RID: 11041 RVA: 0x000490C3 File Offset: 0x000472C3
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x06002B22 RID: 11042 RVA: 0x00002669 File Offset: 0x00000869
		private void Dispose(bool disposing)
		{
		}

		// Token: 0x06002B23 RID: 11043
		[StaticAccessor("GetReflectionProbes()", Type = StaticAccessorType.Dot)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool BuiltinUpdate();

		// Token: 0x06002B24 RID: 11044 RVA: 0x000490D0 File Offset: 0x000472D0
		[RequiredByNativeCode]
		private static BuiltinRuntimeReflectionSystem Internal_BuiltinRuntimeReflectionSystem_New()
		{
			return new BuiltinRuntimeReflectionSystem();
		}
	}
}
