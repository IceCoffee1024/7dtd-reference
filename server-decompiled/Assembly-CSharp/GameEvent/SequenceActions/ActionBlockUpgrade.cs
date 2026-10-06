using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A16 RID: 6678
	[Preserve]
	public class ActionBlockUpgrade : ActionBaseBlockAction
	{
		// Token: 0x0600CBBD RID: 52157 RVA: 0x004A7268 File Offset: 0x004A5468
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

		// Token: 0x0600CBBE RID: 52158 RVA: 0x004A72E1 File Offset: 0x004A54E1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockUpgrade();
		}
	}
}
