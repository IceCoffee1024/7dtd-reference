using System;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x02001DC7 RID: 7623
[PublicizedFrom(EAccessModifier.Internal)]
public static class $BurstDirectCallInitializer
{
	// Token: 0x0600DFF2 RID: 57330 RVA: 0x004FF14C File Offset: 0x004FD34C
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Initialize()
	{
		ChunkProviderGenerateWorldFromRaw.RoadSmooth_00005714$BurstDirectCall.Initialize();
		PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall.Initialize();
		PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall.Initialize();
		PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall.Initialize();
		PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall.Initialize();
		PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall.Initialize();
		PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall.Initialize();
		RawStamp.SmoothAlpha_0000B943$BurstDirectCall.Initialize();
		StampManager.DrawStamp_0000B966$BurstDirectCall.Initialize();
		StampManager.DrawWaterStamp_0000B969$BurstDirectCall.Initialize();
		WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall.Initialize();
		WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall.Initialize();
		WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall.Initialize();
		WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall.Initialize();
	}
}
