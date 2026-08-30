using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000149 RID: 329
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	public struct RenderBuffer
	{
		// Token: 0x0600094E RID: 2382 RVA: 0x0000EFED File Offset: 0x0000D1ED
		[FreeFunction(Name = "RenderBufferScripting::SetLoadAction", HasExplicitThis = true)]
		internal void SetLoadAction(RenderBufferLoadAction action)
		{
			RenderBuffer.SetLoadAction_Injected(ref this, action);
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0000EFF6 File Offset: 0x0000D1F6
		[FreeFunction(Name = "RenderBufferScripting::SetStoreAction", HasExplicitThis = true)]
		internal void SetStoreAction(RenderBufferStoreAction action)
		{
			RenderBuffer.SetStoreAction_Injected(ref this, action);
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0000EFFF File Offset: 0x0000D1FF
		[FreeFunction(Name = "RenderBufferScripting::GetLoadAction", HasExplicitThis = true)]
		internal RenderBufferLoadAction GetLoadAction()
		{
			return RenderBuffer.GetLoadAction_Injected(ref this);
		}

		// Token: 0x06000951 RID: 2385 RVA: 0x0000F007 File Offset: 0x0000D207
		[FreeFunction(Name = "RenderBufferScripting::GetStoreAction", HasExplicitThis = true)]
		internal RenderBufferStoreAction GetStoreAction()
		{
			return RenderBuffer.GetStoreAction_Injected(ref this);
		}

		// Token: 0x06000952 RID: 2386 RVA: 0x0000F00F File Offset: 0x0000D20F
		[FreeFunction(Name = "RenderBufferScripting::GetNativeRenderBufferPtr", HasExplicitThis = true)]
		public IntPtr GetNativeRenderBufferPtr()
		{
			return RenderBuffer.GetNativeRenderBufferPtr_Injected(ref this);
		}

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x06000953 RID: 2387 RVA: 0x0000F018 File Offset: 0x0000D218
		// (set) Token: 0x06000954 RID: 2388 RVA: 0x0000F030 File Offset: 0x0000D230
		internal RenderBufferLoadAction loadAction
		{
			get
			{
				return this.GetLoadAction();
			}
			set
			{
				this.SetLoadAction(value);
			}
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x06000955 RID: 2389 RVA: 0x0000F03C File Offset: 0x0000D23C
		// (set) Token: 0x06000956 RID: 2390 RVA: 0x0000F054 File Offset: 0x0000D254
		internal RenderBufferStoreAction storeAction
		{
			get
			{
				return this.GetStoreAction();
			}
			set
			{
				this.SetStoreAction(value);
			}
		}

		// Token: 0x06000957 RID: 2391
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetLoadAction_Injected(ref RenderBuffer _unity_self, RenderBufferLoadAction action);

		// Token: 0x06000958 RID: 2392
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetStoreAction_Injected(ref RenderBuffer _unity_self, RenderBufferStoreAction action);

		// Token: 0x06000959 RID: 2393
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RenderBufferLoadAction GetLoadAction_Injected(ref RenderBuffer _unity_self);

		// Token: 0x0600095A RID: 2394
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern RenderBufferStoreAction GetStoreAction_Injected(ref RenderBuffer _unity_self);

		// Token: 0x0600095B RID: 2395
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern IntPtr GetNativeRenderBufferPtr_Injected(ref RenderBuffer _unity_self);

		// Token: 0x04000420 RID: 1056
		internal int m_RenderTextureInstanceID;

		// Token: 0x04000421 RID: 1057
		internal IntPtr m_BufferPtr;
	}
}
