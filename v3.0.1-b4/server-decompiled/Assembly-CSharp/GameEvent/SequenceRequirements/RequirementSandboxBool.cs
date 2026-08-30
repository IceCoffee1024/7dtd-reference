using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200191D RID: 6429
	[Preserve]
	public class RequirementSandboxBool : BaseRequirement
	{
		// Token: 0x0600C5CE RID: 50638 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C5CF RID: 50639 RVA: 0x0048B9B3 File Offset: 0x00489BB3
		public override bool CanPerform(Entity target)
		{
			if (SandboxOptionManager.GetBool(this.Option))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C5D0 RID: 50640 RVA: 0x0048B9D2 File Offset: 0x00489BD2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxBool.PropSandboxOption, ref this.Option);
		}

		// Token: 0x0600C5D1 RID: 50641 RVA: 0x0048B9EC File Offset: 0x00489BEC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementSandboxBool
			{
				Invert = this.Invert,
				Option = this.Option
			};
		}

		// Token: 0x040094E7 RID: 38119
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x040094E8 RID: 38120
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";
	}
}
