using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001944 RID: 6468
	[Preserve]
	public class RequirementSandboxFloat : BaseOperationRequirement
	{
		// Token: 0x0600C75B RID: 51035 RVA: 0x00491D76 File Offset: 0x0048FF76
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.Option);
		}

		// Token: 0x0600C75C RID: 51036 RVA: 0x00491D88 File Offset: 0x0048FF88
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C75D RID: 51037 RVA: 0x00491DA5 File Offset: 0x0048FFA5
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxFloat.PropSandboxOption, ref this.Option);
			properties.ParseString(RequirementSandboxFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C75E RID: 51038 RVA: 0x00491DD0 File Offset: 0x0048FFD0
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementSandboxFloat
			{
				Invert = this.Invert,
				operation = this.operation,
				Option = this.Option,
				valueText = this.valueText
			};
		}

		// Token: 0x04009612 RID: 38418
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x04009613 RID: 38419
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x04009614 RID: 38420
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";

		// Token: 0x04009615 RID: 38421
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
