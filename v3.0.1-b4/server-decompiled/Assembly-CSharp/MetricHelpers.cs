using System;
using UnityEngine;

// Token: 0x0200150E RID: 5390
public static class MetricHelpers
{
	// Token: 0x04007D75 RID: 32117
	public static CallbackMetric TextureStreamingCurrent = new CallbackMetric
	{
		Header = "Texture Streaming Current",
		callback = () => string.Format("{0:F2}", Texture.currentTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007D76 RID: 32118
	public static CallbackMetric TextureStreamingTarget = new CallbackMetric
	{
		Header = "Texture Streaming Target",
		callback = () => string.Format("{0:F2}", Texture.targetTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007D77 RID: 32119
	public static CallbackMetric TextureStreamingDesired = new CallbackMetric
	{
		Header = "Texture Streaming Desired",
		callback = () => string.Format("{0:F2}", Texture.desiredTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007D78 RID: 32120
	public static CallbackMetric TextureStreamingNonStreamed = new CallbackMetric
	{
		Header = "Texture Streaming Non-Streamed",
		callback = () => string.Format("{0:F2}", Texture.nonStreamingTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007D79 RID: 32121
	public static CallbackMetric TextureStreamingBudget = new CallbackMetric
	{
		Header = "Texture Streaming Budget",
		callback = () => string.Format("{0:F2}", QualitySettings.streamingMipmapsActive ? QualitySettings.streamingMipmapsMemoryBudget : (-1f))
	};
}
