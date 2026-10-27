using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x020018BC RID: 6332
	[Preserve]
	public class TwitchRequirementSandboxInt : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C319 RID: 49945 RVA: 0x0047C8EE File Offset: 0x0047AAEE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.sandboxOption);
		}

		// Token: 0x0600C31A RID: 49946 RVA: 0x0047C900 File Offset: 0x0047AB00
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.value;
		}

		// Token: 0x0600C31B RID: 49947 RVA: 0x0047C910 File Offset: 0x0047AB10
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxInt.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
			properties.ParseInt(TwitchRequirementSandboxInt.PropValue, ref this.value);
		}

		// Token: 0x040092F9 RID: 37625
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x040092FA RID: 37626
		[PublicizedFrom(EAccessModifier.Protected)]
		public int value;

		// Token: 0x040092FB RID: 37627
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";

		// Token: 0x040092FC RID: 37628
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
