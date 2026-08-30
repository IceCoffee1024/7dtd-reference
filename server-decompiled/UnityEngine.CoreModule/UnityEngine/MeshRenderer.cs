using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x020001BF RID: 447
	[NativeHeader("Runtime/Graphics/Mesh/MeshRenderer.h")]
	public class MeshRenderer : Renderer
	{
		// Token: 0x0600102F RID: 4143 RVA: 0x00002669 File Offset: 0x00000869
		[RequiredByNativeCode]
		private void DontStripMeshRenderer()
		{
		}

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06001030 RID: 4144
		// (set) Token: 0x06001031 RID: 4145
		public extern Mesh additionalVertexStreams
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06001032 RID: 4146
		// (set) Token: 0x06001033 RID: 4147
		public extern Mesh enlightenVertexStream
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000362 RID: 866
		// (get) Token: 0x06001034 RID: 4148
		public extern int subMeshStartIndex
		{
			[NativeName("GetSubMeshStartIndex")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}
