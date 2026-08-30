using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019C2 RID: 6594
	[Preserve]
	public class ActionBlockTriggerMines : ActionBaseBlockAction
	{
		// Token: 0x0600C8D6 RID: 51414 RVA: 0x0049BC56 File Offset: 0x00499E56
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600C8D7 RID: 51415 RVA: 0x0049C930 File Offset: 0x0049AB30
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

		// Token: 0x0600C8D8 RID: 51416 RVA: 0x0049C991 File Offset: 0x0049AB91
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			this.Properties.ParseBool(ActionBlockTriggerMines.PropUseTrigger, ref this.useTrigger);
		}

		// Token: 0x0600C8D9 RID: 51417 RVA: 0x0049C9B0 File Offset: 0x0049ABB0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockTriggerMines
			{
				useTrigger = this.useTrigger
			};
		}

		// Token: 0x04009876 RID: 39030
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool useTrigger;

		// Token: 0x04009877 RID: 39031
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropUseTrigger = "use_trigger";
	}
}
