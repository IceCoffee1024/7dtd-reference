using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000173 RID: 371
	[NativeHeader("Runtime/Camera/Flare.h")]
	public sealed class LensFlare : Behaviour
	{
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000F68 RID: 3944
		// (set) Token: 0x06000F69 RID: 3945
		public extern float brightness
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000F6A RID: 3946
		// (set) Token: 0x06000F6B RID: 3947
		public extern float fadeSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000F6C RID: 3948 RVA: 0x00015B8C File Offset: 0x00013D8C
		// (set) Token: 0x06000F6D RID: 3949 RVA: 0x00015BA2 File Offset: 0x00013DA2
		public Color color
		{
			get
			{
				Color color;
				this.get_color_Injected(out color);
				return color;
			}
			set
			{
				this.set_color_Injected(ref value);
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000F6E RID: 3950
		// (set) Token: 0x06000F6F RID: 3951
		public extern Flare flare
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000F71 RID: 3953
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_color_Injected(out Color ret);

		// Token: 0x06000F72 RID: 3954
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_color_Injected(ref Color value);
	}
}
