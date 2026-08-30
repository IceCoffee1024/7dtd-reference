using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Rendering
{
	// Token: 0x02000463 RID: 1123
	[NativeHeader("Runtime/Graphics/ScriptableRenderLoop/RendererList.h")]
	[MovedFrom("UnityEngine.Rendering.RendererUtils")]
	public struct RendererList
	{
		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x060025A8 RID: 9640 RVA: 0x000403E5 File Offset: 0x0003E5E5
		public bool isValid
		{
			get
			{
				return RendererList.get_isValid_Injected(ref this);
			}
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x000403ED File Offset: 0x0003E5ED
		internal RendererList(UIntPtr ctx, uint indx)
		{
			this.context = ctx;
			this.index = indx;
			this.frame = 0U;
			this.type = 0U;
		}

		// Token: 0x060025AB RID: 9643
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool get_isValid_Injected(ref RendererList _unity_self);

		// Token: 0x04000E52 RID: 3666
		internal UIntPtr context;

		// Token: 0x04000E53 RID: 3667
		internal uint index;

		// Token: 0x04000E54 RID: 3668
		internal uint frame;

		// Token: 0x04000E55 RID: 3669
		internal uint type;

		// Token: 0x04000E56 RID: 3670
		public static readonly RendererList nullRendererList = new RendererList(UIntPtr.Zero, uint.MaxValue);
	}
}
