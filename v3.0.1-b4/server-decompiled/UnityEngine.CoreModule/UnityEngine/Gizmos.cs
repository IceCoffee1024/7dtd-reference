using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x02000137 RID: 311
	[StaticAccessor("GizmoBindings", StaticAccessorType.DoubleColon)]
	[NativeHeader("Runtime/Export/Gizmos/Gizmos.bindings.h")]
	public sealed class Gizmos
	{
		// Token: 0x0600086E RID: 2158 RVA: 0x0000DB4F File Offset: 0x0000BD4F
		[NativeThrows]
		public static void DrawLine(Vector3 from, Vector3 to)
		{
			Gizmos.DrawLine_Injected(ref from, ref to);
		}

		// Token: 0x0600086F RID: 2159
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void DrawLineStrip([Span("count", true)] Vector3* points, int count, bool looped);

		// Token: 0x06000870 RID: 2160 RVA: 0x0000DB5C File Offset: 0x0000BD5C
		public unsafe static void DrawLineStrip(ReadOnlySpan<Vector3> points, bool looped)
		{
			fixed (Vector3* pinnableReference = points.GetPinnableReference())
			{
				Vector3* ptr = pinnableReference;
				Gizmos.DrawLineStrip(ptr, points.Length, looped);
			}
		}

		// Token: 0x06000871 RID: 2161
		[NativeThrows]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void DrawLineList([Span("count", true)] Vector3* points, int count);

		// Token: 0x06000872 RID: 2162 RVA: 0x0000DB8C File Offset: 0x0000BD8C
		public unsafe static void DrawLineList(ReadOnlySpan<Vector3> points)
		{
			bool flag = (points.Length & 1) != 0;
			if (flag)
			{
				throw new UnityException("You cannot draw a line list from an odd number of points, with two points per line the number of points must be even");
			}
			fixed (Vector3* pinnableReference = points.GetPinnableReference())
			{
				Vector3* ptr = pinnableReference;
				Gizmos.DrawLineList(ptr, points.Length);
			}
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x0000DBD3 File Offset: 0x0000BDD3
		[NativeThrows]
		public static void DrawWireSphere(Vector3 center, float radius)
		{
			Gizmos.DrawWireSphere_Injected(ref center, radius);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x0000DBDD File Offset: 0x0000BDDD
		[NativeThrows]
		public static void DrawSphere(Vector3 center, float radius)
		{
			Gizmos.DrawSphere_Injected(ref center, radius);
		}

		// Token: 0x06000875 RID: 2165 RVA: 0x0000DBE7 File Offset: 0x0000BDE7
		[NativeThrows]
		public static void DrawWireCube(Vector3 center, Vector3 size)
		{
			Gizmos.DrawWireCube_Injected(ref center, ref size);
		}

		// Token: 0x06000876 RID: 2166 RVA: 0x0000DBF2 File Offset: 0x0000BDF2
		[NativeThrows]
		public static void DrawCube(Vector3 center, Vector3 size)
		{
			Gizmos.DrawCube_Injected(ref center, ref size);
		}

		// Token: 0x06000877 RID: 2167 RVA: 0x0000DBFD File Offset: 0x0000BDFD
		[NativeThrows]
		public static void DrawMesh(Mesh mesh, int submeshIndex, [DefaultValue("Vector3.zero")] Vector3 position, [DefaultValue("Quaternion.identity")] Quaternion rotation, [DefaultValue("Vector3.one")] Vector3 scale)
		{
			Gizmos.DrawMesh_Injected(mesh, submeshIndex, ref position, ref rotation, ref scale);
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x0000DC0C File Offset: 0x0000BE0C
		[NativeThrows]
		public static void DrawWireMesh(Mesh mesh, int submeshIndex, [DefaultValue("Vector3.zero")] Vector3 position, [DefaultValue("Quaternion.identity")] Quaternion rotation, [DefaultValue("Vector3.one")] Vector3 scale)
		{
			Gizmos.DrawWireMesh_Injected(mesh, submeshIndex, ref position, ref rotation, ref scale);
		}

		// Token: 0x06000879 RID: 2169 RVA: 0x0000DC1B File Offset: 0x0000BE1B
		[NativeThrows]
		public static void DrawIcon(Vector3 center, string name, [DefaultValue("true")] bool allowScaling)
		{
			Gizmos.DrawIcon(center, name, allowScaling, Color.white);
		}

		// Token: 0x0600087A RID: 2170 RVA: 0x0000DC2C File Offset: 0x0000BE2C
		[NativeThrows]
		public static void DrawIcon(Vector3 center, string name, [DefaultValue("true")] bool allowScaling, [DefaultValue("Color(255,255,255,255)")] Color tint)
		{
			Gizmos.DrawIcon_Injected(ref center, name, allowScaling, ref tint);
		}

		// Token: 0x0600087B RID: 2171 RVA: 0x0000DC39 File Offset: 0x0000BE39
		[NativeThrows]
		public static void DrawGUITexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [DefaultValue("null")] Material mat)
		{
			Gizmos.DrawGUITexture_Injected(ref screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, mat);
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x0000DC4C File Offset: 0x0000BE4C
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x0000DC61 File Offset: 0x0000BE61
		public static Color color
		{
			get
			{
				Color color;
				Gizmos.get_color_Injected(out color);
				return color;
			}
			set
			{
				Gizmos.set_color_Injected(ref value);
			}
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x0000DC6C File Offset: 0x0000BE6C
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x0000DC81 File Offset: 0x0000BE81
		public static Matrix4x4 matrix
		{
			get
			{
				Matrix4x4 matrix4x;
				Gizmos.get_matrix_Injected(out matrix4x);
				return matrix4x;
			}
			set
			{
				Gizmos.set_matrix_Injected(ref value);
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000880 RID: 2176
		// (set) Token: 0x06000881 RID: 2177
		public static extern Texture exposure
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000882 RID: 2178
		public static extern float probeSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x0000DC8A File Offset: 0x0000BE8A
		public static void DrawFrustum(Vector3 center, float fov, float maxRange, float minRange, float aspect)
		{
			Gizmos.DrawFrustum_Injected(ref center, fov, maxRange, minRange, aspect);
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x0000DC98 File Offset: 0x0000BE98
		public static void DrawRay(Ray r)
		{
			Gizmos.DrawLine(r.origin, r.origin + r.direction);
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x0000DCBB File Offset: 0x0000BEBB
		public static void DrawRay(Vector3 from, Vector3 direction)
		{
			Gizmos.DrawLine(from, from + direction);
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x0000DCCC File Offset: 0x0000BECC
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position, Quaternion rotation)
		{
			Vector3 one = Vector3.one;
			Gizmos.DrawMesh(mesh, position, rotation, one);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x0000DCEC File Offset: 0x0000BEEC
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, Vector3 position)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Gizmos.DrawMesh(mesh, position, identity, one);
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x0000DD10 File Offset: 0x0000BF10
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Vector3 zero = Vector3.zero;
			Gizmos.DrawMesh(mesh, zero, identity, one);
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x0000DD3A File Offset: 0x0000BF3A
		public static void DrawMesh(Mesh mesh, [DefaultValue("Vector3.zero")] Vector3 position, [DefaultValue("Quaternion.identity")] Quaternion rotation, [DefaultValue("Vector3.one")] Vector3 scale)
		{
			Gizmos.DrawMesh(mesh, -1, position, rotation, scale);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x0000DD48 File Offset: 0x0000BF48
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation)
		{
			Vector3 one = Vector3.one;
			Gizmos.DrawMesh(mesh, submeshIndex, position, rotation, one);
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x0000DD68 File Offset: 0x0000BF68
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, int submeshIndex, Vector3 position)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Gizmos.DrawMesh(mesh, submeshIndex, position, identity, one);
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0000DD90 File Offset: 0x0000BF90
		[ExcludeFromDocs]
		public static void DrawMesh(Mesh mesh, int submeshIndex)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Vector3 zero = Vector3.zero;
			Gizmos.DrawMesh(mesh, submeshIndex, zero, identity, one);
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x0000DDBC File Offset: 0x0000BFBC
		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh, Vector3 position, Quaternion rotation)
		{
			Vector3 one = Vector3.one;
			Gizmos.DrawWireMesh(mesh, position, rotation, one);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x0000DDDC File Offset: 0x0000BFDC
		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh, Vector3 position)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Gizmos.DrawWireMesh(mesh, position, identity, one);
		}

		// Token: 0x0600088F RID: 2191 RVA: 0x0000DE00 File Offset: 0x0000C000
		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Vector3 zero = Vector3.zero;
			Gizmos.DrawWireMesh(mesh, zero, identity, one);
		}

		// Token: 0x06000890 RID: 2192 RVA: 0x0000DE2A File Offset: 0x0000C02A
		public static void DrawWireMesh(Mesh mesh, [DefaultValue("Vector3.zero")] Vector3 position, [DefaultValue("Quaternion.identity")] Quaternion rotation, [DefaultValue("Vector3.one")] Vector3 scale)
		{
			Gizmos.DrawWireMesh(mesh, -1, position, rotation, scale);
		}

		// Token: 0x06000891 RID: 2193 RVA: 0x0000DE38 File Offset: 0x0000C038
		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh, int submeshIndex, Vector3 position, Quaternion rotation)
		{
			Vector3 one = Vector3.one;
			Gizmos.DrawWireMesh(mesh, submeshIndex, position, rotation, one);
		}

		// Token: 0x06000892 RID: 2194 RVA: 0x0000DE58 File Offset: 0x0000C058
		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh, int submeshIndex, Vector3 position)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Gizmos.DrawWireMesh(mesh, submeshIndex, position, identity, one);
		}

		// Token: 0x06000893 RID: 2195 RVA: 0x0000DE80 File Offset: 0x0000C080
		[ExcludeFromDocs]
		public static void DrawWireMesh(Mesh mesh, int submeshIndex)
		{
			Vector3 one = Vector3.one;
			Quaternion identity = Quaternion.identity;
			Vector3 zero = Vector3.zero;
			Gizmos.DrawWireMesh(mesh, submeshIndex, zero, identity, one);
		}

		// Token: 0x06000894 RID: 2196 RVA: 0x0000DEAC File Offset: 0x0000C0AC
		[ExcludeFromDocs]
		public static void DrawIcon(Vector3 center, string name)
		{
			bool flag = true;
			Gizmos.DrawIcon(center, name, flag);
		}

		// Token: 0x06000895 RID: 2197 RVA: 0x0000DEC8 File Offset: 0x0000C0C8
		[ExcludeFromDocs]
		public static void DrawGUITexture(Rect screenRect, Texture texture)
		{
			Material material = null;
			Gizmos.DrawGUITexture(screenRect, texture, material);
		}

		// Token: 0x06000896 RID: 2198 RVA: 0x0000DEE1 File Offset: 0x0000C0E1
		public static void DrawGUITexture(Rect screenRect, Texture texture, [DefaultValue("null")] Material mat)
		{
			Gizmos.DrawGUITexture(screenRect, texture, 0, 0, 0, 0, mat);
		}

		// Token: 0x06000897 RID: 2199 RVA: 0x0000DEF4 File Offset: 0x0000C0F4
		[ExcludeFromDocs]
		public static void DrawGUITexture(Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder)
		{
			Material material = null;
			Gizmos.DrawGUITexture(screenRect, texture, leftBorder, rightBorder, topBorder, bottomBorder, material);
		}

		// Token: 0x06000899 RID: 2201
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawLine_Injected(ref Vector3 from, ref Vector3 to);

		// Token: 0x0600089A RID: 2202
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawWireSphere_Injected(ref Vector3 center, float radius);

		// Token: 0x0600089B RID: 2203
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawSphere_Injected(ref Vector3 center, float radius);

		// Token: 0x0600089C RID: 2204
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawWireCube_Injected(ref Vector3 center, ref Vector3 size);

		// Token: 0x0600089D RID: 2205
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawCube_Injected(ref Vector3 center, ref Vector3 size);

		// Token: 0x0600089E RID: 2206
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawMesh_Injected(Mesh mesh, int submeshIndex, [DefaultValue("Vector3.zero")] ref Vector3 position, [DefaultValue("Quaternion.identity")] ref Quaternion rotation, [DefaultValue("Vector3.one")] ref Vector3 scale);

		// Token: 0x0600089F RID: 2207
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawWireMesh_Injected(Mesh mesh, int submeshIndex, [DefaultValue("Vector3.zero")] ref Vector3 position, [DefaultValue("Quaternion.identity")] ref Quaternion rotation, [DefaultValue("Vector3.one")] ref Vector3 scale);

		// Token: 0x060008A0 RID: 2208
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawIcon_Injected(ref Vector3 center, string name, [DefaultValue("true")] bool allowScaling, [DefaultValue("Color(255,255,255,255)")] ref Color tint);

		// Token: 0x060008A1 RID: 2209
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawGUITexture_Injected(ref Rect screenRect, Texture texture, int leftBorder, int rightBorder, int topBorder, int bottomBorder, [DefaultValue("null")] Material mat);

		// Token: 0x060008A2 RID: 2210
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_color_Injected(out Color ret);

		// Token: 0x060008A3 RID: 2211
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_color_Injected(ref Color value);

		// Token: 0x060008A4 RID: 2212
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_matrix_Injected(out Matrix4x4 ret);

		// Token: 0x060008A5 RID: 2213
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_matrix_Injected(ref Matrix4x4 value);

		// Token: 0x060008A6 RID: 2214
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void DrawFrustum_Injected(ref Vector3 center, float fov, float maxRange, float minRange, float aspect);
	}
}
