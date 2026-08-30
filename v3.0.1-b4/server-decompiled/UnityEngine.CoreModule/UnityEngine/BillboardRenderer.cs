using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200013C RID: 316
	[NativeHeader("Runtime/Graphics/Billboard/BillboardRenderer.h")]
	public sealed class BillboardRenderer : Renderer
	{
		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060008CF RID: 2255
		// (set) Token: 0x060008D0 RID: 2256
		public extern BillboardAsset billboard
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
