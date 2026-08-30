using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Internal;

namespace UnityEngine
{
	// Token: 0x0200014C RID: 332
	[NativeHeader("Runtime/GfxDevice/GfxDevice.h")]
	[StaticAccessor("GetGfxDevice()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Camera/Camera.h")]
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	[NativeHeader("Runtime/Camera/CameraUtil.h")]
	public sealed class GL
	{
		// Token: 0x06000A41 RID: 2625
		[NativeName("ImmediateVertex")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Vertex3(float x, float y, float z);

		// Token: 0x06000A42 RID: 2626 RVA: 0x00010FFC File Offset: 0x0000F1FC
		public static void Vertex(Vector3 v)
		{
			GL.Vertex3(v.x, v.y, v.z);
		}

		// Token: 0x06000A43 RID: 2627
		[NativeName("ImmediateVertices")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal unsafe static extern void Vertices(Vector3* v, Vector3* coords, Vector4* colors, int length);

		// Token: 0x06000A44 RID: 2628
		[NativeName("ImmediateTexCoordAll")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void TexCoord3(float x, float y, float z);

		// Token: 0x06000A45 RID: 2629 RVA: 0x00011017 File Offset: 0x0000F217
		public static void TexCoord(Vector3 v)
		{
			GL.TexCoord3(v.x, v.y, v.z);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00011032 File Offset: 0x0000F232
		public static void TexCoord2(float x, float y)
		{
			GL.TexCoord3(x, y, 0f);
		}

		// Token: 0x06000A47 RID: 2631
		[NativeName("ImmediateTexCoord")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void MultiTexCoord3(int unit, float x, float y, float z);

		// Token: 0x06000A48 RID: 2632 RVA: 0x00011042 File Offset: 0x0000F242
		public static void MultiTexCoord(int unit, Vector3 v)
		{
			GL.MultiTexCoord3(unit, v.x, v.y, v.z);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0001105E File Offset: 0x0000F25E
		public static void MultiTexCoord2(int unit, float x, float y)
		{
			GL.MultiTexCoord3(unit, x, y, 0f);
		}

		// Token: 0x06000A4A RID: 2634
		[NativeName("ImmediateColor")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ImmediateColor(float r, float g, float b, float a);

		// Token: 0x06000A4B RID: 2635 RVA: 0x0001106F File Offset: 0x0000F26F
		public static void Color(Color c)
		{
			GL.ImmediateColor(c.r, c.g, c.b, c.a);
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x06000A4C RID: 2636
		// (set) Token: 0x06000A4D RID: 2637
		public static extern bool wireframe
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x06000A4E RID: 2638
		// (set) Token: 0x06000A4F RID: 2639
		public static extern bool sRGBWrite
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x06000A50 RID: 2640
		// (set) Token: 0x06000A51 RID: 2641
		[NativeProperty("UserBackfaceMode")]
		public static extern bool invertCulling
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000A52 RID: 2642
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Flush();

		// Token: 0x06000A53 RID: 2643
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void RenderTargetBarrier();

		// Token: 0x06000A54 RID: 2644 RVA: 0x00011090 File Offset: 0x0000F290
		private static Matrix4x4 GetWorldViewMatrix()
		{
			Matrix4x4 matrix4x;
			GL.GetWorldViewMatrix_Injected(out matrix4x);
			return matrix4x;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x000110A5 File Offset: 0x0000F2A5
		private static void SetViewMatrix(Matrix4x4 m)
		{
			GL.SetViewMatrix_Injected(ref m);
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000A56 RID: 2646 RVA: 0x000110B0 File Offset: 0x0000F2B0
		// (set) Token: 0x06000A57 RID: 2647 RVA: 0x000110C7 File Offset: 0x0000F2C7
		public static Matrix4x4 modelview
		{
			get
			{
				return GL.GetWorldViewMatrix();
			}
			set
			{
				GL.SetViewMatrix(value);
			}
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x000110D1 File Offset: 0x0000F2D1
		[NativeName("SetWorldMatrix")]
		public static void MultMatrix(Matrix4x4 m)
		{
			GL.MultMatrix_Injected(ref m);
		}

		// Token: 0x06000A59 RID: 2649
		[NativeName("InsertCustomMarker")]
		[Obsolete("IssuePluginEvent(eventID) is deprecated. Use IssuePluginEvent(callback, eventID) instead.", false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void IssuePluginEvent(int eventID);

		// Token: 0x06000A5A RID: 2650
		[Obsolete("SetRevertBackfacing(revertBackFaces) is deprecated. Use invertCulling property instead. (UnityUpgradable) -> invertCulling", false)]
		[NativeName("SetUserBackfaceMode")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetRevertBackfacing(bool revertBackFaces);

		// Token: 0x06000A5B RID: 2651
		[FreeFunction("GLPushMatrixScript")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PushMatrix();

		// Token: 0x06000A5C RID: 2652
		[FreeFunction("GLPopMatrixScript")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void PopMatrix();

		// Token: 0x06000A5D RID: 2653
		[FreeFunction("GLLoadIdentityScript")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LoadIdentity();

		// Token: 0x06000A5E RID: 2654
		[FreeFunction("GLLoadOrthoScript")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LoadOrtho();

		// Token: 0x06000A5F RID: 2655
		[FreeFunction("GLLoadPixelMatrixScript")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void LoadPixelMatrix();

		// Token: 0x06000A60 RID: 2656 RVA: 0x000110DA File Offset: 0x0000F2DA
		[FreeFunction("GLLoadProjectionMatrixScript")]
		public static void LoadProjectionMatrix(Matrix4x4 mat)
		{
			GL.LoadProjectionMatrix_Injected(ref mat);
		}

		// Token: 0x06000A61 RID: 2657
		[FreeFunction("GLInvalidateState")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void InvalidateState();

		// Token: 0x06000A62 RID: 2658 RVA: 0x000110E4 File Offset: 0x0000F2E4
		[FreeFunction("GLGetGPUProjectionMatrix")]
		public static Matrix4x4 GetGPUProjectionMatrix(Matrix4x4 proj, bool renderIntoTexture)
		{
			Matrix4x4 matrix4x;
			GL.GetGPUProjectionMatrix_Injected(ref proj, renderIntoTexture, out matrix4x);
			return matrix4x;
		}

		// Token: 0x06000A63 RID: 2659
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GLLoadPixelMatrixScript(float left, float right, float bottom, float top);

		// Token: 0x06000A64 RID: 2660 RVA: 0x000110FC File Offset: 0x0000F2FC
		public static void LoadPixelMatrix(float left, float right, float bottom, float top)
		{
			GL.GLLoadPixelMatrixScript(left, right, bottom, top);
		}

		// Token: 0x06000A65 RID: 2661
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GLIssuePluginEvent(IntPtr callback, int eventID);

		// Token: 0x06000A66 RID: 2662 RVA: 0x0001110C File Offset: 0x0000F30C
		public static void IssuePluginEvent(IntPtr callback, int eventID)
		{
			bool flag = callback == IntPtr.Zero;
			if (flag)
			{
				throw new ArgumentException("Null callback specified.", "callback");
			}
			GL.GLIssuePluginEvent(callback, eventID);
		}

		// Token: 0x06000A67 RID: 2663
		[FreeFunction("GLBegin", ThrowsException = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void Begin(int mode);

		// Token: 0x06000A68 RID: 2664
		[FreeFunction("GLEnd")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void End();

		// Token: 0x06000A69 RID: 2665 RVA: 0x00011141 File Offset: 0x0000F341
		[FreeFunction]
		private static void GLClear(bool clearDepth, bool clearColor, Color backgroundColor, float depth)
		{
			GL.GLClear_Injected(clearDepth, clearColor, ref backgroundColor, depth);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x0001114D File Offset: 0x0000F34D
		public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor, [DefaultValue("1.0f")] float depth)
		{
			GL.GLClear(clearDepth, clearColor, backgroundColor, depth);
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0001115A File Offset: 0x0000F35A
		public static void Clear(bool clearDepth, bool clearColor, Color backgroundColor)
		{
			GL.GLClear(clearDepth, clearColor, backgroundColor, 1f);
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x0001116B File Offset: 0x0000F36B
		[FreeFunction("SetGLViewport")]
		public static void Viewport(Rect pixelRect)
		{
			GL.Viewport_Injected(ref pixelRect);
		}

		// Token: 0x06000A6D RID: 2669
		[FreeFunction("ClearWithSkybox")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void ClearWithSkybox(bool clearDepth, Camera camera);

		// Token: 0x06000A6F RID: 2671
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetWorldViewMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06000A70 RID: 2672
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void SetViewMatrix_Injected(ref Matrix4x4 m);

		// Token: 0x06000A71 RID: 2673
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void MultMatrix_Injected(ref Matrix4x4 m);

		// Token: 0x06000A72 RID: 2674
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void LoadProjectionMatrix_Injected(ref Matrix4x4 mat);

		// Token: 0x06000A73 RID: 2675
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GetGPUProjectionMatrix_Injected(ref Matrix4x4 proj, bool renderIntoTexture, out Matrix4x4 ret);

		// Token: 0x06000A74 RID: 2676
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void GLClear_Injected(bool clearDepth, bool clearColor, ref Color backgroundColor, float depth);

		// Token: 0x06000A75 RID: 2677
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void Viewport_Injected(ref Rect pixelRect);

		// Token: 0x0400042A RID: 1066
		public const int TRIANGLES = 4;

		// Token: 0x0400042B RID: 1067
		public const int TRIANGLE_STRIP = 5;

		// Token: 0x0400042C RID: 1068
		public const int QUADS = 7;

		// Token: 0x0400042D RID: 1069
		public const int LINES = 1;

		// Token: 0x0400042E RID: 1070
		public const int LINE_STRIP = 2;
	}
}
