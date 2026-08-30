using System;
using UnityEngine.Rendering;

namespace UnityEngine
{
	// Token: 0x02000158 RID: 344
	public struct RenderParams
	{
		// Token: 0x06000AE6 RID: 2790 RVA: 0x00011A38 File Offset: 0x0000FC38
		public RenderParams(Material mat)
		{
			this.layer = 0;
			this.renderingLayerMask = GraphicsSettings.defaultRenderingLayerMask;
			this.rendererPriority = 0;
			this.worldBounds = new Bounds(Vector3.zero, Vector3.zero);
			this.camera = null;
			this.motionVectorMode = MotionVectorGenerationMode.Camera;
			this.reflectionProbeUsage = ReflectionProbeUsage.Off;
			this.material = mat;
			this.matProps = null;
			this.shadowCastingMode = ShadowCastingMode.Off;
			this.receiveShadows = false;
			this.lightProbeUsage = LightProbeUsage.Off;
			this.lightProbeProxyVolume = null;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x06000AE7 RID: 2791 RVA: 0x00011AC0 File Offset: 0x0000FCC0
		// (set) Token: 0x06000AE8 RID: 2792 RVA: 0x00011AC8 File Offset: 0x0000FCC8
		public int layer { readonly get; set; }

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x06000AE9 RID: 2793 RVA: 0x00011AD1 File Offset: 0x0000FCD1
		// (set) Token: 0x06000AEA RID: 2794 RVA: 0x00011AD9 File Offset: 0x0000FCD9
		public uint renderingLayerMask { readonly get; set; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06000AEB RID: 2795 RVA: 0x00011AE2 File Offset: 0x0000FCE2
		// (set) Token: 0x06000AEC RID: 2796 RVA: 0x00011AEA File Offset: 0x0000FCEA
		public int rendererPriority { readonly get; set; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000AED RID: 2797 RVA: 0x00011AF3 File Offset: 0x0000FCF3
		// (set) Token: 0x06000AEE RID: 2798 RVA: 0x00011AFB File Offset: 0x0000FCFB
		public Bounds worldBounds { readonly get; set; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000AEF RID: 2799 RVA: 0x00011B04 File Offset: 0x0000FD04
		// (set) Token: 0x06000AF0 RID: 2800 RVA: 0x00011B0C File Offset: 0x0000FD0C
		public Camera camera { readonly get; set; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000AF1 RID: 2801 RVA: 0x00011B15 File Offset: 0x0000FD15
		// (set) Token: 0x06000AF2 RID: 2802 RVA: 0x00011B1D File Offset: 0x0000FD1D
		public MotionVectorGenerationMode motionVectorMode { readonly get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000AF3 RID: 2803 RVA: 0x00011B26 File Offset: 0x0000FD26
		// (set) Token: 0x06000AF4 RID: 2804 RVA: 0x00011B2E File Offset: 0x0000FD2E
		public ReflectionProbeUsage reflectionProbeUsage { readonly get; set; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000AF5 RID: 2805 RVA: 0x00011B37 File Offset: 0x0000FD37
		// (set) Token: 0x06000AF6 RID: 2806 RVA: 0x00011B3F File Offset: 0x0000FD3F
		public Material material { readonly get; set; }

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000AF7 RID: 2807 RVA: 0x00011B48 File Offset: 0x0000FD48
		// (set) Token: 0x06000AF8 RID: 2808 RVA: 0x00011B50 File Offset: 0x0000FD50
		public MaterialPropertyBlock matProps { readonly get; set; }

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000AF9 RID: 2809 RVA: 0x00011B59 File Offset: 0x0000FD59
		// (set) Token: 0x06000AFA RID: 2810 RVA: 0x00011B61 File Offset: 0x0000FD61
		public ShadowCastingMode shadowCastingMode { readonly get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000AFB RID: 2811 RVA: 0x00011B6A File Offset: 0x0000FD6A
		// (set) Token: 0x06000AFC RID: 2812 RVA: 0x00011B72 File Offset: 0x0000FD72
		public bool receiveShadows { readonly get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000AFD RID: 2813 RVA: 0x00011B7B File Offset: 0x0000FD7B
		// (set) Token: 0x06000AFE RID: 2814 RVA: 0x00011B83 File Offset: 0x0000FD83
		public LightProbeUsage lightProbeUsage { readonly get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000AFF RID: 2815 RVA: 0x00011B8C File Offset: 0x0000FD8C
		// (set) Token: 0x06000B00 RID: 2816 RVA: 0x00011B94 File Offset: 0x0000FD94
		public LightProbeProxyVolume lightProbeProxyVolume { readonly get; set; }
	}
}
