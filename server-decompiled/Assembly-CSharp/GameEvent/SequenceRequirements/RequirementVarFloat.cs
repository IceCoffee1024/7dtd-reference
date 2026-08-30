using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001921 RID: 6433
	[Preserve]
	public class RequirementVarFloat : BaseOperationRequirement
	{
		// Token: 0x0600C5E6 RID: 50662 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C5E7 RID: 50663 RVA: 0x0048BC58 File Offset: 0x00489E58
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			int num = 0;
			this.Owner.EventVariables.ParseVarInt(this.varName, ref num);
			return num;
		}

		// Token: 0x0600C5E8 RID: 50664 RVA: 0x0048BC85 File Offset: 0x00489E85
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C5E9 RID: 50665 RVA: 0x0048BCA2 File Offset: 0x00489EA2
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarFloat.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C5EA RID: 50666 RVA: 0x0048BCCD File Offset: 0x00489ECD
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

		// Token: 0x040094F5 RID: 38133
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040094F6 RID: 38134
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094F7 RID: 38135
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040094F8 RID: 38136
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
