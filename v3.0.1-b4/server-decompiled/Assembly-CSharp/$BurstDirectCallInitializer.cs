using System;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x02001D6E RID: 7534
[PublicizedFrom(EAccessModifier.Internal)]
public static class $BurstDirectCallInitializer
{
	// Token: 0x0600DCD8 RID: 56536 RVA: 0x004F3B1C File Offset: 0x004F1D1C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Initialize()
	{
		ChunkProviderGenerateWorldFromRaw.RoadSmooth_00005672$BurstDirectCall.Initialize();
		PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall.Initialize();
		PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall.Initialize();
		PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall.Initialize();
		PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall.Initialize();
		PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall.Initialize();
		PathingUtils.IsPointOnPath_0000B620$BurstDirectCall.Initialize();
		RawStamp.SmoothAlpha_0000B669$BurstDirectCall.Initialize();
		StampManager.DrawStamp_0000B68C$BurstDirectCall.Initialize();
		StampManager.DrawWaterStamp_0000B68F$BurstDirectCall.Initialize();
		WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall.Initialize();
		WorldBuilder.FinalizeWater_0000B753$BurstDirectCall.Initialize();
		WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall.Initialize();
		WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall.Initialize();
	}
}
