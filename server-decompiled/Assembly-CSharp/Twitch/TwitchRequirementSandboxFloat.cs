using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x0200185E RID: 6238
	[Preserve]
	public class TwitchRequirementSandboxFloat : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C0A8 RID: 49320 RVA: 0x0047498B File Offset: 0x00472B8B
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.sandboxOption);
		}

		// Token: 0x0600C0A9 RID: 49321 RVA: 0x0047499D File Offset: 0x00472B9D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.value;
		}

		// Token: 0x0600C0AA RID: 49322 RVA: 0x004749AC File Offset: 0x00472BAC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxFloat.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
			properties.ParseFloat(TwitchRequirementSandboxFloat.PropValue, ref this.value);
		}

		// Token: 0x04009146 RID: 37190
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x04009147 RID: 37191
		[PublicizedFrom(EAccessModifier.Protected)]
		public float value;

		// Token: 0x04009148 RID: 37192
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";

		// Token: 0x04009149 RID: 37193
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
