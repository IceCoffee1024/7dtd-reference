using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x020018BB RID: 6331
	[Preserve]
	public class TwitchRequirementSandboxFloat : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C314 RID: 49940 RVA: 0x0047C873 File Offset: 0x0047AA73
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.sandboxOption);
		}

		// Token: 0x0600C315 RID: 49941 RVA: 0x0047C885 File Offset: 0x0047AA85
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.value;
		}

		// Token: 0x0600C316 RID: 49942 RVA: 0x0047C894 File Offset: 0x0047AA94
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxFloat.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
			properties.ParseFloat(TwitchRequirementSandboxFloat.PropValue, ref this.value);
		}

		// Token: 0x040092F5 RID: 37621
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x040092F6 RID: 37622
		[PublicizedFrom(EAccessModifier.Protected)]
		public float value;

		// Token: 0x040092F7 RID: 37623
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";

		// Token: 0x040092F8 RID: 37624
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
