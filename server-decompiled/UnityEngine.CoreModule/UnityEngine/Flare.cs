using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000172 RID: 370
	[NativeHeader("Runtime/Camera/Flare.h")]
	public sealed class Flare : Object
	{
		// Token: 0x06000F66 RID: 3942 RVA: 0x00015B78 File Offset: 0x00013D78
		public Flare()
		{
			Flare.Internal_Create(this);
		}

		// Token: 0x06000F67 RID: 3943
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] Flare self);
	}
}
