using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200191F RID: 6431
	[Preserve]
	public class RequirementSandboxInt : BaseOperationRequirement
	{
		// Token: 0x0600C5DA RID: 50650 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C5DB RID: 50651 RVA: 0x0048BAE4 File Offset: 0x00489CE4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetInt(this.Option);
		}

		// Token: 0x0600C5DC RID: 50652 RVA: 0x0048BAF6 File Offset: 0x00489CF6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C5DD RID: 50653 RVA: 0x0048BB0F File Offset: 0x00489D0F
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxInt.PropSandboxOption, ref this.Option);
			properties.ParseString(RequirementSandboxInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C5DE RID: 50654 RVA: 0x0048BB3A File Offset: 0x00489D3A
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

		// Token: 0x040094ED RID: 38125
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x040094EE RID: 38126
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";

		// Token: 0x040094EF RID: 38127
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094F0 RID: 38128
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
