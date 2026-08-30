using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace RaycastPathing
{
	// Token: 0x020018D2 RID: 6354
	[Preserve]
	public class RaycastPathWorldUtils
	{
		// Token: 0x0600C40D RID: 50189 RVA: 0x00484EA0 File Offset: 0x004830A0
		public static Vector3i GetBlockPosition(Vector3 worldPos)
		{
			Vector3i vector3i = new Vector3i(worldPos);
			Vector3 vector = worldPos - vector3i.ToVector3Center();
			return vector3i + Vector3i.FromVector3Rounded(vector);
		}

		// Token: 0x0600C40E RID: 50190 RVA: 0x00484ECF File Offset: 0x004830CF
		public static Vector3 GetCenterPosition(Vector3 worldPos, float scale = 1f)
		{
			return worldPos + Vector3.one * 0.5f * scale;
		}

		// Token: 0x0600C40F RID: 50191 RVA: 0x00484EEC File Offset: 0x004830EC
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

		// Token: 0x0600C410 RID: 50192 RVA: 0x00484F33 File Offset: 0x00483133
		public static bool IsUnderground(Vector3 target)
		{
			return Physics.Raycast(new Ray(target + RaycastPathWorldUtils.underGroundTestOffset - Origin.position, Vector3.down), RaycastPathWorldUtils.underGroundTestOffset.y, 268500992);
		}

		// Token: 0x0600C411 RID: 50193 RVA: 0x00484F68 File Offset: 0x00483168
		public static bool isPosUnderWater(World world, Vector3 pos)
		{
			Vector3i blockPosition = RaycastPathWorldUtils.GetBlockPosition(pos);
			return world.GetBlock(blockPosition).type == 240;
		}

		// Token: 0x0600C412 RID: 50194 RVA: 0x00484F94 File Offset: 0x00483194
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

		// Token: 0x0600C413 RID: 50195 RVA: 0x00485014 File Offset: 0x00483214
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

		// Token: 0x0600C414 RID: 50196 RVA: 0x00485050 File Offset: 0x00483250
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

		// Token: 0x0600C415 RID: 50197 RVA: 0x004851F4 File Offset: 0x004833F4
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

		// Token: 0x0600C416 RID: 50198 RVA: 0x004852F0 File Offset: 0x004834F0
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

		// Token: 0x0600C417 RID: 50199 RVA: 0x00485328 File Offset: 0x00483528
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

		// Token: 0x0600C418 RID: 50200 RVA: 0x004853BB File Offset: 0x004835BB
		public static float getH(Vector3 from, Vector3 to)
		{
			return Mathf.Abs(from.x - to.x) + Mathf.Abs(from.y - to.y) + Mathf.Abs(from.z - to.z);
		}

		// Token: 0x0600C419 RID: 50201 RVA: 0x004853F8 File Offset: 0x004835F8
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

		// Token: 0x0600C41A RID: 50202 RVA: 0x00485524 File Offset: 0x00483724
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

		// Token: 0x0600C41B RID: 50203 RVA: 0x004855F0 File Offset: 0x004837F0
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

		// Token: 0x0600C41C RID: 50204 RVA: 0x00485634 File Offset: 0x00483834
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

		// Token: 0x0600C41D RID: 50205 RVA: 0x0048574C File Offset: 0x0048394C
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

		// Token: 0x0600C41E RID: 50206 RVA: 0x00485794 File Offset: 0x00483994
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

		// Token: 0x0600C41F RID: 50207 RVA: 0x004858B4 File Offset: 0x00483AB4
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

		// Token: 0x0600C420 RID: 50208 RVA: 0x00485930 File Offset: 0x00483B30
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

		// Token: 0x0600C421 RID: 50209 RVA: 0x004859DC File Offset: 0x00483BDC
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

		// Token: 0x0600C422 RID: 50210 RVA: 0x00485A80 File Offset: 0x00483C80
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

		// Token: 0x0600C423 RID: 50211 RVA: 0x00485C20 File Offset: 0x00483E20
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

		// Token: 0x0600C424 RID: 50212 RVA: 0x00485C60 File Offset: 0x00483E60
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool HasChildNodes(World world, Vector3 worldPos, bool debugDraw = false, float duration = 0f)
		{
			int childAirblockCount = RaycastPathWorldUtils.GetChildAirblockCount(world, worldPos, debugDraw, duration);
			return childAirblockCount > 0 && childAirblockCount < RaycastPathWorldUtils.quarterBlockOffsets.Length;
		}

		// Token: 0x0600C425 RID: 50213 RVA: 0x00485C87 File Offset: 0x00483E87
		[PublicizedFrom(EAccessModifier.Private)]
		public static bool AreChildrenAir(World world, Vector3 worldPos, bool debugDraw = false, float duration = 0f)
		{
			return RaycastPathWorldUtils.GetChildAirblockCount(world, worldPos, debugDraw, duration) == RaycastPathWorldUtils.quarterBlockOffsets.Length;
		}

		// Token: 0x04009442 RID: 37954
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

		// Token: 0x04009443 RID: 37955
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

		// Token: 0x04009444 RID: 37956
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

		// Token: 0x04009445 RID: 37957
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

		// Token: 0x04009446 RID: 37958
		[PublicizedFrom(EAccessModifier.Private)]
		public static Vector3 underGroundTestOffset = new Vector3(0f, 255f, 0f);

		// Token: 0x04009447 RID: 37959
		public const int cPathLayer = 1073807360;
	}
}
