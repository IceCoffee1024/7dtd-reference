using System;
using UnityEngine;

// Token: 0x0200137A RID: 4986
public class ImageEffect_TurretView : MonoBehaviour
{
	// Token: 0x06009CF2 RID: 40178 RVA: 0x003B4A03 File Offset: 0x003B2C03
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.material.SetTexture("_BackBuffer", source);
		Graphics.Blit(source, destination, this.material);
	}

	// Token: 0x0400765E RID: 30302
	public Material material;
}
