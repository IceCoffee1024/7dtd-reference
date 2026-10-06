using System;
using UnityEngine;

// Token: 0x0200154B RID: 5451
public static class MetricHelpers
{
	// Token: 0x04007ED1 RID: 32465
	public static CallbackMetric TextureStreamingCurrent = new CallbackMetric
	{
		Header = "Texture Streaming Current",
		callback = () => string.Format("{0:F2}", Texture.currentTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007ED2 RID: 32466
	public static CallbackMetric TextureStreamingTarget = new CallbackMetric
	{
		Header = "Texture Streaming Target",
		callback = () => string.Format("{0:F2}", Texture.targetTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007ED3 RID: 32467
	public static CallbackMetric TextureStreamingDesired = new CallbackMetric
	{
		Header = "Texture Streaming Desired",
		callback = () => string.Format("{0:F2}", Texture.desiredTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007ED4 RID: 32468
	public static CallbackMetric TextureStreamingNonStreamed = new CallbackMetric
	{
		Header = "Texture Streaming Non-Streamed",
		callback = () => string.Format("{0:F2}", Texture.nonStreamingTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007ED5 RID: 32469
	public static CallbackMetric TextureStreamingBudget = new CallbackMetric
	{
		Header = "Texture Streaming Budget",
		callback = () => string.Format("{0:F2}", QualitySettings.streamingMipmapsActive ? QualitySettings.streamingMipmapsMemoryBudget : (-1f))
	};
}
