using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x0200196F RID: 6511
	[Preserve]
	public class RequirementSandboxBool : BaseRequirement
	{
		// Token: 0x0600C8AD RID: 51373 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C8AE RID: 51374 RVA: 0x004961EB File Offset: 0x004943EB
		public override bool CanPerform(Entity target)
		{
			if (SandboxOptionManager.GetBool(this.Option))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C8AF RID: 51375 RVA: 0x0049620A File Offset: 0x0049440A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxBool.PropSandboxOption, ref this.Option);
		}

		// Token: 0x0600C8B0 RID: 51376 RVA: 0x00496224 File Offset: 0x00494424
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementSandboxBool
			{
				Invert = this.Invert,
				Option = this.Option
			};
		}

		// Token: 0x040096D6 RID: 38614
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x040096D7 RID: 38615
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";
	}
}
