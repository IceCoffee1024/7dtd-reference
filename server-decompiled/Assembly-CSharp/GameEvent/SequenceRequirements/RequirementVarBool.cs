using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x020019A3 RID: 6563
	[Preserve]
	public class RequirementVarBool : BaseRequirement
	{
		// Token: 0x0600C9D4 RID: 51668 RVA: 0x00499DC8 File Offset: 0x00497FC8
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

		// Token: 0x0600C9D5 RID: 51669 RVA: 0x00499E18 File Offset: 0x00498018
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarBool.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarBool.PropValue, ref this.valueText);
		}

		// Token: 0x0600C9D6 RID: 51670 RVA: 0x00499E43 File Offset: 0x00498043
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

		// Token: 0x040097C9 RID: 38857
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040097CA RID: 38858
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040097CB RID: 38859
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040097CC RID: 38860
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
