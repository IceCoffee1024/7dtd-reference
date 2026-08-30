using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B9 RID: 6585
	[Preserve]
	public class ActionBlockDowngrade : ActionBaseBlockAction
	{
		// Token: 0x0600C8B1 RID: 51377 RVA: 0x0049C0E0 File Offset: 0x0049A2E0
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

		// Token: 0x0600C8B2 RID: 51378 RVA: 0x0049C164 File Offset: 0x0049A364
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockDowngrade();
		}
	}
}
