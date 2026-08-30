using System;
using System.Runtime.CompilerServices;
using Unity.Burst;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

namespace WorldGenerationEngineFinal
{
	// Token: 0x020016DE RID: 5854
	[BurstCompile(CompileSynchronously = true)]
	public class PathingUtils
	{
		// Token: 0x0600B61E RID: 46622 RVA: 0x0043D775 File Offset: 0x0043B975
		public PathingUtils(WorldBuilder _worldBuilder)
		{
			PathingUtils.worldBuilder = _worldBuilder;
			this.data.nodePool = new PathNodePool(100000);
		}

		// Token: 0x0600B61F RID: 46623 RVA: 0x0043D7B0 File Offset: 0x0043B9B0
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

		// Token: 0x0600B620 RID: 46624 RVA: 0x0043D82C File Offset: 0x0043BA2C
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

		// Token: 0x0600B621 RID: 46625 RVA: 0x0043D8D8 File Offset: 0x0043BAD8
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

		// Token: 0x0600B622 RID: 46626 RVA: 0x0043D9B2 File Offset: 0x0043BBB2
		[PublicizedFrom(EAccessModifier.Private)]
		public void InitPathData()
		{
			this.data.WorldSize = PathingUtils.worldBuilder.WorldSize;
		}

		// Token: 0x0600B623 RID: 46627 RVA: 0x0043D9CC File Offset: 0x0043BBCC
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

		// Token: 0x0600B624 RID: 46628 RVA: 0x0043DA48 File Offset: 0x0043BC48
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

		// Token: 0x0600B625 RID: 46629 RVA: 0x0043DAD2 File Offset: 0x0043BCD2
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static int FindDetailedPath(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver)
		{
			return PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall.Invoke(ref wd, ref data, in startPos, in endPos, _isCountryRoad, _isRiver);
		}

		// Token: 0x0600B626 RID: 46630 RVA: 0x0043DAE4 File Offset: 0x0043BCE4
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DiagonalDist(Vector2i v1, Vector2i v2)
		{
			float num = Utils.FastAbs((float)(v1.x - v2.x));
			float num2 = Utils.FastAbs((float)(v1.y - v2.y));
			return num + num2 + -0.585786f * Utils.FastMin(num, num2);
		}

		// Token: 0x0600B627 RID: 46631 RVA: 0x0043DB2A File Offset: 0x0043BD2A
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static int FindDetailedPath(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver)
		{
			return PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall.Invoke(ref wd, ref data, in startPos, in _endPath, _isCountryRoad, _isRiver);
		}

		// Token: 0x0600B628 RID: 46632 RVA: 0x0043DB39 File Offset: 0x0043BD39
		[BurstCompile(CompileSynchronously = true)]
		[PublicizedFrom(EAccessModifier.Private)]
		public static void CalcPathBounds(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max)
		{
			PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall.Invoke(in _path, out _min, out _max);
		}

		// Token: 0x0600B629 RID: 46633 RVA: 0x0043DB43 File Offset: 0x0043BD43
		[BurstCompile(CompileSynchronously = true)]
		public static float FindClosestPathPoint(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1)
		{
			return PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall.Invoke(in _path, in _startPos, out _destPoint, _step);
		}

		// Token: 0x0600B62A RID: 46634 RVA: 0x0043DB4E File Offset: 0x0043BD4E
		[BurstCompile(CompileSynchronously = true)]
		public static float FindClosestPathPoint(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint)
		{
			return PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall.Invoke(in _path, in _startPos, out _destPoint);
		}

		// Token: 0x0600B62B RID: 46635 RVA: 0x0043DB58 File Offset: 0x0043BD58
		[BurstCompile(CompileSynchronously = true)]
		public static bool IsPointOnPath(in NativeList<Vector2i> _path, in Vector2i _point)
		{
			return PathingUtils.IsPointOnPath_0000B620$BurstDirectCall.Invoke(in _path, in _point);
		}

