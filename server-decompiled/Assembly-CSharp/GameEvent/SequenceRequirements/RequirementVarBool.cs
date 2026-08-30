using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001920 RID: 6432
	[Preserve]
	public class RequirementVarBool : BaseRequirement
	{
		// Token: 0x0600C5E1 RID: 50657 RVA: 0x0048BB9C File Offset: 0x00489D9C
		public override bool CanPerform(Entity target)
		{
			if (target is EntityAlive)
			{
				bool flag = false;
				this.Owner.EventVariables.ParseBool(this.varName, ref flag);
				if (flag == StringParsers.ParseBool(this.valueText, 0, -1, true))
				{
					return !this.Invert;
				}
			}
			return this.Invert;
		}

		// Token: 0x0600C5E2 RID: 50658 RVA: 0x0048BBEC File Offset: 0x00489DEC
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarBool.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarBool.PropValue, ref this.valueText);
		}

		// Token: 0x0600C5E3 RID: 50659 RVA: 0x0048BC17 File Offset: 0x00489E17
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementVarBool
			{
				Invert = this.Invert,
				varName = this.varName,
				valueText = this.valueText
			};
		}

		// Token: 0x040094F1 RID: 38129
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040094F2 RID: 38130
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094F3 RID: 38131
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040094F4 RID: 38132
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
