using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using Unity.Burst;
using WorldGenerationEngineFinal;

[assembly: AssemblyVersion("0.0.0.0")]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(ChunkProviderGenerateWorldFromRaw.RoadSmooth_00005739$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(RawStamp.SmoothAlpha_0000BA57$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(StampManager.DrawStamp_0000BA7A$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(StampManager.DrawWaterStamp_0000BA7D$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.ClearWaterUnderTerrain_0000BB3E$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.FinalizeWater_0000BB41$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.SmoothRoadTerrainTask_0000BB48$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.AdjustHeights_0000BB5B$BurstDirectCall))]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
