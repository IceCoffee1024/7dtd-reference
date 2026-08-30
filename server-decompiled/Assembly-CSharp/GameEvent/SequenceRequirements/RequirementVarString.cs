using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001923 RID: 6435
	[Preserve]
	public class RequirementVarString : BaseOperationRequirement
	{
		// Token: 0x0600C5F4 RID: 50676 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C5F5 RID: 50677 RVA: 0x0048BDDC File Offset: 0x00489FDC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			string empty = string.Empty;
			this.Owner.EventVariables.ParseVarString(this.varName, ref empty);
			return empty;
		}

		// Token: 0x0600C5F6 RID: 50678 RVA: 0x0048BE08 File Offset: 0x0048A008
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return this.valueText;
		}

		// Token: 0x0600C5F7 RID: 50679 RVA: 0x0048BE10 File Offset: 0x0048A010
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarString.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarString.PropValue, ref this.valueText);
		}

		// Token: 0x0600C5F8 RID: 50680 RVA: 0x0048BE3B File Offset: 0x0048A03B
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

		// Token: 0x040094FD RID: 38141
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040094FE RID: 38142
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094FF RID: 38143
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x04009500 RID: 38144
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
