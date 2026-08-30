using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001704 RID: 5892
	[BurstCompile(CompileSynchronously = true)]
	public class PathingUtils
	{
		// Token: 0x0600B7A1 RID: 47009 RVA: 0x004434ED File Offset: 0x004416ED
		public PathingUtils(WorldBuilder _worldBuilder)
		{
			PathingUtils.worldBuilder = _worldBuilder;
			this.data.nodePool = new PathNodePool(100000);
		}

		// Token: 0x0600B7A2 RID: 47010 RVA: 0x00443528 File Offset: 0x00441728
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

		// Token: 0x0600B7A3 RID: 47011 RVA: 0x004435A4 File Offset: 0x004417A4
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

		// Token: 0x0600B7A4 RID: 47012 RVA: 0x00443650 File Offset: 0x00441850
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

		// Token: 0x0600B7A5 RID: 47013 RVA: 0x0044372A File Offset: 0x0044192A
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitPathData()
		{
			this.data.WorldSize = PathingUtils.worldBuilder.WorldSize;
		}

		// Token: 0x0600B7A6 RID: 47014 RVA: 0x00443744 File Offset: 0x00441944
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

		// Token: 0x0600B7A7 RID: 47015 RVA: 0x004437C0 File Offset: 0x004419C0
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

		// Token: 0x0600B7A8 RID: 47016 RVA: 0x0044384A File Offset: 0x00441A4A
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static int FindDetailedPath(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver)
		{
			return PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall.Invoke(ref wd, ref data, in startPos, in endPos, _isCountryRoad, _isRiver);
		}

		// Token: 0x0600B7A9 RID: 47017 RVA: 0x0044385C File Offset: 0x00441A5C
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DiagonalDist(Vector2i v1, Vector2i v2)
		{
			float num = Utils.FastAbs((float)(v1.x - v2.x));
			float num2 = Utils.FastAbs((float)(v1.y - v2.y));
			return num + num2 + -0.585786f * Utils.FastMin(num, num2);
		}

		// Token: 0x0600B7AA RID: 47018 RVA: 0x004438A2 File Offset: 0x00441AA2
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static int FindDetailedPath(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver)
		{
			return PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall.Invoke(ref wd, ref data, in startPos, in _endPath, _isCountryRoad, _isRiver);
		}

		// Token: 0x0600B7AB RID: 47019 RVA: 0x004438B1 File Offset: 0x00441AB1
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CalcPathBounds(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max)
		{
			PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall.Invoke(in _path, out _min, out _max);
		}

		// Token: 0x0600B7AC RID: 47020 RVA: 0x004438BB File Offset: 0x00441ABB
		[BurstCompile(CompileSynchronously = true)]
		public static float FindClosestPathPoint(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1)
		{
			return PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall.Invoke(in _path, in _startPos, out _destPoint, _step);
		}

		// Token: 0x0600B7AD RID: 47021 RVA: 0x004438C6 File Offset: 0x00441AC6
		[BurstCompile(CompileSynchronously = true)]
		public static float FindClosestPathPoint(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint)
		{
			return PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall.Invoke(in _path, in _startPos, out _destPoint);
		}

		// Token: 0x0600B7AE RID: 47022 RVA: 0x004438D0 File Offset: 0x00441AD0
		[BurstCompile(CompileSynchronously = true)]
		public static bool IsPointOnPath(in NativeList<Vector2i> _path, in Vector2i _point)
		{
			return PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall.Invoke(in _path, in _point);
		}

		// Token: 0x0600B7AF RID: 47023 RVA: 0x004438DC File Offset: 0x00441ADC
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsBlocked(ref WorldBuilder.Data wd, int pathX, int pathY, bool isRiver = false)
		{
			Vector2i vector2i = PathingUtils.pathPositionToWorldCenter(pathX, pathY);
			return !wd.InWorldBounds(vector2i.x, vector2i.y) || wd.GetStreetTileDataWorld(vector2i.x, vector2i.y).IsCity || (!isRiver && PathingUtils.IsWater(ref wd, pathX, pathY));
		}

		// Token: 0x0600B7B0 RID: 47024 RVA: 0x00443932 File Offset: 0x00441B32
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool InBounds(ref WorldBuilder.Data wd, Vector2i pos)
		{
			return PathingUtils.InBounds(ref wd, pos.x, pos.y);
		}

		// Token: 0x0600B7B1 RID: 47025 RVA: 0x00443948 File Offset: 0x00441B48
		public static bool InBounds(ref WorldBuilder.Data wd, int pathX, int pathY)
		{
			Vector2i vector2i = PathingUtils.pathPositionToWorldCenter(pathX, pathY);
			return (ulong)vector2i.x < (ulong)((long)wd.WorldSize) && (ulong)vector2i.y < (ulong)((long)wd.WorldSize);
		}

		// Token: 0x0600B7B2 RID: 47026 RVA: 0x00443980 File Offset: 0x00441B80
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

		// Token: 0x0600B7B3 RID: 47027 RVA: 0x004439EE File Offset: 0x00441BEE
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetHeight(ref WorldBuilder.Data wd, Vector2i pos)
		{
			return PathingUtils.GetHeight(ref wd, pos.x, pos.y);
		}

		// Token: 0x0600B7B4 RID: 47028 RVA: 0x00443A02 File Offset: 0x00441C02
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetHeight(ref WorldBuilder.Data wd, int pathX, int pathY)
		{
			return wd.GetHeight(PathingUtils.pathPositionToWorldCenter(pathX, pathY));
		}

		// Token: 0x0600B7B5 RID: 47029 RVA: 0x00443A11 File Offset: 0x00441C11
		public BiomeType GetBiome(Vector2i pos)
		{
			return this.GetBiome(pos.x, pos.y);
		}

		// Token: 0x0600B7B6 RID: 47030 RVA: 0x00443A25 File Offset: 0x00441C25
		public BiomeType GetBiome(int pathX, int pathY)
		{
			return PathingUtils.worldBuilder.GetBiome(PathingUtils.pathPositionToWorldCenter(pathX, pathY));
		}

		// Token: 0x0600B7B7 RID: 47031 RVA: 0x00443A38 File Offset: 0x00441C38
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i pathPositionToWorldCenter(int pathX, int pathY)
		{
			Vector2i vector2i;
			vector2i.x = pathX * 10 + 5;
			vector2i.y = pathY * 10 + 5;
			return vector2i;
		}

		// Token: 0x0600B7B8 RID: 47032 RVA: 0x00443A60 File Offset: 0x00441C60
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i pathPositionToWorldMin(int pathX, int pathY)
		{
			Vector2i vector2i;
			vector2i.x = pathX * 10;
			vector2i.y = pathY * 10;
			return vector2i;
		}

		// Token: 0x0600B7B9 RID: 47033 RVA: 0x00443A84 File Offset: 0x00441C84
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

		// Token: 0x0600B7BA RID: 47034 RVA: 0x00443ADC File Offset: 0x00441CDC
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

		// Token: 0x0600B7BB RID: 47035 RVA: 0x00443B2C File Offset: 0x00441D2C
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

		// Token: 0x0600B7BC RID: 47036 RVA: 0x00443B88 File Offset: 0x00441D88
		public void SetPathBlocked(Vector2i pos, bool isBlocked)
		{
			this.SetPathBlocked(pos.x, pos.y, isBlocked);
		}

		// Token: 0x0600B7BD RID: 47037 RVA: 0x00443B9D File Offset: 0x00441D9D
		public void SetPathBlocked(int x, int y, bool isBlocked)
		{
			this.SetPathBlocked(x, y, isBlocked ? sbyte.MinValue : 0);
		}

		// Token: 0x0600B7BE RID: 47038 RVA: 0x00443BB0 File Offset: 0x00441DB0
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

		// Token: 0x0600B7BF RID: 47039 RVA: 0x00443C16 File Offset: 0x00441E16
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPathBlocked(ref PathingUtils.Data data, int x, int y)
		{
			return (ulong)x >= (ulong)((long)data.pathingGridSize) || (ulong)y >= (ulong)((long)data.pathingGridSize) || data.pathingGrid[x + y * data.pathingGridSize] == sbyte.MinValue;
		}

		// Token: 0x0600B7C0 RID: 47040 RVA: 0x00443C4C File Offset: 0x00441E4C
		public bool IsPointOnHighwayWorld(int x, int y)
		{
			int num = x / 10 + y / 10 * PathingUtils.worldBuilder.data.PathTileGridWidth;
			return PathingUtils.worldBuilder.data.PathTileGrid[num].TileState == PathTile.PathTileStates.Highway;
		}

		// Token: 0x0600B7C1 RID: 47041 RVA: 0x00443C90 File Offset: 0x00441E90
		public void SetupPathingGrid()
		{
			this.data.pathingGridSize = PathingUtils.worldBuilder.WorldSize / 10;
			this.data.pathingGrid = new NativeArray<sbyte>(this.data.pathingGridSize * this.data.pathingGridSize, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x0600B7C2 RID: 47042 RVA: 0x00443CE0 File Offset: 0x00441EE0
		public void Cleanup()
		{
			this.data.pathingGrid.Dispose();
			this.data.pathingGridSize = 0;
			this.data.nodePool.Cleanup();
			this.data.minHeapBinned.Cleanup();
			this.data.closedList.Dispose();
			this.pathTemp.Dispose();
		}

		// Token: 0x0600B7C4 RID: 47044 RVA: 0x00443DD8 File Offset: 0x00441FD8
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

		// Token: 0x0600B7C5 RID: 47045 RVA: 0x00444358 File Offset: 0x00442558
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

		// Token: 0x0600B7C6 RID: 47046 RVA: 0x00444704 File Offset: 0x00442904
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

		// Token: 0x0600B7C7 RID: 47047 RVA: 0x004447C8 File Offset: 0x004429C8
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

		// Token: 0x0600B7C8 RID: 47048 RVA: 0x00444834 File Offset: 0x00442A34
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

		// Token: 0x0600B7C9 RID: 47049 RVA: 0x004448B4 File Offset: 0x00442AB4
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

		// Token: 0x04008951 RID: 35153
		public const int PATHING_GRID_TILE_SIZE = 10;

		// Token: 0x04008952 RID: 35154
		public const int stepSize = 10;

		// Token: 0x04008953 RID: 35155
		public static readonly Vector2i stepHalf = new Vector2i(5, 5);

		// Token: 0x04008954 RID: 35156
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cRoadCountryMaxStepH = 12f;

		// Token: 0x04008955 RID: 35157
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cRoadHighwayMaxStepH = 11f;

		// Token: 0x04008956 RID: 35158
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cHeightCostScale = 0.2f;

		// Token: 0x04008957 RID: 35159
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cNeighborsCount = 8;

		// Token: 0x04008958 RID: 35160
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

		// Token: 0x04008959 RID: 35161
		[PublicizedFrom(EAccessModifier.Private)]
		public static WorldBuilder worldBuilder;

		// Token: 0x0400895A RID: 35162
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeList<Vector2i> pathTemp = new NativeList<Vector2i>(200, Allocator.Persistent);

		// Token: 0x0400895B RID: 35163
		[PublicizedFrom(EAccessModifier.Private)]
		public PathingUtils.Data data;

		// Token: 0x02001705 RID: 5893
		public struct Data
		{
			// Token: 0x0400895C RID: 35164
			public int WorldSize;

			// Token: 0x0400895D RID: 35165
			public NativeArray<sbyte> pathingGrid;

			// Token: 0x0400895E RID: 35166
			public int pathingGridSize;

			// Token: 0x0400895F RID: 35167
			public PathNodePool nodePool;

			// Token: 0x04008960 RID: 35168
			public PathingUtils.MinHeapBinned minHeapBinned;

			// Token: 0x04008961 RID: 35169
			public NativeArray<byte> closedList;

			// Token: 0x04008962 RID: 35170
			public int closedListWidth;

			// Token: 0x04008963 RID: 35171
			public int closedListMinY;

			// Token: 0x04008964 RID: 35172
			public int closedListMaxY;
		}

		// Token: 0x02001706 RID: 5894
		public struct MinHeapBinned
		{
			// Token: 0x0600B7CA RID: 47050 RVA: 0x00444928 File Offset: 0x00442B28
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

			// Token: 0x0600B7CB RID: 47051 RVA: 0x00444964 File Offset: 0x00442B64
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

			// Token: 0x0600B7CC RID: 47052 RVA: 0x004449CC File Offset: 0x00442BCC
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

			// Token: 0x0600B7CD RID: 47053 RVA: 0x00444A7C File Offset: 0x00442C7C
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

			// Token: 0x0600B7CE RID: 47054 RVA: 0x00444B76 File Offset: 0x00442D76
			public void Cleanup()
			{
				this.nodeBins.Dispose();
			}

			// Token: 0x04008965 RID: 35173
			[PublicizedFrom(EAccessModifier.Private)]
			public const int cBins = 32768;

			// Token: 0x04008966 RID: 35174
			[PublicizedFrom(EAccessModifier.Private)]
			public const float cScale = 0.07f;

			// Token: 0x04008967 RID: 35175
			[PublicizedFrom(EAccessModifier.Private)]
			public NativeArray<int> nodeBins;

			// Token: 0x04008968 RID: 35176
			[PublicizedFrom(EAccessModifier.Private)]
			public int lowBin;

			// Token: 0x04008969 RID: 35177
			[PublicizedFrom(EAccessModifier.Private)]
			public int highBin;
		}

		// Token: 0x02001707 RID: 5895
		[PublicizedFrom(EAccessModifier.Private)]
		public enum PathNodeType
		{
			// Token: 0x0400896B RID: 35179
			Free,
			// Token: 0x0400896C RID: 35180
			Road,
			// Token: 0x0400896D RID: 35181
			Prefab,
			// Token: 0x0400896E RID: 35182
			CityLimits = 4,
			// Token: 0x0400896F RID: 35183
			Blocked = 8
		}

		// Token: 0x02001708 RID: 5896
		// (Invoke) Token: 0x0600B7D0 RID: 47056
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate int FindDetailedPath_0000B79D$PostfixBurstDelegate(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver);

		// Token: 0x02001709 RID: 5897
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindDetailedPath_0000B79D$BurstDirectCall
		{
			// Token: 0x0600B7D3 RID: 47059 RVA: 0x00444B83 File Offset: 0x00442D83
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindDetailedPath$BurstManaged(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref Vector2i, bool, bool)).MethodHandle, typeof(PathingUtils.FindDetailedPath_0000B79D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B7D4 RID: 47060 RVA: 0x00444BB0 File Offset: 0x00442DB0
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B7D5 RID: 47061 RVA: 0x00444BC8 File Offset: 0x00442DC8
			public static void Constructor()
			{
				PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindDetailedPath(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref Vector2i, bool, bool)).MethodHandle);
			}

			// Token: 0x0600B7D6 RID: 47062 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B7D7 RID: 47063 RVA: 0x00444BD9 File Offset: 0x00442DD9
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindDetailedPath_0000B79D$BurstDirectCall()
			{
				PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B7D8 RID: 47064 RVA: 0x00444BE0 File Offset: 0x00442DE0
			public static int Invoke(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindDetailedPath_0000B79D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(WorldGenerationEngineFinal.WorldBuilder/Data&,WorldGenerationEngineFinal.PathingUtils/Data&,Vector2i&,Vector2i&,System.Boolean,System.Boolean), ref wd, ref data, ref startPos, ref endPos, _isCountryRoad, _isRiver, functionPointer);
					}
				}
				return PathingUtils.FindDetailedPath$BurstManaged(ref wd, ref data, in startPos, in endPos, _isCountryRoad, _isRiver);
			}

			// Token: 0x04008970 RID: 35184
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008971 RID: 35185
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x0200170A RID: 5898
		// (Invoke) Token: 0x0600B7DA RID: 47066
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate int FindDetailedPath_0000B79F$PostfixBurstDelegate(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver);

		// Token: 0x0200170B RID: 5899
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindDetailedPath_0000B79F$BurstDirectCall
		{
			// Token: 0x0600B7DD RID: 47069 RVA: 0x00444C1F File Offset: 0x00442E1F
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindDetailedPath$BurstManaged(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref NativeList<Vector2i>, bool, bool)).MethodHandle, typeof(PathingUtils.FindDetailedPath_0000B79F$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B7DE RID: 47070 RVA: 0x00444C4C File Offset: 0x00442E4C
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B7DF RID: 47071 RVA: 0x00444C64 File Offset: 0x00442E64
			public static void Constructor()
			{
				PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindDetailedPath(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref NativeList<Vector2i>, bool, bool)).MethodHandle);
			}

			// Token: 0x0600B7E0 RID: 47072 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B7E1 RID: 47073 RVA: 0x00444C75 File Offset: 0x00442E75
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindDetailedPath_0000B79F$BurstDirectCall()
			{
				PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B7E2 RID: 47074 RVA: 0x00444C7C File Offset: 0x00442E7C
			public static int Invoke(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindDetailedPath_0000B79F$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(WorldGenerationEngineFinal.WorldBuilder/Data&,WorldGenerationEngineFinal.PathingUtils/Data&,Vector2i&,Unity.Collections.NativeList`1<Vector2i>&,System.Boolean,System.Boolean), ref wd, ref data, ref startPos, ref _endPath, _isCountryRoad, _isRiver, functionPointer);
					}
				}
				return PathingUtils.FindDetailedPath$BurstManaged(ref wd, ref data, in startPos, in _endPath, _isCountryRoad, _isRiver);
			}

			// Token: 0x04008972 RID: 35186
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008973 RID: 35187
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x0200170C RID: 5900
		// (Invoke) Token: 0x0600B7E4 RID: 47076
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void CalcPathBounds_0000B7A0$PostfixBurstDelegate(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max);

		// Token: 0x0200170D RID: 5901
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class CalcPathBounds_0000B7A0$BurstDirectCall
		{
			// Token: 0x0600B7E7 RID: 47079 RVA: 0x00444CBB File Offset: 0x00442EBB
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.CalcPathBounds$BurstManaged(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle, typeof(PathingUtils.CalcPathBounds_0000B7A0$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B7E8 RID: 47080 RVA: 0x00444CE8 File Offset: 0x00442EE8
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B7E9 RID: 47081 RVA: 0x00444D00 File Offset: 0x00442F00
			public static void Constructor()
			{
				PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.CalcPathBounds(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600B7EA RID: 47082 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B7EB RID: 47083 RVA: 0x00444D11 File Offset: 0x00442F11
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static CalcPathBounds_0000B7A0$BurstDirectCall()
			{
				PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B7EC RID: 47084 RVA: 0x00444D18 File Offset: 0x00442F18
			public static void Invoke(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.CalcPathBounds_0000B7A0$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&,Vector2i&), ref _path, ref _min, ref _max, functionPointer);
						return;
					}
				}
				PathingUtils.CalcPathBounds$BurstManaged(in _path, out _min, out _max);
			}

			// Token: 0x04008974 RID: 35188
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008975 RID: 35189
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x0200170E RID: 5902
		// (Invoke) Token: 0x0600B7EE RID: 47086
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate float FindClosestPathPoint_0000B7A1$PostfixBurstDelegate(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1);

		// Token: 0x0200170F RID: 5903
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindClosestPathPoint_0000B7A1$BurstDirectCall
		{
			// Token: 0x0600B7F1 RID: 47089 RVA: 0x00444D4D File Offset: 0x00442F4D
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindClosestPathPoint$BurstManaged(ref NativeList<Vector2>, ref Vector2, ref Vector2, int)).MethodHandle, typeof(PathingUtils.FindClosestPathPoint_0000B7A1$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B7F2 RID: 47090 RVA: 0x00444D7C File Offset: 0x00442F7C
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B7F3 RID: 47091 RVA: 0x00444D94 File Offset: 0x00442F94
			public static void Constructor()
			{
				PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindClosestPathPoint(ref NativeList<Vector2>, ref Vector2, ref Vector2, int)).MethodHandle);
			}

			// Token: 0x0600B7F4 RID: 47092 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B7F5 RID: 47093 RVA: 0x00444DA5 File Offset: 0x00442FA5
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindClosestPathPoint_0000B7A1$BurstDirectCall()
			{
				PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B7F6 RID: 47094 RVA: 0x00444DAC File Offset: 0x00442FAC
			public static float Invoke(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindClosestPathPoint_0000B7A1$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Unity.Collections.NativeList`1<UnityEngine.Vector2>&,UnityEngine.Vector2&,UnityEngine.Vector2&,System.Int32), ref _path, ref _startPos, ref _destPoint, _step, functionPointer);
					}
				}
				return PathingUtils.FindClosestPathPoint$BurstManaged(in _path, in _startPos, out _destPoint, _step);
			}

			// Token: 0x04008976 RID: 35190
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008977 RID: 35191
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001710 RID: 5904
		// (Invoke) Token: 0x0600B7F8 RID: 47096
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate float FindClosestPathPoint_0000B7A2$PostfixBurstDelegate(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint);

		// Token: 0x02001711 RID: 5905
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindClosestPathPoint_0000B7A2$BurstDirectCall
		{
			// Token: 0x0600B7FB RID: 47099 RVA: 0x00444DE3 File Offset: 0x00442FE3
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindClosestPathPoint$BurstManaged(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle, typeof(PathingUtils.FindClosestPathPoint_0000B7A2$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B7FC RID: 47100 RVA: 0x00444E10 File Offset: 0x00443010
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B7FD RID: 47101 RVA: 0x00444E28 File Offset: 0x00443028
			public static void Constructor()
			{
				PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindClosestPathPoint(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600B7FE RID: 47102 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B7FF RID: 47103 RVA: 0x00444E39 File Offset: 0x00443039
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindClosestPathPoint_0000B7A2$BurstDirectCall()
			{
				PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B800 RID: 47104 RVA: 0x00444E40 File Offset: 0x00443040
			public static float Invoke(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindClosestPathPoint_0000B7A2$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&,Vector2i&), ref _path, ref _startPos, ref _destPoint, functionPointer);
					}
				}
				return PathingUtils.FindClosestPathPoint$BurstManaged(in _path, in _startPos, out _destPoint);
			}

			// Token: 0x04008978 RID: 35192
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008979 RID: 35193
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001712 RID: 5906
		// (Invoke) Token: 0x0600B802 RID: 47106
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate bool IsPointOnPath_0000B7A3$PostfixBurstDelegate(in NativeList<Vector2i> _path, in Vector2i _point);

		// Token: 0x02001713 RID: 5907
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class IsPointOnPath_0000B7A3$BurstDirectCall
		{
			// Token: 0x0600B805 RID: 47109 RVA: 0x00444E75 File Offset: 0x00443075
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.IsPointOnPath$BurstManaged(ref NativeList<Vector2i>, ref Vector2i)).MethodHandle, typeof(PathingUtils.IsPointOnPath_0000B7A3$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B806 RID: 47110 RVA: 0x00444EA4 File Offset: 0x004430A4
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B807 RID: 47111 RVA: 0x00444EBC File Offset: 0x004430BC
			public static void Constructor()
			{
				PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.IsPointOnPath(ref NativeList<Vector2i>, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600B808 RID: 47112 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B809 RID: 47113 RVA: 0x00444ECD File Offset: 0x004430CD
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static IsPointOnPath_0000B7A3$BurstDirectCall()
			{
				PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B80A RID: 47114 RVA: 0x00444ED4 File Offset: 0x004430D4
			public static bool Invoke(in NativeList<Vector2i> _path, in Vector2i _point)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.IsPointOnPath_0000B7A3$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&), ref _path, ref _point, functionPointer);
					}
				}
				return PathingUtils.IsPointOnPath$BurstManaged(in _path, in _point);
			}

			// Token: 0x0400897A RID: 35194
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x0400897B RID: 35195
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
