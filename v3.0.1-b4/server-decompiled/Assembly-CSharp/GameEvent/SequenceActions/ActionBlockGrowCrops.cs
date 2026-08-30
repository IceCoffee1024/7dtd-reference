using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BB RID: 6587
	[Preserve]
	public class ActionBlockGrowCrops : ActionBaseBlockAction
	{
		// Token: 0x0600C8B9 RID: 51385 RVA: 0x0049C264 File Offset: 0x0049A464
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

		// Token: 0x0600C8BA RID: 51386 RVA: 0x0049C2A3 File Offset: 0x0049A4A3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockGrowCrops();
		}
	}
}
