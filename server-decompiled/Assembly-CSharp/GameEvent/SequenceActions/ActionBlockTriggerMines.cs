using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A46 RID: 6726
	[Preserve]
	public class ActionBlockTriggerMines : ActionBaseBlockAction
	{
		// Token: 0x0600CCCB RID: 52427 RVA: 0x004A9F5A File Offset: 0x004A815A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CCCC RID: 52428 RVA: 0x004AACC4 File Offset: 0x004A8EC4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void ProcessChanges(World world, List<BlockChangeInfo> blockChanges)
		{
			for (int i = 0; i < blockChanges.Count; i++)
			{
				BlockMine blockMine = blockChanges[i].blockValue.Block as BlockMine;
				if (blockMine != null)
				{
					blockMine.TriggerMine(base.Owner.Target, world, blockChanges[i].blockValueRef, this.useTrigger);
				}
			}
		}

		// Token: 0x0600CCCD RID: 52429 RVA: 0x004AAD25 File Offset: 0x004A8F25
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			this.Properties.ParseBool(ActionBlockTriggerMines.PropUseTrigger, ref this.useTrigger);
		}

		// Token: 0x0600CCCE RID: 52430 RVA: 0x004AAD44 File Offset: 0x004A8F44
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockTriggerMines
			{
				useTrigger = this.useTrigger
			};
		}

		// Token: 0x04009B56 RID: 39766
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool useTrigger;

		// Token: 0x04009B57 RID: 39767
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropUseTrigger = "use_trigger";
	}
}
