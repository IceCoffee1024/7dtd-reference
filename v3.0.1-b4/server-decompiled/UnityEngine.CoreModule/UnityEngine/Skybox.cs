using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000178 RID: 376
	[NativeHeader("Runtime/Camera/Skybox.h")]
	public sealed class Skybox : Behaviour
	{
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000FE4 RID: 4068
		// (set) Token: 0x06000FE5 RID: 4069
		public extern Material material
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
