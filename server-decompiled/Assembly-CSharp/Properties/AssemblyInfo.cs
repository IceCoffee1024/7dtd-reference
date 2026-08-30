using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using Unity.Burst;
using WorldGenerationEngineFinal;

[assembly: AssemblyVersion("0.0.0.0")]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(ChunkProviderGenerateWorldFromRaw.RoadSmooth_00005672$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.IsPointOnPath_0000B620$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(RawStamp.SmoothAlpha_0000B669$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(StampManager.DrawStamp_0000B68C$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(StampManager.DrawWaterStamp_0000B68F$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.ClearWaterUnderTerrain_0000B750$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.FinalizeWater_0000B753$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.SmoothRoadTerrainTask_0000B75A$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.AdjustHeights_0000B76D$BurstDirectCall))]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
