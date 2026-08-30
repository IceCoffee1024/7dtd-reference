using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000136 RID: 310
	[NativeHeader("Runtime/Graphics/LightingSettings.h")]
	[PreventReadOnlyInstanceModification]
	public sealed class LightingSettings : Object
	{
		// Token: 0x06000865 RID: 2149 RVA: 0x00002669 File Offset: 0x00000869
		[RequiredByNativeCode]
		internal void LightingSettingsDontStripMe()
		{
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x0000DB3E File Offset: 0x0000BD3E
		public LightingSettings()
		{
			LightingSettings.Internal_Create(this);
		}

		// Token: 0x06000867 RID: 2151
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Internal_Create([Writable] LightingSettings self);

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000868 RID: 2152
		// (set) Token: 0x06000869 RID: 2153
		[NativeName("EnableBakedLightmaps")]
		public extern bool bakedGI
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600086A RID: 2154
		// (set) Token: 0x0600086B RID: 2155
		[NativeName("EnableRealtimeLightmaps")]
		public extern bool realtimeGI
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x0600086C RID: 2156
		// (set) Token: 0x0600086D RID: 2157
		[NativeName("RealtimeEnvironmentLighting")]
		public extern bool realtimeEnvironmentLighting
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}
	}
}
