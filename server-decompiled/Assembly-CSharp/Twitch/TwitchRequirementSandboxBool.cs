using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x020018BA RID: 6330
	[Preserve]
	public class TwitchRequirementSandboxBool : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C310 RID: 49936 RVA: 0x0047C80A File Offset: 0x0047AA0A
		public override bool CanPerform(Entity target)
		{
			if (SandboxOptionManager.GetBool(this.sandboxOption))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600C311 RID: 49937 RVA: 0x0047C82C File Offset: 0x0047AA2C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxBool.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
		}

		// Token: 0x040092F3 RID: 37619
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x040092F4 RID: 37620
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";
	}
}
