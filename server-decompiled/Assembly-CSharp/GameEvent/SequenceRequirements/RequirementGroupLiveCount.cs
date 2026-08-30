using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001905 RID: 6405
	[Preserve]
	public class RequirementGroupLiveCount : BaseOperationRequirement
	{
		// Token: 0x0600C561 RID: 50529 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C562 RID: 50530 RVA: 0x0048AA69 File Offset: 0x00488C69
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return this.Owner.GetEntityGroupLiveCount(this.targetGroup);
		}

		// Token: 0x0600C563 RID: 50531 RVA: 0x0048AA81 File Offset: 0x00488C81
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C564 RID: 50532 RVA: 0x0048AA9A File Offset: 0x00488C9A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementGroupLiveCount.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(RequirementGroupLiveCount.PropCount, ref this.valueText);
		}

		// Token: 0x0600C565 RID: 50533 RVA: 0x0048AAC5 File Offset: 0x00488CC5
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

		// Token: 0x040094B6 RID: 38070
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040094B7 RID: 38071
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094B8 RID: 38072
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040094B9 RID: 38073
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCount = "count";
	}
}
