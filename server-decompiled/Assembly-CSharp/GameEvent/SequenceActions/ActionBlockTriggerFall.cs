using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A45 RID: 6725
	[Preserve]
	public class ActionBlockTriggerFall : ActionBaseBlockAction
	{
		// Token: 0x0600CCC7 RID: 52423 RVA: 0x004A9F5A File Offset: 0x004A815A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CCC8 RID: 52424 RVA: 0x004AAC84 File Offset: 0x004A8E84
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			for (int i = 0; i < blockChanges.Count; i++)
			{
				world.AddFallingBlock(blockChanges[i].blockValueRef, false);
			}
		}

		// Token: 0x0600CCC9 RID: 52425 RVA: 0x004AACBA File Offset: 0x004A8EBA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockTriggerFall();
		}
	}
}
