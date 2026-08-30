using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BF RID: 6591
	[Preserve]
	public class ActionBlockReplace : ActionBaseBlockAction
	{
		// Token: 0x0600C8C6 RID: 51398 RVA: 0x0049C4EC File Offset: 0x0049A6EC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool CheckValid(World world, Vector3i currentPos)
		{
			BlockValue block = world.GetBlock(currentPos + Vector3i.down);
			return !block.isair && !block.Block.IsTerrainDecoration;
		}

		// Token: 0x0600C8C7 RID: 51399 RVA: 0x0049C528 File Offset: 0x0049A728
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BlockChangeInfo UpdateBlock(World world, Vector3i currentPos, BlockValue blockValue)
		{
			if (this.blockTo == null)
			{
				return null;
			}
			if (this.emptyOnly && !blockValue.isair)
			{
				return null;
			}
			if (!blockValue.Block.blockMaterial.CanDestroy)
			{
				return null;
			}
			BlockValue blockValue2 = Block.GetBlockValue(this.blockTo[this.random.RandomRange(0, this.blockTo.Length)], false);
			if (blockValue.type != blockValue2.type)
			{
				return new BlockChangeInfo(currentPos, blockValue2, true);
			}
			return null;
		}

		// Token: 0x0600C8C8 RID: 51400 RVA: 0x0049C5A8 File Offset: 0x0049A7A8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			this.Properties.ParseString(ActionBlockReplace.PropBlockTo, ref text);
			if (text != "")
			{
				this.blockTo = text.Split(',', StringSplitOptions.None);
			}
			properties.ParseBool(ActionBlockReplace.PropEmptyOnly, ref this.emptyOnly);
		}

		// Token: 0x0600C8C9 RID: 51401 RVA: 0x0049C601 File Offset: 0x0049A801
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockReplace
			{
				blockTo = this.blockTo,
				emptyOnly = this.emptyOnly
			};
		}

		// Token: 0x0400986B RID: 39019
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] blockTo;

		// Token: 0x0400986C RID: 39020
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool emptyOnly;

		// Token: 0x0400986D RID: 39021
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockTo = "block_to";

		// Token: 0x0400986E RID: 39022
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEmptyOnly = "empty_only";
	}
}
