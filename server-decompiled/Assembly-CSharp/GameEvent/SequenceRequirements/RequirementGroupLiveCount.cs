using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200192B RID: 6443
	[Preserve]
	public class RequirementGroupLiveCount : BaseOperationRequirement
	{
		// Token: 0x0600C6E8 RID: 50920 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C6E9 RID: 50921 RVA: 0x00490DB5 File Offset: 0x0048EFB5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return this.Owner.GetEntityGroupLiveCount(this.targetGroup);
		}

		// Token: 0x0600C6EA RID: 50922 RVA: 0x00490DCD File Offset: 0x0048EFCD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C6EB RID: 50923 RVA: 0x00490DE6 File Offset: 0x0048EFE6
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementGroupLiveCount.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(RequirementGroupLiveCount.PropCount, ref this.valueText);
		}

		// Token: 0x0600C6EC RID: 50924 RVA: 0x00490E11 File Offset: 0x0048F011
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

		// Token: 0x040095DF RID: 38367
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040095E0 RID: 38368
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040095E1 RID: 38369
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040095E2 RID: 38370
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCount = "count";
	}
}
