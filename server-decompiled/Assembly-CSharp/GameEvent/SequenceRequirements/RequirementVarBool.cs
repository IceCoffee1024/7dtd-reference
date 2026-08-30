using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001946 RID: 6470
	[Preserve]
	public class RequirementVarBool : BaseRequirement
	{
		// Token: 0x0600C768 RID: 51048 RVA: 0x00491EE8 File Offset: 0x004900E8
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

		// Token: 0x0600C769 RID: 51049 RVA: 0x00491F38 File Offset: 0x00490138
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarBool.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarBool.PropValue, ref this.valueText);
		}

		// Token: 0x0600C76A RID: 51050 RVA: 0x00491F63 File Offset: 0x00490163
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

		// Token: 0x0400961A RID: 38426
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x0400961B RID: 38427
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x0400961C RID: 38428
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x0400961D RID: 38429
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
