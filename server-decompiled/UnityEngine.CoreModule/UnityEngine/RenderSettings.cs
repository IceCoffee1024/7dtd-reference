using System;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000167 RID: 359
	[StaticAccessor("GetRenderSettings()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Camera/RenderSettings.h")]
	[NativeHeader("Runtime/Graphics/QualitySettingsTypes.h")]
	public sealed class RenderSettings : Object
	{
		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000D30 RID: 3376 RVA: 0x000133B4 File Offset: 0x000115B4
		// (set) Token: 0x06000D31 RID: 3377 RVA: 0x000133CB File Offset: 0x000115CB
		[Obsolete("Use RenderSettings.ambientIntensity instead (UnityUpgradable) -> ambientIntensity", false)]
		public static float ambientSkyboxAmount
		{
			get
			{
				return RenderSettings.ambientIntensity;
			}
			set
			{
				RenderSettings.ambientIntensity = value;
			}
		}

		// Token: 0x06000D32 RID: 3378 RVA: 0x000111F6 File Offset: 0x0000F3F6
		private RenderSettings()
		{
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000D33 RID: 3379
		// (set) Token: 0x06000D34 RID: 3380
		[NativeProperty("UseFog")]
		public static extern bool fog
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000D35 RID: 3381
		// (set) Token: 0x06000D36 RID: 3382
		[NativeProperty("LinearFogStart")]
		public static extern float fogStartDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000D37 RID: 3383
		// (set) Token: 0x06000D38 RID: 3384
		[NativeProperty("LinearFogEnd")]
		public static extern float fogEndDistance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000D39 RID: 3385
		// (set) Token: 0x06000D3A RID: 3386
		public static extern FogMode fogMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000D3B RID: 3387 RVA: 0x000133D8 File Offset: 0x000115D8
		// (set) Token: 0x06000D3C RID: 3388 RVA: 0x000133ED File Offset: 0x000115ED
		public static Color fogColor
		{
			get
			{
				Color color;
				RenderSettings.get_fogColor_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_fogColor_Injected(ref value);
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000D3D RID: 3389
		// (set) Token: 0x06000D3E RID: 3390
		public static extern float fogDensity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000D3F RID: 3391
		// (set) Token: 0x06000D40 RID: 3392
		public static extern AmbientMode ambientMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000D41 RID: 3393 RVA: 0x000133F8 File Offset: 0x000115F8
		// (set) Token: 0x06000D42 RID: 3394 RVA: 0x0001340D File Offset: 0x0001160D
		public static Color ambientSkyColor
		{
			get
			{
				Color color;
				RenderSettings.get_ambientSkyColor_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_ambientSkyColor_Injected(ref value);
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000D43 RID: 3395 RVA: 0x00013418 File Offset: 0x00011618
		// (set) Token: 0x06000D44 RID: 3396 RVA: 0x0001342D File Offset: 0x0001162D
		public static Color ambientEquatorColor
		{
			get
			{
				Color color;
				RenderSettings.get_ambientEquatorColor_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_ambientEquatorColor_Injected(ref value);
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000D45 RID: 3397 RVA: 0x00013438 File Offset: 0x00011638
		// (set) Token: 0x06000D46 RID: 3398 RVA: 0x0001344D File Offset: 0x0001164D
		public static Color ambientGroundColor
		{
			get
			{
				Color color;
				RenderSettings.get_ambientGroundColor_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_ambientGroundColor_Injected(ref value);
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000D47 RID: 3399
		// (set) Token: 0x06000D48 RID: 3400
		public static extern float ambientIntensity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000D49 RID: 3401 RVA: 0x00013458 File Offset: 0x00011658
		// (set) Token: 0x06000D4A RID: 3402 RVA: 0x0001346D File Offset: 0x0001166D
		[NativeProperty("AmbientSkyColor")]
		public static Color ambientLight
		{
			get
			{
				Color color;
				RenderSettings.get_ambientLight_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_ambientLight_Injected(ref value);
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000D4B RID: 3403 RVA: 0x00013478 File Offset: 0x00011678
		// (set) Token: 0x06000D4C RID: 3404 RVA: 0x0001348D File Offset: 0x0001168D
		public static Color subtractiveShadowColor
		{
			get
			{
				Color color;
				RenderSettings.get_subtractiveShadowColor_Injected(out color);
				return color;
			}
			set
			{
				RenderSettings.set_subtractiveShadowColor_Injected(ref value);
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000D4D RID: 3405
		// (set) Token: 0x06000D4E RID: 3406
		[NativeProperty("SkyboxMaterial")]
		public static extern Material skybox
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000D4F RID: 3407
		// (set) Token: 0x06000D50 RID: 3408
		public static extern Light sun
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000D51 RID: 3409 RVA: 0x00013498 File Offset: 0x00011698
		// (set) Token: 0x06000D52 RID: 3410 RVA: 0x000134AD File Offset: 0x000116AD
		public static SphericalHarmonicsL2 ambientProbe
		{
			[NativeMethod("GetFinalAmbientProbe")]
			get
			{
				SphericalHarmonicsL2 sphericalHarmonicsL;
				RenderSettings.get_ambientProbe_Injected(out sphericalHarmonicsL);
				return sphericalHarmonicsL;
			}
			set
			{
				RenderSettings.set_ambientProbe_Injected(ref value);
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000D53 RID: 3411 RVA: 0x000134B8 File Offset: 0x000116B8
		// (set) Token: 0x06000D54 RID: 3412 RVA: 0x000134ED File Offset: 0x000116ED
		[Obsolete("RenderSettings.customReflection has been deprecated in favor of RenderSettings.customReflectionTexture.", false)]
		public static Cubemap customReflection
		{
			get
			{
				Cubemap cubemap = RenderSettings.customReflectionTexture as Cubemap;
				bool flag = cubemap == null;
				if (flag)
				{
					throw new ArgumentException("RenderSettings.customReflection is currently not referencing a cubemap.");
				}
				return cubemap;
			}
			[NativeThrows]
			set
			{
				RenderSettings.customReflectionTexture = value;
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000D55 RID: 3413
		// (set) Token: 0x06000D56 RID: 3414
		[NativeProperty("CustomReflection")]
		public static extern Texture customReflectionTexture
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[NativeThrows]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000D57 RID: 3415
		// (set) Token: 0x06000D58 RID: 3416
		public static extern float reflectionIntensity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000D59 RID: 3417
		// (set) Token: 0x06000D5A RID: 3418
		public static extern int reflectionBounces
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000D5B RID: 3419
		[NativeProperty("GeneratedSkyboxReflection")]
		internal static extern Cubemap defaultReflection
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000D5C RID: 3420
		// (set) Token: 0x06000D5D RID: 3421
		public static extern DefaultReflectionMode defaultReflectionMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000D5E RID: 3422
		// (set) Token: 0x06000D5F RID: 3423
		public static extern int defaultReflectionResolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000D60 RID: 3424
		// (set) Token: 0x06000D61 RID: 3425
		public static extern float haloStrength
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000D62 RID: 3426
		// (set) Token: 0x06000D63 RID: 3427
		public static extern float flareStrength
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000D64 RID: 3428
		// (set) Token: 0x06000D65 RID: 3429
		public static extern float flareFadeSpeed
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000D66 RID: 3430
		[FreeFunction("GetRenderSettings")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern Object GetRenderSettings();

		// Token: 0x06000D67 RID: 3431
		[StaticAccessor("RenderSettingsScripting", StaticAccessorType.DoubleColon)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		internal static extern void Reset();

		// Token: 0x06000D68 RID: 3432
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_fogColor_Injected(out Color ret);

		// Token: 0x06000D69 RID: 3433
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_fogColor_Injected(ref Color value);

		// Token: 0x06000D6A RID: 3434
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_ambientSkyColor_Injected(out Color ret);

		// Token: 0x06000D6B RID: 3435
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_ambientSkyColor_Injected(ref Color value);

		// Token: 0x06000D6C RID: 3436
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_ambientEquatorColor_Injected(out Color ret);

		// Token: 0x06000D6D RID: 3437
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_ambientEquatorColor_Injected(ref Color value);

		// Token: 0x06000D6E RID: 3438
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_ambientGroundColor_Injected(out Color ret);

		// Token: 0x06000D6F RID: 3439
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_ambientGroundColor_Injected(ref Color value);

		// Token: 0x06000D70 RID: 3440
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_ambientLight_Injected(out Color ret);

		// Token: 0x06000D71 RID: 3441
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_ambientLight_Injected(ref Color value);

		// Token: 0x06000D72 RID: 3442
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_subtractiveShadowColor_Injected(out Color ret);

		// Token: 0x06000D73 RID: 3443
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_subtractiveShadowColor_Injected(ref Color value);

		// Token: 0x06000D74 RID: 3444
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_ambientProbe_Injected(out SphericalHarmonicsL2 ret);

		// Token: 0x06000D75 RID: 3445
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_ambientProbe_Injected(ref SphericalHarmonicsL2 value);
	}
}
