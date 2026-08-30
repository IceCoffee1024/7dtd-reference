using System;
using Pathfinding;

namespace GamePath
{
	// Token: 0x020018D8 RID: 6360
	[PublicizedFrom(EAccessModifier.Internal)]
	public class TraversalProviderNoBreak : ITraversalProvider
	{
		// Token: 0x0600C440 RID: 50240 RVA: 0x00486F7C File Offset: 0x0048517C
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

		// Token: 0x0600C441 RID: 50241 RVA: 0x00486FF0 File Offset: 0x004851F0
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

		// Token: 0x0600C442 RID: 50242 RVA: 0x00487053 File Offset: 0x00485253
		public uint GetTraversalCost(Path path, GraphNode node)
		{
			return node.Penalty;
		}
	}
}
