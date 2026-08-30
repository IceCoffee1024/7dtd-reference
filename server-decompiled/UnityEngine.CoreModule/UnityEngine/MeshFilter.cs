using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000179 RID: 377
	[NativeHeader("Runtime/Graphics/Mesh/MeshFilter.h")]
	[RequireComponent(typeof(Transform))]
	public sealed class MeshFilter : Component
	{
		// Token: 0x06000FE7 RID: 4071 RVA: 0x00002669 File Offset: 0x00000869
		[RequiredByNativeCode]
		private void DontStripMeshFilter()
		{
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06000FE8 RID: 4072
		// (set) Token: 0x06000FE9 RID: 4073
		public extern Mesh sharedMesh
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06000FEA RID: 4074
		// (set) Token: 0x06000FEB RID: 4075
		public extern Mesh mesh
		{
			[NativeName("GetInstantiatedMeshFromScript")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeName("SetInstantiatedMesh")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
