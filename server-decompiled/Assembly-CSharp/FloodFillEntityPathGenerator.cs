using System;
using System.Collections;
using System.Collections.Generic;
using RaycastPathing;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020008EA RID: 2282
[Preserve]
public class FloodFillEntityPathGenerator : RaycastEntityPathGenerator
{
	// Token: 0x060041A1 RID: 16801 RVA: 0x0019CA2A File Offset: 0x0019AC2A
	public FloodFillEntityPathGenerator(World _world, EntityAlive _entity)
		: base(_world, _entity)
	{
	}

	// Token: 0x060041A2 RID: 16802 RVA: 0x0019CA34 File Offset: 0x0019AC34
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitPath(Vector3 start, Vector3 end)
	{
		this.Path = new FloodFillPath(start, end);
	}

	// Token: 0x060041A3 RID: 16803 RVA: 0x0019CA43 File Offset: 0x0019AC43
	[PublicizedFrom(EAccessModifier.Protected)]
	public override IEnumerator BuildPathProc()
	{
		FloodFillPath path = this.Path as FloodFillPath;
		FloodFillNode floodFillNode = new FloodFillNode(path.Start, 1f, 0);
		path.open.Add(floodFillNode);
		while (path.open.Count > 0)
		{
			FloodFillNode lowestScore = path.getLowestScore();
			path.open.Remove(lowestScore);
			path.closed.Add(lowestScore);
			if (lowestScore.BlockPos == this.Path.TargetBlockPos)
			{
				break;
			}
			List<FloodFillNode> list = this.AddNeighborNodes(base.GameWorld, path, lowestScore);
			this.ProcNeighborNodes(list, lowestScore);
			FloodFillNode floodFillNode2 = this.IsNeighborEnd(list);
			if (floodFillNode2 != null)
			{
				path.closed.Add(floodFillNode2);
				break;
			}
			if (path.closed.Count > 1536)
			{
				Log.Warning("Search Exausted.");
				break;
			}
			yield return new WaitForSeconds(this.debugTick);
		}
		RaycastNode raycastNode = path.closed[path.closed.Count - 1];
		while (raycastNode.Parent != null)
		{
			this.Path.AddNode(raycastNode);
			raycastNode = raycastNode.Parent;
		}
		yield return this.<>n__0();
		yield break;
	}

	// Token: 0x060041A4 RID: 16804 RVA: 0x0019CA54 File Offset: 0x0019AC54
	[PublicizedFrom(EAccessModifier.Private)]
	public bool IsValidNeighbor(World world, FloodFillPath path, FloodFillNode current, FloodFillNode adjacent)
	{
		if (adjacent.nodeType != cPathNodeType.Solid || adjacent.BlockPos == path.TargetBlockPos)
		{
			if (adjacent.nodeType == cPathNodeType.Door && RaycastPathUtils.IsPositionBlocked(current.Center, adjacent.Center, 65536, false))
			{
				TileEntityComposite tileEntityComposite = world.GetTileEntity(adjacent.BlockPos) as TileEntityComposite;
				TEFeatureDoor tefeatureDoor;
				if (tileEntityComposite == null || !tileEntityComposite.TryGetSelfOrFeature<TEFeatureDoor>(out tefeatureDoor) || !tefeatureDoor.IsOpen())
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	// Token: 0x060041A5 RID: 16805 RVA: 0x0019CAD0 File Offset: 0x0019ACD0
	[PublicizedFrom(EAccessModifier.Private)]
	public List<FloodFillNode> AddNeighborNodes(World world, FloodFillPath path, FloodFillNode current)
	{
		List<FloodFillNode> list = RaycastPathWorldUtils.ScanNeighborNodes(base.GameWorld, path, current, false);
		for (int i = 0; i < list.Count; i++)
		{
			FloodFillNode floodFillNode = list[i];
			if (this.IsValidNeighbor(world, path, current, floodFillNode) && !path.IsPosOpen(floodFillNode.Position) && !path.IsPosClosed(floodFillNode.Position))
			{
				path.open.Add(floodFillNode);
			}
		}
		return list;
	}

	// Token: 0x060041A6 RID: 16806 RVA: 0x0019CB3C File Offset: 0x0019AD3C
	[PublicizedFrom(EAccessModifier.Private)]
	public FloodFillNode IsNeighborEnd(List<FloodFillNode> currentNeighbors)
	{
		for (int i = 0; i < currentNeighbors.Count; i++)
		{
			FloodFillNode floodFillNode = currentNeighbors[i];
			if (floodFillNode.BlockPos == this.Path.TargetBlockPos)
			{
				return floodFillNode;
			}
		}
		return null;
	}

	// Token: 0x060041A7 RID: 16807 RVA: 0x0019CB80 File Offset: 0x0019AD80
	[PublicizedFrom(EAccessModifier.Private)]
	public FloodFillNode ProcNeighborNodes(List<FloodFillNode> currentNeighbors, FloodFillNode current)
	{
		List<FloodFillNode> list = new List<FloodFillNode>();
		for (int i = 0; i < currentNeighbors.Count; i++)
		{
			FloodFillNode floodFillNode = currentNeighbors[i];
			if (floodFillNode.ChildAirBlocks.Count > 0)
			{
				for (int j = 0; j < floodFillNode.ChildAirBlocks.Count; j++)
				{
					FloodFillNode floodFillNode2 = floodFillNode.Children[j] as FloodFillNode;
					list.Add(floodFillNode2);
				}
			}
		}
		List<FloodFillNode> list2 = new List<FloodFillNode>();
		for (int k = 0; k < current.ChildAirBlocks.Count; k++)
		{
			FloodFillNode floodFillNode3 = current.ChildAirBlocks[k] as FloodFillNode;
			list2.Add(floodFillNode3);
			for (int l = 0; l < list.Count; l++)
			{
				FloodFillNode floodFillNode4 = list[l];
				if (floodFillNode4 != null && !RaycastPathUtils.IsPointBlocked(floodFillNode3.Position, floodFillNode4.Position, 65536, true, this.debugTick))
				{
					list2.Add(floodFillNode4);
				}
			}
		}
		Vector3 vector = Vector3.zero;
		Vector3 vector2 = Vector3.zero;
		for (int m = 0; m < list2.Count - 1; m++)
		{
			FloodFillNode floodFillNode5 = list2[m];
			if (floodFillNode5 != null)
			{
				FloodFillNode floodFillNode6 = list2[m + 1];
				if (m < 1)
				{
					vector = Vector3.Min(floodFillNode5.Min, floodFillNode6.Min);
					vector2 = Vector3.Max(floodFillNode5.Max, floodFillNode6.Max);
				}
				else
				{
					vector = Vector3.Min(vector, floodFillNode6.Min);
					vector2 = Vector3.Max(vector2, floodFillNode6.Max);
				}
			}
		}
		FloodFillNode floodFillNode7 = new FloodFillNode(vector, vector2, 1f, 0);
		RaycastPathUtils.DrawNode(floodFillNode7, Color.green, 5f);
		current.SetWaypoint(floodFillNode7);
		return floodFillNode7;
	}

	// Token: 0x04003582 RID: 13698
	[PublicizedFrom(EAccessModifier.Private)]
	public float debugTick;
}
