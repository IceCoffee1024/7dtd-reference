using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001945 RID: 6469
	[Preserve]
	public class RequirementSandboxInt : BaseOperationRequirement
	{
		// Token: 0x0600C761 RID: 51041 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C762 RID: 51042 RVA: 0x00491E30 File Offset: 0x00490030
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetInt(this.Option);
		}

		// Token: 0x0600C763 RID: 51043 RVA: 0x00491E42 File Offset: 0x00490042
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C764 RID: 51044 RVA: 0x00491E5B File Offset: 0x0049005B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxInt.PropSandboxOption, ref this.Option);
			properties.ParseString(RequirementSandboxInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C765 RID: 51045 RVA: 0x00491E86 File Offset: 0x00490086
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementSandboxInt
			{
				Invert = this.Invert,
				operation = this.operation,
				Option = this.Option,
				valueText = this.valueText
			};
		}

		// Token: 0x04009616 RID: 38422
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x04009617 RID: 38423
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";

		// Token: 0x04009618 RID: 38424
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x04009619 RID: 38425
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
