using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using UnityEngine.Bindings;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Scripting;
using UnityEngineInternal;

namespace UnityEngine
{
	// Token: 0x02000166 RID: 358
	[NativeHeader("Runtime/Graphics/GraphicsScriptBindings.h")]
	[NativeHeader("Runtime/Graphics/Renderer.h")]
	[UsedByNativeCode]
	[RequireComponent(typeof(Transform))]
	public class Renderer : Component
	{
		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x0001300C File Offset: 0x0001120C
		// (set) Token: 0x06000CBB RID: 3259 RVA: 0x00013027 File Offset: 0x00011227
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Use shadowCastingMode instead.", false)]
		public bool castShadows
		{
			get
			{
				return this.shadowCastingMode > ShadowCastingMode.Off;
			}
			set
			{
				this.shadowCastingMode = (value ? ShadowCastingMode.On : ShadowCastingMode.Off);
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000CBC RID: 3260 RVA: 0x00013038 File Offset: 0x00011238
		// (set) Token: 0x06000CBD RID: 3261 RVA: 0x00013053 File Offset: 0x00011253
		[Obsolete("Use motionVectorGenerationMode instead.", false)]
		public bool motionVectors
		{
			get
			{
				return this.motionVectorGenerationMode == MotionVectorGenerationMode.Object;
			}
			set
			{
				this.motionVectorGenerationMode = (value ? MotionVectorGenerationMode.Object : MotionVectorGenerationMode.Camera);
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00013064 File Offset: 0x00011264
		// (set) Token: 0x06000CBF RID: 3263 RVA: 0x0001307F File Offset: 0x0001127F
		[Obsolete("Use lightProbeUsage instead.", false)]
		public bool useLightProbes
		{
			get
			{
				return this.lightProbeUsage > LightProbeUsage.Off;
			}
			set
			{
				this.lightProbeUsage = (value ? LightProbeUsage.BlendProbes : LightProbeUsage.Off);
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00013090 File Offset: 0x00011290
		// (set) Token: 0x06000CC1 RID: 3265 RVA: 0x000130A6 File Offset: 0x000112A6
		public Bounds bounds
		{
			[FreeFunction(Name = "RendererScripting::GetWorldBounds", HasExplicitThis = true)]
			get
			{
				Bounds bounds;
				this.get_bounds_Injected(out bounds);
				return bounds;
			}
			[NativeName("SetWorldAABB")]
			set
			{
				this.set_bounds_Injected(ref value);
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x000130B0 File Offset: 0x000112B0
		// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x000130C6 File Offset: 0x000112C6
		public Bounds localBounds
		{
			[FreeFunction(Name = "RendererScripting::GetLocalBounds", HasExplicitThis = true)]
			get
			{
				Bounds bounds;
				this.get_localBounds_Injected(out bounds);
				return bounds;
			}
			[NativeName("SetLocalAABB")]
			set
			{
				this.set_localBounds_Injected(ref value);
			}
		}

		// Token: 0x06000CC4 RID: 3268
		[NativeName("ResetWorldAABB")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetBounds();

		// Token: 0x06000CC5 RID: 3269
		[NativeName("ResetLocalAABB")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void ResetLocalBounds();

		// Token: 0x06000CC6 RID: 3270 RVA: 0x000130D0 File Offset: 0x000112D0
		[FreeFunction(Name = "RendererScripting::SetStaticLightmapST", HasExplicitThis = true)]
		private void SetStaticLightmapST(Vector4 st)
		{
			this.SetStaticLightmapST_Injected(ref st);
		}

		// Token: 0x06000CC7 RID: 3271
		[FreeFunction(Name = "RendererScripting::GetMaterial", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Material GetMaterial();

		// Token: 0x06000CC8 RID: 3272
		[FreeFunction(Name = "RendererScripting::GetSharedMaterial", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Material GetSharedMaterial();

		// Token: 0x06000CC9 RID: 3273
		[FreeFunction(Name = "RendererScripting::SetMaterial", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMaterial(Material m);

		// Token: 0x06000CCA RID: 3274
		[FreeFunction(Name = "RendererScripting::GetMaterialArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Material[] GetMaterialArray();

		// Token: 0x06000CCB RID: 3275
		[FreeFunction(Name = "RendererScripting::GetMaterialArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CopyMaterialArray([Out] Material[] m);

		// Token: 0x06000CCC RID: 3276
		[FreeFunction(Name = "RendererScripting::GetSharedMaterialArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void CopySharedMaterialArray([Out] Material[] m);

		// Token: 0x06000CCD RID: 3277
		[FreeFunction(Name = "RendererScripting::SetMaterialArray", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetMaterialArray([NotNull("ArgumentNullException")] Material[] m, int length);

		// Token: 0x06000CCE RID: 3278 RVA: 0x000130DA File Offset: 0x000112DA
		private void SetMaterialArray(Material[] m)
		{
			this.SetMaterialArray(m, (m != null) ? m.Length : 0);
		}

		// Token: 0x06000CCF RID: 3279
		[FreeFunction(Name = "RendererScripting::SetPropertyBlock", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_SetPropertyBlock(MaterialPropertyBlock properties);

		// Token: 0x06000CD0 RID: 3280
		[FreeFunction(Name = "RendererScripting::GetPropertyBlock", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_GetPropertyBlock([NotNull("ArgumentNullException")] MaterialPropertyBlock dest);

		// Token: 0x06000CD1 RID: 3281
		[FreeFunction(Name = "RendererScripting::SetPropertyBlockMaterialIndex", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_SetPropertyBlockMaterialIndex(MaterialPropertyBlock properties, int materialIndex);

		// Token: 0x06000CD2 RID: 3282
		[FreeFunction(Name = "RendererScripting::GetPropertyBlockMaterialIndex", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void Internal_GetPropertyBlockMaterialIndex([NotNull("ArgumentNullException")] MaterialPropertyBlock dest, int materialIndex);

		// Token: 0x06000CD3 RID: 3283
		[FreeFunction(Name = "RendererScripting::HasPropertyBlock", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern bool HasPropertyBlock();

		// Token: 0x06000CD4 RID: 3284 RVA: 0x000130EE File Offset: 0x000112EE
		public void SetPropertyBlock(MaterialPropertyBlock properties)
		{
			this.Internal_SetPropertyBlock(properties);
		}

		// Token: 0x06000CD5 RID: 3285 RVA: 0x000130F9 File Offset: 0x000112F9
		public void SetPropertyBlock(MaterialPropertyBlock properties, int materialIndex)
		{
			this.Internal_SetPropertyBlockMaterialIndex(properties, materialIndex);
		}

		// Token: 0x06000CD6 RID: 3286 RVA: 0x00013105 File Offset: 0x00011305
		public void GetPropertyBlock(MaterialPropertyBlock properties)
		{
			this.Internal_GetPropertyBlock(properties);
		}

		// Token: 0x06000CD7 RID: 3287 RVA: 0x00013110 File Offset: 0x00011310
		public void GetPropertyBlock(MaterialPropertyBlock properties, int materialIndex)
		{
			this.Internal_GetPropertyBlockMaterialIndex(properties, materialIndex);
		}

		// Token: 0x06000CD8 RID: 3288
		[FreeFunction(Name = "RendererScripting::GetClosestReflectionProbes", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetClosestReflectionProbesInternal(object result);

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000CD9 RID: 3289
		// (set) Token: 0x06000CDA RID: 3290
		public extern bool enabled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000CDB RID: 3291
		public extern bool isVisible
		{
			[NativeName("IsVisibleInScene")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000CDC RID: 3292
		// (set) Token: 0x06000CDD RID: 3293
		public extern ShadowCastingMode shadowCastingMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000CDE RID: 3294
		// (set) Token: 0x06000CDF RID: 3295
		public extern bool receiveShadows
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000CE0 RID: 3296
		// (set) Token: 0x06000CE1 RID: 3297
		public extern bool forceRenderingOff
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000CE2 RID: 3298
		[NativeName("GetIsStaticShadowCaster")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern bool GetIsStaticShadowCaster();

		// Token: 0x06000CE3 RID: 3299
		[NativeName("SetIsStaticShadowCaster")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetIsStaticShadowCaster(bool value);

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000CE4 RID: 3300 RVA: 0x0001311C File Offset: 0x0001131C
		// (set) Token: 0x06000CE5 RID: 3301 RVA: 0x00013134 File Offset: 0x00011334
		public bool staticShadowCaster
		{
			get
			{
				return this.GetIsStaticShadowCaster();
			}
			set
			{
				this.SetIsStaticShadowCaster(value);
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000CE6 RID: 3302
		// (set) Token: 0x06000CE7 RID: 3303
		public extern MotionVectorGenerationMode motionVectorGenerationMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000CE8 RID: 3304
		// (set) Token: 0x06000CE9 RID: 3305
		public extern LightProbeUsage lightProbeUsage
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000CEA RID: 3306
		// (set) Token: 0x06000CEB RID: 3307
		public extern ReflectionProbeUsage reflectionProbeUsage
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000CEC RID: 3308
		// (set) Token: 0x06000CED RID: 3309
		public extern uint renderingLayerMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000CEE RID: 3310
		// (set) Token: 0x06000CEF RID: 3311
		public extern int rendererPriority
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000CF0 RID: 3312
		// (set) Token: 0x06000CF1 RID: 3313
		public extern RayTracingMode rayTracingMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000CF2 RID: 3314
		// (set) Token: 0x06000CF3 RID: 3315
		public extern string sortingLayerName
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000CF4 RID: 3316
		// (set) Token: 0x06000CF5 RID: 3317
		public extern int sortingLayerID
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000CF6 RID: 3318
		// (set) Token: 0x06000CF7 RID: 3319
		public extern int sortingOrder
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000CF8 RID: 3320
		internal extern uint sortingKey
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000CF9 RID: 3321
		// (set) Token: 0x06000CFA RID: 3322
		internal extern int sortingGroupID
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000CFB RID: 3323
		// (set) Token: 0x06000CFC RID: 3324
		internal extern int sortingGroupOrder
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x06000CFD RID: 3325
		internal extern uint sortingGroupKey
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000CFE RID: 3326
		// (set) Token: 0x06000CFF RID: 3327
		[NativeProperty("IsDynamicOccludee")]
		public extern bool allowOcclusionWhenDynamic
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000D00 RID: 3328
		// (set) Token: 0x06000D01 RID: 3329
		[NativeProperty("StaticBatchRoot")]
		internal extern Transform staticBatchRootTransform
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000D02 RID: 3330
		internal extern int staticBatchIndex
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x06000D03 RID: 3331
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal extern void SetStaticBatchInfo(int firstSubMesh, int subMeshCount);

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000D04 RID: 3332
		public extern bool isPartOfStaticBatch
		{
			[NativeName("IsPartOfStaticBatch")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000D05 RID: 3333 RVA: 0x00013140 File Offset: 0x00011340
		public Matrix4x4 worldToLocalMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_worldToLocalMatrix_Injected(out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000D06 RID: 3334 RVA: 0x00013158 File Offset: 0x00011358
		public Matrix4x4 localToWorldMatrix
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_localToWorldMatrix_Injected(out matrix4x);
				return matrix4x;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000D07 RID: 3335
		// (set) Token: 0x06000D08 RID: 3336
		public extern GameObject lightProbeProxyVolumeOverride
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000D09 RID: 3337
		// (set) Token: 0x06000D0A RID: 3338
		public extern Transform probeAnchor
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000D0B RID: 3339
		[NativeName("GetLightmapIndexInt")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetLightmapIndex(LightmapType lt);

		// Token: 0x06000D0C RID: 3340
		[NativeName("SetLightmapIndexInt")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLightmapIndex(int index, LightmapType lt);

		// Token: 0x06000D0D RID: 3341 RVA: 0x00013170 File Offset: 0x00011370
		[NativeName("GetLightmapST")]
		private Vector4 GetLightmapST(LightmapType lt)
		{
			Vector4 vector;
			this.GetLightmapST_Injected(lt, out vector);
			return vector;
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00013187 File Offset: 0x00011387
		[NativeName("SetLightmapST")]
		private void SetLightmapST(Vector4 st, LightmapType lt)
		{
			this.SetLightmapST_Injected(ref st, lt);
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000D0F RID: 3343 RVA: 0x00013194 File Offset: 0x00011394
		// (set) Token: 0x06000D10 RID: 3344 RVA: 0x000131AD File Offset: 0x000113AD
		public int lightmapIndex
		{
			get
			{
				return this.GetLightmapIndex(LightmapType.StaticLightmap);
			}
			set
			{
				this.SetLightmapIndex(value, LightmapType.StaticLightmap);
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000D11 RID: 3345 RVA: 0x000131BC File Offset: 0x000113BC
		// (set) Token: 0x06000D12 RID: 3346 RVA: 0x000131D5 File Offset: 0x000113D5
		public int realtimeLightmapIndex
		{
			get
			{
				return this.GetLightmapIndex(LightmapType.DynamicLightmap);
			}
			set
			{
				this.SetLightmapIndex(value, LightmapType.DynamicLightmap);
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x000131E4 File Offset: 0x000113E4
		// (set) Token: 0x06000D14 RID: 3348 RVA: 0x000131FD File Offset: 0x000113FD
		public Vector4 lightmapScaleOffset
		{
			get
			{
				return this.GetLightmapST(LightmapType.StaticLightmap);
			}
			set
			{
				this.SetStaticLightmapST(value);
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x00013208 File Offset: 0x00011408
		// (set) Token: 0x06000D16 RID: 3350 RVA: 0x00013221 File Offset: 0x00011421
		public Vector4 realtimeLightmapScaleOffset
		{
			get
			{
				return this.GetLightmapST(LightmapType.DynamicLightmap);
			}
			set
			{
				this.SetLightmapST(value, LightmapType.DynamicLightmap);
			}
		}

		// Token: 0x06000D17 RID: 3351
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern int GetMaterialCount();

		// Token: 0x06000D18 RID: 3352
		[NativeName("GetMaterialArray")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern Material[] GetSharedMaterialArray();

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x00013230 File Offset: 0x00011430
		// (set) Token: 0x06000D1A RID: 3354 RVA: 0x00013248 File Offset: 0x00011448
		public Material[] materials
		{
			get
			{
				return this.GetMaterialArray();
			}
			set
			{
				this.SetMaterialArray(value);
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x00013254 File Offset: 0x00011454
		// (set) Token: 0x06000D1C RID: 3356 RVA: 0x0001326C File Offset: 0x0001146C
		public Material material
		{
			get
			{
				return this.GetMaterial();
			}
			set
			{
				this.SetMaterial(value);
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x00013278 File Offset: 0x00011478
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x0001326C File Offset: 0x0001146C
		public Material sharedMaterial
		{
			get
			{
				return this.GetSharedMaterial();
			}
			set
			{
				this.SetMaterial(value);
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000D1F RID: 3359 RVA: 0x00013290 File Offset: 0x00011490
		// (set) Token: 0x06000D20 RID: 3360 RVA: 0x00013248 File Offset: 0x00011448
		public Material[] sharedMaterials
		{
			get
			{
				return this.GetSharedMaterialArray();
			}
			set
			{
				this.SetMaterialArray(value);
			}
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x000132A8 File Offset: 0x000114A8
		public void GetMaterials(List<Material> m)
		{
			bool flag = m == null;
			if (flag)
			{
				throw new ArgumentNullException("The result material list cannot be null.", "m");
			}
			NoAllocHelpers.EnsureListElemCount<Material>(m, this.GetMaterialCount());
			this.CopyMaterialArray(NoAllocHelpers.ExtractArrayFromListT<Material>(m));
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x000132E8 File Offset: 0x000114E8
		public void SetSharedMaterials(List<Material> materials)
		{
			bool flag = materials == null;
			if (flag)
			{
				throw new ArgumentNullException("The material list to set cannot be null.", "materials");
			}
			this.SetMaterialArray(NoAllocHelpers.ExtractArrayFromListT<Material>(materials), materials.Count);
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00013324 File Offset: 0x00011524
		public void SetMaterials(List<Material> materials)
		{
			bool flag = materials == null;
			if (flag)
			{
				throw new ArgumentNullException("The material list to set cannot be null.", "materials");
			}
			this.SetMaterialArray(NoAllocHelpers.ExtractArrayFromListT<Material>(materials), materials.Count);
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x00013360 File Offset: 0x00011560
		public void GetSharedMaterials(List<Material> m)
		{
			bool flag = m == null;
			if (flag)
			{
				throw new ArgumentNullException("The result material list cannot be null.", "m");
			}
			NoAllocHelpers.EnsureListElemCount<Material>(m, this.GetMaterialCount());
			this.CopySharedMaterialArray(NoAllocHelpers.ExtractArrayFromListT<Material>(m));
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x000133A0 File Offset: 0x000115A0
		public void GetClosestReflectionProbes(List<ReflectionProbeBlendInfo> result)
		{
			this.GetClosestReflectionProbesInternal(result);
		}

		// Token: 0x06000D27 RID: 3367
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bounds_Injected(out Bounds ret);

		// Token: 0x06000D28 RID: 3368
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_bounds_Injected(ref Bounds value);

		// Token: 0x06000D29 RID: 3369
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_localBounds_Injected(out Bounds ret);

		// Token: 0x06000D2A RID: 3370
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_localBounds_Injected(ref Bounds value);

		// Token: 0x06000D2B RID: 3371
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetStaticLightmapST_Injected(ref Vector4 st);

		// Token: 0x06000D2C RID: 3372
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_worldToLocalMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06000D2D RID: 3373
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_localToWorldMatrix_Injected(out Matrix4x4 ret);

		// Token: 0x06000D2E RID: 3374
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void GetLightmapST_Injected(LightmapType lt, out Vector4 ret);

		// Token: 0x06000D2F RID: 3375
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void SetLightmapST_Injected(ref Vector4 st, LightmapType lt);
	}
}
