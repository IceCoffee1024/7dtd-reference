using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BE RID: 6590
	[Preserve]
	public class ActionBlockPickup : ActionBaseBlockAction
	{
		// Token: 0x0600C8C2 RID: 51394 RVA: 0x0049BC56 File Offset: 0x00499E56
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600C8C3 RID: 51395 RVA: 0x0049C48C File Offset: 0x0049A68C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			for (int i = 0; i < blockChanges.Count; i++)
			{
				BlockChangeInfo blockChangeInfo = blockChanges[i];
				blockChangeInfo.blockValue.Block.PickupOrDrop(world, blockChangeInfo.blockValueRef, blockChangeInfo.blockValue, base.Owner.Target as EntityPlayer, true);
			}
		}

		// Token: 0x0600C8C4 RID: 51396 RVA: 0x0049C4E5 File Offset: 0x0049A6E5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockPickup();
		}
	}
}
