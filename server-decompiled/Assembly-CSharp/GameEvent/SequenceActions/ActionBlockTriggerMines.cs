using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A15 RID: 6677
	[Preserve]
	public class ActionBlockTriggerMines : ActionBaseBlockAction
	{
		// Token: 0x0600CBB7 RID: 52151 RVA: 0x004A645E File Offset: 0x004A465E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CBB8 RID: 52152 RVA: 0x004A71C8 File Offset: 0x004A53C8
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

		// Token: 0x0600CBB9 RID: 52153 RVA: 0x004A7229 File Offset: 0x004A5429
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			this.Properties.ParseBool(ActionBlockTriggerMines.PropUseTrigger, ref this.useTrigger);
		}

		// Token: 0x0600CBBA RID: 52154 RVA: 0x004A7248 File Offset: 0x004A5448
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockTriggerMines
			{
				useTrigger = this.useTrigger
			};
		}

		// Token: 0x04009A6D RID: 39533
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool useTrigger;

		// Token: 0x04009A6E RID: 39534
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropUseTrigger = "use_trigger";
	}
}
