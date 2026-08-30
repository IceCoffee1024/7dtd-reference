using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001837 RID: 6199
	[Preserve]
	public class TwitchRequirementSandboxBool : BaseTwitchOperationRequirement
	{
		// Token: 0x0600BF1F RID: 48927 RVA: 0x0046EB86 File Offset: 0x0046CD86
		public override bool CanPerform(Entity target)
		{
			if (SandboxOptionManager.GetBool(this.sandboxOption))
			{
				return !this.Invert;
			}
			return this.Invert;
		}

		// Token: 0x0600BF20 RID: 48928 RVA: 0x0046EBA8 File Offset: 0x0046CDA8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxBool.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
		}

		// Token: 0x04009028 RID: 36904
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x04009029 RID: 36905
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";
	}
}
