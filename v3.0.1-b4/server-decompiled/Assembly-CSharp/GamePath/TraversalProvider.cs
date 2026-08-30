using System;
using Pathfinding;

namespace GamePath
{
	// Token: 0x020018B1 RID: 6321
	[PublicizedFrom(EAccessModifier.Internal)]
	public class TraversalProvider : ITraversalProvider
	{
		// Token: 0x0600C2B5 RID: 49845 RVA: 0x00480DBC File Offset: 0x0047EFBC
		public bool CanTraverseConnection(Path path, Connection conn)
		{
			return conn.node.Walkable && ((path.enabledTags >> (int)conn.node.Tag) & 1) != 0;
		}

		// Token: 0x0600C2B6 RID: 49846 RVA: 0x00480DE8 File Offset: 0x0047EFE8
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

		// Token: 0x0600C2B7 RID: 49847 RVA: 0x00480E3D File Offset: 0x0047F03D
		public uint GetTraversalCost(Path path, GraphNode node)
		{
			return (uint)(node.Penalty * path.CostScale);
		}
	}
}
