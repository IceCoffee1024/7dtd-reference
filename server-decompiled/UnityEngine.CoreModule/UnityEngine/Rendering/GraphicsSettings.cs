using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Bindings;

namespace UnityEngine.Rendering
{
	// Token: 0x0200042E RID: 1070
	[StaticAccessor("GetGraphicsSettings()", StaticAccessorType.Dot)]
	[NativeHeader("Runtime/Camera/GraphicsSettings.h")]
	public sealed class GraphicsSettings : Object
	{
		// Token: 0x060021FB RID: 8699 RVA: 0x000111F6 File Offset: 0x0000F3F6
		private GraphicsSettings()
		{
		}

		// Token: 0x1700066C RID: 1644
		// (get) Token: 0x060021FC RID: 8700
		// (set) Token: 0x060021FD RID: 8701
		public static extern TransparencySortMode transparencySortMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060021FE RID: 8702 RVA: 0x00038BD4 File Offset: 0x00036DD4
		// (set) Token: 0x060021FF RID: 8703 RVA: 0x00038BE9 File Offset: 0x00036DE9
		public static Vector3 transparencySortAxis
		{
			get
			{
				Vector3 vector;
				GraphicsSettings.get_transparencySortAxis_Injected(out vector);
				return vector;
			}
			set
			{
				GraphicsSettings.set_transparencySortAxis_Injected(ref value);
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x06002200 RID: 8704
		// (set) Token: 0x06002201 RID: 8705
		public static extern bool realtimeDirectRectangularAreaLights
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x06002202 RID: 8706
		// (set) Token: 0x06002203 RID: 8707
		public static extern bool lightsUseLinearIntensity
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000670 RID: 1648
		// (get) Token: 0x06002204 RID: 8708
		// (set) Token: 0x06002205 RID: 8709
		public static extern bool lightsUseColorTemperature
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000671 RID: 1649
		// (get) Token: 0x06002206 RID: 8710
		// (set) Token: 0x06002207 RID: 8711
		public static extern uint defaultRenderingLayerMask
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000672 RID: 1650
		// (get) Token: 0x06002208 RID: 8712
		// (set) Token: 0x06002209 RID: 8713
		public static extern bool useScriptableRenderPipelineBatching
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000673 RID: 1651
		// (get) Token: 0x0600220A RID: 8714
		// (set) Token: 0x0600220B RID: 8715
		public static extern bool logWhenShaderIsCompiled
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000674 RID: 1652
		// (get) Token: 0x0600220C RID: 8716
		// (set) Token: 0x0600220D RID: 8717
		public static extern bool disableBuiltinCustomRenderTextureUpdate
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x17000675 RID: 1653
		// (get) Token: 0x0600220E RID: 8718
		public static extern VideoShadersIncludeMode videoShadersIncludeMode
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000676 RID: 1654
		// (get) Token: 0x0600220F RID: 8719
		// (set) Token: 0x06002210 RID: 8720
		public static extern LightProbeOutsideHullStrategy lightProbeOutsideHullStrategy
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x06002211 RID: 8721
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern bool HasShaderDefine(GraphicsTier tier, BuiltinShaderDefine defineHash);

		// Token: 0x06002212 RID: 8722 RVA: 0x00038BF4 File Offset: 0x00036DF4
		public static bool HasShaderDefine(BuiltinShaderDefine defineHash)
		{
			return GraphicsSettings.HasShaderDefine(Graphics.activeTier, defineHash);
		}

		// Token: 0x17000677 RID: 1655
		// (get) Token: 0x06002213 RID: 8723
		[NativeName("CurrentRenderPipeline")]
		private static extern ScriptableObject INTERNAL_currentRenderPipeline
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}

		// Token: 0x17000678 RID: 1656
		// (get) Token: 0x06002214 RID: 8724 RVA: 0x00038C14 File Offset: 0x00036E14
		public static RenderPipelineAsset currentRenderPipeline
		{
			get
			{
				return GraphicsSettings.INTERNAL_currentRenderPipeline as RenderPipelineAsset;
			}
		}

		// Token: 0x17000679 RID: 1657
		// (get) Token: 0x06002215 RID: 8725 RVA: 0x00038C30 File Offset: 0x00036E30
		// (set) Token: 0x06002216 RID: 8726 RVA: 0x00038C47 File Offset: 0x00036E47
		public static RenderPipelineAsset renderPipelineAsset
		{
			get
			{
				return GraphicsSettings.defaultRenderPipeline;
			}
			set
			{
				GraphicsSettings.defaultRenderPipeline = value;
			}
		}

		// Token: 0x1700067A RID: 1658
		// (get) Token: 0x06002217 RID: 8727
		// (set) Token: 0x06002218 RID: 8728
		[NativeName("DefaultRenderPipeline")]
		private static extern ScriptableObject INTERNAL_defaultRenderPipeline
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700067B RID: 1659
		// (get) Token: 0x06002219 RID: 8729 RVA: 0x00038C54 File Offset: 0x00036E54
		// (set) Token: 0x0600221A RID: 8730 RVA: 0x00038C70 File Offset: 0x00036E70
		public static RenderPipelineAsset defaultRenderPipeline
		{
			get
			{
				return GraphicsSettings.INTERNAL_defaultRenderPipeline as RenderPipelineAsset;
			}
			set
			{
				GraphicsSettings.INTERNAL_defaultRenderPipeline = value;
			}
		}

		// Token: 0x0600221B RID: 8731
		[NativeName("GetAllConfiguredRenderPipelinesForScript")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern ScriptableObject[] GetAllConfiguredRenderPipelines();

		// Token: 0x1700067C RID: 1660
		// (get) Token: 0x0600221C RID: 8732 RVA: 0x00038C7C File Offset: 0x00036E7C
		public static RenderPipelineAsset[] allConfiguredRenderPipelines
		{
			get
			{
				return GraphicsSettings.GetAllConfiguredRenderPipelines().Cast<RenderPipelineAsset>().ToArray<RenderPipelineAsset>();
			}
		}

		// Token: 0x0600221D RID: 8733
		[FreeFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Object GetGraphicsSettings();

		// Token: 0x0600221E RID: 8734
		[NativeName("SetShaderModeScript")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetShaderMode(BuiltinShaderType type, BuiltinShaderMode mode);

		// Token: 0x0600221F RID: 8735
		[NativeName("GetShaderModeScript")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern BuiltinShaderMode GetShaderMode(BuiltinShaderType type);

		// Token: 0x06002220 RID: 8736
		[NativeName("SetCustomShaderScript")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern void SetCustomShader(BuiltinShaderType type, Shader shader);

		// Token: 0x06002221 RID: 8737
		[NativeName("GetCustomShaderScript")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public static extern Shader GetCustomShader(BuiltinShaderType type);

		// Token: 0x06002222 RID: 8738 RVA: 0x00038C9D File Offset: 0x00036E9D
		public static void RegisterRenderPipelineSettings<T>(RenderPipelineGlobalSettings settings) where T : RenderPipeline
		{
			GraphicsSettings.RegisterRenderPipeline(typeof(T).FullName, settings);
		}

		// Token: 0x06002223 RID: 8739
		[NativeName("RegisterRenderPipelineSettings")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void RegisterRenderPipeline(string renderpipelineName, Object settings);

		// Token: 0x06002224 RID: 8740 RVA: 0x00038CB6 File Offset: 0x00036EB6
		public static void UnregisterRenderPipelineSettings<T>() where T : RenderPipeline
		{
			GraphicsSettings.UnregisterRenderPipeline(typeof(T).FullName);
		}

		// Token: 0x06002225 RID: 8741
		[NativeName("UnregisterRenderPipelineSettings")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void UnregisterRenderPipeline(string renderpipelineName);

		// Token: 0x06002226 RID: 8742 RVA: 0x00038CD0 File Offset: 0x00036ED0
		public static RenderPipelineGlobalSettings GetSettingsForRenderPipeline<T>() where T : RenderPipeline
		{
			return GraphicsSettings.GetSettingsForRenderPipeline(typeof(T).FullName) as RenderPipelineGlobalSettings;
		}

		// Token: 0x06002227 RID: 8743
		[NativeName("GetSettingsForRenderPipeline")]
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern Object GetSettingsForRenderPipeline(string renderpipelineName);

		// Token: 0x1700067D RID: 1661
		// (get) Token: 0x06002228 RID: 8744
		// (set) Token: 0x06002229 RID: 8745
		public static extern bool cameraRelativeLightCulling
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x1700067E RID: 1662
		// (get) Token: 0x0600222A RID: 8746
		// (set) Token: 0x0600222B RID: 8747
		public static extern bool cameraRelativeShadowCulling
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		// Token: 0x0600222C RID: 8748
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void get_transparencySortAxis_Injected(out Vector3 ret);

		// Token: 0x0600222D RID: 8749
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void set_transparencySortAxis_Injected(ref Vector3 value);
	}
}
