using System;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x02001D95 RID: 7573
[PublicizedFrom(EAccessModifier.Internal)]
public static class $BurstDirectCallInitializer
{
	// Token: 0x0600DE79 RID: 56953 RVA: 0x004FA590 File Offset: 0x004F8790
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Initialize()
	{
		ChunkProviderGenerateWorldFromRaw.RoadSmooth_000056FA$BurstDirectCall.Initialize();
		PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall.Initialize();
		PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall.Initialize();
		PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall.Initialize();
		PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall.Initialize();
		PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall.Initialize();
		PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall.Initialize();
		RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall.Initialize();
		StampManager.DrawStamp_0000B80F$BurstDirectCall.Initialize();
		StampManager.DrawWaterStamp_0000B812$BurstDirectCall.Initialize();
		WorldBuilder.ClearWaterUnderTerrain_0000B8D3$BurstDirectCall.Initialize();
		WorldBuilder.FinalizeWater_0000B8D6$BurstDirectCall.Initialize();
		WorldBuilder.SmoothRoadTerrainTask_0000B8DD$BurstDirectCall.Initialize();
		WorldBuilder.AdjustHeights_0000B8F0$BurstDirectCall.Initialize();
	}
}
