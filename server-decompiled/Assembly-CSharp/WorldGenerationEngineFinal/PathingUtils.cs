using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001730 RID: 5936
	[BurstCompile(CompileSynchronously = true)]
	public class PathingUtils
	{
		// Token: 0x0600B8F8 RID: 47352 RVA: 0x00447A0D File Offset: 0x00445C0D
		public PathingUtils(WorldBuilder _worldBuilder)
		{
			PathingUtils.worldBuilder = _worldBuilder;
			this.data.nodePool = new PathNodePool(100000);
		}

		// Token: 0x0600B8F9 RID: 47353 RVA: 0x00447A48 File Offset: 0x00445C48
		public int GetPathCost(Vector2i start, Vector2i end, bool isCountryRoad = false)
		{
			this.InitPathData();
			int num = 0;
			WorldBuilder worldBuilder = PathingUtils.worldBuilder;
			Vector2i vector2i = start / 10;
			Vector2i vector2i2 = end / 10;
			PathNode pathNode;
			for (int i = PathingUtils.FindDetailedPath(ref worldBuilder.data, ref this.data, in vector2i, in vector2i2, isCountryRoad, false); i >= 0; i = pathNode.pathNext)
			{
				this.data.nodePool.Node(i, out pathNode);
				num++;
			}
			this.data.nodePool.ReturnAll();
			return num;
		}

		// Token: 0x0600B8FA RID: 47354 RVA: 0x00447AC4 File Offset: 0x00445CC4
		public NativeList<Vector2i> GetPath(Vector2i _start, Vector2i _end, bool _isCountryRoad, bool _isRiver = false)
		{
			this.InitPathData();
			WorldBuilder worldBuilder = PathingUtils.worldBuilder;
			Vector2i vector2i = _start / 10;
			Vector2i vector2i2 = _end / 10;
			int i = PathingUtils.FindDetailedPath(ref worldBuilder.data, ref this.data, in vector2i, in vector2i2, _isCountryRoad, _isRiver);
			this.pathTemp.Clear();
			while (i >= 0)
			{
				PathNode pathNode;
				this.data.nodePool.Node(i, out pathNode);
				Vector2i vector2i3 = pathNode.position * 10 + PathingUtils.stepHalf;
				this.pathTemp.Add(in vector2i3);
				i = pathNode.pathNext;
			}
			this.data.nodePool.ReturnAll();
			return this.pathTemp;
		}

		// Token: 0x0600B8FB RID: 47355 RVA: 0x00447B70 File Offset: 0x00445D70
		public Vector2i GetPathPoint(Vector2i _start, ref NativeList<Vector2> _endPath, bool _isCountryRoad, bool _isRiver, out int _cost)
		{
			this.InitPathData();
			this.ConvertPathToTemp(ref _endPath);
			Vector2i vector2i = Vector2i.min;
			int num = 0;
			WorldBuilder worldBuilder = PathingUtils.worldBuilder;
			Vector2i vector2i2 = _start / 10;
			int i = PathingUtils.FindDetailedPath(ref worldBuilder.data, ref this.data, in vector2i2, in this.pathTemp, _isCountryRoad, _isRiver);
			if (i >= 0)
			{
				PathNode pathNode;
				this.data.nodePool.Node(i, out pathNode);
				vector2i = pathNode.position * 10 + PathingUtils.stepHalf;
				Vector2 vector = vector2i.AsVector2();
				Vector2 vector2;
				if (PathingUtils.FindClosestPathPoint(in _endPath, in vector, out vector2, 1) < 400f)
				{
					vector2i = new Vector2i(vector2);
				}
				while (i >= 0)
				{
					this.data.nodePool.Node(i, out pathNode);
					num++;
					i = pathNode.pathNext;
				}
			}
			this.data.nodePool.ReturnAll();
			_cost = num;
			return vector2i;
		}

		// Token: 0x0600B8FC RID: 47356 RVA: 0x00447C4A File Offset: 0x00445E4A
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitPathData()
		{
			this.data.WorldSize = PathingUtils.worldBuilder.WorldSize;
		}

		// Token: 0x0600B8FD RID: 47357 RVA: 0x00447C64 File Offset: 0x00445E64
		[PublicizedFrom(EAccessModifier.Private)]
		public void ConvertPathToTemp(ref NativeList<Vector2> _path)
		{
			this.pathTemp.Clear();
			for (int i = 0; i < _path.Length; i++)
			{
				Vector2i vector2i;
				vector2i.x = ((int)_path[i].x + PathingUtils.stepHalf.x) / 10;
				vector2i.y = ((int)_path[i].y + PathingUtils.stepHalf.y) / 10;
				this.pathTemp.Add(in vector2i);
			}
		}

		// Token: 0x0600B8FE RID: 47358 RVA: 0x00447CE0 File Offset: 0x00445EE0
		[PublicizedFrom(EAccessModifier.Private)]
		public unsafe static void ClosedListInit(ref PathingUtils.Data data)
		{
			int num = data.WorldSize / 10 + 1;
			if (data.closedList.IsCreated)
			{
				int num2 = data.closedListMinY * data.closedListWidth;
				int num3 = (data.closedListMaxY + 1) * data.closedListWidth;
				void* ptr = (void*)((byte*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<byte>(data.closedList) + num2);
				UnsafeUtility.MemSet(ptr, 0, (long)(num3 - num2));
			}
			if (!data.closedList.IsCreated || data.closedListWidth != num)
			{
				data.closedListWidth = num;
				data.closedList = new NativeArray<byte>(num * num, Allocator.Persistent, NativeArrayOptions.ClearMemory);
			}
		}

		// Token: 0x0600B8FF RID: 47359 RVA: 0x00447D6A File Offset: 0x00445F6A
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static int FindDetailedPath(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver)
		{
			return PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall.Invoke(ref wd, ref data, in startPos, in endPos, _isCountryRoad, _isRiver);
		}

		// Token: 0x0600B900 RID: 47360 RVA: 0x00447D7C File Offset: 0x00445F7C
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DiagonalDist(Vector2i v1, Vector2i v2)
		{
			float num = Utils.FastAbs((float)(v1.x - v2.x));
			float num2 = Utils.FastAbs((float)(v1.y - v2.y));
			return num + num2 + -0.585786f * Utils.FastMin(num, num2);
		}

		// Token: 0x0600B901 RID: 47361 RVA: 0x00447DC2 File Offset: 0x00445FC2
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static int FindDetailedPath(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver)
		{
			return PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall.Invoke(ref wd, ref data, in startPos, in _endPath, _isCountryRoad, _isRiver);
		}

		// Token: 0x0600B902 RID: 47362 RVA: 0x00447DD1 File Offset: 0x00445FD1
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CalcPathBounds(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max)
		{
			PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall.Invoke(in _path, out _min, out _max);
		}

		// Token: 0x0600B903 RID: 47363 RVA: 0x00447DDB File Offset: 0x00445FDB
		[BurstCompile(CompileSynchronously = true)]
		public static float FindClosestPathPoint(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1)
		{
			return PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall.Invoke(in _path, in _startPos, out _destPoint, _step);
		}

		// Token: 0x0600B904 RID: 47364 RVA: 0x00447DE6 File Offset: 0x00445FE6
		[BurstCompile(CompileSynchronously = true)]
		public static float FindClosestPathPoint(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint)
		{
			return PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall.Invoke(in _path, in _startPos, out _destPoint);
		}

		// Token: 0x0600B905 RID: 47365 RVA: 0x00447DF0 File Offset: 0x00445FF0
		[BurstCompile(CompileSynchronously = true)]
		public static bool IsPointOnPath(in NativeList<Vector2i> _path, in Vector2i _point)
		{
			return PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall.Invoke(in _path, in _point);
		}

		// Token: 0x0600B906 RID: 47366 RVA: 0x00447DFC File Offset: 0x00445FFC
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsBlocked(ref WorldBuilder.Data wd, int pathX, int pathY, bool isRiver = false)
		{
			Vector2i vector2i = PathingUtils.pathPositionToWorldCenter(pathX, pathY);
			return !wd.InWorldBounds(vector2i.x, vector2i.y) || wd.GetStreetTileDataWorld(vector2i.x, vector2i.y).IsCity || (!isRiver && PathingUtils.IsWater(ref wd, pathX, pathY));
		}

		// Token: 0x0600B907 RID: 47367 RVA: 0x00447E52 File Offset: 0x00446052
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool InBounds(ref WorldBuilder.Data wd, Vector2i pos)
		{
			return PathingUtils.InBounds(ref wd, pos.x, pos.y);
		}

		// Token: 0x0600B908 RID: 47368 RVA: 0x00447E68 File Offset: 0x00446068
		public static bool InBounds(ref WorldBuilder.Data wd, int pathX, int pathY)
		{
			Vector2i vector2i = PathingUtils.pathPositionToWorldCenter(pathX, pathY);
			return (ulong)vector2i.x < (ulong)((long)wd.WorldSize) && (ulong)vector2i.y < (ulong)((long)wd.WorldSize);
		}

		// Token: 0x0600B909 RID: 47369 RVA: 0x00447EA0 File Offset: 0x004460A0
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsWater(ref WorldBuilder.Data wd, int pathX, int pathY)
		{
			Vector2i vector2i = PathingUtils.pathPositionToWorldMin(pathX, pathY);
			if (wd.GetStreetTileDataWorld(vector2i.x, vector2i.y).OverlapsWater)
			{
				for (int i = vector2i.y; i < vector2i.y + 10; i++)
				{
					for (int j = vector2i.x; j < vector2i.x + 10; j++)
					{
						if (wd.GetWater(j, i) > 0)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600B90A RID: 47370 RVA: 0x00447F0E File Offset: 0x0044610E
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetHeight(ref WorldBuilder.Data wd, Vector2i pos)
		{
			return PathingUtils.GetHeight(ref wd, pos.x, pos.y);
		}

		// Token: 0x0600B90B RID: 47371 RVA: 0x00447F22 File Offset: 0x00446122
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetHeight(ref WorldBuilder.Data wd, int pathX, int pathY)
		{
			return wd.GetHeight(PathingUtils.pathPositionToWorldCenter(pathX, pathY));
		}

		// Token: 0x0600B90C RID: 47372 RVA: 0x00447F31 File Offset: 0x00446131
		public BiomeType GetBiome(Vector2i pos)
		{
			return this.GetBiome(pos.x, pos.y);
		}

		// Token: 0x0600B90D RID: 47373 RVA: 0x00447F45 File Offset: 0x00446145
		public BiomeType GetBiome(int pathX, int pathY)
		{
			return PathingUtils.worldBuilder.GetBiome(PathingUtils.pathPositionToWorldCenter(pathX, pathY));
		}

		// Token: 0x0600B90E RID: 47374 RVA: 0x00447F58 File Offset: 0x00446158
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i pathPositionToWorldCenter(int pathX, int pathY)
		{
			Vector2i vector2i;
			vector2i.x = pathX * 10 + 5;
			vector2i.y = pathY * 10 + 5;
			return vector2i;
		}

		// Token: 0x0600B90F RID: 47375 RVA: 0x00447F80 File Offset: 0x00446180
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i pathPositionToWorldMin(int pathX, int pathY)
		{
			Vector2i vector2i;
			vector2i.x = pathX * 10;
			vector2i.y = pathY * 10;
			return vector2i;
		}

		// Token: 0x0600B910 RID: 47376 RVA: 0x00447FA4 File Offset: 0x004461A4
		public void AddMoveLimitArea(Rect r)
		{
			int num = (int)r.xMin;
			int num2 = (int)r.yMin;
			num /= 10;
			num2 /= 10;
			for (int i = 0; i < 15; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					if (j != 7 && i != 7)
					{
						this.SetPathBlocked(num + j, num2 + i, true);
					}
				}
			}
		}

		// Token: 0x0600B911 RID: 47377 RVA: 0x00447FFC File Offset: 0x004461FC
		public void RemoveFullyBlockedArea(Rect r)
		{
			int num = (int)r.xMin;
			int num2 = (int)r.yMin;
			num /= 10;
			num2 /= 10;
			for (int i = 0; i < 15; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					this.SetPathBlocked(num + j, num2 + i, false);
				}
			}
		}

		// Token: 0x0600B912 RID: 47378 RVA: 0x0044804C File Offset: 0x0044624C
		public void AddFullyBlockedArea(Rect r)
		{
			int num = (int)(r.xMin + 0.5f);
			int num2 = (int)(r.yMin + 0.5f);
			num /= 10;
			num2 /= 10;
			for (int i = 0; i < 15; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					this.SetPathBlocked(num + j, num2 + i, true);
				}
			}
		}

		// Token: 0x0600B913 RID: 47379 RVA: 0x004480A8 File Offset: 0x004462A8
		public void SetPathBlocked(Vector2i pos, bool isBlocked)
		{
			this.SetPathBlocked(pos.x, pos.y, isBlocked);
		}

		// Token: 0x0600B914 RID: 47380 RVA: 0x004480BD File Offset: 0x004462BD
		public void SetPathBlocked(int x, int y, bool isBlocked)
		{
			this.SetPathBlocked(x, y, isBlocked ? sbyte.MinValue : 0);
		}

		// Token: 0x0600B915 RID: 47381 RVA: 0x004480D0 File Offset: 0x004462D0
		public void SetPathBlocked(int x, int y, sbyte costMult)
		{
			if (!this.data.pathingGrid.IsCreated)
			{
				this.SetupPathingGrid();
			}
			if ((ulong)x >= (ulong)((long)this.data.pathingGridSize) || (ulong)y >= (ulong)((long)this.data.pathingGridSize))
			{
				return;
			}
			this.data.pathingGrid[x + y * this.data.pathingGridSize] = costMult;
		}

		// Token: 0x0600B916 RID: 47382 RVA: 0x00448136 File Offset: 0x00446336
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPathBlocked(ref PathingUtils.Data data, int x, int y)
		{
			return (ulong)x >= (ulong)((long)data.pathingGridSize) || (ulong)y >= (ulong)((long)data.pathingGridSize) || data.pathingGrid[x + y * data.pathingGridSize] == sbyte.MinValue;
		}

		// Token: 0x0600B917 RID: 47383 RVA: 0x0044816C File Offset: 0x0044636C
		public bool IsPointOnHighwayWorld(int x, int y)
		{
			int num = x / 10 + y / 10 * PathingUtils.worldBuilder.data.PathTileGridWidth;
			return PathingUtils.worldBuilder.data.PathTileGrid[num].TileState == PathTile.PathTileStates.Highway;
		}

		// Token: 0x0600B918 RID: 47384 RVA: 0x004481B0 File Offset: 0x004463B0
		public void SetupPathingGrid()
		{
			this.data.pathingGridSize = PathingUtils.worldBuilder.WorldSize / 10;
			this.data.pathingGrid = new NativeArray<sbyte>(this.data.pathingGridSize * this.data.pathingGridSize, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x0600B919 RID: 47385 RVA: 0x00448200 File Offset: 0x00446400
		public void Cleanup()
		{
			this.data.pathingGrid.Dispose();
			this.data.pathingGridSize = 0;
			this.data.nodePool.Cleanup();
			this.data.minHeapBinned.Cleanup();
			this.data.closedList.Dispose();
			this.pathTemp.Dispose();
		}

		// Token: 0x0600B91B RID: 47387 RVA: 0x004482F8 File Offset: 0x004464F8
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FindDetailedPath$BurstManaged(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver)
		{
			if (!PathingUtils.InBounds(ref wd, startPos) || !PathingUtils.InBounds(ref wd, endPos))
			{
				return -1;
			}
			PathingUtils.ClosedListInit(ref data);
			data.closedList[startPos.x + startPos.y * data.closedListWidth] = 1;
			data.closedListMinY = startPos.y;
			data.closedListMaxY = startPos.y;
			data.minHeapBinned.Init();
			int num = data.nodePool.Alloc();
			data.nodePool.Node(num).Set(startPos, 0f, 0f, -1);
			data.minHeapBinned.Add(ref data, num);
			Vector2i vector2i = new Vector2i(Utils.FastMin(startPos.x, endPos.x), Utils.FastMin(startPos.y, endPos.y));
			Vector2i vector2i2 = new Vector2i(Utils.FastMax(startPos.x, endPos.x), Utils.FastMax(startPos.y, endPos.y));
			int num2 = Utils.FastMax(0, vector2i.x - 200);
			int num3 = Utils.FastMax(0, vector2i.y - 200);
			int num4 = Utils.FastMin(vector2i2.x + 200, data.closedListWidth - 1);
			int num5 = Utils.FastMin(vector2i2.y + 200, data.closedListWidth - 1);
			float num6 = (_isCountryRoad ? 12f : 11f);
			int num7 = 20000;
			int num8;
			while ((num8 = data.minHeapBinned.ExtractFirst(ref data)) >= 0 && --num7 >= 0)
			{
				ref PathNode ptr = ref data.nodePool.Node(num8);
				Vector2i position = ptr.position;
				if (position == endPos)
				{
					return num8;
				}
				for (int i = 0; i < 8; i++)
				{
					Vector2i vector2i3 = PathingUtils.neighborOffset8Ways[i];
					Vector2i vector2i4 = ptr.position + vector2i3;
					if (vector2i4.x >= num2 && vector2i4.y >= num3 && vector2i4.x < num4 && vector2i4.y < num5)
					{
						int num9 = vector2i4.x + vector2i4.y * data.closedListWidth;
						if (data.closedList[num9] == 0)
						{
							bool flag = vector2i4 == endPos;
							if (!flag)
							{
								bool flag2 = PathingUtils.IsPathBlocked(ref data, vector2i4.x, vector2i4.y);
								if (!flag2)
								{
									flag2 = PathingUtils.IsBlocked(ref wd, vector2i4.x, vector2i4.y, _isRiver);
								}
								if (flag2)
								{
									data.closedList[num9] = 1;
									data.closedListMinY = Utils.FastMin(data.closedListMinY, vector2i4.y);
									data.closedListMaxY = Utils.FastMax(data.closedListMaxY, vector2i4.y);
									goto IL_054C;
								}
							}
							float num10 = Utils.FastAbs(PathingUtils.GetHeight(ref wd, position) - PathingUtils.GetHeight(ref wd, vector2i4));
							if (num10 <= num6)
							{
								float num11 = PathingUtils.DiagonalDist(vector2i4, endPos);
								num11 *= 1.4f;
								bool flag3 = vector2i3.x != 0 && vector2i3.y != 0;
								bool flag4 = true;
								if (!_isCountryRoad)
								{
									float num12 = Vector2i.DistanceSqr(vector2i4, startPos);
									float num13 = Vector2i.DistanceSqr(vector2i4, endPos);
									if (num12 <= 8.410001f || num13 <= 8.410001f)
									{
										flag4 = false;
									}
									Vector2i vector2i5 = vector2i4 * 10;
									if (wd.GetStreetTileDataWorld(vector2i5.x, vector2i5.y).ConnectedHighwayCount >= 3)
									{
										goto IL_054C;
									}
									if (!flag)
									{
										bool flag5 = wd.PathTileGrid[vector2i4.x + vector2i4.y * wd.PathTileGridWidth].TileState == PathTile.PathTileStates.Highway;
										if (flag5)
										{
											goto IL_054C;
										}
										if (flag3)
										{
											for (int j = 0; j < 2; j++)
											{
												int num14 = ptr.position.x;
												int num15 = ptr.position.y;
												if (j == 0)
												{
													num14 += vector2i3.x;
												}
												else
												{
													num15 += vector2i3.y;
												}
												flag5 = PathingUtils.IsPathBlocked(ref data, num14, num15);
												if (flag5)
												{
													break;
												}
												flag5 = PathingUtils.IsBlocked(ref wd, num14, num15, false);
												if (flag5)
												{
													break;
												}
												if (wd.PathTileGrid[num14 + num15 * wd.PathTileGridWidth].TileState == PathTile.PathTileStates.Highway)
												{
													flag5 = true;
													break;
												}
											}
											if (flag5)
											{
												goto IL_054C;
											}
										}
									}
									num10 = Utils.FastMax(0f, num10 - 0.5f);
									num10 *= 3f;
								}
								num10 *= 0.2f;
								float num16 = 1f;
								if (flag3)
								{
									num16 += 0.414214f;
								}
								if (flag4 && data.pathingGrid.IsCreated)
								{
									int num17 = (int)data.pathingGrid[vector2i4.x + vector2i4.y * data.pathingGridSize];
									if (num17 > 0)
									{
										num16 *= (float)num17;
									}
								}
								data.closedList[num9] = 1;
								data.closedListMinY = Utils.FastMin(data.closedListMinY, vector2i4.y);
								data.closedListMaxY = Utils.FastMax(data.closedListMaxY, vector2i4.y);
								int num18 = data.nodePool.Alloc();
								ref PathNode ptr2 = ref data.nodePool.Node(num18);
								float num19 = ptr.travelledCost + num16 + num10;
								float num20 = num19 + num11;
								ptr2.Set(vector2i4, num19, num20, num8);
								data.minHeapBinned.Add(ref data, num18);
							}
						}
					}
					IL_054C:;
				}
			}
			return -1;
		}

		// Token: 0x0600B91C RID: 47388 RVA: 0x00448878 File Offset: 0x00446A78
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FindDetailedPath$BurstManaged(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver)
		{
			PathingUtils.ClosedListInit(ref data);
			data.closedList[startPos.x + startPos.y * data.closedListWidth] = 1;
			data.closedListMinY = startPos.y;
			data.closedListMaxY = startPos.y;
			data.minHeapBinned.Init();
			int num = data.nodePool.Alloc();
			data.nodePool.Node(num).Set(startPos, 0f, 0f, -1);
			data.minHeapBinned.Add(ref data, num);
			Vector2i vector2i;
			Vector2i vector2i2;
			PathingUtils.CalcPathBounds(in _endPath, out vector2i, out vector2i2);
			int num2 = Utils.FastMax(0, vector2i.x - 200);
			int num3 = Utils.FastMax(0, vector2i.y - 200);
			int num4 = Utils.FastMin(vector2i2.x + 200, data.closedListWidth - 1);
			int num5 = Utils.FastMin(vector2i2.y + 200, data.closedListWidth - 1);
			float num6 = (_isCountryRoad ? 12f : 11f);
			int num7 = 20000;
			int num8;
			while ((num8 = data.minHeapBinned.ExtractFirst(ref data)) >= 0 && --num7 >= 0)
			{
				ref PathNode ptr = ref data.nodePool.Node(num8);
				Vector2i position = ptr.position;
				if (PathingUtils.IsPointOnPath(in _endPath, in position))
				{
					return num8;
				}
				for (int i = 0; i < 8; i++)
				{
					Vector2i vector2i3 = PathingUtils.neighborOffset8Ways[i];
					Vector2i vector2i4 = ptr.position + vector2i3;
					if (vector2i4.x >= num2 && vector2i4.y >= num3 && vector2i4.x < num4 && vector2i4.y < num5)
					{
						int num9 = vector2i4.x + vector2i4.y * data.closedListWidth;
						if (data.closedList[num9] == 0)
						{
							Vector2i vector2i5;
							PathingUtils.FindClosestPathPoint(in _endPath, in vector2i4, out vector2i5);
							if (vector2i4 != vector2i5)
							{
								bool flag = PathingUtils.IsPathBlocked(ref data, vector2i4.x, vector2i4.y);
								if (!flag)
								{
									flag = PathingUtils.IsBlocked(ref wd, vector2i4.x, vector2i4.y, _isRiver);
								}
								if (flag)
								{
									data.closedList[num9] = 1;
									data.closedListMinY = Utils.FastMin(data.closedListMinY, vector2i4.y);
									data.closedListMaxY = Utils.FastMax(data.closedListMaxY, vector2i4.y);
									goto IL_037A;
								}
							}
							float num10 = Utils.FastAbs(PathingUtils.GetHeight(ref wd, position) - PathingUtils.GetHeight(ref wd, vector2i4));
							if (num10 <= num6)
							{
								float num11 = Vector2i.Distance(vector2i4, vector2i5);
								num11 *= 1.4f;
								num10 *= 0.2f;
								num10 += 1f;
								if (vector2i3.x != 0 && vector2i3.y != 0)
								{
									num10 += 0.414214f;
								}
								if (data.pathingGrid.IsCreated)
								{
									int num12 = (int)data.pathingGrid[vector2i4.x + vector2i4.y * data.pathingGridSize];
									if (num12 > 0)
									{
										num10 *= (float)num12;
									}
								}
								data.closedList[num9] = 1;
								data.closedListMinY = Utils.FastMin(data.closedListMinY, vector2i4.y);
								data.closedListMaxY = Utils.FastMax(data.closedListMaxY, vector2i4.y);
								int num13 = data.nodePool.Alloc();
								ref PathNode ptr2 = ref data.nodePool.Node(num13);
								float num14 = ptr.travelledCost + num10;
								float num15 = num14 + num11;
								ptr2.Set(vector2i4, num14, num15, num8);
								data.minHeapBinned.Add(ref data, num13);
							}
						}
					}
					IL_037A:;
				}
			}
			return -1;
		}

		// Token: 0x0600B91D RID: 47389 RVA: 0x00448C24 File Offset: 0x00446E24
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void CalcPathBounds$BurstManaged(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max)
		{
			_min = Vector2i.max;
			_max = Vector2i.min;
			NativeList<Vector2i> nativeList = _path;
			foreach (Vector2i vector2i in nativeList)
			{
				_min.x = Utils.FastMin(_min.x, vector2i.x);
				_min.y = Utils.FastMin(_min.y, vector2i.y);
				_max.x = Utils.FastMax(_max.x, vector2i.x);
				_max.y = Utils.FastMax(_max.y, vector2i.y);
			}
		}

		// Token: 0x0600B91E RID: 47390 RVA: 0x00448CE8 File Offset: 0x00446EE8
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FindClosestPathPoint$BurstManaged(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1)
		{
			_destPoint = Vector2.zero;
			float num = float.MaxValue;
			int length = _path.Length;
			for (int i = 0; i < length; i += _step)
			{
				NativeList<Vector2> nativeList = _path;
				Vector2 vector = nativeList[i];
				float sqrMagnitude = (_startPos - vector).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					_destPoint = vector;
				}
			}
			return num;
		}

		// Token: 0x0600B91F RID: 47391 RVA: 0x00448D54 File Offset: 0x00446F54
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float FindClosestPathPoint$BurstManaged(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint)
		{
			_destPoint = Vector2i.zero;
			float num = float.MaxValue;
			NativeList<Vector2i> nativeList = _path;
			foreach (Vector2i vector2i in nativeList)
			{
				float num2 = Vector2i.DistanceSqr(_startPos, vector2i);
				if (num2 < num)
				{
					num = num2;
					_destPoint = vector2i;
				}
			}
			return num;
		}

		// Token: 0x0600B920 RID: 47392 RVA: 0x00448DD4 File Offset: 0x00446FD4
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Internal)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPointOnPath$BurstManaged(in NativeList<Vector2i> _path, in Vector2i _point)
		{
			NativeList<Vector2i> nativeList = _path;
			foreach (Vector2i vector2i in nativeList)
			{
				if (vector2i.x == _point.x && vector2i.y == _point.y)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04008A16 RID: 35350
		public const int PATHING_GRID_TILE_SIZE = 10;

		// Token: 0x04008A17 RID: 35351
		public const int stepSize = 10;

		// Token: 0x04008A18 RID: 35352
		public static readonly Vector2i stepHalf = new Vector2i(5, 5);

		// Token: 0x04008A19 RID: 35353
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cRoadCountryMaxStepH = 12f;

		// Token: 0x04008A1A RID: 35354
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cRoadHighwayMaxStepH = 11f;

		// Token: 0x04008A1B RID: 35355
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cHeightCostScale = 0.2f;

		// Token: 0x04008A1C RID: 35356
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cNeighborsCount = 8;

		// Token: 0x04008A1D RID: 35357
		[PublicizedFrom(EAccessModifier.Private)]
		public static readonly Vector2i[] neighborOffset8Ways = new Vector2i[]
		{
			new Vector2i(0, 1),
			new Vector2i(1, 1),
			new Vector2i(1, 0),
			new Vector2i(1, -1),
			new Vector2i(0, -1),
			new Vector2i(-1, -1),
			new Vector2i(-1, 0),
			new Vector2i(-1, 1)
		};

		// Token: 0x04008A1E RID: 35358
		[PublicizedFrom(EAccessModifier.Private)]
		public static WorldBuilder worldBuilder;

		// Token: 0x04008A1F RID: 35359
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeList<Vector2i> pathTemp = new NativeList<Vector2i>(200, Allocator.Persistent);

		// Token: 0x04008A20 RID: 35360
		[PublicizedFrom(EAccessModifier.Private)]
		public PathingUtils.Data data;

		// Token: 0x02001731 RID: 5937
		public struct Data
		{
			// Token: 0x04008A21 RID: 35361
			public int WorldSize;

			// Token: 0x04008A22 RID: 35362
			public NativeArray<sbyte> pathingGrid;

			// Token: 0x04008A23 RID: 35363
			public int pathingGridSize;

			// Token: 0x04008A24 RID: 35364
			public PathNodePool nodePool;

			// Token: 0x04008A25 RID: 35365
			public PathingUtils.MinHeapBinned minHeapBinned;

			// Token: 0x04008A26 RID: 35366
			public NativeArray<byte> closedList;

			// Token: 0x04008A27 RID: 35367
			public int closedListWidth;

			// Token: 0x04008A28 RID: 35368
			public int closedListMinY;

			// Token: 0x04008A29 RID: 35369
			public int closedListMaxY;
		}

		// Token: 0x02001732 RID: 5938
		public struct MinHeapBinned
		{
			// Token: 0x0600B921 RID: 47393 RVA: 0x00448E48 File Offset: 0x00447048
			public void Init()
			{
				if (!this.nodeBins.IsCreated)
				{
					this.nodeBins = new NativeArray<int>(32768, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
					this.lowBin = 0;
					this.highBin = 32767;
				}
				this.Reset();
			}

			// Token: 0x0600B922 RID: 47394 RVA: 0x00448E84 File Offset: 0x00447084
			public unsafe void Reset()
			{
				if (this.lowBin <= this.highBin)
				{
					int num = UnsafeUtility.SizeOf<int>();
					void* ptr = (void*)((byte*)NativeArrayUnsafeUtility.GetUnsafeBufferPointerWithoutChecks<int>(this.nodeBins) + this.lowBin * num);
					UnsafeUtility.MemSet(ptr, byte.MaxValue, (long)((this.highBin - this.lowBin + 1) * num));
				}
				this.lowBin = 32768;
				this.highBin = 0;
			}

			// Token: 0x0600B923 RID: 47395 RVA: 0x00448EEC File Offset: 0x004470EC
			public int ExtractFirst(ref PathingUtils.Data data)
			{
				if (this.lowBin <= this.highBin)
				{
					int num = this.nodeBins[this.lowBin];
					PathNode pathNode;
					data.nodePool.Node(num, out pathNode);
					this.nodeBins[this.lowBin] = pathNode.listNext;
					if (pathNode.listNext < 0)
					{
						int num2;
						do
						{
							num2 = this.lowBin + 1;
							this.lowBin = num2;
						}
						while (num2 <= this.highBin && this.nodeBins[this.lowBin] < 0);
						if (this.lowBin > this.highBin)
						{
							this.lowBin = 32768;
							this.highBin = 0;
						}
					}
					return num;
				}
				return -1;
			}

			// Token: 0x0600B924 RID: 47396 RVA: 0x00448F9C File Offset: 0x0044719C
			public void Add(ref PathingUtils.Data data, int _nodeIndex)
			{
				ref PathNode ptr = ref data.nodePool.Node(_nodeIndex);
				int num = (int)(ptr.totalCost * 0.07f);
				if (num >= 32768)
				{
					num = 32767;
				}
				if (num < this.lowBin)
				{
					this.lowBin = num;
				}
				if (num > this.highBin)
				{
					this.highBin = num;
				}
				int num2 = this.nodeBins[num];
				if (num2 < 0)
				{
					ptr.listNext = -1;
					this.nodeBins[num] = _nodeIndex;
					return;
				}
				ref PathNode ptr2 = ref data.nodePool.Node(num2);
				if (ptr.totalCost <= ptr2.totalCost)
				{
					ptr.listNext = num2;
					this.nodeBins[num] = _nodeIndex;
					return;
				}
				ref PathNode ptr3 = ref ptr2;
				int i;
				for (i = ptr3.listNext; i >= 0; i = ptr3.listNext)
				{
					ref PathNode ptr4 = ref data.nodePool.Node(i);
					if (ptr.totalCost <= ptr4.totalCost)
					{
						break;
					}
					ptr3 = ref ptr4;
				}
				ptr.listNext = i;
				ptr3.listNext = _nodeIndex;
			}

			// Token: 0x0600B925 RID: 47397 RVA: 0x00449096 File Offset: 0x00447296
			public void Cleanup()
			{
				this.nodeBins.Dispose();
			}

			// Token: 0x04008A2A RID: 35370
			[PublicizedFrom(EAccessModifier.Private)]
			public const int cBins = 32768;

			// Token: 0x04008A2B RID: 35371
			[PublicizedFrom(EAccessModifier.Private)]
			public const float cScale = 0.07f;

			// Token: 0x04008A2C RID: 35372
			[PublicizedFrom(EAccessModifier.Private)]
			public NativeArray<int> nodeBins;

			// Token: 0x04008A2D RID: 35373
			[PublicizedFrom(EAccessModifier.Private)]
			public int lowBin;

			// Token: 0x04008A2E RID: 35374
			[PublicizedFrom(EAccessModifier.Private)]
			public int highBin;
		}

		// Token: 0x02001733 RID: 5939
		[PublicizedFrom(EAccessModifier.Private)]
		public enum PathNodeType
		{
			// Token: 0x04008A30 RID: 35376
			Free,
			// Token: 0x04008A31 RID: 35377
			Road,
			// Token: 0x04008A32 RID: 35378
			Prefab,
			// Token: 0x04008A33 RID: 35379
			CityLimits = 4,
			// Token: 0x04008A34 RID: 35380
			Blocked = 8
		}

		// Token: 0x02001734 RID: 5940
		// (Invoke) Token: 0x0600B927 RID: 47399
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate int FindDetailedPath_0000B8F4$PostfixBurstDelegate(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver);

		// Token: 0x02001735 RID: 5941
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindDetailedPath_0000B8F4$BurstDirectCall
		{
			// Token: 0x0600B92A RID: 47402 RVA: 0x004490A3 File Offset: 0x004472A3
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindDetailedPath$BurstManaged(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref Vector2i, bool, bool)).MethodHandle, typeof(PathingUtils.FindDetailedPath_0000B8F4$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B92B RID: 47403 RVA: 0x004490D0 File Offset: 0x004472D0
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B92C RID: 47404 RVA: 0x004490E8 File Offset: 0x004472E8
			public static void Constructor()
			{
				PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindDetailedPath(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref Vector2i, bool, bool)).MethodHandle);
			}

			// Token: 0x0600B92D RID: 47405 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B92E RID: 47406 RVA: 0x004490F9 File Offset: 0x004472F9
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindDetailedPath_0000B8F4$BurstDirectCall()
			{
				PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B92F RID: 47407 RVA: 0x00449100 File Offset: 0x00447300
			public static int Invoke(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindDetailedPath_0000B8F4$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(WorldGenerationEngineFinal.WorldBuilder/Data&,WorldGenerationEngineFinal.PathingUtils/Data&,Vector2i&,Vector2i&,System.Boolean,System.Boolean), ref wd, ref data, ref startPos, ref endPos, _isCountryRoad, _isRiver, functionPointer);
					}
				}
				return PathingUtils.FindDetailedPath$BurstManaged(ref wd, ref data, in startPos, in endPos, _isCountryRoad, _isRiver);
			}

			// Token: 0x04008A35 RID: 35381
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A36 RID: 35382
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001736 RID: 5942
		// (Invoke) Token: 0x0600B931 RID: 47409
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate int FindDetailedPath_0000B8F6$PostfixBurstDelegate(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver);

		// Token: 0x02001737 RID: 5943
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindDetailedPath_0000B8F6$BurstDirectCall
		{
			// Token: 0x0600B934 RID: 47412 RVA: 0x0044913F File Offset: 0x0044733F
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindDetailedPath$BurstManaged(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref NativeList<Vector2i>, bool, bool)).MethodHandle, typeof(PathingUtils.FindDetailedPath_0000B8F6$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B935 RID: 47413 RVA: 0x0044916C File Offset: 0x0044736C
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B936 RID: 47414 RVA: 0x00449184 File Offset: 0x00447384
			public static void Constructor()
			{
				PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindDetailedPath(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref NativeList<Vector2i>, bool, bool)).MethodHandle);
			}

			// Token: 0x0600B937 RID: 47415 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B938 RID: 47416 RVA: 0x00449195 File Offset: 0x00447395
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindDetailedPath_0000B8F6$BurstDirectCall()
			{
				PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B939 RID: 47417 RVA: 0x0044919C File Offset: 0x0044739C
			public static int Invoke(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindDetailedPath_0000B8F6$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(WorldGenerationEngineFinal.WorldBuilder/Data&,WorldGenerationEngineFinal.PathingUtils/Data&,Vector2i&,Unity.Collections.NativeList`1<Vector2i>&,System.Boolean,System.Boolean), ref wd, ref data, ref startPos, ref _endPath, _isCountryRoad, _isRiver, functionPointer);
					}
				}
				return PathingUtils.FindDetailedPath$BurstManaged(ref wd, ref data, in startPos, in _endPath, _isCountryRoad, _isRiver);
			}

			// Token: 0x04008A37 RID: 35383
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A38 RID: 35384
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001738 RID: 5944
		// (Invoke) Token: 0x0600B93B RID: 47419
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void CalcPathBounds_0000B8F7$PostfixBurstDelegate(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max);

		// Token: 0x02001739 RID: 5945
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class CalcPathBounds_0000B8F7$BurstDirectCall
		{
			// Token: 0x0600B93E RID: 47422 RVA: 0x004491DB File Offset: 0x004473DB
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.CalcPathBounds$BurstManaged(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle, typeof(PathingUtils.CalcPathBounds_0000B8F7$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B93F RID: 47423 RVA: 0x00449208 File Offset: 0x00447408
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B940 RID: 47424 RVA: 0x00449220 File Offset: 0x00447420
			public static void Constructor()
			{
				PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.CalcPathBounds(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600B941 RID: 47425 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B942 RID: 47426 RVA: 0x00449231 File Offset: 0x00447431
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static CalcPathBounds_0000B8F7$BurstDirectCall()
			{
				PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B943 RID: 47427 RVA: 0x00449238 File Offset: 0x00447438
			public static void Invoke(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.CalcPathBounds_0000B8F7$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&,Vector2i&), ref _path, ref _min, ref _max, functionPointer);
						return;
					}
				}
				PathingUtils.CalcPathBounds$BurstManaged(in _path, out _min, out _max);
			}

			// Token: 0x04008A39 RID: 35385
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A3A RID: 35386
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x0200173A RID: 5946
		// (Invoke) Token: 0x0600B945 RID: 47429
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate float FindClosestPathPoint_0000B8F8$PostfixBurstDelegate(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1);

		// Token: 0x0200173B RID: 5947
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindClosestPathPoint_0000B8F8$BurstDirectCall
		{
			// Token: 0x0600B948 RID: 47432 RVA: 0x0044926D File Offset: 0x0044746D
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindClosestPathPoint$BurstManaged(ref NativeList<Vector2>, ref Vector2, ref Vector2, int)).MethodHandle, typeof(PathingUtils.FindClosestPathPoint_0000B8F8$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B949 RID: 47433 RVA: 0x0044929C File Offset: 0x0044749C
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B94A RID: 47434 RVA: 0x004492B4 File Offset: 0x004474B4
			public static void Constructor()
			{
				PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindClosestPathPoint(ref NativeList<Vector2>, ref Vector2, ref Vector2, int)).MethodHandle);
			}

			// Token: 0x0600B94B RID: 47435 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B94C RID: 47436 RVA: 0x004492C5 File Offset: 0x004474C5
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindClosestPathPoint_0000B8F8$BurstDirectCall()
			{
				PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B94D RID: 47437 RVA: 0x004492CC File Offset: 0x004474CC
			public static float Invoke(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindClosestPathPoint_0000B8F8$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Unity.Collections.NativeList`1<UnityEngine.Vector2>&,UnityEngine.Vector2&,UnityEngine.Vector2&,System.Int32), ref _path, ref _startPos, ref _destPoint, _step, functionPointer);
					}
				}
				return PathingUtils.FindClosestPathPoint$BurstManaged(in _path, in _startPos, out _destPoint, _step);
			}

			// Token: 0x04008A3B RID: 35387
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A3C RID: 35388
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x0200173C RID: 5948
		// (Invoke) Token: 0x0600B94F RID: 47439
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate float FindClosestPathPoint_0000B8F9$PostfixBurstDelegate(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint);

		// Token: 0x0200173D RID: 5949
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindClosestPathPoint_0000B8F9$BurstDirectCall
		{
			// Token: 0x0600B952 RID: 47442 RVA: 0x00449303 File Offset: 0x00447503
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindClosestPathPoint$BurstManaged(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle, typeof(PathingUtils.FindClosestPathPoint_0000B8F9$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B953 RID: 47443 RVA: 0x00449330 File Offset: 0x00447530
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B954 RID: 47444 RVA: 0x00449348 File Offset: 0x00447548
			public static void Constructor()
			{
				PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindClosestPathPoint(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600B955 RID: 47445 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B956 RID: 47446 RVA: 0x00449359 File Offset: 0x00447559
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindClosestPathPoint_0000B8F9$BurstDirectCall()
			{
				PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B957 RID: 47447 RVA: 0x00449360 File Offset: 0x00447560
			public static float Invoke(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindClosestPathPoint_0000B8F9$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&,Vector2i&), ref _path, ref _startPos, ref _destPoint, functionPointer);
					}
				}
				return PathingUtils.FindClosestPathPoint$BurstManaged(in _path, in _startPos, out _destPoint);
			}

			// Token: 0x04008A3D RID: 35389
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A3E RID: 35390
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x0200173E RID: 5950
		// (Invoke) Token: 0x0600B959 RID: 47449
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate bool IsPointOnPath_0000B8FA$PostfixBurstDelegate(in NativeList<Vector2i> _path, in Vector2i _point);

		// Token: 0x0200173F RID: 5951
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class IsPointOnPath_0000B8FA$BurstDirectCall
		{
			// Token: 0x0600B95C RID: 47452 RVA: 0x00449395 File Offset: 0x00447595
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.IsPointOnPath$BurstManaged(ref NativeList<Vector2i>, ref Vector2i)).MethodHandle, typeof(PathingUtils.IsPointOnPath_0000B8FA$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B95D RID: 47453 RVA: 0x004493C4 File Offset: 0x004475C4
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B95E RID: 47454 RVA: 0x004493DC File Offset: 0x004475DC
			public static void Constructor()
			{
				PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.IsPointOnPath(ref NativeList<Vector2i>, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600B95F RID: 47455 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B960 RID: 47456 RVA: 0x004493ED File Offset: 0x004475ED
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static IsPointOnPath_0000B8FA$BurstDirectCall()
			{
				PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B961 RID: 47457 RVA: 0x004493F4 File Offset: 0x004475F4
			public static bool Invoke(in NativeList<Vector2i> _path, in Vector2i _point)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.IsPointOnPath_0000B8FA$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&), ref _path, ref _point, functionPointer);
					}
				}
				return PathingUtils.IsPointOnPath$BurstManaged(in _path, in _point);
			}

			// Token: 0x04008A3F RID: 35391
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008A40 RID: 35392
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
