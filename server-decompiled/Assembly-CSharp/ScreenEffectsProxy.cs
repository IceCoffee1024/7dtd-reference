using System;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200139B RID: 5019
[PostProcess(typeof(ScreenEffectsProxyRenderer), PostProcessEvent.AfterStack, "Custom/Screen Effects Proxy", false)]
[Serializable]
public sealed class ScreenEffectsProxy : PostProcessEffectSettings
{
	// Token: 0x06009D8E RID: 40334 RVA: 0x003B8B3E File Offset: 0x003B6D3E
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
