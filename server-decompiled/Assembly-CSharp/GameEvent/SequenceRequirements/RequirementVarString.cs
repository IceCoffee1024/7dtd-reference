using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x020019A6 RID: 6566
	[Preserve]
	public class RequirementVarString : BaseOperationRequirement
	{
		// Token: 0x0600C9E7 RID: 51687 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C9E8 RID: 51688 RVA: 0x0049A008 File Offset: 0x00498208
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			string empty = string.Empty;
			this.Owner.EventVariables.ParseVarString(this.varName, ref empty);
			return empty;
		}

		// Token: 0x0600C9E9 RID: 51689 RVA: 0x0049A034 File Offset: 0x00498234
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.valueText;
		}

		// Token: 0x0600C9EA RID: 51690 RVA: 0x0049A03C File Offset: 0x0049823C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarString.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarString.PropValue, ref this.valueText);
		}

		// Token: 0x0600C9EB RID: 51691 RVA: 0x0049A067 File Offset: 0x00498267
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementVarString
			{
				Invert = this.Invert,
				operation = this.operation,
				varName = this.varName,
				valueText = this.valueText
			};
		}

		// Token: 0x040097D5 RID: 38869
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040097D6 RID: 38870
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040097D7 RID: 38871
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040097D8 RID: 38872
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
