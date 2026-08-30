using System;
using Pathfinding;

namespace GamePath
{
	// Token: 0x020018D7 RID: 6359
	[PublicizedFrom(EAccessModifier.Internal)]
	public class TraversalProvider : ITraversalProvider
	{
		// Token: 0x0600C43C RID: 50236 RVA: 0x00486EE8 File Offset: 0x004850E8
		public bool CanTraverseConnection(Path path, Connection conn)
		{
			return conn.node.Walkable && ((path.enabledTags >> (int)conn.node.Tag) & 1) != 0;
		}

		// Token: 0x0600C43D RID: 50237 RVA: 0x00486F14 File Offset: 0x00485114
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

		// Token: 0x0600C43E RID: 50238 RVA: 0x00486F69 File Offset: 0x00485169
		public uint GetTraversalCost(Path path, GraphNode node)
		{
			return (uint)(node.Penalty * path.CostScale);
		}
	}
}
