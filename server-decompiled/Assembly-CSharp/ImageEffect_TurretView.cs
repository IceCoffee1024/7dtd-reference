using System;
using UnityEngine;

// Token: 0x02001368 RID: 4968
public class ImageEffect_TurretView : MonoBehaviour
{
	// Token: 0x06009C9A RID: 40090 RVA: 0x003B25FF File Offset: 0x003B07FF
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.material.SetTexture("_BackBuffer", source);
		Graphics.Blit(source, destination, this.material);
	}

	// Token: 0x0400761D RID: 30237
	public Material material;
}
