using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000151 RID: 337
	[NativeHeader("Runtime/Graphics/LightmapSettings.h")]
	[StaticAccessor("GetLightmapSettings()")]
	public sealed class LightmapSettings : Object
	{
		// Token: 0x06000A88 RID: 2696 RVA: 0x000111F6 File Offset: 0x0000F3F6
		private LightmapSettings()
		{
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000A89 RID: 2697
		// (set) Token: 0x06000A8A RID: 2698
		public static extern LightmapData[] lightmaps
		{
			[FreeFunction]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction(ThrowsException = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			[param: Unmarshalled]
			set;
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000A8B RID: 2699
		// (set) Token: 0x06000A8C RID: 2700
		public static extern LightmapsMode lightmapsMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction(ThrowsException = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000A8D RID: 2701
		// (set) Token: 0x06000A8E RID: 2702
		public static extern LightProbes lightProbes
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeName("SetLightProbes")]
			[FreeFunction]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000A8F RID: 2703
		[NativeName("ResetAndAwakeFromLoad")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Reset();

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000A90 RID: 2704 RVA: 0x00011200 File Offset: 0x0000F400
		// (set) Token: 0x06000A91 RID: 2705 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("Use lightmapsMode instead.", false)]
		public static LightmapsModeLegacy lightmapsModeLegacy
		{
			get
			{
				return LightmapsModeLegacy.Single;
			}
			set
			{
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000A92 RID: 2706 RVA: 0x00011214 File Offset: 0x0000F414
		// (set) Token: 0x06000A93 RID: 2707 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("Use QualitySettings.desiredColorSpace instead.", false)]
		public static ColorSpace bakedColorSpace
		{
			get
			{
				return QualitySettings.desiredColorSpace;
			}
			set
			{
			}
		}
	}
}
