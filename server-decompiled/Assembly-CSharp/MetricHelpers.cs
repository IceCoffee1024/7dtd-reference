using System;
using UnityEngine;

// Token: 0x02001529 RID: 5417
public static class MetricHelpers
{
	// Token: 0x04007E4E RID: 32334
	public static CallbackMetric TextureStreamingCurrent = new CallbackMetric
	{
		Header = "Texture Streaming Current",
		callback = () => string.Format("{0:F2}", Texture.currentTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007E4F RID: 32335
	public static CallbackMetric TextureStreamingTarget = new CallbackMetric
	{
		Header = "Texture Streaming Target",
		callback = () => string.Format("{0:F2}", Texture.targetTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007E50 RID: 32336
	public static CallbackMetric TextureStreamingDesired = new CallbackMetric
	{
		Header = "Texture Streaming Desired",
		callback = () => string.Format("{0:F2}", Texture.desiredTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007E51 RID: 32337
	public static CallbackMetric TextureStreamingNonStreamed = new CallbackMetric
	{
		Header = "Texture Streaming Non-Streamed",
		callback = () => string.Format("{0:F2}", Texture.nonStreamingTextureMemory * 9.5367431640625E-07)
	};

	// Token: 0x04007E52 RID: 32338
	public static CallbackMetric TextureStreamingBudget = new CallbackMetric
	{
		Header = "Texture Streaming Budget",
		callback = () => string.Format("{0:F2}", QualitySettings.streamingMipmapsActive ? QualitySettings.streamingMipmapsMemoryBudget : (-1f))
	};
}
