using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A42 RID: 6722
	[Preserve]
	public class ActionBlockPickup : ActionBaseBlockAction
	{
		// Token: 0x0600CCB7 RID: 52407 RVA: 0x004A9F5A File Offset: 0x004A815A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CCB8 RID: 52408 RVA: 0x004AA820 File Offset: 0x004A8A20
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			for (int i = 0; i < blockChanges.Count; i++)
			{
				BlockChangeInfo blockChangeInfo = blockChanges[i];
				blockChangeInfo.blockValue.Block.PickupOrDrop(world, blockChangeInfo.blockValueRef, blockChangeInfo.blockValue, base.Owner.Target as EntityPlayer, true);
			}
		}

		// Token: 0x0600CCB9 RID: 52409 RVA: 0x004AA879 File Offset: 0x004A8A79
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockPickup();
		}
	}
}
