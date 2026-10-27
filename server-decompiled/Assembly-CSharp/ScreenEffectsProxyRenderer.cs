using System;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200139C RID: 5020
public sealed class ScreenEffectsProxyRenderer : PostProcessEffectRendererProxy<ScreenEffectsProxy>
{
	// Token: 0x06009D90 RID: 40336 RVA: 0x003B8B78 File Offset: 0x003B6D78
	public override void AddSubRenderersTo(List<PostProcessEffectRenderer> renderList)
	{
		foreach (ScreenEffects.ScreenEffect screenEffect in ScreenEffects.Instance.activeEffects)
		{
			renderList.Add(screenEffect);
		}
	}
}
