using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001922 RID: 6434
	[Preserve]
	public class RequirementVarInt : BaseOperationRequirement
	{
		// Token: 0x0600C5ED RID: 50669 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C5EE RID: 50670 RVA: 0x0048BD1C File Offset: 0x00489F1C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			int num = 0;
			this.Owner.EventVariables.ParseVarInt(this.varName, ref num);
			return num;
		}

		// Token: 0x0600C5EF RID: 50671 RVA: 0x0048BD49 File Offset: 0x00489F49
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C5F0 RID: 50672 RVA: 0x0048BD62 File Offset: 0x00489F62
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarInt.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C5F1 RID: 50673 RVA: 0x0048BD8D File Offset: 0x00489F8D
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

		// Token: 0x040094F9 RID: 38137
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040094FA RID: 38138
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094FB RID: 38139
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040094FC RID: 38140
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
