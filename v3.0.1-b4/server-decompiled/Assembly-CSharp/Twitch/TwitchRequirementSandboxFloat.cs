using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001838 RID: 6200
	[Preserve]
	public class TwitchRequirementSandboxFloat : BaseTwitchOperationRequirement
	{
		// Token: 0x0600BF23 RID: 48931 RVA: 0x0046EBEF File Offset: 0x0046CDEF
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.sandboxOption);
		}

		// Token: 0x0600BF24 RID: 48932 RVA: 0x0046EC01 File Offset: 0x0046CE01
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.value;
		}

		// Token: 0x0600BF25 RID: 48933 RVA: 0x0046EC10 File Offset: 0x0046CE10
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxFloat.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
			properties.ParseFloat(TwitchRequirementSandboxFloat.PropValue, ref this.value);
		}

		// Token: 0x0400902A RID: 36906
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x0400902B RID: 36907
		[PublicizedFrom(EAccessModifier.Protected)]
		public float value;

		// Token: 0x0400902C RID: 36908
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";

		// Token: 0x0400902D RID: 36909
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
