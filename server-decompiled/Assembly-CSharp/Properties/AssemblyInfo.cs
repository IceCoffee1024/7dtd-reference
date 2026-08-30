using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using Unity.Burst;
using WorldGenerationEngineFinal;

[assembly: AssemblyVersion("0.0.0.0")]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(ChunkProviderGenerateWorldFromRaw.RoadSmooth_000056FA$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(RawStamp.SmoothAlpha_0000B7EC$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(StampManager.DrawStamp_0000B80F$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(StampManager.DrawWaterStamp_0000B812$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.ClearWaterUnderTerrain_0000B8D3$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.FinalizeWater_0000B8D6$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.SmoothRoadTerrainTask_0000B8DD$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.AdjustHeights_0000B8F0$BurstDirectCall))]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
