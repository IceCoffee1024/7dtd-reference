using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A0C RID: 6668
	[Preserve]
	public class ActionBlockDowngrade : ActionBaseBlockAction
	{
		// Token: 0x0600CB92 RID: 52114 RVA: 0x004A6978 File Offset: 0x004A4B78
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

		// Token: 0x0600CB93 RID: 52115 RVA: 0x004A69FC File Offset: 0x004A4BFC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockDowngrade();
		}
	}
}
