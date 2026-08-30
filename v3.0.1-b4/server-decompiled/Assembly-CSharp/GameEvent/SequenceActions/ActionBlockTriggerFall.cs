using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C1 RID: 6593
	[Preserve]
	public class ActionBlockTriggerFall : ActionBaseBlockAction
	{
		// Token: 0x0600C8D2 RID: 51410 RVA: 0x0049BC56 File Offset: 0x00499E56
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600C8D3 RID: 51411 RVA: 0x0049C8F0 File Offset: 0x0049AAF0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			for (int i = 0; i < blockChanges.Count; i++)
			{
				world.AddFallingBlock(blockChanges[i].blockValueRef, false);
			}
		}

		// Token: 0x0600C8D4 RID: 51412 RVA: 0x0049C926 File Offset: 0x0049AB26
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockTriggerFall();
		}
	}
}
