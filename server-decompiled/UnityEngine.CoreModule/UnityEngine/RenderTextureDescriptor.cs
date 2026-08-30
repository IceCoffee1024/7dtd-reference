using System;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Internal;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x020001D9 RID: 473
	public struct RenderTextureDescriptor
	{
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x06001439 RID: 5177 RVA: 0x0001C7FC File Offset: 0x0001A9FC
		// (set) Token: 0x0600143A RID: 5178 RVA: 0x0001C804 File Offset: 0x0001AA04
		public int width { readonly get; set; }

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x0600143B RID: 5179 RVA: 0x0001C80D File Offset: 0x0001AA0D
		// (set) Token: 0x0600143C RID: 5180 RVA: 0x0001C815 File Offset: 0x0001AA15
		public int height { readonly get; set; }

		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x0001C81E File Offset: 0x0001AA1E
		// (set) Token: 0x0600143E RID: 5182 RVA: 0x0001C826 File Offset: 0x0001AA26
		public int msaaSamples { readonly get; set; }

		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x0600143F RID: 5183 RVA: 0x0001C82F File Offset: 0x0001AA2F
		// (set) Token: 0x06001440 RID: 5184 RVA: 0x0001C837 File Offset: 0x0001AA37
		public int volumeDepth { readonly get; set; }

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001441 RID: 5185 RVA: 0x0001C840 File Offset: 0x0001AA40
		// (set) Token: 0x06001442 RID: 5186 RVA: 0x0001C848 File Offset: 0x0001AA48
		public int mipCount { readonly get; set; }

		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x06001443 RID: 5187 RVA: 0x0001C854 File Offset: 0x0001AA54
		// (set) Token: 0x06001444 RID: 5188 RVA: 0x0001C86C File Offset: 0x0001AA6C
		public GraphicsFormat graphicsFormat
		{
			get
			{
				return this._graphicsFormat;
			}
			set
			{
				this._graphicsFormat = value;
				this.SetOrClearRenderTextureCreationFlag(GraphicsFormatUtility.IsSRGBFormat(value), RenderTextureCreationFlags.SRGB);
				this.depthBufferBits = this.depthBufferBits;
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x06001445 RID: 5189 RVA: 0x0001C891 File Offset: 0x0001AA91
		// (set) Token: 0x06001446 RID: 5190 RVA: 0x0001C899 File Offset: 0x0001AA99
		public GraphicsFormat stencilFormat { readonly get; set; }

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x06001447 RID: 5191 RVA: 0x0001C8A2 File Offset: 0x0001AAA2
		// (set) Token: 0x06001448 RID: 5192 RVA: 0x0001C8AA File Offset: 0x0001AAAA
		public GraphicsFormat depthStencilFormat { readonly get; set; }

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06001449 RID: 5193 RVA: 0x0001C8B4 File Offset: 0x0001AAB4
		// (set) Token: 0x0600144A RID: 5194 RVA: 0x0001C8F0 File Offset: 0x0001AAF0
		public RenderTextureFormat colorFormat
		{
			get
			{
				bool flag = this.graphicsFormat > GraphicsFormat.None;
				RenderTextureFormat renderTextureFormat;
				if (flag)
				{
					renderTextureFormat = GraphicsFormatUtility.GetRenderTextureFormat(this.graphicsFormat);
				}
				else
				{
					renderTextureFormat = ((this.shadowSamplingMode != ShadowSamplingMode.None) ? RenderTextureFormat.Shadowmap : RenderTextureFormat.Depth);
				}
				return renderTextureFormat;
			}
			set
			{
				GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(value, this.sRGB);
				this.graphicsFormat = SystemInfo.GetCompatibleFormat(graphicsFormat, FormatUsage.Render);
			}
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600144B RID: 5195 RVA: 0x0001C91C File Offset: 0x0001AB1C
		// (set) Token: 0x0600144C RID: 5196 RVA: 0x0001C93C File Offset: 0x0001AB3C
		public bool sRGB
		{
			get
			{
				return GraphicsFormatUtility.IsSRGBFormat(this.graphicsFormat);
			}
			set
			{
				this.graphicsFormat = ((value && QualitySettings.activeColorSpace == ColorSpace.Linear && this.colorFormat != RenderTextureFormat.R8 && this.colorFormat != RenderTextureFormat.RG16) ? GraphicsFormatUtility.GetSRGBFormat(this.graphicsFormat) : GraphicsFormatUtility.GetLinearFormat(this.graphicsFormat));
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x0600144D RID: 5197 RVA: 0x0001C988 File Offset: 0x0001AB88
		// (set) Token: 0x0600144E RID: 5198 RVA: 0x0001C9A5 File Offset: 0x0001ABA5
		public int depthBufferBits
		{
			get
			{
				return GraphicsFormatUtility.GetDepthBits(this.depthStencilFormat);
			}
			set
			{
				this.depthStencilFormat = RenderTexture.GetDepthStencilFormatLegacy(value, this.graphicsFormat);
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x0600144F RID: 5199 RVA: 0x0001C9BB File Offset: 0x0001ABBB
		// (set) Token: 0x06001450 RID: 5200 RVA: 0x0001C9C3 File Offset: 0x0001ABC3
		public TextureDimension dimension { readonly get; set; }

		// Token: 0x17000428 RID: 1064
		// (get) Token: 0x06001451 RID: 5201 RVA: 0x0001C9CC File Offset: 0x0001ABCC
		// (set) Token: 0x06001452 RID: 5202 RVA: 0x0001C9D4 File Offset: 0x0001ABD4
		public ShadowSamplingMode shadowSamplingMode { readonly get; set; }

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x06001453 RID: 5203 RVA: 0x0001C9DD File Offset: 0x0001ABDD
		// (set) Token: 0x06001454 RID: 5204 RVA: 0x0001C9E5 File Offset: 0x0001ABE5
		public VRTextureUsage vrUsage { readonly get; set; }

		// Token: 0x1700042A RID: 1066
		// (get) Token: 0x06001455 RID: 5205 RVA: 0x0001C9F0 File Offset: 0x0001ABF0
		public RenderTextureCreationFlags flags
		{
			get
			{
				return this._flags;
			}
		}

		// Token: 0x1700042B RID: 1067
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x0001CA08 File Offset: 0x0001AC08
		// (set) Token: 0x06001457 RID: 5207 RVA: 0x0001CA10 File Offset: 0x0001AC10
		public RenderTextureMemoryless memoryless { readonly get; set; }

		// Token: 0x06001458 RID: 5208 RVA: 0x0001CA19 File Offset: 0x0001AC19
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height)
		{
			this = new RenderTextureDescriptor(width, height, RenderTextureFormat.Default);
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x0001CA26 File Offset: 0x0001AC26
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat)
		{
			this = new RenderTextureDescriptor(width, height, colorFormat, 0);
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x0001CA34 File Offset: 0x0001AC34
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat, int depthBufferBits)
		{
			this = new RenderTextureDescriptor(width, height, colorFormat, depthBufferBits, Texture.GenerateAllMips);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x0001CA48 File Offset: 0x0001AC48
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, int depthBufferBits)
		{
			this = new RenderTextureDescriptor(width, height, colorFormat, depthBufferBits, Texture.GenerateAllMips);
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0001CA5C File Offset: 0x0001AC5C
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, RenderTextureFormat colorFormat, int depthBufferBits, int mipCount)
		{
			this = new RenderTextureDescriptor(width, height, colorFormat, depthBufferBits, mipCount, RenderTextureReadWrite.Linear);
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x0001CA70 File Offset: 0x0001AC70
		public RenderTextureDescriptor(int width, int height, [DefaultValue("RenderTextureFormat.Default")] RenderTextureFormat colorFormat, [DefaultValue("0")] int depthBufferBits, [DefaultValue("Texture.GenerateAllMips")] int mipCount, [DefaultValue("RenderTextureReadWrite.Linear")] RenderTextureReadWrite readWrite)
		{
			GraphicsFormat graphicsFormat = GraphicsFormatUtility.GetGraphicsFormat(colorFormat, readWrite);
			GraphicsFormat compatibleFormat = SystemInfo.GetCompatibleFormat(graphicsFormat, FormatUsage.Render);
			this = new RenderTextureDescriptor(width, height, compatibleFormat, RenderTexture.GetDepthStencilFormatLegacy(depthBufferBits, colorFormat), mipCount);
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x0001CAA8 File Offset: 0x0001ACA8
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, int depthBufferBits, int mipCount)
		{
			this = default(RenderTextureDescriptor);
			this._flags = RenderTextureCreationFlags.AutoGenerateMips | RenderTextureCreationFlags.AllowVerticalFlip;
			this.width = width;
			this.height = height;
			this.volumeDepth = 1;
			this.msaaSamples = 1;
			this.graphicsFormat = colorFormat;
			this.depthStencilFormat = RenderTexture.GetDepthStencilFormatLegacy(depthBufferBits, colorFormat);
			this.mipCount = mipCount;
			this.dimension = TextureDimension.Tex2D;
			this.shadowSamplingMode = ShadowSamplingMode.None;
			this.vrUsage = VRTextureUsage.None;
			this.memoryless = RenderTextureMemoryless.None;
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0001CB28 File Offset: 0x0001AD28
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, GraphicsFormat depthStencilFormat)
		{
			this = new RenderTextureDescriptor(width, height, colorFormat, depthStencilFormat, Texture.GenerateAllMips);
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x0001CB3C File Offset: 0x0001AD3C
		[ExcludeFromDocs]
		public RenderTextureDescriptor(int width, int height, GraphicsFormat colorFormat, GraphicsFormat depthStencilFormat, int mipCount)
		{
			this = default(RenderTextureDescriptor);
			this._flags = RenderTextureCreationFlags.AutoGenerateMips | RenderTextureCreationFlags.AllowVerticalFlip;
			this.width = width;
			this.height = height;
			this.volumeDepth = 1;
			this.msaaSamples = 1;
			this.graphicsFormat = colorFormat;
			this.depthStencilFormat = depthStencilFormat;
			this.mipCount = mipCount;
			this.dimension = TextureDimension.Tex2D;
			this.shadowSamplingMode = ShadowSamplingMode.None;
			this.vrUsage = VRTextureUsage.None;
			this.memoryless = RenderTextureMemoryless.None;
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x0001CBB8 File Offset: 0x0001ADB8
		private void SetOrClearRenderTextureCreationFlag(bool value, RenderTextureCreationFlags flag)
		{
			if (value)
			{
				this._flags |= flag;
			}
			else
			{
				this._flags &= ~flag;
			}
		}

		// Token: 0x1700042C RID: 1068
		// (get) Token: 0x06001462 RID: 5218 RVA: 0x0001CBF0 File Offset: 0x0001ADF0
		// (set) Token: 0x06001463 RID: 5219 RVA: 0x0001CC0D File Offset: 0x0001AE0D
		public bool useMipMap
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.MipMap) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.MipMap);
			}
		}

		// Token: 0x1700042D RID: 1069
		// (get) Token: 0x06001464 RID: 5220 RVA: 0x0001CC1C File Offset: 0x0001AE1C
		// (set) Token: 0x06001465 RID: 5221 RVA: 0x0001CC39 File Offset: 0x0001AE39
		public bool autoGenerateMips
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.AutoGenerateMips) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.AutoGenerateMips);
			}
		}

		// Token: 0x1700042E RID: 1070
		// (get) Token: 0x06001466 RID: 5222 RVA: 0x0001CC48 File Offset: 0x0001AE48
		// (set) Token: 0x06001467 RID: 5223 RVA: 0x0001CC66 File Offset: 0x0001AE66
		public bool enableRandomWrite
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.EnableRandomWrite) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.EnableRandomWrite);
			}
		}

		// Token: 0x1700042F RID: 1071
		// (get) Token: 0x06001468 RID: 5224 RVA: 0x0001CC74 File Offset: 0x0001AE74
		// (set) Token: 0x06001469 RID: 5225 RVA: 0x0001CC95 File Offset: 0x0001AE95
		public bool bindMS
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.BindMS) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.BindMS);
			}
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x0600146A RID: 5226 RVA: 0x0001CCA8 File Offset: 0x0001AEA8
		// (set) Token: 0x0600146B RID: 5227 RVA: 0x0001CCC6 File Offset: 0x0001AEC6
		internal bool createdFromScript
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.CreatedFromScript) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.CreatedFromScript);
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x0600146C RID: 5228 RVA: 0x0001CCD4 File Offset: 0x0001AED4
		// (set) Token: 0x0600146D RID: 5229 RVA: 0x0001CCF5 File Offset: 0x0001AEF5
		public bool useDynamicScale
		{
			get
			{
				return (this._flags & RenderTextureCreationFlags.DynamicallyScalable) > (RenderTextureCreationFlags)0;
			}
			set
			{
				this.SetOrClearRenderTextureCreationFlag(value, RenderTextureCreationFlags.DynamicallyScalable);
			}
		}

		// Token: 0x04000668 RID: 1640
		private GraphicsFormat _graphicsFormat;

		// Token: 0x0400066E RID: 1646
		private RenderTextureCreationFlags _flags;
	}
}
