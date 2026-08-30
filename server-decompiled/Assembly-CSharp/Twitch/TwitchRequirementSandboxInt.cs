using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x0200185F RID: 6239
	[Preserve]
	public class TwitchRequirementSandboxInt : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C0AD RID: 49325 RVA: 0x00474A06 File Offset: 0x00472C06
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.sandboxOption);
		}

		// Token: 0x0600C0AE RID: 49326 RVA: 0x00474A18 File Offset: 0x00472C18
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.value;
		}

		// Token: 0x0600C0AF RID: 49327 RVA: 0x00474A28 File Offset: 0x00472C28
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxInt.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
			properties.ParseInt(TwitchRequirementSandboxInt.PropValue, ref this.value);
		}

		// Token: 0x0400914A RID: 37194
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x0400914B RID: 37195
		[PublicizedFrom(EAccessModifier.Protected)]
		public int value;

		// Token: 0x0400914C RID: 37196
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";

		// Token: 0x0400914D RID: 37197
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
