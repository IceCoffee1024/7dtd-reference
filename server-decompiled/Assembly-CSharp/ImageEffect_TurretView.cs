using System;
using UnityEngine;

// Token: 0x02001348 RID: 4936
public class ImageEffect_TurretView : MonoBehaviour
{
	// Token: 0x06009BEE RID: 39918 RVA: 0x003ADA83 File Offset: 0x003ABC83
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.material.SetTexture("_BackBuffer", source);
		Graphics.Blit(source, destination, this.material);
	}

	// Token: 0x040075A2 RID: 30114
	public Material material;
}