		// Token: 0x0600B62C RID: 46636 RVA: 0x0043DB64 File Offset: 0x0043BD64
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool IsBlocked(ref WorldBuilder.Data wd, int pathX, int pathY, bool isRiver = false)
		{
			Vector2i vector2i = PathingUtils.pathPositionToWorldCenter(pathX, pathY);
			return !wd.InWorldBounds(vector2i.x, vector2i.y) || wd.GetStreetTileDataWorld(vector2i.x, vector2i.y).IsCity || (!isRiver && PathingUtils.IsWater(ref wd, pathX, pathY));
		}

		// Token: 0x0600B62D RID: 46637 RVA: 0x0043DBBA File Offset: 0x0043BDBA
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool InBounds(ref WorldBuilder.Data wd, Vector2i pos)
		{
			return PathingUtils.InBounds(ref wd, pos.x, pos.y);
		}

		// Token: 0x0600B62E RID: 46638 RVA: 0x0043DBD0 File Offset: 0x0043BDD0
		public static bool InBounds(ref WorldBuilder.Data wd, int pathX, int pathY)
		{
			Vector2i vector2i = PathingUtils.pathPositionToWorldCenter(pathX, pathY);
			return (ulong)vector2i.x < (ulong)((long)wd.WorldSize) && (ulong)vector2i.y < (ulong)((long)wd.WorldSize);
		}

		// Token: 0x0600B62F RID: 46639 RVA: 0x0043DC08 File Offset: 0x0043BE08
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

		// Token: 0x0600B630 RID: 46640 RVA: 0x0043DC76 File Offset: 0x0043BE76
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetHeight(ref WorldBuilder.Data wd, Vector2i pos)
		{
			return PathingUtils.GetHeight(ref wd, pos.x, pos.y);
		}

		// Token: 0x0600B631 RID: 46641 RVA: 0x0043DC8A File Offset: 0x0043BE8A
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetHeight(ref WorldBuilder.Data wd, int pathX, int pathY)
		{
			return wd.GetHeight(PathingUtils.pathPositionToWorldCenter(pathX, pathY));
		}

		// Token: 0x0600B632 RID: 46642 RVA: 0x0043DC99 File Offset: 0x0043BE99
		public BiomeType GetBiome(Vector2i pos)
		{
			return this.GetBiome(pos.x, pos.y);
		}

		// Token: 0x0600B633 RID: 46643 RVA: 0x0043DCAD File Offset: 0x0043BEAD
		public BiomeType GetBiome(int pathX, int pathY)
		{
			return PathingUtils.worldBuilder.GetBiome(PathingUtils.pathPositionToWorldCenter(pathX, pathY));
		}

		// Token: 0x0600B634 RID: 46644 RVA: 0x0043DCC0 File Offset: 0x0043BEC0
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i pathPositionToWorldCenter(int pathX, int pathY)
		{
			Vector2i vector2i;
			vector2i.x = pathX * 10 + 5;
			vector2i.y = pathY * 10 + 5;
			return vector2i;
		}

		// Token: 0x0600B635 RID: 46645 RVA: 0x0043DCE8 File Offset: 0x0043BEE8
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2i pathPositionToWorldMin(int pathX, int pathY)
		{
			Vector2i vector2i;
			vector2i.x = pathX * 10;
			vector2i.y = pathY * 10;
			return vector2i;
		}

		// Token: 0x0600B636 RID: 46646 RVA: 0x0043DD0C File Offset: 0x0043BF0C
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

		// Token: 0x0600B637 RID: 46647 RVA: 0x0043DD64 File Offset: 0x0043BF64
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

		// Token: 0x0600B638 RID: 46648 RVA: 0x0043DDB4 File Offset: 0x0043BFB4
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

		// Token: 0x0600B639 RID: 46649 RVA: 0x0043DE10 File Offset: 0x0043C010
		public void SetPathBlocked(Vector2i pos, bool isBlocked)
		{
			this.SetPathBlocked(pos.x, pos.y, isBlocked);
		}

