using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Bindings;

namespace UnityEngine
{
	// Token: 0x02000163 RID: 355
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	[NativeHeader("Runtime/Graphics/TrailRenderer.h")]
	public sealed class TrailRenderer : Renderer
	{
		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000B8C RID: 2956 RVA: 0x00011EE8 File Offset: 0x000100E8
		[Obsolete("Use positionCount instead (UnityUpgradable) -> positionCount", false)]
		public int numPositions
		{
			get
			{
				return this.positionCount;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000B8D RID: 2957
		// (set) Token: 0x06000B8E RID: 2958
		public extern float time
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000B8F RID: 2959
		// (set) Token: 0x06000B90 RID: 2960
		public extern float startWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000B91 RID: 2961
		// (set) Token: 0x06000B92 RID: 2962
		public extern float endWidth
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000B93 RID: 2963
		// (set) Token: 0x06000B94 RID: 2964
		public extern float widthMultiplier
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000B95 RID: 2965
		// (set) Token: 0x06000B96 RID: 2966
		public extern bool autodestruct
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000B97 RID: 2967
		// (set) Token: 0x06000B98 RID: 2968
		public extern bool emitting
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000B99 RID: 2969
		// (set) Token: 0x06000B9A RID: 2970
		public extern int numCornerVertices
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000B9B RID: 2971
		// (set) Token: 0x06000B9C RID: 2972
		public extern int numCapVertices
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000B9D RID: 2973
		// (set) Token: 0x06000B9E RID: 2974
		public extern float minVertexDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06000B9F RID: 2975 RVA: 0x00011F00 File Offset: 0x00010100
		// (set) Token: 0x06000BA0 RID: 2976 RVA: 0x00011F16 File Offset: 0x00010116
		public Color startColor
		{
			get
			{
				Color color;
				this.get_startColor_Injected(out color);
				return color;
			}
			set
			{
				this.set_startColor_Injected(ref value);
			}
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000BA1 RID: 2977 RVA: 0x00011F20 File Offset: 0x00010120
		// (set) Token: 0x06000BA2 RID: 2978 RVA: 0x00011F36 File Offset: 0x00010136
		public Color endColor
		{
			get
			{
				Color color;
				this.get_endColor_Injected(out color);
				return color;
			}
			set
			{
				this.set_endColor_Injected(ref value);
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000BA3 RID: 2979
		[NativeProperty("PositionsCount")]
		public extern int positionCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000BA4 RID: 2980 RVA: 0x00011F40 File Offset: 0x00010140
		public void SetPosition(int index, Vector3 position)
		{
			this.SetPosition_Injected(index, ref position);
		}

		// Token: 0x06000BA5 RID: 2981 RVA: 0x00011F4C File Offset: 0x0001014C
		public Vector3 GetPosition(int index)
		{
			Vector3 vector;
			this.GetPosition_Injected(index, out vector);
			return vector;
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000BA6 RID: 2982 RVA: 0x00011F64 File Offset: 0x00010164
		// (set) Token: 0x06000BA7 RID: 2983 RVA: 0x00011F7A File Offset: 0x0001017A
		public Vector2 textureScale
		{
			get
			{
				Vector2 vector;
				this.get_textureScale_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_textureScale_Injected(ref value);
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000BA8 RID: 2984
		// (set) Token: 0x06000BA9 RID: 2985
		public extern float shadowBias
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000BAA RID: 2986
		// (set) Token: 0x06000BAB RID: 2987
		public extern bool generateLightingData
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000BAC RID: 2988
		// (set) Token: 0x06000BAD RID: 2989
		public extern LineTextureMode textureMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000BAE RID: 2990
		// (set) Token: 0x06000BAF RID: 2991
		public extern LineAlignment alignment
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000BB0 RID: 2992
		// (set) Token: 0x06000BB1 RID: 2993
		public extern SpriteMaskInteraction maskInteraction
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000BB2 RID: 2994
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Clear();

		// Token: 0x06000BB3 RID: 2995 RVA: 0x00011F84 File Offset: 0x00010184
		public void BakeMesh(Mesh mesh, bool useTransform = false)
		{
			this.BakeMesh(mesh, Camera.main, useTransform);
		}

		// Token: 0x06000BB4 RID: 2996
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void BakeMesh([NotNull("ArgumentNullException")] Mesh mesh, [NotNull("ArgumentNullException")] Camera camera, bool useTransform = false);

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x00011F98 File Offset: 0x00010198
		// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x00011FB0 File Offset: 0x000101B0
		public AnimationCurve widthCurve
		{
			get
			{
				return this.GetWidthCurveCopy();
			}
			set
			{
				this.SetWidthCurve(value);
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x00011FBC File Offset: 0x000101BC
		// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x00011FD4 File Offset: 0x000101D4
		public Gradient colorGradient
		{
			get
			{
				return this.GetColorGradientCopy();
			}
			set
			{
				this.SetColorGradient(value);
			}
		}

		// Token: 0x06000BB9 RID: 3001
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern AnimationCurve GetWidthCurveCopy();

		// Token: 0x06000BBA RID: 3002
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetWidthCurve([NotNull("ArgumentNullException")] AnimationCurve curve);

		// Token: 0x06000BBB RID: 3003
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Gradient GetColorGradientCopy();

		// Token: 0x06000BBC RID: 3004
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetColorGradient([NotNull("ArgumentNullException")] Gradient curve);

		// Token: 0x06000BBD RID: 3005
		[FreeFunction(Name = "TrailRendererScripting::GetPositions", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetPositions([NotNull("ArgumentNullException")] [Out] Vector3[] positions);

		// Token: 0x06000BBE RID: 3006
		[FreeFunction(Name = "TrailRendererScripting::GetVisiblePositions", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern int GetVisiblePositions([NotNull("ArgumentNullException")] [Out] Vector3[] positions);

		// Token: 0x06000BBF RID: 3007
		[FreeFunction(Name = "TrailRendererScripting::SetPositions", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void SetPositions([NotNull("ArgumentNullException")] Vector3[] positions);

		// Token: 0x06000BC0 RID: 3008 RVA: 0x00011FDF File Offset: 0x000101DF
		[FreeFunction(Name = "TrailRendererScripting::AddPosition", HasExplicitThis = true)]
		public void AddPosition(Vector3 position)
		{
			this.AddPosition_Injected(ref position);
		}

		// Token: 0x06000BC1 RID: 3009
		[FreeFunction(Name = "TrailRendererScripting::AddPositions", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddPositions([NotNull("ArgumentNullException")] Vector3[] positions);

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00011FE9 File Offset: 0x000101E9
		public void SetPositions(NativeArray<Vector3> positions)
		{
			this.SetPositionsWithNativeContainer((IntPtr)positions.GetUnsafeReadOnlyPtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x00012007 File Offset: 0x00010207
		public void SetPositions(NativeSlice<Vector3> positions)
		{
			this.SetPositionsWithNativeContainer((IntPtr)positions.GetUnsafeReadOnlyPtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x00012028 File Offset: 0x00010228
		public int GetPositions([Out] NativeArray<Vector3> positions)
		{
			return this.GetPositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00012054 File Offset: 0x00010254
		public int GetPositions([Out] NativeSlice<Vector3> positions)
		{
			return this.GetPositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x00012080 File Offset: 0x00010280
		public int GetVisiblePositions([Out] NativeArray<Vector3> positions)
		{
			return this.GetVisiblePositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x000120AC File Offset: 0x000102AC
		public int GetVisiblePositions([Out] NativeSlice<Vector3> positions)
		{
			return this.GetVisiblePositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x000120D7 File Offset: 0x000102D7
		public void AddPositions([Out] NativeArray<Vector3> positions)
		{
			this.AddPositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000BC9 RID: 3017 RVA: 0x000120F5 File Offset: 0x000102F5
		public void AddPositions([Out] NativeSlice<Vector3> positions)
		{
			this.AddPositionsWithNativeContainer((IntPtr)positions.GetUnsafePtr<Vector3>(), positions.Length);
		}

		// Token: 0x06000BCA RID: 3018
		[FreeFunction(Name = "TrailRendererScripting::SetPositionsWithNativeContainer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPositionsWithNativeContainer(IntPtr positions, int count);

		// Token: 0x06000BCB RID: 3019
		[FreeFunction(Name = "TrailRendererScripting::GetPositionsWithNativeContainer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetPositionsWithNativeContainer(IntPtr positions, int length);

		// Token: 0x06000BCC RID: 3020
		[FreeFunction(Name = "TrailRendererScripting::GetVisiblePositionsWithNativeContainer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetVisiblePositionsWithNativeContainer(IntPtr positions, int length);

		// Token: 0x06000BCD RID: 3021
		[FreeFunction(Name = "TrailRendererScripting::AddPositionsWithNativeContainer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddPositionsWithNativeContainer(IntPtr positions, int length);

		// Token: 0x06000BCF RID: 3023
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_startColor_Injected(out Color ret);

		// Token: 0x06000BD0 RID: 3024
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_startColor_Injected(ref Color value);

		// Token: 0x06000BD1 RID: 3025
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_endColor_Injected(out Color ret);

		// Token: 0x06000BD2 RID: 3026
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_endColor_Injected(ref Color value);

		// Token: 0x06000BD3 RID: 3027
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetPosition_Injected(int index, ref Vector3 position);

		// Token: 0x06000BD4 RID: 3028
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetPosition_Injected(int index, out Vector3 ret);

		// Token: 0x06000BD5 RID: 3029
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_textureScale_Injected(out Vector2 ret);

		// Token: 0x06000BD6 RID: 3030
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_textureScale_Injected(ref Vector2 value);

		// Token: 0x06000BD7 RID: 3031
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void AddPosition_Injected(ref Vector3 position);
	}
}
