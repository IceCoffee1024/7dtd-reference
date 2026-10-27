using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x020019A1 RID: 6561
	[Preserve]
	public class RequirementSandboxFloat : BaseOperationRequirement
	{
		// Token: 0x0600C9C7 RID: 51655 RVA: 0x00499C56 File Offset: 0x00497E56
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.Option);
		}

		// Token: 0x0600C9C8 RID: 51656 RVA: 0x00499C68 File Offset: 0x00497E68
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C9C9 RID: 51657 RVA: 0x00499C85 File Offset: 0x00497E85
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxFloat.PropSandboxOption, ref this.Option);
			properties.ParseString(RequirementSandboxFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C9CA RID: 51658 RVA: 0x00499CB0 File Offset: 0x00497EB0
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

		// Token: 0x040097C1 RID: 38849
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x040097C2 RID: 38850
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040097C3 RID: 38851
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";

		// Token: 0x040097C4 RID: 38852
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
