using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x020001C4 RID: 452
	[NativeHeader("Runtime/Graphics/LOD/LODGroup.h")]
	[StaticAccessor("GetLODGroupManager()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Graphics/LOD/LODUtility.h")]
	[NativeHeader("Runtime/Graphics/LOD/LODGroupManager.h")]
	public class LODGroup : Component
	{
		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06001042 RID: 4162 RVA: 0x00015F28 File Offset: 0x00014128
		// (set) Token: 0x06001043 RID: 4163 RVA: 0x00015F3E File Offset: 0x0001413E
		public Vector3 localReferencePoint
		{
			get
			{
				Vector3 vector;
				this.get_localReferencePoint_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_localReferencePoint_Injected(ref value);
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06001044 RID: 4164
		// (set) Token: 0x06001045 RID: 4165
		public extern float size
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001046 RID: 4166
		public extern int lodCount
		{
			[NativeMethod("GetLODCount")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001047 RID: 4167
		// (set) Token: 0x06001048 RID: 4168
		public extern bool lastLODBillboard
		{
			[NativeMethod("GetLastLODIsBillboard")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeMethod("SetLastLODIsBillboard")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001049 RID: 4169
		// (set) Token: 0x0600104A RID: 4170
		public extern LODFadeMode fadeMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600104B RID: 4171
		// (set) Token: 0x0600104C RID: 4172
		public extern bool animateCrossFading
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600104D RID: 4173
		// (set) Token: 0x0600104E RID: 4174
		public extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600104F RID: 4175
		[FreeFunction("UpdateLODGroupBoundingBox", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RecalculateBounds();

		// Token: 0x06001050 RID: 4176
		[FreeFunction("GetLODs_Binding", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern LOD[] GetLODs();

		// Token: 0x06001051 RID: 4177 RVA: 0x00015F48 File Offset: 0x00014148
		[Obsolete("Use SetLODs instead.")]
		public void SetLODS(LOD[] lods)
		{
			this.SetLODs(lods);
		}

		// Token: 0x06001052 RID: 4178
		[FreeFunction("SetLODs_Binding", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetLODs([Unmarshalled] LOD[] lods);

		// Token: 0x06001053 RID: 4179
		[FreeFunction("ForceLODLevel", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ForceLOD(int index);

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001054 RID: 4180
		// (set) Token: 0x06001055 RID: 4181
		[StaticAccessor("GetLODGroupManager()")]
		public static extern float crossFadeAnimationDuration
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001056 RID: 4182 RVA: 0x00015F54 File Offset: 0x00014154
		internal Vector3 worldReferencePoint
		{
			get
			{
				Vector3 vector;
				this.get_worldReferencePoint_Injected(out vector);
				return vector;
			}
		}

		// Token: 0x06001058 RID: 4184
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_localReferencePoint_Injected(out Vector3 ret);

		// Token: 0x06001059 RID: 4185
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_localReferencePoint_Injected(ref Vector3 value);

		// Token: 0x0600105A RID: 4186
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_worldReferencePoint_Injected(out Vector3 ret);
	}
}
