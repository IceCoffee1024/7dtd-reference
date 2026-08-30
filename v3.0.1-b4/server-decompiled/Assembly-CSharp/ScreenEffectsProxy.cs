using System;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x0200134F RID: 4943
[PostProcess(typeof(ScreenEffectsProxyRenderer), PostProcessEvent.AfterStack, "Custom/Screen Effects Proxy", false)]
[Serializable]
public sealed class ScreenEffectsProxy : PostProcessEffectSettings
{
	// Token: 0x06009B8F RID: 39823 RVA: 0x003AC916 File Offset: 0x003AAB16
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
