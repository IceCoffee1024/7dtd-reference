using System;
using UnityEngine;

// Token: 0x0200132E RID: 4910
public class ImageEffect_TurretView : MonoBehaviour
{
	// Token: 0x06009AF3 RID: 39667 RVA: 0x003A87DB File Offset: 0x003A69DB
	[PublicizedFrom(EAccessModifier.Private)]
	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		this.material.SetTexture("_BackBuffer", source);
		Graphics.Blit(source, destination, this.material);
	}

	// Token: 0x040074CA RID: 29898
	public Material material;
}
