using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001996 RID: 6550
	[Preserve]
	public class RequirementIsBlock : BaseRequirement
	{
		// Token: 0x0600C995 RID: 51605 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C996 RID: 51606 RVA: 0x004994CF File Offset: 0x004976CF
		public override bool CanPerform(Entity target)
		{
			return this.CheckBlock();
		}

		// Token: 0x0600C997 RID: 51607 RVA: 0x004994DC File Offset: 0x004976DC
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool CheckBlock()
		{
			if (this.Owner.TargetPosition == Vector3.zero)
			{
				return false;
			}
			WorldBase world = GameManager.Instance.World;
			Vector3i vector3i = new Vector3i(Utils.Fastfloor(this.Owner.TargetPosition.x), Utils.Fastfloor(this.Owner.TargetPosition.y), Utils.Fastfloor(this.Owner.TargetPosition.z));
			if (world.GetBlock(vector3i).Block.GetBlockName().EqualsCaseInsensitive(this.BlockName))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C998 RID: 51608 RVA: 0x00499582 File Offset: 0x00497782
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementIsBlock.PropBlockName))
			{
				this.BlockName = properties.Values[RequirementIsBlock.PropBlockName];
			}
		}

		// Token: 0x0600C999 RID: 51609 RVA: 0x004995B3 File Offset: 0x004977B3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementIsBlock
			{
				BlockName = this.BlockName,
				Invert = this.Invert
			};
		}

		// Token: 0x040097A6 RID: 38822
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BlockName = "";

		// Token: 0x040097A7 RID: 38823
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockName = "block_name";
	}
}
