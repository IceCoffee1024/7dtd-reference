using System;
using Pathfinding;

namespace GamePath
{
	// Token: 0x02001904 RID: 6404
	[PublicizedFrom(EAccessModifier.Internal)]
	public class TraversalProviderNoBreak : ITraversalProvider
	{
		// Token: 0x0600C598 RID: 50584 RVA: 0x0048B4C4 File Offset: 0x004896C4
		public bool CanTraverseConnection(Path path, Connection conn)
		{
			if (!conn.node.Walkable)
			{
				return false;
			}
			if (conn.tag == 3U)
			{
				TEFeatureDoor tefeatureDoor = conn.payload as TEFeatureDoor;
				if (tefeatureDoor != null)
				{
					if (tefeatureDoor.IsOpen())
					{
						return true;
					}
					if (((path.enabledTags >> 3) & 1) == 0)
					{
						return false;
					}
					bool flag;
					if (!tefeatureDoor.CanOpen(out flag))
					{
						return false;
					}
				}
				return true;
			}
			return ((path.enabledTags >> (int)conn.node.Tag) & 1) != 0;
		}

		// Token: 0x0600C599 RID: 50585 RVA: 0x0048B538 File Offset: 0x00489738
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
			return ((path.enabledTags >> (int)node.Tag) & 1) != 0 && node.Penalty < 1000U;
		}

		// Token: 0x0600C59A RID: 50586 RVA: 0x0048B59B File Offset: 0x0048979B
		public uint GetTraversalCost(Path path, GraphNode node)
		{
			return node.Penalty;
		}
	}
}
