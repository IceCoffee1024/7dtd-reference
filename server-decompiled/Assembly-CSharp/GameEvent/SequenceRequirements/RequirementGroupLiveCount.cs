using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001988 RID: 6536
	[Preserve]
	public class RequirementGroupLiveCount : BaseOperationRequirement
	{
		// Token: 0x0600C954 RID: 51540 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C955 RID: 51541 RVA: 0x00498C95 File Offset: 0x00496E95
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return this.Owner.GetEntityGroupLiveCount(this.targetGroup);
		}

		// Token: 0x0600C956 RID: 51542 RVA: 0x00498CAD File Offset: 0x00496EAD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C957 RID: 51543 RVA: 0x00498CC6 File Offset: 0x00496EC6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementGroupLiveCount.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(RequirementGroupLiveCount.PropCount, ref this.valueText);
		}

		// Token: 0x0600C958 RID: 51544 RVA: 0x00498CF1 File Offset: 0x00496EF1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementGroupLiveCount
			{
				Invert = this.Invert,
				operation = this.operation,
				targetGroup = this.targetGroup,
				valueText = this.valueText
			};
		}

		// Token: 0x0400978E RID: 38798
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x0400978F RID: 38799
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x04009790 RID: 38800
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x04009791 RID: 38801
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCount = "count";
	}
}
