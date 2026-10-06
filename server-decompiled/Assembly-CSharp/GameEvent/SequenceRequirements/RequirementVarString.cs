using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001975 RID: 6517
	[Preserve]
	public class RequirementVarString : BaseOperationRequirement
	{
		// Token: 0x0600C8D3 RID: 51411 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C8D4 RID: 51412 RVA: 0x00496614 File Offset: 0x00494814
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			string empty = string.Empty;
			this.Owner.EventVariables.ParseVarString(this.varName, ref empty);
			return empty;
		}

		// Token: 0x0600C8D5 RID: 51413 RVA: 0x00496640 File Offset: 0x00494840
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.valueText;
		}

		// Token: 0x0600C8D6 RID: 51414 RVA: 0x00496648 File Offset: 0x00494848
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarString.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarString.PropValue, ref this.valueText);
		}

		// Token: 0x0600C8D7 RID: 51415 RVA: 0x00496673 File Offset: 0x00494873
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

		// Token: 0x040096EC RID: 38636
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040096ED RID: 38637
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096EE RID: 38638
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040096EF RID: 38639
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
