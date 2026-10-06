using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001889 RID: 6281
	[Preserve]
	public class TwitchRequirementSandboxBool : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C1FC RID: 49660 RVA: 0x00478E5E File Offset: 0x0047705E
		public override bool CanPerform(Entity target)
		{
			if (SandboxOptionManager.GetBool(this.sandboxOption))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C1FD RID: 49661 RVA: 0x00478E80 File Offset: 0x00477080
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxBool.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
		}

		// Token: 0x0400920A RID: 37386
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x0400920B RID: 37387
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";
	}
}
