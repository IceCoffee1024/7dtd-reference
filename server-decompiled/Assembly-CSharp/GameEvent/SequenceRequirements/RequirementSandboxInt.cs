using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001971 RID: 6513
	[Preserve]
	public class RequirementSandboxInt : BaseOperationRequirement
	{
		// Token: 0x0600C8B9 RID: 51385 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C8BA RID: 51386 RVA: 0x0049631C File Offset: 0x0049451C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetInt(this.Option);
		}

		// Token: 0x0600C8BB RID: 51387 RVA: 0x0049632E File Offset: 0x0049452E
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C8BC RID: 51388 RVA: 0x00496347 File Offset: 0x00494547
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxInt.PropSandboxOption, ref this.Option);
			properties.ParseString(RequirementSandboxInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C8BD RID: 51389 RVA: 0x00496372 File Offset: 0x00494572
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementSandboxInt
			{
				Invert = this.Invert,
				operation = this.operation,
				Option = this.Option,
				valueText = this.valueText
			};
		}

		// Token: 0x040096DC RID: 38620
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x040096DD RID: 38621
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";

		// Token: 0x040096DE RID: 38622
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096DF RID: 38623
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
