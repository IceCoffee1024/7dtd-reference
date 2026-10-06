using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A0E RID: 6670
	[Preserve]
	public class ActionBlockGrowCrops : ActionBaseBlockAction
	{
		// Token: 0x0600CB9A RID: 52122 RVA: 0x004A6AFC File Offset: 0x004A4CFC
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

		// Token: 0x0600CB9B RID: 52123 RVA: 0x004A6B3B File Offset: 0x004A4D3B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockGrowCrops();
		}
	}
}
