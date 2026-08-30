using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200191E RID: 6430
	[Preserve]
	public class RequirementSandboxFloat : BaseOperationRequirement
	{
		// Token: 0x0600C5D4 RID: 50644 RVA: 0x0048BA2A File Offset: 0x00489C2A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.Option);
		}

		// Token: 0x0600C5D5 RID: 50645 RVA: 0x0048BA3C File Offset: 0x00489C3C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C5D6 RID: 50646 RVA: 0x0048BA59 File Offset: 0x00489C59
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxFloat.PropSandboxOption, ref this.Option);
			properties.ParseString(RequirementSandboxFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C5D7 RID: 50647 RVA: 0x0048BA84 File Offset: 0x00489C84
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

		// Token: 0x040094E9 RID: 38121
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x040094EA RID: 38122
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094EB RID: 38123
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";

		// Token: 0x040094EC RID: 38124
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
