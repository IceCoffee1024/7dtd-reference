using System;
using SandboxOptions;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x020019A2 RID: 6562
	[Preserve]
	public class RequirementSandboxInt : BaseOperationRequirement
	{
		// Token: 0x0600C9CD RID: 51661 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C9CE RID: 51662 RVA: 0x00499D10 File Offset: 0x00497F10
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			return SandboxOptionManager.GetInt(this.Option);
		}

		// Token: 0x0600C9CF RID: 51663 RVA: 0x00499D22 File Offset: 0x00497F22
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C9D0 RID: 51664 RVA: 0x00499D3B File Offset: 0x00497F3B
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseEnum<global::SandboxOptions.SandboxOptions>(RequirementSandboxInt.PropSandboxOption, ref this.Option);
			properties.ParseString(RequirementSandboxInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C9D1 RID: 51665 RVA: 0x00499D66 File Offset: 0x00497F66
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

		// Token: 0x040097C5 RID: 38853
		[PublicizedFrom(EAccessModifier.Protected)]
		public global::SandboxOptions.SandboxOptions Option = global::SandboxOptions.SandboxOptions.Max;

		// Token: 0x040097C6 RID: 38854
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropSandboxOption = "option";

		// Token: 0x040097C7 RID: 38855
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040097C8 RID: 38856
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
