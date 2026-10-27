using System;
using UnityEngine;

// Token: 0x0200155D RID: 5469
public static class MetricHelpers
{
	// Token: 0x04007F12 RID: 32530
	public static CallbackMetric TextureStreamingCurrent = new CallbackMetric
	{
		Header = "Texture Streaming Current",
		callback = () => string.Format("{0:F2}", Texture.currentTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007F13 RID: 32531
	public static CallbackMetric TextureStreamingTarget = new CallbackMetric
	{
		Header = "Texture Streaming Target",
		callback = () => string.Format("{0:F2}", Texture.targetTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007F14 RID: 32532
	public static CallbackMetric TextureStreamingDesired = new CallbackMetric
	{
		Header = "Texture Streaming Desired",
		callback = () => string.Format("{0:F2}", Texture.desiredTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007F15 RID: 32533
	public static CallbackMetric TextureStreamingNonStreamed = new CallbackMetric
	{
		Header = "Texture Streaming Non-Streamed",
		callback = () => string.Format("{0:F2}", Texture.nonStreamingTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007F16 RID: 32534
	public static CallbackMetric TextureStreamingBudget = new CallbackMetric
	{
		Header = "Texture Streaming Budget",
		callback = () => string.Format("{0:F2}", QualitySettings.streamingMipmapsActive ? QualitySettings.streamingMipmapsMemoryBudget : (-1f))
	};
}
