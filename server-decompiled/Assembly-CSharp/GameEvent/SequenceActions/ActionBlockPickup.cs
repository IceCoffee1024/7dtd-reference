using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E5 RID: 6629
	[Preserve]
	public class ActionBlockPickup : ActionBaseBlockAction
	{
		// Token: 0x0600CA4B RID: 51787 RVA: 0x004A1FBA File Offset: 0x004A01BA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CA4C RID: 51788 RVA: 0x004A2880 File Offset: 0x004A0A80
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			for (int i = 0; i < blockChanges.Count; i++)
			{
				BlockChangeInfo blockChangeInfo = blockChanges[i];
				blockChangeInfo.blockValue.Block.PickupOrDrop(world, blockChangeInfo.blockValueRef, blockChangeInfo.blockValue, base.Owner.Target as EntityPlayer, true);
			}
		}

		// Token: 0x0600CA4D RID: 51789 RVA: 0x004A28D9 File Offset: 0x004A0AD9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockPickup();
		}
	}
}
