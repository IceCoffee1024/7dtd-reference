using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000236 RID: 566
	[UsedByNativeCode]
	[NativeHeader("Runtime/Mono/MonoBehaviour.h")]
	public class Behaviour : Component
	{
		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x0600185D RID: 6237
		// (set) Token: 0x0600185E RID: 6238
		[NativeProperty]
		[RequiredByNativeCode]
		public extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x0600185F RID: 6239
		[NativeProperty]
		public extern bool isActiveAndEnabled
		{
			[NativeMethod("IsAddedToManager")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
