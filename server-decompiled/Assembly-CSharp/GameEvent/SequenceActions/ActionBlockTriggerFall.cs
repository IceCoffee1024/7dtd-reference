using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A14 RID: 6676
	[Preserve]
	public class ActionBlockTriggerFall : ActionBaseBlockAction
	{
		// Token: 0x0600CBB3 RID: 52147 RVA: 0x004A645E File Offset: 0x004A465E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CBB4 RID: 52148 RVA: 0x004A7188 File Offset: 0x004A5388
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			for (int i = 0; i < blockChanges.Count; i++)
			{
				world.AddFallingBlock(blockChanges[i].blockValueRef, false);
			}
		}

		// Token: 0x0600CBB5 RID: 52149 RVA: 0x004A71BE File Offset: 0x004A53BE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockTriggerFall();
		}
	}
}