		// Token: 0x0600B63A RID: 46650 RVA: 0x0043DE25 File Offset: 0x0043C025
		public void SetPathBlocked(int x, int y, bool isBlocked)
		{
			this.SetPathBlocked(x, y, isBlocked ? sbyte.MinValue : 0);
		}

		// Token: 0x0600B63B RID: 46651 RVA: 0x0043DE38 File Offset: 0x0043C038
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

		// Token: 0x0600B63C RID: 46652 RVA: 0x0043DE9E File Offset: 0x0043C09E
		[PublicizedFrom(EAccessModifier.Private)]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsPathBlocked(ref PathingUtils.Data data, int x, int y)
		{
			return (ulong)x >= (ulong)((long)data.pathingGridSize) || (ulong)y >= (ulong)((long)data.pathingGridSize) || data.pathingGrid[x + y * data.pathingGridSize] == sbyte.MinValue;
		}

		// Token: 0x0600B63D RID: 46653 RVA: 0x0043DED4 File Offset: 0x0043C0D4
		public bool IsPointOnHighwayWorld(int x, int y)
		{
			int num = x / 10 + y / 10 * PathingUtils.worldBuilder.data.PathTileGridWidth;
			return PathingUtils.worldBuilder.data.PathTileGrid[num].TileState == PathTile.PathTileStates.Highway;
		}

		// Token: 0x0600B63E RID: 46654 RVA: 0x0043DF18 File Offset: 0x0043C118
		public void SetupPathingGrid()
		{
			this.data.pathingGridSize = PathingUtils.worldBuilder.WorldSize / 10;
			this.data.pathingGrid = new NativeArray<sbyte>(this.data.pathingGridSize * this.data.pathingGridSize, Allocator.Persistent, NativeArrayOptions.ClearMemory);
		}

		// Token: 0x0600B63F RID: 46655 RVA: 0x0043DF68 File Offset: 0x0043C168
		public void Cleanup()
		{
			this.data.pathingGrid.Dispose();
			this.data.pathingGridSize = 0;
			this.data.nodePool.Cleanup();
			this.data.minHeapBinned.Cleanup();
			this.data.closedList.Dispose();
			this.pathTemp.Dispose();
		}

		// Token: 0x0600B641 RID: 46657 RVA: 0x0043E060 File Offset: 0x0043C260
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

		// Token: 0x0600B642 RID: 46658 RVA: 0x0043E5E0 File Offset: 0x0043C7E0
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

		// Token: 0x0600B643 RID: 46659 RVA: 0x0043E98C File Offset: 0x0043CB8C
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

		// Token: 0x0600B644 RID: 46660 RVA: 0x0043EA50 File Offset: 0x0043CC50
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

		// Token: 0x0600B645 RID: 46661 RVA: 0x0043EABC File Offset: 0x0043CCBC
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

		// Token: 0x0600B646 RID: 46662 RVA: 0x0043EB3C File Offset: 0x0043CD3C
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

		// Token: 0x04008835 RID: 34869
		public const int PATHING_GRID_TILE_SIZE = 10;

		// Token: 0x04008836 RID: 34870
		public const int stepSize = 10;

		// Token: 0x04008837 RID: 34871
		public static readonly Vector2i stepHalf = new Vector2i(5, 5);

		// Token: 0x04008838 RID: 34872
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cRoadCountryMaxStepH = 12f;

		// Token: 0x04008839 RID: 34873
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cRoadHighwayMaxStepH = 11f;

		// Token: 0x0400883A RID: 34874
		[PublicizedFrom(EAccessModifier.Private)]
		public const float cHeightCostScale = 0.2f;

		// Token: 0x0400883B RID: 34875
		[PublicizedFrom(EAccessModifier.Private)]
		public const int cNeighborsCount = 8;

		// Token: 0x0400883C RID: 34876
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

		// Token: 0x0400883D RID: 34877
		[PublicizedFrom(EAccessModifier.Private)]
		public static WorldBuilder worldBuilder;

		// Token: 0x0400883E RID: 34878
		[PublicizedFrom(EAccessModifier.Private)]
		public NativeList<Vector2i> pathTemp = new NativeList<Vector2i>(200, Allocator.Persistent);

