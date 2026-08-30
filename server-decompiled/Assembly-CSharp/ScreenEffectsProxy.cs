using System;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02001369 RID: 4969
[PostProcess(typeof(ScreenEffectsProxyRenderer), PostProcessEvent.AfterStack, "Custom/Screen Effects Proxy", false)]
[Serializable]
public sealed class ScreenEffectsProxy : PostProcessEffectSettings
{
	// Token: 0x06009C8A RID: 40074 RVA: 0x003B1BBE File Offset: 0x003AFDBE
	public override bool IsEnabledAndSupported(PostProcessRenderContext context)
	{
		if (base.IsEnabledAndSupported(context))
		{
			ScreenEffects instance = ScreenEffects.Instance;
			if (instance != null && instance.activeEffects.Count > 0)
			{
				return ScreenEffects.Instance.isActiveAndEnabled;
			}
		}
		return false;
	}
}
