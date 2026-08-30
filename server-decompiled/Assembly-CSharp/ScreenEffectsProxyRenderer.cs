using System;
using System.Collections.Generic;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02001350 RID: 4944
public sealed class ScreenEffectsProxyRenderer : PostProcessEffectRendererProxy<ScreenEffectsProxy>
{
	// Token: 0x06009B91 RID: 39825 RVA: 0x003AC950 File Offset: 0x003AAB50
	public override void AddSubRenderersTo(List<PostProcessEffectRenderer> renderList)
	{
		foreach (ScreenEffects.ScreenEffect screenEffect in ScreenEffects.Instance.activeEffects)
		{
			renderList.Add(screenEffect);
		}
	}
}