		// Token: 0x0400883F RID: 34879
		[PublicizedFrom(EAccessModifier.Private)]
		public PathingUtils.Data data;

		// Token: 0x020016DF RID: 5855
		public struct Data
		{
			// Token: 0x04008840 RID: 34880
			public int WorldSize;

			// Token: 0x04008841 RID: 34881
			public NativeArray<sbyte> pathingGrid;

			// Token: 0x04008842 RID: 34882
			public int pathingGridSize;

			// Token: 0x04008843 RID: 34883
			public PathNodePool nodePool;

			// Token: 0x04008844 RID: 34884
			public PathingUtils.MinHeapBinned minHeapBinned;

			// Token: 0x04008845 RID: 34885
			public NativeArray<byte> closedList;

			// Token: 0x04008846 RID: 34886
			public int closedListWidth;

			// Token: 0x04008847 RID: 34887
			public int closedListMinY;

			// Token: 0x04008848 RID: 34888
			public int closedListMaxY;
		}

		// Token: 0x020016E0 RID: 5856
		public struct MinHeapBinned
		{
			// Token: 0x0600B647 RID: 46663 RVA: 0x0043EBB0 File Offset: 0x0043CDB0
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

			// Token: 0x0600B648 RID: 46664 RVA: 0x0043EBEC File Offset: 0x0043CDEC
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

			// Token: 0x0600B649 RID: 46665 RVA: 0x0043EC54 File Offset: 0x0043CE54
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

			// Token: 0x0600B64A RID: 46666 RVA: 0x0043ED04 File Offset: 0x0043CF04
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

			// Token: 0x0600B64B RID: 46667 RVA: 0x0043EDFE File Offset: 0x0043CFFE
			public void Cleanup()
			{
				this.nodeBins.Dispose();
			}

			// Token: 0x04008849 RID: 34889
			[PublicizedFrom(EAccessModifier.Private)]
			public const int cBins = 32768;

			// Token: 0x0400884A RID: 34890
			[PublicizedFrom(EAccessModifier.Private)]
			public const float cScale = 0.07f;

			// Token: 0x0400884B RID: 34891
			[PublicizedFrom(EAccessModifier.Private)]
			public NativeArray<int> nodeBins;

			// Token: 0x0400884C RID: 34892
			[PublicizedFrom(EAccessModifier.Private)]
			public int lowBin;

			// Token: 0x0400884D RID: 34893
			[PublicizedFrom(EAccessModifier.Private)]
			public int highBin;
		}

		// Token: 0x020016E1 RID: 5857
		[PublicizedFrom(EAccessModifier.Private)]
		public enum PathNodeType
		{
			// Token: 0x0400884F RID: 34895
			Free,
			// Token: 0x04008850 RID: 34896
			Road,
			// Token: 0x04008851 RID: 34897
			Prefab,
			// Token: 0x04008852 RID: 34898
			CityLimits = 4,
			// Token: 0x04008853 RID: 34899
			Blocked = 8
		}

		// Token: 0x020016E2 RID: 5858
		// (Invoke) Token: 0x0600B64D RID: 46669
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate int FindDetailedPath_0000B61A$PostfixBurstDelegate(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver);

		// Token: 0x020016E3 RID: 5859
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindDetailedPath_0000B61A$BurstDirectCall
		{
			// Token: 0x0600B650 RID: 46672 RVA: 0x0043EE0B File Offset: 0x0043D00B
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindDetailedPath$BurstManaged(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref Vector2i, bool, bool)).MethodHandle, typeof(PathingUtils.FindDetailedPath_0000B61A$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B651 RID: 46673 RVA: 0x0043EE38 File Offset: 0x0043D038
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B652 RID: 46674 RVA: 0x0043EE50 File Offset: 0x0043D050
			public static void Constructor()
			{
				PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindDetailedPath(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref Vector2i, bool, bool)).MethodHandle);
			}

