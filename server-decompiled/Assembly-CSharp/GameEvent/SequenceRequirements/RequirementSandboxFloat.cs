using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001970 RID: 6512
	[Preserve]
	public class RequirementSandboxFloat : BaseOperationRequirement
	{
		// Token: 0x0600C8B3 RID: 51379 RVA: 0x00496262 File Offset: 0x00494462
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetFloat(this.Option);
		}

		// Token: 0x0600C8B4 RID: 51380 RVA: 0x00496274 File Offset: 0x00494474
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C8B5 RID: 51381 RVA: 0x00496291 File Offset: 0x00494491
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxFloat.PropSandboxOption, ref this.Option);
			properties.ParseString(RequirementSandboxFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C8B6 RID: 51382 RVA: 0x004962BC File Offset: 0x004944BC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementSandboxFloat
			{
				Invert = this.Invert,
				operation = this.operation,
				Option = this.Option,
				valueText = this.valueText
			};
		}

		// Token: 0x040096D8 RID: 38616
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x040096D9 RID: 38617
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096DA RID: 38618
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";

		// Token: 0x040096DB RID: 38619
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
