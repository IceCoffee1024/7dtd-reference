using System;
using UnityEngine.Rendering.PostProcessing;

// Token: 0x02001389 RID: 5001
[PostProcess(typeof(ScreenEffectsProxyRenderer), PostProcessEvent.AfterStack, "Custom/Screen Effects Proxy", false)]
[Serializable]
public sealed class ScreenEffectsProxy : PostProcessEffectSettings
{
	// Token: 0x06009D36 RID: 40246 RVA: 0x003B673A File Offset: 0x003B493A
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
