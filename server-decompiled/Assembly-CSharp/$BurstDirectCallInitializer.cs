using System;
using UnityEngine;
using WorldGenerationEngineFinal;

// Token: 0x02001DF7 RID: 7671
[PublicizedFrom(EAccessModifier.Internal)]
public static class $BurstDirectCallInitializer
{
	// Token: 0x0600E106 RID: 57606 RVA: 0x00502CE4 File Offset: 0x00500EE4
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
	[PublicizedFrom(EAccessModifier.Private)]
	public static void Initialize()
	{
		ChunkProviderGenerateWorldFromRaw.RoadSmooth_00005739$BurstDirectCall.Initialize();
		PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall.Initialize();
		PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall.Initialize();
		PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall.Initialize();
		PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall.Initialize();
		PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall.Initialize();
		PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall.Initialize();
		RawStamp.SmoothAlpha_0000BA57$BurstDirectCall.Initialize();
		StampManager.DrawStamp_0000BA7A$BurstDirectCall.Initialize();
		StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall.Initialize();
		WorldBuilder.ClearWaterUnderTerrain_0000BB3E$BurstDirectCall.Initialize();
		WorldBuilder.FinalizeWater_0000BB41$BurstDirectCall.Initialize();
		WorldBuilder.SmoothRoadTerrainTask_0000BB48$BurstDirectCall.Initialize();
		WorldBuilder.AdjustHeights_0000BB5B$BurstDirectCall.Initialize();
	}
}
