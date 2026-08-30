using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x0200185D RID: 6237
	[Preserve]
	public class TwitchRequirementSandboxBool : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C0A4 RID: 49316 RVA: 0x00474922 File Offset: 0x00472B22
		public override bool CanPerform(Entity target)
		{
			if (SandboxOptionManager.GetBool(this.sandboxOption))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C0A5 RID: 49317 RVA: 0x00474944 File Offset: 0x00472B44
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxBool.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
		}

		// Token: 0x04009144 RID: 37188
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x04009145 RID: 37189
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";
	}
}
