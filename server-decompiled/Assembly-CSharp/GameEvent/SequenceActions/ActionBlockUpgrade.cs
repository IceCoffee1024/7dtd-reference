using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019EA RID: 6634
	[Preserve]
	public class ActionBlockUpgrade : ActionBaseBlockAction
	{
		// Token: 0x0600CA65 RID: 51813 RVA: 0x004A2DC4 File Offset: 0x004A0FC4
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

		// Token: 0x0600CA66 RID: 51814 RVA: 0x004A2E3D File Offset: 0x004A103D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockUpgrade();
		}
	}
}
