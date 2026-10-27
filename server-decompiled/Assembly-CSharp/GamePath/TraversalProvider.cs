using System;
using Pathfinding;

namespace GamePath
{
	// Token: 0x02001934 RID: 6452
	[PublicizedFrom(EAccessModifier.Internal)]
	public class TraversalProvider : ITraversalProvider
	{
		// Token: 0x0600C6A8 RID: 50856 RVA: 0x0048EDDC File Offset: 0x0048CFDC
		public bool CanTraverseConnection(Path path, Connection conn)
		{
			return conn.node.Walkable && ((path.enabledTags >> (int)conn.node.Tag) & 1) != 0;
		}

		// Token: 0x0600C6A9 RID: 50857 RVA: 0x0048EE08 File Offset: 0x0048D008
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

		// Token: 0x0600C6AA RID: 50858 RVA: 0x0048EE5D File Offset: 0x0048D05D
		public uint GetTraversalCost(Path path, GraphNode node)
		{
			return (uint)(node.Penalty * path.CostScale);
		}
	}
}
