using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E0 RID: 6624
	[Preserve]
	public class ActionBlockDowngrade : ActionBaseBlockAction
	{
		// Token: 0x0600CA3A RID: 51770 RVA: 0x004A24D4 File Offset: 0x004A06D4
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

		// Token: 0x0600CA3B RID: 51771 RVA: 0x004A2558 File Offset: 0x004A0758
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockDowngrade();
		}
	}
}
