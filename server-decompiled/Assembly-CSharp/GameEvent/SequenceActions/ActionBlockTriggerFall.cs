using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E8 RID: 6632
	[Preserve]
	public class ActionBlockTriggerFall : ActionBaseBlockAction
	{
		// Token: 0x0600CA5B RID: 51803 RVA: 0x004A1FBA File Offset: 0x004A01BA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CA5C RID: 51804 RVA: 0x004A2CE4 File Offset: 0x004A0EE4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			for (int i = 0; i < blockChanges.Count; i++)
			{
				world.AddFallingBlock(blockChanges[i].blockValueRef, false);
			}
		}

		// Token: 0x0600CA5D RID: 51805 RVA: 0x004A2D1A File Offset: 0x004A0F1A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockTriggerFall();
		}
	}
}
