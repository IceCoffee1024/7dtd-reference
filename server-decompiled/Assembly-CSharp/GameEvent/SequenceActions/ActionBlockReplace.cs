using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A43 RID: 6723
	[Preserve]
	public class ActionBlockReplace : ActionBaseBlockAction
	{
		// Token: 0x0600CCBB RID: 52411 RVA: 0x004AA880 File Offset: 0x004A8A80
		[PublicizedFrom(EAccessModifier.Protected)]
		public override bool CheckValid(World world, Vector3i currentPos)
		{
			BlockValue block = world.GetBlock(currentPos + Vector3i.down);
			return !block.isair && !block.Block.IsTerrainDecoration;
		}

		// Token: 0x0600CCBC RID: 52412 RVA: 0x004AA8BC File Offset: 0x004A8ABC
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

		// Token: 0x0600CCBD RID: 52413 RVA: 0x004AA93C File Offset: 0x004A8B3C
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

		// Token: 0x0600CCBE RID: 52414 RVA: 0x004AA995 File Offset: 0x004A8B95
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionBlockReplace
			{
				blockTo = this.blockTo,
				emptyOnly = this.emptyOnly
			};
		}

		// Token: 0x04009B4B RID: 39755
		[PublicizedFrom(EAccessModifier.Protected)]
		public string[] blockTo;

		// Token: 0x04009B4C RID: 39756
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool emptyOnly;

		// Token: 0x04009B4D RID: 39757
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockTo = "block_to";

		// Token: 0x04009B4E RID: 39758
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropEmptyOnly = "empty_only";
	}
}
