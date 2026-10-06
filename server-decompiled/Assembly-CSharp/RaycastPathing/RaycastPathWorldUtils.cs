using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018FE RID: 6398
	[Preserve]
	public class RaycastPathWorldUtils
	{
		// Token: 0x0600C565 RID: 50533 RVA: 0x004893E8 File Offset: 0x004875E8
		public static Vector3i GetBlockPosition(Vector3 worldPos)
		{
			Vector3i vector3i = new Vector3i(worldPos);
			Vector3 vector = worldPos - vector3i.ToVector3Center();
			return vector3i + Vector3i.FromVector3Rounded(vector);
		}

		// Token: 0x0600C566 RID: 50534 RVA: 0x00489417 File Offset: 0x00487617
		public static Vector3 GetCenterPosition(Vector3 worldPos, float scale = 1f)
		{
			return worldPos + Vector3.one * 0.5f * scale;
		}

		// Token: 0x0600C567 RID: 50535 RVA: 0x00489434 File Offset: 0x00487634
		public static bool IsConfinedSpace(World world, Vector3 point, float dist, bool debugDraw = false)
		{
			int num = 0;
			List<RaycastNode> list = RaycastPathWorldUtils.ScanBlocksAround(world, point, dist, debugDraw);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].nodeType == cPathNodeType.Air)
				{
					num++;
				}
			}
			return (float)num < dist * dist;
		}

		// Token: 0x0600C568 RID: 50536 RVA: 0x0048947B File Offset: 0x0048767B
		public static bool IsUnderground(Vector3 target)
		{
			return Physics.Raycast(new Ray(target + RaycastPathWorldUtils.underGroundTestOffset - Origin.position, Vector3.down), RaycastPathWorldUtils.underGroundTestOffset.y, 268500992);
		}

		// Token: 0x0600C569 RID: 50537 RVA: 0x004894B0 File Offset: 0x004876B0
		public static bool isPosUnderWater(World world, Vector3 pos)
		{
			Vector3i blockPosition = RaycastPathWorldUtils.GetBlockPosition(pos);
			return world.GetBlock(blockPosition).type == 240;
		}

		// Token: 0x0600C56A RID: 50538 RVA: 0x004894DC File Offset: 0x004876DC
		public static cPathNodeType getBlockType(World world, RaycastNode current, Vector3 adjacentCenter)
		{
			Vector3i vector3i = World.worldToBlockPos(adjacentCenter);
			BlockValue block = world.GetBlock(vector3i);
			if (block.Block.shape.IsSolidSpace || block.Block.shape.IsSolidCube)
			{
				return cPathNodeType.Solid;
			}
			cPathNodeType cPathNodeType;
			if (block.isair)
			{
				cPathNodeType = cPathNodeType.Air;
			}
			else if (block.Block.HasTag(BlockTags.Door))
			{
				cPathNodeType = cPathNodeType.Door;
			}
			else if (RaycastPathWorldUtils.HasSubBlocks(world, current, adjacentCenter, false, 0f))
			{
				cPathNodeType = cPathNodeType.Half;
			}
			else
			{
				cPathNodeType = cPathNodeType.Solid;
			}
			return cPathNodeType;
		}

		// Token: 0x0600C56B RID: 50539 RVA: 0x0048955C File Offset: 0x0048775C
		public static cPathNodeType getBlockType(World world, Vector3i pos)
		{
			BlockValue block = world.GetBlock(pos);
			cPathNodeType cPathNodeType;
			if (block.isair)
			{
				cPathNodeType = cPathNodeType.Air;
			}
			else if (block.Block.HasTag(BlockTags.Door))
			{
				cPathNodeType = cPathNodeType.Door;
			}
			else
			{
				cPathNodeType = cPathNodeType.Solid;
			}
			return cPathNodeType;
		}

		// Token: 0x0600C56C RID: 50540 RVA: 0x00489598 File Offset: 0x00487798
		public static List<RaycastNode> ScanBlocksAround(World world, Vector3 point, float dist, bool debugDraw = false)
		{
			List<RaycastNode> list = new List<RaycastNode>();
			List<RaycastNode> list2 = new List<RaycastNode>();
			RaycastNode raycastNode = new RaycastNode(RaycastPathWorldUtils.GetBlockPosition(point), 1f, 0);
			list.Add(raycastNode);
			if (debugDraw)
			{
				RaycastPathUtils.DrawBounds(raycastNode.Center, Color.yellow, 0.95f, 1f);
			}
			while (list.Count > 0)
			{
				RaycastNode raycastNode2 = list[0];
				list.RemoveAt(0);
				list2.Add(raycastNode2);
				for (int i = 0; i < RaycastPathWorldUtils.mainBlockAxis.Length; i++)
				{
					Vector3 pos = raycastNode2.Center + RaycastPathWorldUtils.mainBlockAxis[i];
					float magnitude = (raycastNode.Center - pos).magnitude;
					if (list.Find((RaycastNode n) => n.Center == pos) == null && list2.Find((RaycastNode n) => n.Center == pos) == null && magnitude < dist * 0.5f)
					{
						RaycastNode raycastNode3 = new RaycastNode(pos, 1f, 0);
						raycastNode3.SetParent(raycastNode2);
						cPathNodeType blockType = RaycastPathWorldUtils.getBlockType(world, raycastNode3.BlockPos);
						raycastNode3.SetType(blockType);
						list.Add(raycastNode3);
						if (debugDraw)
						{
							if (blockType != cPathNodeType.Air)
							{
								if (blockType == cPathNodeType.Solid)
								{
									RaycastPathUtils.DrawBounds(raycastNode3.Center, Color.red, 1f, 0.95f);
								}
							}
							else
							{
								RaycastPathUtils.DrawBounds(raycastNode3.Center, Color.cyan, 1f, 0.95f);
							}
						}
					}
				}
			}
			return list2;
		}

		// Token: 0x0600C56D RID: 50541 RVA: 0x0048973C File Offset: 0x0048793C
		public static List<RaycastNode> ScanVolume(World world, Vector3 pos, bool useTarget = false, bool useDiagnols = false, bool debugDraw = false, float duration = 0f)
		{
			List<RaycastNode> list = new List<RaycastNode>();
			RaycastNode raycastNode = new RaycastNode(pos, 1f, 0);
			if (useTarget)
			{
				if (world.GetBlock(raycastNode.BlockPos).isair)
				{
					raycastNode.SetType(cPathNodeType.Air);
				}
				list.Add(raycastNode);
			}
			for (int i = 0; i < RaycastPathWorldUtils.mainBlockAxis.Length; i++)
			{
				RaycastNode raycastNode2 = RaycastPathWorldUtils.InitNeighborNode(world, raycastNode, raycastNode.BlockPos + RaycastPathWorldUtils.mainBlockAxis[i], debugDraw, duration);
				list.Add(raycastNode2);
			}
			if (useDiagnols)
			{
				for (int j = 0; j < RaycastPathWorldUtils.diagonalBlockAxis.Length; j++)
				{
					Vector3i vector3i = raycastNode.BlockPos + RaycastPathWorldUtils.diagonalBlockAxis[j];
					RaycastNode raycastNode3 = RaycastPathWorldUtils.InitNeighborNode(world, raycastNode, vector3i, debugDraw, duration);
					list.Add(raycastNode3);
				}
			}
			if (debugDraw)
			{
				for (int k = 0; k < list.Count; k++)
				{
					RaycastPathUtils.DrawNode(list[k], Color.yellow, duration);
				}
			}
			return list;
		}

		// Token: 0x0600C56E RID: 50542 RVA: 0x00489838 File Offset: 0x00487A38
		public static RaycastNode FindNodeType(List<RaycastNode> nodes, cPathNodeType targetType = cPathNodeType.Air)
		{
			RaycastNode raycastNode = null;
			for (int i = 0; i < nodes.Count; i++)
			{
				if (nodes[i].nodeType == targetType)
				{
					return nodes[i];
				}
			}
			return raycastNode;
		}

		// Token: 0x0600C56F RID: 50543 RVA: 0x00489870 File Offset: 0x00487A70
		[PublicizedFrom(EAccessModifier.Private)]
		public static RaycastNode InitNeighborNode(World world, RaycastNode current, Vector3i adjacentPos, bool debugDraw = false, float duration = 0f)
		{
			Vector3 vector = World.blockToTransformPos(adjacentPos) + Vector3.up * 0.5f;
			cPathNodeType blockType = RaycastPathWorldUtils.getBlockType(world, current, vector);
			RaycastNode raycastNode = new RaycastNode(vector, 1f, 0);
			raycastNode.SetType(blockType);
			raycastNode.SetParent(current);
			current.AddNeighbor(raycastNode);
			if (blockType == cPathNodeType.Half)
			{
				List<RaycastNode> list = RaycastPathWorldUtils.ScanChildBlocksAround(world, current, vector, debugDraw, duration);
				for (int i = 0; i < list.Count; i++)
				{
					RaycastNode raycastNode2 = list[i];
					raycastNode2.SetParent(raycastNode);
					raycastNode.AddChild(raycastNode2);
				}
			}
			return raycastNode;
		}

		// Token: 0x0600C570 RID: 50544 RVA: 0x00489903 File Offset: 0x00487B03
		public static float getH(Vector3 from, Vector3 to)
		{
			return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y) + Mathf.Abs(from.z - to.z);
		}

		// Token: 0x0600C571 RID: 50545 RVA: 0x00489940 File Offset: 0x00487B40
		public static List<FloodFillNode> ScanNeighborNodes(World world, FloodFillPath path, FloodFillNode current, bool useDiagnols = false)
		{
			List<FloodFillNode> list = new List<FloodFillNode>();
			for (int i = 0; i < RaycastPathWorldUtils.mainBlockAxis.Length; i++)
			{
				FloodFillNode floodFillNode = RaycastPathWorldUtils.BuildNeighborNode(world, path, current, current.BlockPos + RaycastPathWorldUtils.mainBlockAxis[i], false);
				list.Add(floodFillNode);
			}
			if (useDiagnols)
			{
				for (int j = 0; j < RaycastPathWorldUtils.diagonalBlockAxis.Length; j++)
				{
					Vector3 vector = current.Center + RaycastPathWorldUtils.diagonalBlockAxis[j];
					Vector3 normalized = (current.Center - vector).normalized;
					Vector3i vector3i = current.BlockPos + RaycastPathWorldUtils.diagonalBlockAxis[j];
					if (!RaycastPathUtils.IsPointBlocked(current.Center, current.Center + normalized * 1.5f, 65536, false, 0f) && !RaycastPathUtils.IsPointBlocked(current.Center, current.Center - normalized * 1.5f, 65536, false, 0f))
					{
						FloodFillNode floodFillNode2 = RaycastPathWorldUtils.BuildNeighborNode(world, path, current, vector3i, false);
						list.Add(floodFillNode2);
					}
				}
			}
			return list;
		}

		// Token: 0x0600C572 RID: 50546 RVA: 0x00489A6C File Offset: 0x00487C6C
		public static FloodFillNode BuildNeighborNode(World world, FloodFillPath path, FloodFillNode current, Vector3i adjacentPos, bool debugDraw = false)
		{
			Vector3 vector = World.blockToTransformPos(adjacentPos) + Vector3.up * 0.5f;
			cPathNodeType blockType = RaycastPathWorldUtils.getBlockType(world, current, vector);
			FloodFillNode floodFillNode = new FloodFillNode(vector, 1f, 0);
			floodFillNode.SetType(blockType);
			floodFillNode.SetParent(current);
			current.AddNeighbor(floodFillNode);
			if (blockType == cPathNodeType.Half)
			{
				List<RaycastNode> list = RaycastPathWorldUtils.ScanChildBlocksAround(world, current, vector, debugDraw, 0f);
				for (int i = 0; i < list.Count; i++)
				{
					RaycastNode raycastNode = list[i];
					raycastNode.SetParent(current);
					floodFillNode.AddChild(raycastNode);
				}
			}
			floodFillNode.G = current.G + 1f;
			floodFillNode.Heuristic = RaycastPathWorldUtils.getH(floodFillNode.BlockPos, path.TargetBlockPos);
			return floodFillNode;
		}

		// Token: 0x0600C573 RID: 50547 RVA: 0x00489B38 File Offset: 0x00487D38
		public static bool HasSubBlocks(World world, RaycastNode current, Vector3 spacialCenter, bool debugDraw = false, float duration = 0f)
		{
			int num = 0;
			List<RaycastNode> list = RaycastPathWorldUtils.ScanChildBlocksAround(world, current, spacialCenter, debugDraw, duration);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].nodeType == cPathNodeType.Air)
				{
					num++;
				}
			}
			return num > 0;
		}

		// Token: 0x0600C574 RID: 50548 RVA: 0x00489B7C File Offset: 0x00487D7C
		public static List<RaycastNode> ScanChildBlocksAround(World world, RaycastNode current, Vector3 spacialCenter, bool debugDraw = false, float duration = 0f)
		{
			List<RaycastNode> list = new List<RaycastNode>();
			for (int i = 0; i < RaycastPathWorldUtils.quarterBlockOffsets.Length; i++)
			{
				RaycastNode raycastNode = new RaycastNode(spacialCenter + RaycastPathWorldUtils.quarterBlockOffsets[i], 0.5f, current.Depth + 1);
				list.Add(raycastNode);
			}
			for (int j = 0; j < list.Count; j++)
			{
				RaycastNode raycastNode2 = list[j];
				Vector3 position = raycastNode2.Position;
				if (!RaycastPathUtils.IsPointBlocked(spacialCenter, position, 65536, debugDraw, duration) && !RaycastPathUtils.IsPointBlocked(current.Position, spacialCenter, 65536, debugDraw, duration))
				{
					raycastNode2.SetType(cPathNodeType.Air);
				}
				else
				{
					raycastNode2.SetType(cPathNodeType.Solid);
				}
				if (debugDraw)
				{
					if (raycastNode2.nodeType == cPathNodeType.Air)
					{
						RaycastPathUtils.DrawNode(raycastNode2, Color.cyan, duration);
					}
					else if (raycastNode2.nodeType == cPathNodeType.Solid)
					{
						RaycastPathUtils.DrawNode(raycastNode2, Color.magenta, duration);
					}
				}
			}
			list.Sort(delegate(RaycastNode x, RaycastNode y)
			{
				if (Vector3.Distance(current.Position, x.Position) >= Vector3.Distance(current.Position, y.Position))
				{
					return 1;
				}
				return -1;
			});
			return list;
		}

		// Token: 0x0600C575 RID: 50549 RVA: 0x00489C94 File Offset: 0x00487E94
		public static List<RaycastNode> ScanPath(World world, Vector3 fromPos, List<Vector3> path, bool useDiagnols, bool debugDraw = false, float duration = 0f)
		{
			List<RaycastNode> list = new List<RaycastNode>();
			Vector3 vector = fromPos;
			for (int i = 0; i < path.Count; i++)
			{
				list.Add(RaycastPathWorldUtils.ScanPoint(world, path[i], vector, useDiagnols, debugDraw, duration));
				vector = path[i];
			}
			return list;
		}

		// Token: 0x0600C576 RID: 50550 RVA: 0x00489CDC File Offset: 0x00487EDC
		public static RaycastNode ScanPoint(World world, Vector3 worldPos, Vector3 fromPos, bool useDiagnols, bool debugDraw = false, float duration = 0f)
		{
			RaycastNode raycastNode = RaycastPathWorldUtils.CreateNode(world, World.worldToBlockPos(worldPos).ToVector3Center(), null, false, 0f);
			for (int i = 0; i < RaycastPathWorldUtils.mainBlockAxis.Length; i++)
			{
				RaycastPathWorldUtils.CreateNode(world, raycastNode.BlockPos.ToVector3Center() + RaycastPathWorldUtils.mainBlockAxis[i], raycastNode, false, 0f);
			}
			if (useDiagnols)
			{
				for (int j = 0; j < RaycastPathWorldUtils.diagonalBlockAxis.Length; j++)
				{
					RaycastPathWorldUtils.CreateNode(world, raycastNode.BlockPos.ToVector3Center() + RaycastPathWorldUtils.diagonalBlockAxis[j], raycastNode, false, 0f);
				}
			}
			if (debugDraw)
			{
				if (raycastNode.nodeType == cPathNodeType.Solid)
				{
					RaycastPathUtils.DrawNode(raycastNode, Color.magenta, duration);
				}
				if (raycastNode.nodeType == cPathNodeType.Air)
				{
					RaycastPathUtils.DrawNode(raycastNode, Color.cyan, duration);
				}
				if (raycastNode.nodeType == cPathNodeType.Door)
				{
					RaycastPathUtils.DrawNode(raycastNode, Color.blue, duration);
				}
			}
			if (raycastNode.nodeType == cPathNodeType.Half)
			{
				RaycastNode raycastNode2 = RaycastPathWorldUtils.ProcNeighborNodes(raycastNode.Neighbors, raycastNode);
				raycastNode.SetWaypoint(raycastNode2);
			}
			return raycastNode;
		}

		// Token: 0x0600C577 RID: 50551 RVA: 0x00489DFC File Offset: 0x00487FFC
		public static cPathNodeType GetBlockType(World world, Vector3 toPos)
		{
			Vector3i vector3i = World.worldToBlockPos(toPos);
			BlockValue block = world.GetBlock(vector3i);
			Vector3 vector = vector3i.ToVector3Center();
			if (block.isair)
			{
				return cPathNodeType.Air;
			}
			if (block.Block.shape.IsSolidSpace || block.Block.shape.IsSolidCube)
			{
				return cPathNodeType.Solid;
			}
			if (block.Block.HasTag(BlockTags.Door))
			{
				return cPathNodeType.Door;
			}
			if (RaycastPathWorldUtils.HasChildNodes(world, vector, false, 0f))
			{
				return cPathNodeType.Half;
			}
			return cPathNodeType.Air;
		}

		// Token: 0x0600C578 RID: 50552 RVA: 0x00489E78 File Offset: 0x00488078
		[PublicizedFrom(EAccessModifier.Private)]
		public static RaycastNode CreateNode(World world, Vector3 worldPos, RaycastNode parent = null, bool debugDraw = false, float duration = 0f)
		{
			RaycastNode raycastNode = new RaycastNode(worldPos, 1f, 0);
			cPathNodeType blockType = RaycastPathWorldUtils.GetBlockType(world, worldPos);
			raycastNode.SetType(blockType);
			if (parent != null)
			{
				raycastNode.SetParent(parent);
				parent.AddNeighbor(raycastNode);
			}
			if (blockType == cPathNodeType.Half)
			{
				List<RaycastNode> list = RaycastPathWorldUtils.CreateChildNodes(world, worldPos, debugDraw, duration);
				for (int i = 0; i < list.Count; i++)
				{
					RaycastNode raycastNode2 = list[i];
					raycastNode2.SetParent(raycastNode);
					raycastNode.AddChild(raycastNode2);
					if (debugDraw)
					{
						if (raycastNode2.nodeType == cPathNodeType.Air)
						{
							RaycastPathUtils.DrawNode(raycastNode2, Color.cyan, duration);
						}
						else if (raycastNode2.nodeType == cPathNodeType.Solid)
						{
							RaycastPathUtils.DrawNode(raycastNode2, Color.magenta, duration);
						}
					}
				}
			}
			return raycastNode;
		}

		// Token: 0x0600C579 RID: 50553 RVA: 0x00489F24 File Offset: 0x00488124
		public static List<RaycastNode> CreateChildNodes(World world, Vector3 worldPos, bool debugDraw = false, float duration = 0f)
		{
			List<RaycastNode> list = new List<RaycastNode>();
			Vector3 vector = World.worldToBlockPos(worldPos).ToVector3Center();
			for (int i = 0; i < RaycastPathWorldUtils.quarterBlockOffsets.Length; i++)
			{
				RaycastNode raycastNode = new RaycastNode(vector + RaycastPathWorldUtils.quarterBlockOffsets[i], 0.5f, 0);
				list.Add(raycastNode);
			}
			for (int j = 0; j < list.Count; j++)
			{
				RaycastNode raycastNode2 = list[j];
				if (!RaycastPathUtils.IsPointBlocked(vector, raycastNode2.Position, 1073807360, debugDraw, duration))
				{
					raycastNode2.SetType(cPathNodeType.Air);
				}
				else
				{
					raycastNode2.SetType(cPathNodeType.Solid);
				}
			}
			return list;
		}

		// Token: 0x0600C57A RID: 50554 RVA: 0x00489FC8 File Offset: 0x004881C8
		[PublicizedFrom(EAccessModifier.Private)]
		public static RaycastNode ProcNeighborNodes(List<RaycastNode> currentNeighbors, RaycastNode current)
		{
			List<RaycastNode> list = new List<RaycastNode>();
			for (int i = 0; i < currentNeighbors.Count; i++)
			{
				RaycastNode raycastNode = currentNeighbors[i];
				if (raycastNode.ChildAirBlocks.Count > 0)
				{
					for (int j = 0; j < raycastNode.ChildAirBlocks.Count; j++)
					{
						RaycastNode raycastNode2 = raycastNode.Children[j];
						list.Add(raycastNode2);
					}
				}
			}
			List<RaycastNode> list2 = new List<RaycastNode>();
			for (int k = 0; k < current.ChildAirBlocks.Count; k++)
			{
				RaycastNode raycastNode3 = current.ChildAirBlocks[k];
				list2.Add(raycastNode3);
				for (int l = 0; l < list.Count; l++)
				{
					RaycastNode raycastNode4 = list[l];
					if (!RaycastPathUtils.IsPointBlocked(raycastNode3.Position, raycastNode4.Position, 65536, true, 0f))
					{
						list2.Add(raycastNode4);
					}
				}
			}
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			for (int m = 0; m < list2.Count - 1; m++)
			{
				RaycastNode raycastNode5 = list2[m];
				RaycastNode raycastNode6 = list2[m + 1];
				if (m < 1)
				{
					vector = Vector3.Min(raycastNode5.Min, raycastNode6.Min);
					vector2 = Vector3.Max(raycastNode5.Max, raycastNode6.Max);
				}
				else
				{
					vector = Vector3.Min(vector, raycastNode6.Min);
					vector2 = Vector3.Max(vector2, raycastNode6.Max);
				}
			}
			RaycastNode raycastNode7 = new RaycastNode(vector, vector2, 1f, 0);
			RaycastPathUtils.DrawNode(raycastNode7, Color.green, 5f);
			current.SetWaypoint(raycastNode7);
			return raycastNode7;
		}

		// Token: 0x0600C57B RID: 50555 RVA: 0x0048A168 File Offset: 0x00488368
		[PublicizedFrom(EAccessModifier.Private)]
		public static int GetChildAirblockCount(World world, Vector3 worldPos, bool debugDraw = false, float duration = 0f)
		{
			int num = 0;
			List<RaycastNode> list = RaycastPathWorldUtils.CreateChildNodes(world, worldPos, debugDraw, duration);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].nodeType == cPathNodeType.Air)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600C57C RID: 50556 RVA: 0x0048A1A8 File Offset: 0x004883A8
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool HasChildNodes(World world, Vector3 worldPos, bool debugDraw = false, float duration = 0f)
		{
			int childAirblockCount = RaycastPathWorldUtils.GetChildAirblockCount(world, worldPos, debugDraw, duration);
			return childAirblockCount > 0 && childAirblockCount < RaycastPathWorldUtils.quarterBlockOffsets.Length;
		}

		// Token: 0x0600C57D RID: 50557 RVA: 0x0048A1CF File Offset: 0x004883CF
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool AreChildrenAir(World world, Vector3 worldPos, bool debugDraw = false, float duration = 0f)
		{
			return RaycastPathWorldUtils.GetChildAirblockCount(world, worldPos, debugDraw, duration) == RaycastPathWorldUtils.quarterBlockOffsets.Length;
		}

		// Token: 0x04009508 RID: 38152
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3i[] mainBlockAxis = new Vector3i[]
		{
			new Vector3i(0, 0, 1),
			new Vector3i(1, 0, 0),
			new Vector3i(0, 0, -1),
			new Vector3i(-1, 0, 0),
			new Vector3i(0, 1, 0),
			new Vector3i(0, -1, 0)
		};

		// Token: 0x04009509 RID: 38153
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3i[] diagonalBlockAxis = new Vector3i[]
		{
			new Vector3i(0, 1, 1),
			new Vector3i(0, -1, 1),
			new Vector3i(1, 1, 0),
			new Vector3i(1, -1, 0),
			new Vector3i(0, 1, -1),
			new Vector3i(0, -1, -1),
			new Vector3i(-1, 1, 0),
			new Vector3i(-1, -1, 0),
			new Vector3i(1, 0, 1),
			new Vector3i(1, 1, 1),
			new Vector3i(1, -1, 1),
			new Vector3i(1, 0, -1),
			new Vector3i(1, 1, -1),
			new Vector3i(1, -1, -1),
			new Vector3i(-1, 0, 1),
			new Vector3i(-1, 1, 1),
			new Vector3i(-1, -1, 1),
			new Vector3i(-1, 0, -1),
			new Vector3i(-1, 1, -1),
			new Vector3i(-1, -1, -1)
		};

		// Token: 0x0400950A RID: 38154
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3[] subBlockOffsets = new Vector3[]
		{
			new Vector3(1f, 1f, 1f),
			new Vector3(1f, 1f, 0f),
			new Vector3(0f, 1f, 1f),
			new Vector3(0f, 1f, 0f),
			new Vector3(1f, 0f, 1f),
			new Vector3(1f, 0f, 0f),
			new Vector3(0f, 0f, 1f),
			new Vector3(0f, 0f, 0f)
		};

		// Token: 0x0400950B RID: 38155
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3[] quarterBlockOffsets = new Vector3[]
		{
			new Vector3(0.25f, 0.25f, 0.25f),
			new Vector3(-0.25f, 0.25f, 0.25f),
			new Vector3(0.25f, 0.25f, -0.25f),
			new Vector3(-0.25f, 0.25f, -0.25f),
			new Vector3(0.25f, -0.25f, 0.25f),
			new Vector3(-0.25f, -0.25f, 0.25f),
			new Vector3(0.25f, -0.25f, -0.25f),
			new Vector3(-0.25f, -0.25f, -0.25f)
		};

		// Token: 0x0400950C RID: 38156
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 underGroundTestOffset = new Vector3(0f, 255f, 0f);

		// Token: 0x0400950D RID: 38157
		public const int cPathLayer = 1073807360;
	}
}
