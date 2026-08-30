using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001939 RID: 6457
	[Preserve]
	public class RequirementIsBlock : BaseRequirement
	{
		// Token: 0x0600C729 RID: 50985 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C72A RID: 50986 RVA: 0x004915EF File Offset: 0x0048F7EF
		public override bool CanPerform(Entity target)
		{
			return this.CheckBlock();
		}

		// Token: 0x0600C72B RID: 50987 RVA: 0x004915FC File Offset: 0x0048F7FC
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

		// Token: 0x0600C72C RID: 50988 RVA: 0x004916A2 File Offset: 0x0048F8A2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementIsBlock.PropBlockName))
			{
				this.BlockName = properties.Values[RequirementIsBlock.PropBlockName];
			}
		}

		// Token: 0x0600C72D RID: 50989 RVA: 0x004916D3 File Offset: 0x0048F8D3
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementIsBlock
			{
				BlockName = this.BlockName,
				Invert = this.Invert
			};
		}

		// Token: 0x040095F7 RID: 38391
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BlockName = "";

		// Token: 0x040095F8 RID: 38392
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockName = "block_name";
	}
}
