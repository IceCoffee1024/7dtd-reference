using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001972 RID: 6514
	[Preserve]
	public class RequirementVarBool : BaseRequirement
	{
		// Token: 0x0600C8C0 RID: 51392 RVA: 0x004963D4 File Offset: 0x004945D4
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

		// Token: 0x0600C8C1 RID: 51393 RVA: 0x00496424 File Offset: 0x00494624
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarBool.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarBool.PropValue, ref this.valueText);
		}

		// Token: 0x0600C8C2 RID: 51394 RVA: 0x0049644F File Offset: 0x0049464F
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

		// Token: 0x040096E0 RID: 38624
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040096E1 RID: 38625
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096E2 RID: 38626
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040096E3 RID: 38627
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
