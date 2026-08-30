using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000174 RID: 372
	[NativeHeader("Runtime/Camera/Projector.h")]
	public sealed class Projector : Behaviour
	{
		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000F73 RID: 3955
		// (set) Token: 0x06000F74 RID: 3956
		public extern float nearClipPlane
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000F75 RID: 3957
		// (set) Token: 0x06000F76 RID: 3958
		public extern float farClipPlane
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000F77 RID: 3959
		// (set) Token: 0x06000F78 RID: 3960
		public extern float fieldOfView
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000F79 RID: 3961
		// (set) Token: 0x06000F7A RID: 3962
		public extern float aspectRatio
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000F7B RID: 3963
		// (set) Token: 0x06000F7C RID: 3964
		public extern bool orthographic
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000F7D RID: 3965
		// (set) Token: 0x06000F7E RID: 3966
		public extern float orthographicSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000F7F RID: 3967
		// (set) Token: 0x06000F80 RID: 3968
		public extern int ignoreLayers
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000F81 RID: 3969
		// (set) Token: 0x06000F82 RID: 3970
		public extern Material material
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
