using System;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200138A RID: 5002
public sealed class ScreenEffectsProxyRenderer : PostProcessEffectRendererProxy<ScreenEffectsProxy>
{
	// Token: 0x06009D38 RID: 40248 RVA: 0x003B6774 File Offset: 0x003B4974
	public override void AddSubRenderersTo(List<PostProcessEffectRenderer> renderList)
	{
		foreach (ScreenEffects.ScreenEffect screenEffect in ScreenEffects.Instance.activeEffects)
		{
			renderList.Add(screenEffect);
		}
	}
}
