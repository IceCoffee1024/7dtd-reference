using System;
using Unity.Collections;

namespace UnityEngine.Rendering.RendererUtils
{
	// Token: 0x0200048D RID: 1165
	public struct RendererListDesc
	{
		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x06002816 RID: 10262 RVA: 0x00044AED File Offset: 0x00042CED
		// (set) Token: 0x06002817 RID: 10263 RVA: 0x00044AF5 File Offset: 0x00042CF5
		internal CullingResults cullingResult { readonly get; private set; }

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002818 RID: 10264 RVA: 0x00044AFE File Offset: 0x00042CFE
		// (set) Token: 0x06002819 RID: 10265 RVA: 0x00044B06 File Offset: 0x00042D06
		internal Camera camera { readonly get; set; }

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x0600281A RID: 10266 RVA: 0x00044B0F File Offset: 0x00042D0F
		// (set) Token: 0x0600281B RID: 10267 RVA: 0x00044B17 File Offset: 0x00042D17
		internal ShaderTagId passName { readonly get; private set; }

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x0600281C RID: 10268 RVA: 0x00044B20 File Offset: 0x00042D20
		// (set) Token: 0x0600281D RID: 10269 RVA: 0x00044B28 File Offset: 0x00042D28
		internal ShaderTagId[] passNames { readonly get; private set; }

		// Token: 0x0600281E RID: 10270 RVA: 0x00044B34 File Offset: 0x00042D34
		public RendererListDesc(ShaderTagId passName, CullingResults cullingResult, Camera camera)
		{
			this = default(RendererListDesc);
			this.passName = passName;
			this.passNames = null;
			this.cullingResult = cullingResult;
			this.camera = camera;
			this.layerMask = -1;
			this.renderingLayerMask = uint.MaxValue;
			this.overrideMaterialPassIndex = 0;
			this.overrideShaderPassIndex = 0;
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x00044B88 File Offset: 0x00042D88
		public RendererListDesc(ShaderTagId[] passNames, CullingResults cullingResult, Camera camera)
		{
			this = default(RendererListDesc);
			this.passNames = passNames;
			this.passName = ShaderTagId.none;
			this.cullingResult = cullingResult;
			this.camera = camera;
			this.layerMask = -1;
			this.renderingLayerMask = uint.MaxValue;
			this.overrideMaterialPassIndex = 0;
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x00044BD8 File Offset: 0x00042DD8
		public bool IsValid()
		{
			bool flag = this.camera == null || (this.passName == ShaderTagId.none && (this.passNames == null || this.passNames.Length == 0));
			return !flag;
		}

		// Token: 0x06002821 RID: 10273 RVA: 0x00044C30 File Offset: 0x00042E30
		public static RendererListParams ConvertToParameters(in RendererListDesc desc)
		{
			RendererListDesc rendererListDesc = desc;
			bool flag = !rendererListDesc.IsValid();
			RendererListParams rendererListParams;
			if (flag)
			{
				rendererListParams = RendererListParams.Invalid;
			}
			else
			{
				RendererListParams rendererListParams2 = default(RendererListParams);
				SortingSettings sortingSettings = new SortingSettings(desc.camera)
				{
					criteria = desc.sortingCriteria
				};
				DrawingSettings drawingSettings = new DrawingSettings(RendererListDesc.s_EmptyName, sortingSettings)
				{
					perObjectData = desc.rendererConfiguration
				};
				bool flag2 = desc.passName != ShaderTagId.none;
				if (flag2)
				{
					Debug.Assert(desc.passNames == null);
					drawingSettings.SetShaderPassName(0, desc.passName);
				}
				else
				{
					for (int i = 0; i < desc.passNames.Length; i++)
					{
						drawingSettings.SetShaderPassName(i, desc.passNames[i]);
					}
				}
				bool flag3 = desc.overrideShader != null;
				if (flag3)
				{
					drawingSettings.overrideShader = desc.overrideShader;
					drawingSettings.overrideShaderPassIndex = desc.overrideShaderPassIndex;
				}
				bool flag4 = desc.overrideMaterial != null;
				if (flag4)
				{
					drawingSettings.overrideMaterial = desc.overrideMaterial;
					drawingSettings.overrideMaterialPassIndex = desc.overrideMaterialPassIndex;
				}
				FilteringSettings filteringSettings = new FilteringSettings(new RenderQueueRange?(desc.renderQueueRange), desc.layerMask, desc.renderingLayerMask, 0)
				{
					excludeMotionVectorObjects = desc.excludeObjectMotionVectors
				};
				rendererListParams2.cullingResults = desc.cullingResult;
				rendererListParams2.drawSettings = drawingSettings;
				rendererListParams2.filteringSettings = filteringSettings;
				rendererListParams2.tagName = ShaderTagId.none;
				rendererListParams2.isPassTagName = false;
				bool flag5 = desc.stateBlock != null && desc.stateBlock != null;
				if (flag5)
				{
					NativeArray<RenderStateBlock> nativeArray = new NativeArray<RenderStateBlock>(1, Allocator.Temp, NativeArrayOptions.ClearMemory);
					nativeArray[0] = desc.stateBlock.Value;
					rendererListParams2.stateBlocks = new NativeArray<RenderStateBlock>?(nativeArray);
					NativeArray<ShaderTagId> nativeArray2 = new NativeArray<ShaderTagId>(1, Allocator.Temp, NativeArrayOptions.ClearMemory);
					nativeArray2[0] = ShaderTagId.none;
					rendererListParams2.tagValues = new NativeArray<ShaderTagId>?(nativeArray2);
				}
				rendererListParams = rendererListParams2;
			}
			return rendererListParams;
		}

		// Token: 0x04000F35 RID: 3893
		public SortingCriteria sortingCriteria;

		// Token: 0x04000F36 RID: 3894
		public PerObjectData rendererConfiguration;

		// Token: 0x04000F37 RID: 3895
		public RenderQueueRange renderQueueRange;

		// Token: 0x04000F38 RID: 3896
		public RenderStateBlock? stateBlock;

		// Token: 0x04000F39 RID: 3897
		public Shader overrideShader;

		// Token: 0x04000F3A RID: 3898
		public Material overrideMaterial;

		// Token: 0x04000F3B RID: 3899
		public bool excludeObjectMotionVectors;

		// Token: 0x04000F3C RID: 3900
		public int layerMask;

		// Token: 0x04000F3D RID: 3901
		public uint renderingLayerMask;

		// Token: 0x04000F3E RID: 3902
		public int overrideMaterialPassIndex;

		// Token: 0x04000F3F RID: 3903
		public int overrideShaderPassIndex;

		// Token: 0x04000F44 RID: 3908
		private static readonly ShaderTagId s_EmptyName = new ShaderTagId("");
	}
}
