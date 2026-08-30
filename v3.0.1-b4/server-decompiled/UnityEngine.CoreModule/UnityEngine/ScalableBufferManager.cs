using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x0200014D RID: 333
	[StaticAccessor("ScalableBufferManager::GetInstance()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/GfxDevice/ScalableBufferManager.h")]
	public static class ScalableBufferManager
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000A76 RID: 2678
		public static extern float widthScaleFactor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000A77 RID: 2679
		public static extern float heightScaleFactor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000A78 RID: 2680
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ResizeBuffers(float widthScale, float heightScale);
	}
}
