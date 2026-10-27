using System;
using System.Collections;
using System.Collections.Generic;
using RaycastPathing;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020008F4 RID: 2292
[Preserve]
public class FloodFillEntityPathGenerator : RaycastEntityPathGenerator
{
	// Token: 0x060041C4 RID: 16836 RVA: 0x0019DAD2 File Offset: 0x0019BCD2
	public FloodFillEntityPathGenerator(World _world, EntityAlive _entity)
		: base(_world, _entity)
	{
	}

	// Token: 0x060041C5 RID: 16837 RVA: 0x0019DADC File Offset: 0x0019BCDC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void InitPath(Vector3 start, Vector3 end)
	{
		this.Path = new FloodFillPath(start, end);
	}

	// Token: 0x060041C6 RID: 16838 RVA: 0x0019DAEB File Offset: 0x0019BCEB
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

	// Token: 0x060041C7 RID: 16839 RVA: 0x0019DAFC File Offset: 0x0019BCFC
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

	// Token: 0x060041C8 RID: 16840 RVA: 0x0019DB78 File Offset: 0x0019BD78
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

	// Token: 0x060041C9 RID: 16841 RVA: 0x0019DBE4 File Offset: 0x0019BDE4
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

	// Token: 0x060041CA RID: 16842 RVA: 0x0019DC28 File Offset: 0x0019BE28
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

	// Token: 0x040035A2 RID: 13730
	[PublicizedFrom(EAccessModifier.Private)]
	public float debugTick;
}
