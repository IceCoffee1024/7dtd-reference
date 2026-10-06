using System;
using Pathfinding;

namespace GamePath
{
	// Token: 0x02001903 RID: 6403
	[PublicizedFrom(EAccessModifier.Internal)]
	public class TraversalProvider : ITraversalProvider
	{
		// Token: 0x0600C594 RID: 50580 RVA: 0x0048B430 File Offset: 0x00489630
		public bool CanTraverseConnection(Path path, Connection conn)
		{
			return conn.node.Walkable && ((path.enabledTags >> (int)conn.node.Tag) & 1) != 0;
		}

		// Token: 0x0600C595 RID: 50581 RVA: 0x0048B45C File Offset: 0x0048965C
		public bool CanTraverse(Path path, GraphNode node, int gridDirection)
		{
			if (!node.Walkable)
			{
				return false;
			}
			if (!path.CanBreakBlocks && gridDirection >= 0)
			{
				AstarVoxelGrid.VoxelNode voxelNode = node as AstarVoxelGrid.VoxelNode;
				if (voxelNode != null && ((int)voxelNode.BlockerFlags & (17 << gridDirection)) > 0)
				{
					return false;
				}
			}
			return ((path.enabledTags >> (int)node.Tag) & 1) != 0;
		}

		// Token: 0x0600C596 RID: 50582 RVA: 0x0048B4B1 File Offset: 0x004896B1
		public uint GetTraversalCost(Path path, GraphNode node)
		{
			return (uint)(node.Penalty * path.CostScale);
		}
	}
}
