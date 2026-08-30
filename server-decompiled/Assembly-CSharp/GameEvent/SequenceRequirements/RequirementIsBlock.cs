using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001913 RID: 6419
	[Preserve]
	public class RequirementIsBlock : BaseRequirement
	{
		// Token: 0x0600C5A2 RID: 50594 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C5A3 RID: 50595 RVA: 0x0048B2A3 File Offset: 0x004894A3
		public override bool CanPerform(Entity target)
		{
			return this.CheckBlock();
		}

		// Token: 0x0600C5A4 RID: 50596 RVA: 0x0048B2B0 File Offset: 0x004894B0
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

		// Token: 0x0600C5A5 RID: 50597 RVA: 0x0048B356 File Offset: 0x00489556
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementIsBlock.PropBlockName))
			{
				this.BlockName = properties.Values[RequirementIsBlock.PropBlockName];
			}
		}

		// Token: 0x0600C5A6 RID: 50598 RVA: 0x0048B387 File Offset: 0x00489587
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementIsBlock
			{
				BlockName = this.BlockName,
				Invert = this.Invert
			};
		}

		// Token: 0x040094CE RID: 38094
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BlockName = "";

		// Token: 0x040094CF RID: 38095
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockName = "block_name";
	}
}
