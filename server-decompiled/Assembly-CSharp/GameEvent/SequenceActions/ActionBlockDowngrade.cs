using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A3D RID: 6717
	[Preserve]
	public class ActionBlockDowngrade : ActionBaseBlockAction
	{
		// Token: 0x0600CCA6 RID: 52390 RVA: 0x004AA474 File Offset: 0x004A8674
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				BlockValue blockValue2 = blockValue.Block.DowngradeBlock;
				blockValue2 = BlockPlaceholderMap.Instance.Replace(currentPos, blockValue2, GameManager.Instance.World.GetGameRandom(), false);
				blockValue2.rotation = blockValue.rotation;
				blockValue2.meta = blockValue.meta;
				if (!blockValue2.isair)
				{
					world.AddPendingDowngradeBlock(currentPos);
					return new BlockChangeInfo(currentPos, blockValue2);
				}
			}
			return null;
		}

		// Token: 0x0600CCA7 RID: 52391 RVA: 0x004AA4F8 File Offset: 0x004A86F8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockDowngrade();
		}
	}
}
