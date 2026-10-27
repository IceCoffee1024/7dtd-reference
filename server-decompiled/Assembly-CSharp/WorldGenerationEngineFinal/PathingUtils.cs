using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x02001761 RID: 5985
	[BurstCompile(CompileSynchronously = true)]
	public class PathingUtils
	{
		// Token: 0x0600BA0C RID: 47628 RVA: 0x0044B3B9 File Offset: 0x004495B9
		public PathingUtils(WorldBuilder _worldBuilder)
		{
			PathingUtils.worldBuilder = _worldBuilder;
			this.data.nodePool = new PathNodePool(100000);
		}

		// Token: 0x0600BA0D RID: 47629 RVA: 0x0044B3F4 File Offset: 0x004495F4
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

		// Token: 0x0600BA0E RID: 47630 RVA: 0x0044B470 File Offset: 0x00449670
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

		// Token: 0x0600BA0F RID: 47631 RVA: 0x0044B51C File Offset: 0x0044971C
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

		// Token: 0x0600BA10 RID: 47632 RVA: 0x0044B5F6 File Offset: 0x004497F6
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitPathData()
		{
			this.data.WorldSize = PathingUtils.worldBuilder.WorldSize;
		}

		// Token: 0x0600BA11 RID: 47633 RVA: 0x0044B610 File Offset: 0x00449810
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

		// Token: 0x0600BA12 RID: 47634 RVA: 0x0044B68C File Offset: 0x0044988C
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

		// Token: 0x0600BA13 RID: 47635 RVA: 0x0044B716 File Offset: 0x00449916
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static int FindDetailedPath(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver)
		{
			return PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall.Invoke(ref wd, ref data, in startPos, in endPos, _isCountryRoad, _isRiver);
		}

		// Token: 0x0600BA14 RID: 47636 RVA: 0x0044B728 File Offset: 0x00449928
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DiagonalDist(Vector2i v1, Vector2i v2)
		{
			float num = Utils.FastAbs((float)(v1.x - v2.x));
			float num2 = Utils.FastAbs((float)(v1.y - v2.y));
			return num + num2 + -0.585786f * Utils.FastMin(num, num2);
		}

		// Token: 0x0600BA15 RID: 47637 RVA: 0x0044B76E File Offset: 0x0044996E
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static int FindDetailedPath(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver)
		{
			return PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall.Invoke(ref wd, ref data, in startPos, in _endPath, _isCountryRoad, _isRiver);
		}

		// Token: 0x0600BA16 RID: 47638 RVA: 0x0044B77D File Offset: 0x0044997D
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CalcPathBounds(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max)
		{
			PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall.Invoke(in _path, out _min, out _max);
		}

		// Token: 0x0600BA17 RID: 47639 RVA: 0x0044B787 File Offset: 0x00449987
		[BurstCompile(CompileSynchronously = true)]
		public static float FindClosestPathPoint(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1)
		{
			return PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall.Invoke(in _path, in _startPos, out _destPoint, _step);
		}

		// Token: 0x0600BA18 RID: 47640 RVA: 0x0044B792 File Offset: 0x00449992
		[BurstCompile(CompileSynchronously = true)]
		public static float FindClosestPathPoint(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint)
		{
			return PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall.Invoke(in _path, in _startPos, out _destPoint);
		}

		// Token: 0x0600BA19 RID: 47641 RVA: 0x0044B79C File Offset: 0x0044999C
		[BurstCompile(CompileSynchronously = true)]
		public static bool IsPointOnPath(in NativeList<Vector2i> _path, in Vector2i _point)
		{
			return PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall.Invoke(in _path, in _point);
		}

		// Token: 0x0600BA1A RID: 47642 RVA: 0x0044B7A8 File Offset: 0x004499A8
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsBlocked(ref WorldBuilder.Data wd, int pathX, int pathY, bool isRiver = false)
		{
			Vector2i vector2i = PathingUtils.pathPositionToWorldCenter(pathX, pathY);
			return !wd.InWorldBounds(vector2i.x, vector2i.y) || wd.GetStreetTileDataWorld(vector2i.x, vector2i.y).IsCity || (!isRiver && PathingUtils.IsWater(ref wd, pathX, pathY));
		}

		// Token: 0x0600BA1B RID: 47643 RVA: 0x0044B7FE File Offset: 0x004499FE
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool InBounds(ref WorldBuilder.Data wd, Vector2i pos)
		{
			return PathingUtils.InBounds(ref wd, pos.x, pos.y);
		}

		// Token: 0x0600BA1C RID: 47644 RVA: 0x0044B814 File Offset: 0x00449A14
		public static bool InBounds(ref WorldBuilder.Data wd, int pathX, int pathY)
		{
			Vector2i vector2i = PathingUtils.pathPositionToWorldCenter(pathX, pathY);
			return (ulong)vector2i.x < (ulong)((long)wd.WorldSize) && (ulong)vector2i.y < (ulong)((long)wd.WorldSize);
		}

		// Token: 0x0600BA1D RID: 47645 RVA: 0x0044B84C File Offset: 0x00449A4C
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

		// Token: 0x0600BA1E RID: 47646 RVA: 0x0044B8BA File Offset: 0x00449ABA
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetHeight(ref WorldBuilder.Data wd, Vector2i pos)
		{
			return PathingUtils.GetHeight(ref wd, pos.x, pos.y);
		}

		// Token: 0x0600BA1F RID: 47647 RVA: 0x0044B8CE File Offset: 0x00449ACE
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetHeight(ref WorldBuilder.Data wd, int pathX, int pathY)
		{
			return wd.GetHeight(PathingUtils.pathPositionToWorldCenter(pathX, pathY));
		}

		// Token: 0x0600BA20 RID: 47648 RVA: 0x0044B8DD File Offset: 0x00449ADD
		public BiomeType GetBiome(Vector2i pos)
		{
			return this.GetBiome(pos.x, pos.y);
		}

		// Token: 0x0600BA21 RID: 47649 RVA: 0x0044B8F1 File Offset: 0x00449AF1
		public BiomeType GetBiome(int pathX, int pathY)
		{
			return PathingUtils.worldBuilder.GetBiome(PathingUtils.pathPositionToWorldCenter(pathX, pathY));
		}

		// Token: 0x0600BA22 RID: 47650 RVA: 0x0044B904 File Offset: 0x00449B04
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i pathPositionToWorldCenter(int pathX, int pathY)
		{
			Vector2i vector2i;
			vector2i.x = pathX * 10 + 5;
			vector2i.y = pathY * 10 + 5;
			return vector2i;
		}

		// Token: 0x0600BA23 RID: 47651 RVA: 0x0044B92C File Offset: 0x00449B2C
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i pathPositionToWorldMin(int pathX, int pathY)
		{
			Vector2i vector2i;
			vector2i.x = pathX * 10;
			vector2i.y = pathY * 10;
			return vector2i;
		}

		// Token: 0x0600BA24 RID: 47652 RVA: 0x0044B950 File Offset: 0x00449B50
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

		// Token: 0x0600BA25 RID: 47653 RVA: 0x0044B9A8 File Offset: 0x00449BA8
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

		// Token: 0x0600BA26 RID: 47654 RVA: 0x0044B9F8 File Offset: 0x00449BF8
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

		// Token: 0x0600BA27 RID: 47655 RVA: 0x0044BA54 File Offset: 0x00449C54
		public void SetPathBlocked(Vector2i pos, bool isBlocked)
		{
			this.SetPathBlocked(pos.x, pos.y, isBlocked);
		}

		// Token: 0x0600BA28 RID: 47656 RVA: 0x0044BA69 File Offset: 0x00449C69
		public void SetPathBlocked(int x, int y, bool isBlocked)
		{
			this.SetPathBlocked(x, y, isBlocked ? sbyte.MinValue : 0);
		}

		// Token: 0x0600BA29 RID: 47657 RVA: 0x0044BA7C File Offset: 0x00449C7C
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

		// Token: 0x0600BA2A RID: 47658 RVA: 0x0044BAE2 File Offset: 0x00449CE2
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPathBlocked(ref PathingUtils.Data data, int x, int y)
		{
			return (ulong)x >= (ulong)((long)data.pathingGridSize) || (ulong)y >= (ulong)((long)data.pathingGridSize) || data.pathingGrid[x + y * data.pathingGridSize] == sbyte.MinValue;
		}

		// Token: 0x0600BA2B RID: 47659 RVA: 0x0044BB18 File Offset: 0x00449D18
		public bool IsPointOnHighwayWorld(int x, int y)
		{
			int num = x / 10 + y / 10 * PathingUtils.worldBuilder.data.PathTileGridWidth;
			return PathingUtils.worldBuilder.data.PathTileGrid[num].TileState == PathTile.PathTileStates.Highway;
		}

		// Token: 0x0600BA2C RID: 47660 RVA: 0x0044BB5C File Offset: 0x00449D5C
		public void SetupPathingGrid()
		{
			this.data.pathingGridSize = PathingUtils.worldBuilder.WorldSize / 10;
			this.data.pathingGrid = new NativeArray<sbyte>(this.data.pathingGridSize * this.data.pathingGridSize, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x0600BA2D RID: 47661 RVA: 0x0044BBAC File Offset: 0x00449DAC
		public void Cleanup()
		{
			this.data.pathingGrid.Dispose();
			this.data.pathingGridSize = 0;
			this.data.nodePool.Cleanup();
			this.data.minHeapBinned.Cleanup();
			this.data.closedList.Dispose();
			this.pathTemp.Dispose();
		}

		// Token: 0x0600BA2F RID: 47663 RVA: 0x0044BCA4 File Offset: 0x00449EA4
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

		// Token: 0x0600BA30 RID: 47664 RVA: 0x0044C224 File Offset: 0x0044A424
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

		// Token: 0x0600BA31 RID: 47665 RVA: 0x0044C5D0 File Offset: 0x0044A7D0
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

		// Token: 0x0600BA32 RID: 47666 RVA: 0x0044C694 File Offset: 0x0044A894
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

		// Token: 0x0600BA33 RID: 47667 RVA: 0x0044C700 File Offset: 0x0044A900
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

		// Token: 0x0600BA34 RID: 47668 RVA: 0x0044C780 File Offset: 0x0044A980
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

		// Token: 0x04008AFF RID: 35583
		public const int PATHING_GRID_TILE_SIZE = 10;

		// Token: 0x04008B00 RID: 35584
		public const int stepSize = 10;

		// Token: 0x04008B01 RID: 35585
		public static readonly Vector2i stepHalf = new Vector2i(5, 5);

		// Token: 0x04008B02 RID: 35586
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cRoadCountryMaxStepH = 12f;

		// Token: 0x04008B03 RID: 35587
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cRoadHighwayMaxStepH = 11f;

		// Token: 0x04008B04 RID: 35588
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cHeightCostScale = 0.2f;

		// Token: 0x04008B05 RID: 35589
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cNeighborsCount = 8;

		// Token: 0x04008B06 RID: 35590
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

		// Token: 0x04008B07 RID: 35591
		[PublicizedFrom(EAccessModifier.Private)]
		public static WorldBuilder worldBuilder;

		// Token: 0x04008B08 RID: 35592
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeList<Vector2i> pathTemp = new NativeList<Vector2i>(200, Allocator.Persistent);

		// Token: 0x04008B09 RID: 35593
		[PublicizedFrom(EAccessModifier.Private)]
		public PathingUtils.Data data;

		// Token: 0x02001762 RID: 5986
		public struct Data
		{
			// Token: 0x04008B0A RID: 35594
			public int WorldSize;

			// Token: 0x04008B0B RID: 35595
			public NativeArray<sbyte> pathingGrid;

			// Token: 0x04008B0C RID: 35596
			public int pathingGridSize;

			// Token: 0x04008B0D RID: 35597
			public PathNodePool nodePool;

			// Token: 0x04008B0E RID: 35598
			public PathingUtils.MinHeapBinned minHeapBinned;

			// Token: 0x04008B0F RID: 35599
			public NativeArray<byte> closedList;

			// Token: 0x04008B10 RID: 35600
			public int closedListWidth;

			// Token: 0x04008B11 RID: 35601
			public int closedListMinY;

			// Token: 0x04008B12 RID: 35602
			public int closedListMaxY;
		}

		// Token: 0x02001763 RID: 5987
		public struct MinHeapBinned
		{
			// Token: 0x0600BA35 RID: 47669 RVA: 0x0044C7F4 File Offset: 0x0044A9F4
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

			// Token: 0x0600BA36 RID: 47670 RVA: 0x0044C830 File Offset: 0x0044AA30
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

			// Token: 0x0600BA37 RID: 47671 RVA: 0x0044C898 File Offset: 0x0044AA98
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

			// Token: 0x0600BA38 RID: 47672 RVA: 0x0044C948 File Offset: 0x0044AB48
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

			// Token: 0x0600BA39 RID: 47673 RVA: 0x0044CA42 File Offset: 0x0044AC42
			public void Cleanup()
			{
				this.nodeBins.Dispose();
			}

			// Token: 0x04008B13 RID: 35603
			[PublicizedFrom(EAccessModifier.Private)]
			public const int cBins = 32768;

			// Token: 0x04008B14 RID: 35604
			[PublicizedFrom(EAccessModifier.Private)]
			public const float cScale = 0.07f;

			// Token: 0x04008B15 RID: 35605
			[PublicizedFrom(EAccessModifier.Private)]
			public NativeArray<int> nodeBins;

			// Token: 0x04008B16 RID: 35606
			[PublicizedFrom(EAccessModifier.Private)]
			public int lowBin;

			// Token: 0x04008B17 RID: 35607
			[PublicizedFrom(EAccessModifier.Private)]
			public int highBin;
		}

		// Token: 0x02001764 RID: 5988
		[PublicizedFrom(EAccessModifier.Private)]
		public enum PathNodeType
		{
			// Token: 0x04008B19 RID: 35609
			Free,
			// Token: 0x04008B1A RID: 35610
			Road,
			// Token: 0x04008B1B RID: 35611
			Prefab,
			// Token: 0x04008B1C RID: 35612
			CityLimits = 4,
			// Token: 0x04008B1D RID: 35613
			Blocked = 8
		}

		// Token: 0x02001765 RID: 5989
		// (Invoke) Token: 0x0600BA3B RID: 47675
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate int FindDetailedPath_0000BA08$PostfixBurstDelegate(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver);

		// Token: 0x02001766 RID: 5990
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindDetailedPath_0000BA08$BurstDirectCall
		{
			// Token: 0x0600BA3E RID: 47678 RVA: 0x0044CA4F File Offset: 0x0044AC4F
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindDetailedPath$BurstManaged(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref Vector2i, bool, bool)).MethodHandle, typeof(PathingUtils.FindDetailedPath_0000BA08$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BA3F RID: 47679 RVA: 0x0044CA7C File Offset: 0x0044AC7C
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BA40 RID: 47680 RVA: 0x0044CA94 File Offset: 0x0044AC94
			public static void Constructor()
			{
				PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindDetailedPath(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref Vector2i, bool, bool)).MethodHandle);
			}

			// Token: 0x0600BA41 RID: 47681 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BA42 RID: 47682 RVA: 0x0044CAA5 File Offset: 0x0044ACA5
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindDetailedPath_0000BA08$BurstDirectCall()
			{
				PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BA43 RID: 47683 RVA: 0x0044CAAC File Offset: 0x0044ACAC
			public static int Invoke(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindDetailedPath_0000BA08$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(WorldGenerationEngineFinal.WorldBuilder/Data&,WorldGenerationEngineFinal.PathingUtils/Data&,Vector2i&,Vector2i&,System.Boolean,System.Boolean), ref wd, ref data, ref startPos, ref endPos, _isCountryRoad, _isRiver, functionPointer);
					}
				}
				return PathingUtils.FindDetailedPath$BurstManaged(ref wd, ref data, in startPos, in endPos, _isCountryRoad, _isRiver);
			}

			// Token: 0x04008B1E RID: 35614
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008B1F RID: 35615
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001767 RID: 5991
		// (Invoke) Token: 0x0600BA45 RID: 47685
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate int FindDetailedPath_0000BA0A$PostfixBurstDelegate(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver);

		// Token: 0x02001768 RID: 5992
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindDetailedPath_0000BA0A$BurstDirectCall
		{
			// Token: 0x0600BA48 RID: 47688 RVA: 0x0044CAEB File Offset: 0x0044ACEB
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindDetailedPath$BurstManaged(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref NativeList<Vector2i>, bool, bool)).MethodHandle, typeof(PathingUtils.FindDetailedPath_0000BA0A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BA49 RID: 47689 RVA: 0x0044CB18 File Offset: 0x0044AD18
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BA4A RID: 47690 RVA: 0x0044CB30 File Offset: 0x0044AD30
			public static void Constructor()
			{
				PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindDetailedPath(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref NativeList<Vector2i>, bool, bool)).MethodHandle);
			}

			// Token: 0x0600BA4B RID: 47691 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BA4C RID: 47692 RVA: 0x0044CB41 File Offset: 0x0044AD41
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindDetailedPath_0000BA0A$BurstDirectCall()
			{
				PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BA4D RID: 47693 RVA: 0x0044CB48 File Offset: 0x0044AD48
			public static int Invoke(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindDetailedPath_0000BA0A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(WorldGenerationEngineFinal.WorldBuilder/Data&,WorldGenerationEngineFinal.PathingUtils/Data&,Vector2i&,Unity.Collections.NativeList`1<Vector2i>&,System.Boolean,System.Boolean), ref wd, ref data, ref startPos, ref _endPath, _isCountryRoad, _isRiver, functionPointer);
					}
				}
				return PathingUtils.FindDetailedPath$BurstManaged(ref wd, ref data, in startPos, in _endPath, _isCountryRoad, _isRiver);
			}

			// Token: 0x04008B20 RID: 35616
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008B21 RID: 35617
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x02001769 RID: 5993
		// (Invoke) Token: 0x0600BA4F RID: 47695
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void CalcPathBounds_0000BA0B$PostfixBurstDelegate(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max);

		// Token: 0x0200176A RID: 5994
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class CalcPathBounds_0000BA0B$BurstDirectCall
		{
			// Token: 0x0600BA52 RID: 47698 RVA: 0x0044CB87 File Offset: 0x0044AD87
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.CalcPathBounds$BurstManaged(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle, typeof(PathingUtils.CalcPathBounds_0000BA0B$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BA53 RID: 47699 RVA: 0x0044CBB4 File Offset: 0x0044ADB4
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BA54 RID: 47700 RVA: 0x0044CBCC File Offset: 0x0044ADCC
			public static void Constructor()
			{
				PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.CalcPathBounds(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600BA55 RID: 47701 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BA56 RID: 47702 RVA: 0x0044CBDD File Offset: 0x0044ADDD
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static CalcPathBounds_0000BA0B$BurstDirectCall()
			{
				PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BA57 RID: 47703 RVA: 0x0044CBE4 File Offset: 0x0044ADE4
			public static void Invoke(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.CalcPathBounds_0000BA0B$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&,Vector2i&), ref _path, ref _min, ref _max, functionPointer);
						return;
					}
				}
				PathingUtils.CalcPathBounds$BurstManaged(in _path, out _min, out _max);
			}

			// Token: 0x04008B22 RID: 35618
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008B23 RID: 35619
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x0200176B RID: 5995
		// (Invoke) Token: 0x0600BA59 RID: 47705
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate float FindClosestPathPoint_0000BA0C$PostfixBurstDelegate(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1);

		// Token: 0x0200176C RID: 5996
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindClosestPathPoint_0000BA0C$BurstDirectCall
		{
			// Token: 0x0600BA5C RID: 47708 RVA: 0x0044CC19 File Offset: 0x0044AE19
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindClosestPathPoint$BurstManaged(ref NativeList<Vector2>, ref Vector2, ref Vector2, int)).MethodHandle, typeof(PathingUtils.FindClosestPathPoint_0000BA0C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BA5D RID: 47709 RVA: 0x0044CC48 File Offset: 0x0044AE48
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BA5E RID: 47710 RVA: 0x0044CC60 File Offset: 0x0044AE60
			public static void Constructor()
			{
				PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindClosestPathPoint(ref NativeList<Vector2>, ref Vector2, ref Vector2, int)).MethodHandle);
			}

			// Token: 0x0600BA5F RID: 47711 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BA60 RID: 47712 RVA: 0x0044CC71 File Offset: 0x0044AE71
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindClosestPathPoint_0000BA0C$BurstDirectCall()
			{
				PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BA61 RID: 47713 RVA: 0x0044CC78 File Offset: 0x0044AE78
			public static float Invoke(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindClosestPathPoint_0000BA0C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Unity.Collections.NativeList`1<UnityEngine.Vector2>&,UnityEngine.Vector2&,UnityEngine.Vector2&,System.Int32), ref _path, ref _startPos, ref _destPoint, _step, functionPointer);
					}
				}
				return PathingUtils.FindClosestPathPoint$BurstManaged(in _path, in _startPos, out _destPoint, _step);
			}

			// Token: 0x04008B24 RID: 35620
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008B25 RID: 35621
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x0200176D RID: 5997
		// (Invoke) Token: 0x0600BA63 RID: 47715
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate float FindClosestPathPoint_0000BA0D$PostfixBurstDelegate(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint);

		// Token: 0x0200176E RID: 5998
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindClosestPathPoint_0000BA0D$BurstDirectCall
		{
			// Token: 0x0600BA66 RID: 47718 RVA: 0x0044CCAF File Offset: 0x0044AEAF
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindClosestPathPoint$BurstManaged(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle, typeof(PathingUtils.FindClosestPathPoint_0000BA0D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BA67 RID: 47719 RVA: 0x0044CCDC File Offset: 0x0044AEDC
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BA68 RID: 47720 RVA: 0x0044CCF4 File Offset: 0x0044AEF4
			public static void Constructor()
			{
				PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindClosestPathPoint(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600BA69 RID: 47721 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BA6A RID: 47722 RVA: 0x0044CD05 File Offset: 0x0044AF05
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindClosestPathPoint_0000BA0D$BurstDirectCall()
			{
				PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BA6B RID: 47723 RVA: 0x0044CD0C File Offset: 0x0044AF0C
			public static float Invoke(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindClosestPathPoint_0000BA0D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&,Vector2i&), ref _path, ref _startPos, ref _destPoint, functionPointer);
					}
				}
				return PathingUtils.FindClosestPathPoint$BurstManaged(in _path, in _startPos, out _destPoint);
			}

			// Token: 0x04008B26 RID: 35622
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008B27 RID: 35623
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x0200176F RID: 5999
		// (Invoke) Token: 0x0600BA6D RID: 47725
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate bool IsPointOnPath_0000BA0E$PostfixBurstDelegate(in NativeList<Vector2i> _path, in Vector2i _point);

		// Token: 0x02001770 RID: 6000
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class IsPointOnPath_0000BA0E$BurstDirectCall
		{
			// Token: 0x0600BA70 RID: 47728 RVA: 0x0044CD41 File Offset: 0x0044AF41
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.IsPointOnPath$BurstManaged(ref NativeList<Vector2i>, ref Vector2i)).MethodHandle, typeof(PathingUtils.IsPointOnPath_0000BA0E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall.Pointer;
			}

			// Token: 0x0600BA71 RID: 47729 RVA: 0x0044CD70 File Offset: 0x0044AF70
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600BA72 RID: 47730 RVA: 0x0044CD88 File Offset: 0x0044AF88
			public static void Constructor()
			{
				PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.IsPointOnPath(ref NativeList<Vector2i>, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600BA73 RID: 47731 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600BA74 RID: 47732 RVA: 0x0044CD99 File Offset: 0x0044AF99
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static IsPointOnPath_0000BA0E$BurstDirectCall()
			{
				PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall.Constructor();
			}

			// Token: 0x0600BA75 RID: 47733 RVA: 0x0044CDA0 File Offset: 0x0044AFA0
			public static bool Invoke(in NativeList<Vector2i> _path, in Vector2i _point)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.IsPointOnPath_0000BA0E$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&), ref _path, ref _point, functionPointer);
					}
				}
				return PathingUtils.IsPointOnPath$BurstManaged(in _path, in _point);
			}

			// Token: 0x04008B28 RID: 35624
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008B29 RID: 35625
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
