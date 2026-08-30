using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001943 RID: 6467
	[Preserve]
	public class RequirementSandboxBool : BaseRequirement
	{
		// Token: 0x0600C755 RID: 51029 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C756 RID: 51030 RVA: 0x00491CFF File Offset: 0x0048FEFF
		public override bool CanPerform(Entity target)
		{
			if (SandboxOptionManager.GetBool(this.Option))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C757 RID: 51031 RVA: 0x00491D1E File Offset: 0x0048FF1E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxBool.PropSandboxOption, ref this.Option);
		}

		// Token: 0x0600C758 RID: 51032 RVA: 0x00491D38 File Offset: 0x0048FF38
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementSandboxBool
			{
				Invert = this.Invert,
				Option = this.Option
			};
		}

		// Token: 0x04009610 RID: 38416
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x04009611 RID: 38417
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";
	}
}
