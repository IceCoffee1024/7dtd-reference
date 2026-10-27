using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x020019A0 RID: 6560
	[Preserve]
	public class RequirementSandboxBool : BaseRequirement
	{
		// Token: 0x0600C9C1 RID: 51649 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C9C2 RID: 51650 RVA: 0x00499BDF File Offset: 0x00497DDF
		public override bool CanPerform(Entity target)
		{
			if (SandboxOptionManager.GetBool(this.Option))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C9C3 RID: 51651 RVA: 0x00499BFE File Offset: 0x00497DFE
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxBool.PropSandboxOption, ref this.Option);
		}

		// Token: 0x0600C9C4 RID: 51652 RVA: 0x00499C18 File Offset: 0x00497E18
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementSandboxBool
			{
				Invert = this.Invert,
				Option = this.Option
			};
		}

		// Token: 0x040097BF RID: 38847
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x040097C0 RID: 38848
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";
	}
}