			// Token: 0x0600B653 RID: 46675 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B654 RID: 46676 RVA: 0x0043EE61 File Offset: 0x0043D061
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindDetailedPath_0000B61A$BurstDirectCall()
			{
				PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B655 RID: 46677 RVA: 0x0043EE68 File Offset: 0x0043D068
			public static int Invoke(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in Vector2i endPos, bool _isCountryRoad, bool _isRiver)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindDetailedPath_0000B61A$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(WorldGenerationEngineFinal.WorldBuilder/Data&,WorldGenerationEngineFinal.PathingUtils/Data&,Vector2i&,Vector2i&,System.Boolean,System.Boolean), ref wd, ref data, ref startPos, ref endPos, _isCountryRoad, _isRiver, functionPointer);
					}
				}
				return PathingUtils.FindDetailedPath$BurstManaged(ref wd, ref data, in startPos, in endPos, _isCountryRoad, _isRiver);
			}

			// Token: 0x04008854 RID: 34900
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008855 RID: 34901
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x020016E4 RID: 5860
		// (Invoke) Token: 0x0600B657 RID: 46679
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate int FindDetailedPath_0000B61C$PostfixBurstDelegate(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver);

		// Token: 0x020016E5 RID: 5861
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindDetailedPath_0000B61C$BurstDirectCall
		{
			// Token: 0x0600B65A RID: 46682 RVA: 0x0043EEA7 File Offset: 0x0043D0A7
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindDetailedPath$BurstManaged(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref NativeList<Vector2i>, bool, bool)).MethodHandle, typeof(PathingUtils.FindDetailedPath_0000B61C$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B65B RID: 46683 RVA: 0x0043EED4 File Offset: 0x0043D0D4
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B65C RID: 46684 RVA: 0x0043EEEC File Offset: 0x0043D0EC
			public static void Constructor()
			{
				PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindDetailedPath(ref WorldBuilder.Data, ref PathingUtils.Data, ref Vector2i, ref NativeList<Vector2i>, bool, bool)).MethodHandle);
			}

			// Token: 0x0600B65D RID: 46685 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B65E RID: 46686 RVA: 0x0043EEFD File Offset: 0x0043D0FD
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindDetailedPath_0000B61C$BurstDirectCall()
			{
				PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B65F RID: 46687 RVA: 0x0043EF04 File Offset: 0x0043D104
			public static int Invoke(ref WorldBuilder.Data wd, ref PathingUtils.Data data, in Vector2i startPos, in NativeList<Vector2i> _endPath, bool _isCountryRoad, bool _isRiver)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindDetailedPath_0000B61C$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Int32(WorldGenerationEngineFinal.WorldBuilder/Data&,WorldGenerationEngineFinal.PathingUtils/Data&,Vector2i&,Unity.Collections.NativeList`1<Vector2i>&,System.Boolean,System.Boolean), ref wd, ref data, ref startPos, ref _endPath, _isCountryRoad, _isRiver, functionPointer);
					}
				}
				return PathingUtils.FindDetailedPath$BurstManaged(ref wd, ref data, in startPos, in _endPath, _isCountryRoad, _isRiver);
			}

			// Token: 0x04008856 RID: 34902
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008857 RID: 34903
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x020016E6 RID: 5862
		// (Invoke) Token: 0x0600B661 RID: 46689
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate void CalcPathBounds_0000B61D$PostfixBurstDelegate(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max);

		// Token: 0x020016E7 RID: 5863
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class CalcPathBounds_0000B61D$BurstDirectCall
		{
			// Token: 0x0600B664 RID: 46692 RVA: 0x0043EF43 File Offset: 0x0043D143
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.CalcPathBounds$BurstManaged(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle, typeof(PathingUtils.CalcPathBounds_0000B61D$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B665 RID: 46693 RVA: 0x0043EF70 File Offset: 0x0043D170
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B666 RID: 46694 RVA: 0x0043EF88 File Offset: 0x0043D188
			public static void Constructor()
			{
				PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.CalcPathBounds(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600B667 RID: 46695 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B668 RID: 46696 RVA: 0x0043EF99 File Offset: 0x0043D199
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static CalcPathBounds_0000B61D$BurstDirectCall()
			{
				PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B669 RID: 46697 RVA: 0x0043EFA0 File Offset: 0x0043D1A0
			public static void Invoke(in NativeList<Vector2i> _path, out Vector2i _min, out Vector2i _max)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.CalcPathBounds_0000B61D$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						calli(System.Void(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&,Vector2i&), ref _path, ref _min, ref _max, functionPointer);
						return;
					}
				}
				PathingUtils.CalcPathBounds$BurstManaged(in _path, out _min, out _max);
			}

			// Token: 0x04008858 RID: 34904
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x04008859 RID: 34905
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x020016E8 RID: 5864
		// (Invoke) Token: 0x0600B66B RID: 46699
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate float FindClosestPathPoint_0000B61E$PostfixBurstDelegate(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1);

		// Token: 0x020016E9 RID: 5865
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindClosestPathPoint_0000B61E$BurstDirectCall
		{
			// Token: 0x0600B66E RID: 46702 RVA: 0x0043EFD5 File Offset: 0x0043D1D5
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindClosestPathPoint$BurstManaged(ref NativeList<Vector2>, ref Vector2, ref Vector2, int)).MethodHandle, typeof(PathingUtils.FindClosestPathPoint_0000B61E$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B66F RID: 46703 RVA: 0x0043F004 File Offset: 0x0043D204
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B670 RID: 46704 RVA: 0x0043F01C File Offset: 0x0043D21C
			public static void Constructor()
			{
				PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindClosestPathPoint(ref NativeList<Vector2>, ref Vector2, ref Vector2, int)).MethodHandle);
			}

			// Token: 0x0600B671 RID: 46705 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B672 RID: 46706 RVA: 0x0043F02D File Offset: 0x0043D22D
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindClosestPathPoint_0000B61E$BurstDirectCall()
			{
				PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B673 RID: 46707 RVA: 0x0043F034 File Offset: 0x0043D234
			public static float Invoke(in NativeList<Vector2> _path, in Vector2 _startPos, out Vector2 _destPoint, int _step = 1)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindClosestPathPoint_0000B61E$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Unity.Collections.NativeList`1<UnityEngine.Vector2>&,UnityEngine.Vector2&,UnityEngine.Vector2&,System.Int32), ref _path, ref _startPos, ref _destPoint, _step, functionPointer);
					}
				}
				return PathingUtils.FindClosestPathPoint$BurstManaged(in _path, in _startPos, out _destPoint, _step);
			}

			// Token: 0x0400885A RID: 34906
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x0400885B RID: 34907
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x020016EA RID: 5866
		// (Invoke) Token: 0x0600B675 RID: 46709
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate float FindClosestPathPoint_0000B61F$PostfixBurstDelegate(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint);

		// Token: 0x020016EB RID: 5867
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class FindClosestPathPoint_0000B61F$BurstDirectCall
		{
			// Token: 0x0600B678 RID: 46712 RVA: 0x0043F06B File Offset: 0x0043D26B
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.FindClosestPathPoint$BurstManaged(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle, typeof(PathingUtils.FindClosestPathPoint_0000B61F$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B679 RID: 46713 RVA: 0x0043F098 File Offset: 0x0043D298
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B67A RID: 46714 RVA: 0x0043F0B0 File Offset: 0x0043D2B0
			public static void Constructor()
			{
				PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.FindClosestPathPoint(ref NativeList<Vector2i>, ref Vector2i, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600B67B RID: 46715 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B67C RID: 46716 RVA: 0x0043F0C1 File Offset: 0x0043D2C1
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static FindClosestPathPoint_0000B61F$BurstDirectCall()
			{
				PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B67D RID: 46717 RVA: 0x0043F0C8 File Offset: 0x0043D2C8
			public static float Invoke(in NativeList<Vector2i> _path, in Vector2i _startPos, out Vector2i _destPoint)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.FindClosestPathPoint_0000B61F$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Single(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&,Vector2i&), ref _path, ref _startPos, ref _destPoint, functionPointer);
					}
				}
				return PathingUtils.FindClosestPathPoint$BurstManaged(in _path, in _startPos, out _destPoint);
			}

			// Token: 0x0400885C RID: 34908
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x0400885D RID: 34909
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}

		// Token: 0x020016EC RID: 5868
		// (Invoke) Token: 0x0600B67F RID: 46719
		[PublicizedFrom(EAccessModifier.Internal)]
		public delegate bool IsPointOnPath_0000B620$PostfixBurstDelegate(in NativeList<Vector2i> _path, in Vector2i _point);

		// Token: 0x020016ED RID: 5869
		[PublicizedFrom(EAccessModifier.Internal)]
		public static class IsPointOnPath_0000B620$BurstDirectCall
		{
			// Token: 0x0600B682 RID: 46722 RVA: 0x0043F0FD File Offset: 0x0043D2FD
			[BurstDiscard]
			[PublicizedFrom(EAccessModifier.Private)]
			public static void GetFunctionPointerDiscard(ref IntPtr A_0)
			{
				if (PathingUtils.IsPointOnPath_0000B620$BurstDirectCall.Pointer == 0)
				{
					PathingUtils.IsPointOnPath_0000B620$BurstDirectCall.Pointer = BurstCompiler.GetILPPMethodFunctionPointer2(PathingUtils.IsPointOnPath_0000B620$BurstDirectCall.DeferredCompilation, methodof(PathingUtils.IsPointOnPath$BurstManaged(ref NativeList<Vector2i>, ref Vector2i)).MethodHandle, typeof(PathingUtils.IsPointOnPath_0000B620$PostfixBurstDelegate).TypeHandle);
				}
				A_0 = PathingUtils.IsPointOnPath_0000B620$BurstDirectCall.Pointer;
			}

			// Token: 0x0600B683 RID: 46723 RVA: 0x0043F12C File Offset: 0x0043D32C
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr GetFunctionPointer()
			{
				IntPtr intPtr = (IntPtr)0;
				PathingUtils.IsPointOnPath_0000B620$BurstDirectCall.GetFunctionPointerDiscard(ref intPtr);
				return intPtr;
			}

			// Token: 0x0600B684 RID: 46724 RVA: 0x0043F144 File Offset: 0x0043D344
			public static void Constructor()
			{
				PathingUtils.IsPointOnPath_0000B620$BurstDirectCall.DeferredCompilation = BurstCompiler.CompileILPPMethod2(methodof(PathingUtils.IsPointOnPath(ref NativeList<Vector2i>, ref Vector2i)).MethodHandle);
			}

			// Token: 0x0600B685 RID: 46725 RVA: 0x000027FC File Offset: 0x000009FC
			public static void Initialize()
			{
			}

			// Token: 0x0600B686 RID: 46726 RVA: 0x0043F155 File Offset: 0x0043D355
			// Note: this type is marked as 'beforefieldinit'.
			[PublicizedFrom(EAccessModifier.Private)]
			static IsPointOnPath_0000B620$BurstDirectCall()
			{
				PathingUtils.IsPointOnPath_0000B620$BurstDirectCall.Constructor();
			}

			// Token: 0x0600B687 RID: 46727 RVA: 0x0043F15C File Offset: 0x0043D35C
			public static bool Invoke(in NativeList<Vector2i> _path, in Vector2i _point)
			{
				if (BurstCompiler.IsEnabled)
				{
					IntPtr functionPointer = PathingUtils.IsPointOnPath_0000B620$BurstDirectCall.GetFunctionPointer();
					if (functionPointer != 0)
					{
						return calli(System.Boolean(Unity.Collections.NativeList`1<Vector2i>&,Vector2i&), ref _path, ref _point, functionPointer);
					}
				}
				return PathingUtils.IsPointOnPath$BurstManaged(in _path, in _point);
			}

			// Token: 0x0400885E RID: 34910
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr Pointer;

			// Token: 0x0400885F RID: 34911
			[PublicizedFrom(EAccessModifier.Private)]
			public static IntPtr DeferredCompilation;
		}
	}
}
