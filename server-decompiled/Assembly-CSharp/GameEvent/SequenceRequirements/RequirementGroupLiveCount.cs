using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001957 RID: 6487
	[Preserve]
	public class RequirementGroupLiveCount : BaseOperationRequirement
	{
		// Token: 0x0600C840 RID: 51264 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C841 RID: 51265 RVA: 0x004952A1 File Offset: 0x004934A1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return this.Owner.GetEntityGroupLiveCount(this.targetGroup);
		}

		// Token: 0x0600C842 RID: 51266 RVA: 0x004952B9 File Offset: 0x004934B9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C843 RID: 51267 RVA: 0x004952D2 File Offset: 0x004934D2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementGroupLiveCount.PropTargetGroup, ref this.targetGroup);
			properties.ParseString(RequirementGroupLiveCount.PropCount, ref this.valueText);
		}

		// Token: 0x0600C844 RID: 51268 RVA: 0x004952FD File Offset: 0x004934FD
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

		// Token: 0x040096A5 RID: 38565
		[PublicizedFrom(EAccessModifier.Protected)]
		public string targetGroup = "";

		// Token: 0x040096A6 RID: 38566
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096A7 RID: 38567
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTargetGroup = "target_group";

		// Token: 0x040096A8 RID: 38568
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCount = "count";
	}
}
