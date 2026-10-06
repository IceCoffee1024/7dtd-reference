using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A11 RID: 6673
	[Preserve]
	public class ActionBlockPickup : ActionBaseBlockAction
	{
		// Token: 0x0600CBA3 RID: 52131 RVA: 0x004A645E File Offset: 0x004A465E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CBA4 RID: 52132 RVA: 0x004A6D24 File Offset: 0x004A4F24
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			for (int i = 0; i < blockChanges.Count; i++)
			{
				BlockChangeInfo blockChangeInfo = blockChanges[i];
				blockChangeInfo.blockValue.Block.PickupOrDrop(world, blockChangeInfo.blockValueRef, blockChangeInfo.blockValue, base.Owner.Target as EntityPlayer, true);
			}
		}

		// Token: 0x0600CBA5 RID: 52133 RVA: 0x004A6D7D File Offset: 0x004A4F7D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockPickup();
		}
	}
}
