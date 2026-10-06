using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A12 RID: 6674
	[Preserve]
	public class ActionBlockReplace : ActionBaseBlockAction
	{
		// Token: 0x0600CBA7 RID: 52135 RVA: 0x004A6D84 File Offset: 0x004A4F84
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool CheckValid(World world, Vector3i currentPos)
		{
			BlockValue block = world.GetBlock(currentPos + Vector3i.down);
			return !block.isair && !block.Block.IsTerrainDecoration;
		}

		// Token: 0x0600CBA8 RID: 52136 RVA: 0x004A6DC0 File Offset: 0x004A4FC0
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

		// Token: 0x0600CBA9 RID: 52137 RVA: 0x004A6E40 File Offset: 0x004A5040
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

		// Token: 0x0600CBAA RID: 52138 RVA: 0x004A6E99 File Offset: 0x004A5099
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockReplace
			{
				blockTo = this.blockTo,
				emptyOnly = this.emptyOnly
			};
		}

		// Token: 0x04009A62 RID: 39522
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] blockTo;

		// Token: 0x04009A63 RID: 39523
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool emptyOnly;

		// Token: 0x04009A64 RID: 39524
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockTo = "block_to";

		// Token: 0x04009A65 RID: 39525
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEmptyOnly = "empty_only";
	}
}
