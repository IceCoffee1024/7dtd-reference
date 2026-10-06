using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x0200188A RID: 6282
	[Preserve]
	public class TwitchRequirementSandboxFloat : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C200 RID: 49664 RVA: 0x00478EC7 File Offset: 0x004770C7
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.sandboxOption);
		}

		// Token: 0x0600C201 RID: 49665 RVA: 0x00478ED9 File Offset: 0x004770D9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.value;
		}

		// Token: 0x0600C202 RID: 49666 RVA: 0x00478EE8 File Offset: 0x004770E8
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxFloat.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
			properties.ParseFloat(TwitchRequirementSandboxFloat.PropValue, ref this.value);
		}

		// Token: 0x0400920C RID: 37388
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x0400920D RID: 37389
		[PublicizedFrom(EAccessModifier.Protected)]
		public float value;

		// Token: 0x0400920E RID: 37390
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";

		// Token: 0x0400920F RID: 37391
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
