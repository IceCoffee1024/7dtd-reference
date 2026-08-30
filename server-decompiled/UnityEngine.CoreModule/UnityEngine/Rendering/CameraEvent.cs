using System;

namespace UnityEngine.Rendering
{
	// Token: 0x020003FE RID: 1022
	public enum CameraEvent
	{
		// Token: 0x04000BC4 RID: 3012
		BeforeDepthTexture,
		// Token: 0x04000BC5 RID: 3013
		AfterDepthTexture,
		// Token: 0x04000BC6 RID: 3014
		BeforeDepthNormalsTexture,
		// Token: 0x04000BC7 RID: 3015
		AfterDepthNormalsTexture,
		// Token: 0x04000BC8 RID: 3016
		BeforeGBuffer,
		// Token: 0x04000BC9 RID: 3017
		AfterGBuffer,
		// Token: 0x04000BCA RID: 3018
		BeforeLighting,
		// Token: 0x04000BCB RID: 3019
		AfterLighting,
		// Token: 0x04000BCC RID: 3020
		BeforeFinalPass,
		// Token: 0x04000BCD RID: 3021
		AfterFinalPass,
		// Token: 0x04000BCE RID: 3022
		BeforeForwardOpaque,
		// Token: 0x04000BCF RID: 3023
		AfterForwardOpaque,
		// Token: 0x04000BD0 RID: 3024
		BeforeImageEffectsOpaque,
		// Token: 0x04000BD1 RID: 3025
		AfterImageEffectsOpaque,
		// Token: 0x04000BD2 RID: 3026
		BeforeSkybox,
		// Token: 0x04000BD3 RID: 3027
		AfterSkybox,
		// Token: 0x04000BD4 RID: 3028
		BeforeForwardAlpha,
		// Token: 0x04000BD5 RID: 3029
		AfterForwardAlpha,
		// Token: 0x04000BD6 RID: 3030
		BeforeImageEffects,
		// Token: 0x04000BD7 RID: 3031
		AfterImageEffects,
		// Token: 0x04000BD8 RID: 3032
		AfterEverything,
		// Token: 0x04000BD9 RID: 3033
		BeforeReflections,
		// Token: 0x04000BDA RID: 3034
		AfterReflections,
		// Token: 0x04000BDB RID: 3035
		BeforeHaloAndLensFlares,
		// Token: 0x04000BDC RID: 3036
		AfterHaloAndLensFlares
	}
}
