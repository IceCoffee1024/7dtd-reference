using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x02001839 RID: 6201
	[Preserve]
	public class TwitchRequirementSandboxInt : BaseTwitchOperationRequirement
	{
		// Token: 0x0600BF28 RID: 48936 RVA: 0x0046EC6A File Offset: 0x0046CE6A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.sandboxOption);
		}

		// Token: 0x0600BF29 RID: 48937 RVA: 0x0046EC7C File Offset: 0x0046CE7C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.value;
		}

		// Token: 0x0600BF2A RID: 48938 RVA: 0x0046EC8C File Offset: 0x0046CE8C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxInt.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
			properties.ParseInt(TwitchRequirementSandboxInt.PropValue, ref this.value);
		}

		// Token: 0x0400902E RID: 36910
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x0400902F RID: 36911
		[PublicizedFrom(EAccessModifier.Protected)]
		public int value;

		// Token: 0x04009030 RID: 36912
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";

		// Token: 0x04009031 RID: 36913
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
