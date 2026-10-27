using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A47 RID: 6727
	[Preserve]
	public class ActionBlockUpgrade : ActionBaseBlockAction
	{
		// Token: 0x0600CCD1 RID: 52433 RVA: 0x004AAD64 File Offset: 0x004A8F64
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				BlockValue blockValue2 = blockValue.Block.UpgradeBlock;
				blockValue2 = BlockPlaceholderMap.Instance.Replace(blockValue2, GameEventManager.Current.Random, currentPos.x, currentPos.z, false);
				blockValue2.rotation = blockValue.rotation;
				blockValue2.meta = blockValue.meta;
				if (!blockValue2.isair)
				{
					return new BlockChangeInfo(currentPos, blockValue2);
				}
			}
			return null;
		}

		// Token: 0x0600CCD2 RID: 52434 RVA: 0x004AADDD File Offset: 0x004A8FDD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockUpgrade();
		}
	}
}
