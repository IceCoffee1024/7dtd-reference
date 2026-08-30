using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000135 RID: 309
	[NativeHeader("Runtime/GI/DynamicGI.h")]
	public sealed class DynamicGI
	{
		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000851 RID: 2129
		// (set) Token: 0x06000852 RID: 2130
		public static extern float indirectScale
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000853 RID: 2131
		// (set) Token: 0x06000854 RID: 2132
		public static extern float updateThreshold
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000855 RID: 2133
		// (set) Token: 0x06000856 RID: 2134
		public static extern int materialUpdateTimeSlice
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x0000DB34 File Offset: 0x0000BD34
		public static void SetEmissive(Renderer renderer, Color color)
		{
			DynamicGI.SetEmissive_Injected(renderer, ref color);
		}

		// Token: 0x06000858 RID: 2136
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetEnvironmentData([NotNull("ArgumentNullException")] float[] input);

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000859 RID: 2137
		// (set) Token: 0x0600085A RID: 2138
		public static extern bool synchronousMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600085B RID: 2139
		public static extern bool isConverged
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600085C RID: 2140
		internal static extern int scheduledMaterialUpdatesCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600085D RID: 2141
		// (set) Token: 0x0600085E RID: 2142
		internal static extern bool asyncMaterialUpdates
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600085F RID: 2143
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void UpdateEnvironment();

		// Token: 0x06000860 RID: 2144 RVA: 0x00002669 File Offset: 0x00000869
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("DynamicGI.UpdateMaterials(Renderer) is deprecated; instead, use extension method from RendererExtensions: 'renderer.UpdateGIMaterials()' (UnityUpgradable).", true)]
		public static void UpdateMaterials(Renderer renderer)
		{
		}

		// Token: 0x06000861 RID: 2145 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("DynamicGI.UpdateMaterials(Terrain) is deprecated; instead, use extension method from TerrainExtensions: 'terrain.UpdateGIMaterials()' (UnityUpgradable).", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void UpdateMaterials(Object renderer)
		{
		}

		// Token: 0x06000862 RID: 2146 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("DynamicGI.UpdateMaterials(Terrain, int, int, int, int) is deprecated; instead, use extension method from TerrainExtensions: 'terrain.UpdateGIMaterials(x, y, width, height)' (UnityUpgradable).", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public static void UpdateMaterials(Object renderer, int x, int y, int width, int height)
		{
		}

		// Token: 0x06000864 RID: 2148
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetEmissive_Injected(Renderer renderer, ref Color color);
	}
}
