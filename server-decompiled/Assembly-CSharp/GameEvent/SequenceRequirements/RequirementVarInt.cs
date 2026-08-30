using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001948 RID: 6472
	[Preserve]
	public class RequirementVarInt : BaseOperationRequirement
	{
		// Token: 0x0600C774 RID: 51060 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C775 RID: 51061 RVA: 0x00492068 File Offset: 0x00490268
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			int num = 0;
			this.Owner.EventVariables.ParseVarInt(this.varName, ref num);
			return num;
		}

		// Token: 0x0600C776 RID: 51062 RVA: 0x00492095 File Offset: 0x00490295
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C777 RID: 51063 RVA: 0x004920AE File Offset: 0x004902AE
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarInt.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C778 RID: 51064 RVA: 0x004920D9 File Offset: 0x004902D9
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementVarInt
			{
				Invert = this.Invert,
				operation = this.operation,
				varName = this.varName,
				valueText = this.valueText
			};
		}

		// Token: 0x04009622 RID: 38434
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x04009623 RID: 38435
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x04009624 RID: 38436
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x04009625 RID: 38437
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
