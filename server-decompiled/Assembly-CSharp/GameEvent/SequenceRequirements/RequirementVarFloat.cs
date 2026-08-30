using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001947 RID: 6471
	[Preserve]
	public class RequirementVarFloat : BaseOperationRequirement
	{
		// Token: 0x0600C76D RID: 51053 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C76E RID: 51054 RVA: 0x00491FA4 File Offset: 0x004901A4
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			int num = 0;
			this.Owner.EventVariables.ParseVarInt(this.varName, ref num);
			return num;
		}

		// Token: 0x0600C76F RID: 51055 RVA: 0x00491FD1 File Offset: 0x004901D1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C770 RID: 51056 RVA: 0x00491FEE File Offset: 0x004901EE
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarFloat.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C771 RID: 51057 RVA: 0x00492019 File Offset: 0x00490219
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementVarFloat
			{
				Invert = this.Invert,
				operation = this.operation,
				varName = this.varName,
				valueText = this.valueText
			};
		}

		// Token: 0x0400961E RID: 38430
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x0400961F RID: 38431
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x04009620 RID: 38432
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x04009621 RID: 38433
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
