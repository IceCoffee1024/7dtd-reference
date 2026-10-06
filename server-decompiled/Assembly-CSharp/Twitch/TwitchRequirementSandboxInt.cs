using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace Twitch
{
	// Token: 0x0200188B RID: 6283
	[Preserve]
	public class TwitchRequirementSandboxInt : BaseTwitchOperationRequirement
	{
		// Token: 0x0600C205 RID: 49669 RVA: 0x00478F42 File Offset: 0x00477142
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.sandboxOption);
		}

		// Token: 0x0600C206 RID: 49670 RVA: 0x00478F54 File Offset: 0x00477154
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.value;
		}

		// Token: 0x0600C207 RID: 49671 RVA: 0x00478F64 File Offset: 0x00477164
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			string text = "";
			properties.ParseString(TwitchRequirementSandboxInt.PropSandbox, ref text);
			this.sandboxOption = Enum.Parse<global::SandboxOptions.SandboxOptions>(text);
			properties.ParseInt(TwitchRequirementSandboxInt.PropValue, ref this.value);
		}

		// Token: 0x04009210 RID: 37392
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions sandboxOption;

		// Token: 0x04009211 RID: 37393
		[PublicizedFrom(EAccessModifier.Protected)]
		public int value;

		// Token: 0x04009212 RID: 37394
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandbox = "sandbox_option";

		// Token: 0x04009213 RID: 37395
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
