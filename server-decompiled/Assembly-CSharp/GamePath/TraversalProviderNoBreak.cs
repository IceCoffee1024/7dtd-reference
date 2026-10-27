using System;
using Pathfinding;

namespace GamePath
{
	// Token: 0x02001935 RID: 6453
	[PublicizedFrom(EAccessModifier.Internal)]
	public class TraversalProviderNoBreak : ITraversalProvider
	{
		// Token: 0x0600C6AC RID: 50860 RVA: 0x0048EE70 File Offset: 0x0048D070
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

		// Token: 0x0600C6AD RID: 50861 RVA: 0x0048EEE4 File Offset: 0x0048D0E4
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

		// Token: 0x0600C6AE RID: 50862 RVA: 0x0048EF47 File Offset: 0x0048D147
		public uint GetTraversalCost(Path path, GraphNode node)
		{
			return node.Penalty;
		}
	}
}
