using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000177 RID: 375
	[NativeHeader("Runtime/Export/Graphics/Light.bindings.h")]
	[NativeHeader("Runtime/Camera/Light.h")]
	[RequireComponent(typeof(Transform))]
	[RequireComponent(typeof(Transform))]
	public sealed class Light : Behaviour
	{
		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000F84 RID: 3972
		// (set) Token: 0x06000F85 RID: 3973
		[NativeProperty("LightType")]
		public extern LightType type
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000F86 RID: 3974
		// (set) Token: 0x06000F87 RID: 3975
		[NativeProperty("LightShape")]
		public extern LightShape shape
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000F88 RID: 3976
		// (set) Token: 0x06000F89 RID: 3977
		public extern float spotAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000F8A RID: 3978
		// (set) Token: 0x06000F8B RID: 3979
		public extern float innerSpotAngle
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000F8C RID: 3980 RVA: 0x00015BAC File Offset: 0x00013DAC
		// (set) Token: 0x06000F8D RID: 3981 RVA: 0x00015BC2 File Offset: 0x00013DC2
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

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000F8E RID: 3982
		// (set) Token: 0x06000F8F RID: 3983
		public extern float colorTemperature
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000F90 RID: 3984
		// (set) Token: 0x06000F91 RID: 3985
		public extern bool useColorTemperature
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000F92 RID: 3986
		// (set) Token: 0x06000F93 RID: 3987
		public extern float intensity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000F94 RID: 3988
		// (set) Token: 0x06000F95 RID: 3989
		public extern float bounceIntensity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000F96 RID: 3990
		// (set) Token: 0x06000F97 RID: 3991
		public extern bool useBoundingSphereOverride
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000F98 RID: 3992 RVA: 0x00015BCC File Offset: 0x00013DCC
		// (set) Token: 0x06000F99 RID: 3993 RVA: 0x00015BE2 File Offset: 0x00013DE2
		public Vector4 boundingSphereOverride
		{
			get
			{
				Vector4 vector;
				this.get_boundingSphereOverride_Injected(out vector);
				return vector;
			}
			set
			{
				this.set_boundingSphereOverride_Injected(ref value);
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000F9A RID: 3994
		// (set) Token: 0x06000F9B RID: 3995
		public extern bool useViewFrustumForShadowCasterCull
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000F9C RID: 3996
		// (set) Token: 0x06000F9D RID: 3997
		public extern int shadowCustomResolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000F9E RID: 3998
		// (set) Token: 0x06000F9F RID: 3999
		public extern float shadowBias
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000FA0 RID: 4000
		// (set) Token: 0x06000FA1 RID: 4001
		public extern float shadowNormalBias
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000FA2 RID: 4002
		// (set) Token: 0x06000FA3 RID: 4003
		public extern float shadowNearPlane
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000FA4 RID: 4004
		// (set) Token: 0x06000FA5 RID: 4005
		public extern bool useShadowMatrixOverride
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x00015BEC File Offset: 0x00013DEC
		// (set) Token: 0x06000FA7 RID: 4007 RVA: 0x00015C02 File Offset: 0x00013E02
		public Matrix4x4 shadowMatrixOverride
		{
			get
			{
				Matrix4x4 matrix4x;
				this.get_shadowMatrixOverride_Injected(out matrix4x);
				return matrix4x;
			}
			set
			{
				this.set_shadowMatrixOverride_Injected(ref value);
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000FA8 RID: 4008
		// (set) Token: 0x06000FA9 RID: 4009
		public extern float range
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000FAA RID: 4010
		// (set) Token: 0x06000FAB RID: 4011
		public extern Flare flare
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000FAC RID: 4012 RVA: 0x00015C0C File Offset: 0x00013E0C
		// (set) Token: 0x06000FAD RID: 4013 RVA: 0x00015C22 File Offset: 0x00013E22
		public LightBakingOutput bakingOutput
		{
			get
			{
				LightBakingOutput lightBakingOutput;
				this.get_bakingOutput_Injected(out lightBakingOutput);
				return lightBakingOutput;
			}
			set
			{
				this.set_bakingOutput_Injected(ref value);
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000FAE RID: 4014
		// (set) Token: 0x06000FAF RID: 4015
		public extern int cullingMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000FB0 RID: 4016
		// (set) Token: 0x06000FB1 RID: 4017
		public extern int renderingLayerMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000FB2 RID: 4018
		// (set) Token: 0x06000FB3 RID: 4019
		public extern LightShadowCasterMode lightShadowCasterMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06000FB4 RID: 4020
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void Reset();

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000FB5 RID: 4021
		// (set) Token: 0x06000FB6 RID: 4022
		public extern LightShadows shadows
		{
			[NativeMethod("GetShadowType")]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("Light_Bindings::SetShadowType", HasExplicitThis = true, ThrowsException = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000FB7 RID: 4023
		// (set) Token: 0x06000FB8 RID: 4024
		public extern float shadowStrength
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("Light_Bindings::SetShadowStrength", HasExplicitThis = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000FB9 RID: 4025
		// (set) Token: 0x06000FBA RID: 4026
		public extern LightShadowResolution shadowResolution
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("Light_Bindings::SetShadowResolution", HasExplicitThis = true, ThrowsException = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000FBB RID: 4027 RVA: 0x00015C2C File Offset: 0x00013E2C
		// (set) Token: 0x06000FBC RID: 4028 RVA: 0x00002669 File Offset: 0x00000869
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Obsolete("Shadow softness is removed in Unity 5.0+", true)]
		public float shadowSoftness
		{
			get
			{
				return 4f;
			}
			set
			{
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000FBD RID: 4029 RVA: 0x00015C44 File Offset: 0x00013E44
		// (set) Token: 0x06000FBE RID: 4030 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("Shadow softness is removed in Unity 5.0+", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public float shadowSoftnessFade
		{
			get
			{
				return 1f;
			}
			set
			{
			}
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000FBF RID: 4031
		// (set) Token: 0x06000FC0 RID: 4032
		public extern float[] layerShadowCullDistances
		{
			[FreeFunction("Light_Bindings::GetLayerShadowCullDistances", HasExplicitThis = true, ThrowsException = false)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("Light_Bindings::SetLayerShadowCullDistances", HasExplicitThis = true, ThrowsException = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000FC1 RID: 4033
		// (set) Token: 0x06000FC2 RID: 4034
		public extern float cookieSize
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000FC3 RID: 4035
		// (set) Token: 0x06000FC4 RID: 4036
		public extern Texture cookie
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000FC5 RID: 4037
		// (set) Token: 0x06000FC6 RID: 4038
		public extern LightRenderMode renderMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[FreeFunction("Light_Bindings::SetRenderMode", HasExplicitThis = true, ThrowsException = true)]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000FC7 RID: 4039 RVA: 0x00015C5C File Offset: 0x00013E5C
		// (set) Token: 0x06000FC8 RID: 4040 RVA: 0x00015C74 File Offset: 0x00013E74
		[Obsolete("warning bakedIndex has been removed please use bakingOutput.isBaked instead.", true)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public int bakedIndex
		{
			get
			{
				return this.m_BakedIndex;
			}
			set
			{
				this.m_BakedIndex = value;
			}
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00015C7E File Offset: 0x00013E7E
		public void AddCommandBuffer(LightEvent evt, CommandBuffer buffer)
		{
			this.AddCommandBuffer(evt, buffer, ShadowMapPass.All);
		}

		// Token: 0x06000FCA RID: 4042
		[FreeFunction("Light_Bindings::AddCommandBuffer", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddCommandBuffer(LightEvent evt, CommandBuffer buffer, ShadowMapPass shadowPassMask);

		// Token: 0x06000FCB RID: 4043 RVA: 0x00015C8F File Offset: 0x00013E8F
		public void AddCommandBufferAsync(LightEvent evt, CommandBuffer buffer, ComputeQueueType queueType)
		{
			this.AddCommandBufferAsync(evt, buffer, ShadowMapPass.All, queueType);
		}

		// Token: 0x06000FCC RID: 4044
		[FreeFunction("Light_Bindings::AddCommandBufferAsync", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void AddCommandBufferAsync(LightEvent evt, CommandBuffer buffer, ShadowMapPass shadowPassMask, ComputeQueueType queueType);

		// Token: 0x06000FCD RID: 4045
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveCommandBuffer(LightEvent evt, CommandBuffer buffer);

		// Token: 0x06000FCE RID: 4046
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveCommandBuffers(LightEvent evt);

		// Token: 0x06000FCF RID: 4047
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern void RemoveAllCommandBuffers();

		// Token: 0x06000FD0 RID: 4048
		[FreeFunction("Light_Bindings::GetCommandBuffers", HasExplicitThis = true)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern CommandBuffer[] GetCommandBuffers(LightEvent evt);

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000FD1 RID: 4049
		public extern int commandBufferCount
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x00015CA4 File Offset: 0x00013EA4
		// (set) Token: 0x06000FD3 RID: 4051 RVA: 0x00015CBB File Offset: 0x00013EBB
		[Obsolete("Use QualitySettings.pixelLightCount instead.")]
		public static int pixelLightCount
		{
			get
			{
				return QualitySettings.pixelLightCount;
			}
			set
			{
				QualitySettings.pixelLightCount = value;
			}
		}

		// Token: 0x06000FD4 RID: 4052
		[FreeFunction("Light_Bindings::GetLights")]
		[Obsolete("Light.GetLights has been deprecated, use FindObjectsOfType in combination with light.cullingmask/light.type", false)]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Light[] GetLights(LightType type, int layer);

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x00015CC8 File Offset: 0x00013EC8
		// (set) Token: 0x06000FD6 RID: 4054 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("light.shadowConstantBias was removed, use light.shadowBias", true)]
		public float shadowConstantBias
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00015CE0 File Offset: 0x00013EE0
		// (set) Token: 0x06000FD8 RID: 4056 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("light.shadowObjectSizeBias was removed, use light.shadowBias", true)]
		public float shadowObjectSizeBias
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x00015CF8 File Offset: 0x00013EF8
		// (set) Token: 0x06000FDA RID: 4058 RVA: 0x00002669 File Offset: 0x00000869
		[Obsolete("light.attenuate was removed; all lights always attenuate now", true)]
		public bool attenuate
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x06000FDC RID: 4060
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_color_Injected(out Color ret);

		// Token: 0x06000FDD RID: 4061
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_color_Injected(ref Color value);

		// Token: 0x06000FDE RID: 4062
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_boundingSphereOverride_Injected(out Vector4 ret);

		// Token: 0x06000FDF RID: 4063
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_boundingSphereOverride_Injected(ref Vector4 value);

		// Token: 0x06000FE0 RID: 4064
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_shadowMatrixOverride_Injected(out Matrix4x4 ret);

		// Token: 0x06000FE1 RID: 4065
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_shadowMatrixOverride_Injected(ref Matrix4x4 value);

		// Token: 0x06000FE2 RID: 4066
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void get_bakingOutput_Injected(out LightBakingOutput ret);

		// Token: 0x06000FE3 RID: 4067
		[MethodImpl(MethodImplOptions.InternalCall)]
		private extern void set_bakingOutput_Injected(ref LightBakingOutput value);

		// Token: 0x0400049F RID: 1183
		private int m_BakedIndex;
	}
}
