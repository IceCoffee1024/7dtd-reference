using System;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Scripting;

namespace UnityEngine
{
	// Token: 0x02000150 RID: 336
	[NativeHeader("Runtime/Graphics/LightmapData.h")]
	[UsedByNativeCode]
	[StructLayout(LayoutKind.Sequential)]
	public sealed class LightmapData
	{
		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000A7F RID: 2687 RVA: 0x00011174 File Offset: 0x0000F374
		// (set) Token: 0x06000A80 RID: 2688 RVA: 0x0001118C File Offset: 0x0000F38C
		[Obsolete("Use lightmapColor property (UnityUpgradable) -> lightmapColor", false)]
		public Texture2D lightmapLight
		{
			get
			{
				return this.m_Light;
			}
			set
			{
				this.m_Light = value;
			}
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000A81 RID: 2689 RVA: 0x00011198 File Offset: 0x0000F398
		// (set) Token: 0x06000A82 RID: 2690 RVA: 0x0001118C File Offset: 0x0000F38C
		public Texture2D lightmapColor
		{
			get
			{
				return this.m_Light;
			}
			set
			{
				this.m_Light = value;
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000A83 RID: 2691 RVA: 0x000111B0 File Offset: 0x0000F3B0
		// (set) Token: 0x06000A84 RID: 2692 RVA: 0x000111C8 File Offset: 0x0000F3C8
		public Texture2D lightmapDir
		{
			get
			{
				return this.m_Dir;
			}
			set
			{
				this.m_Dir = value;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000A85 RID: 2693 RVA: 0x000111D4 File Offset: 0x0000F3D4
		// (set) Token: 0x06000A86 RID: 2694 RVA: 0x000111EC File Offset: 0x0000F3EC
		public Texture2D shadowMask
		{
			get
			{
				return this.m_ShadowMask;
			}
			set
			{
				this.m_ShadowMask = value;
			}
		}

		// Token: 0x0400043B RID: 1083
		internal Texture2D m_Light;

		// Token: 0x0400043C RID: 1084
		internal Texture2D m_Dir;

		// Token: 0x0400043D RID: 1085
		internal Texture2D m_ShadowMask;
	}
}
