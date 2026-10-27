using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A3F RID: 6719
	[Preserve]
	public class ActionBlockGrowCrops : ActionBaseBlockAction
	{
		// Token: 0x0600CCAE RID: 52398 RVA: 0x004AA5F8 File Offset: 0x004A87F8
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				BlockPlantGrowing blockPlantGrowing = blockValue.Block as BlockPlantGrowing;
				if (blockPlantGrowing != null)
				{
					blockValue = blockPlantGrowing.ForceNextGrowStage(world, currentPos, blockValue);
					return new BlockChangeInfo(currentPos, blockValue);
				}
			}
			return null;
		}

		// Token: 0x0600CCAF RID: 52399 RVA: 0x004AA637 File Offset: 0x004A8837
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockGrowCrops();
		}
	}
}
