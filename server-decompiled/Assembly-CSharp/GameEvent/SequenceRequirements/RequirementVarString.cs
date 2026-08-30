using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001949 RID: 6473
	[Preserve]
	public class RequirementVarString : BaseOperationRequirement
	{
		// Token: 0x0600C77B RID: 51067 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C77C RID: 51068 RVA: 0x00492128 File Offset: 0x00490328
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			string empty = string.Empty;
			this.Owner.EventVariables.ParseVarString(this.varName, ref empty);
			return empty;
		}

		// Token: 0x0600C77D RID: 51069 RVA: 0x00492154 File Offset: 0x00490354
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.valueText;
		}

		// Token: 0x0600C77E RID: 51070 RVA: 0x0049215C File Offset: 0x0049035C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarString.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarString.PropValue, ref this.valueText);
		}

		// Token: 0x0600C77F RID: 51071 RVA: 0x00492187 File Offset: 0x00490387
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

		// Token: 0x04009626 RID: 38438
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x04009627 RID: 38439
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x04009628 RID: 38440
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x04009629 RID: 38441
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
