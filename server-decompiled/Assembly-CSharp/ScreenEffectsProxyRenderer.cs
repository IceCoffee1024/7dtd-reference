using System;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200136A RID: 4970
public sealed class ScreenEffectsProxyRenderer : PostProcessEffectRendererProxy<ScreenEffectsProxy>
{
	// Token: 0x06009C8C RID: 40076 RVA: 0x003B1BF8 File Offset: 0x003AFDF8
	public override void AddSubRenderersTo(List<PostProcessEffectRenderer> renderList)
	{
		foreach (ScreenEffects.ScreenEffect screenEffect in ScreenEffects.Instance.activeEffects)
		{
			renderList.Add(screenEffect);
		}
	}
}
