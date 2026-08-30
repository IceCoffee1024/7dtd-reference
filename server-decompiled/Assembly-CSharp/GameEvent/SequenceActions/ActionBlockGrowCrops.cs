using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E2 RID: 6626
	[Preserve]
	public class ActionBlockGrowCrops : ActionBaseBlockAction
	{
		// Token: 0x0600CA42 RID: 51778 RVA: 0x004A2658 File Offset: 0x004A0858
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

		// Token: 0x0600CA43 RID: 51779 RVA: 0x004A2697 File Offset: 0x004A0897
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockGrowCrops();
		}
	}
}
