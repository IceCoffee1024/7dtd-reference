using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E9 RID: 6633
	[Preserve]
	public class ActionBlockTriggerMines : ActionBaseBlockAction
	{
		// Token: 0x0600CA5F RID: 51807 RVA: 0x004A1FBA File Offset: 0x004A01BA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (!blockValue.isair)
			{
				return new BlockChangeInfo(currentPos, blockValue, true);
			}
			return null;
		}

		// Token: 0x0600CA60 RID: 51808 RVA: 0x004A2D24 File Offset: 0x004A0F24
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

		// Token: 0x0600CA61 RID: 51809 RVA: 0x004A2D85 File Offset: 0x004A0F85
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			this.Properties.ParseBool(ActionBlockTriggerMines.PropUseTrigger, ref this.useTrigger);
		}

		// Token: 0x0600CA62 RID: 51810 RVA: 0x004A2DA4 File Offset: 0x004A0FA4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockTriggerMines
			{
				useTrigger = this.useTrigger
			};
		}

		// Token: 0x040099A7 RID: 39335
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool useTrigger;

		// Token: 0x040099A8 RID: 39336
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropUseTrigger = "use_trigger";
	}
}
