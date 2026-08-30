using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000171 RID: 369
	[NativeHeader("Runtime/Camera/OcclusionArea.h")]
	public sealed class OcclusionArea : Component
	{
		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000F5D RID: 3933 RVA: 0x00015B38 File Offset: 0x00013D38
		// (set) Token: 0x06000F5E RID: 3934 RVA: 0x00015B4E File Offset: 0x00013D4E
		public Vector3 center
		{
			get
			{
				Vector3 vector;
				this.get_center_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_center_Injected(ref value);
			}
		}

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000F5F RID: 3935 RVA: 0x00015B58 File Offset: 0x00013D58
		// (set) Token: 0x06000F60 RID: 3936 RVA: 0x00015B6E File Offset: 0x00013D6E
		public Vector3 size
		{
			get
			{
				Vector3 vector;
				this.get_size_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_size_Injected(ref value);
			}
		}

		// Token: 0x06000F62 RID: 3938
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_center_Injected(out Vector3 ret);

		// Token: 0x06000F63 RID: 3939
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_center_Injected(ref Vector3 value);

		// Token: 0x06000F64 RID: 3940
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_size_Injected(out Vector3 ret);

		// Token: 0x06000F65 RID: 3941
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_size_Injected(ref Vector3 value);
	}
}
