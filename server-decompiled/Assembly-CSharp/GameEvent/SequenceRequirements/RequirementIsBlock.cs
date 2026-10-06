using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001965 RID: 6501
	[Preserve]
	public class RequirementIsBlock : BaseRequirement
	{
		// Token: 0x0600C881 RID: 51329 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C882 RID: 51330 RVA: 0x00495ADB File Offset: 0x00493CDB
		public override bool CanPerform(Entity target)
		{
			return this.CheckBlock();
		}

		// Token: 0x0600C883 RID: 51331 RVA: 0x00495AE8 File Offset: 0x00493CE8
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

		// Token: 0x0600C884 RID: 51332 RVA: 0x00495B8E File Offset: 0x00493D8E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			if (properties.Values.ContainsKey(RequirementIsBlock.PropBlockName))
			{
				this.BlockName = properties.Values[RequirementIsBlock.PropBlockName];
			}
		}

		// Token: 0x0600C885 RID: 51333 RVA: 0x00495BBF File Offset: 0x00493DBF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementIsBlock
			{
				BlockName = this.BlockName,
				Invert = this.Invert
			};
		}

		// Token: 0x040096BD RID: 38589
		[PublicizedFrom(EAccessModifier.Protected)]
		public string BlockName = "";

		// Token: 0x040096BE RID: 38590
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropBlockName = "block_name";
	}
}
