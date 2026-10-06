using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using Unity.Burst;
using WorldGenerationEngineFinal;

[assembly: AssemblyVersion("0.0.0.0")]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(ChunkProviderGenerateWorldFromRaw.RoadSmooth_00005714$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(RawStamp.SmoothAlpha_0000B943$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(StampManager.DrawStamp_0000B966$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(StampManager.DrawWaterStamp_0000B969$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.ClearWaterUnderTerrain_0000BA2A$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.FinalizeWater_0000BA2D$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.SmoothRoadTerrainTask_0000BA34$BurstDirectCall))]
[assembly: BurstCompiler.StaticTypeReinitAttribute(typeof(WorldBuilder.AdjustHeights_0000BA47$BurstDirectCall))]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
