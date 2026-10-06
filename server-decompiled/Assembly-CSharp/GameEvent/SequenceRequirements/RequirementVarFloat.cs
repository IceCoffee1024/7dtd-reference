using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001973 RID: 6515
	[Preserve]
	public class RequirementVarFloat : BaseOperationRequirement
	{
		// Token: 0x0600C8C5 RID: 51397 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C8C6 RID: 51398 RVA: 0x00496490 File Offset: 0x00494690
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			int num = 0;
			this.Owner.EventVariables.ParseVarInt(this.varName, ref num);
			return num;
		}

		// Token: 0x0600C8C7 RID: 51399 RVA: 0x004964BD File Offset: 0x004946BD
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C8C8 RID: 51400 RVA: 0x004964DA File Offset: 0x004946DA
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarFloat.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C8C9 RID: 51401 RVA: 0x00496505 File Offset: 0x00494705
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

		// Token: 0x040096E4 RID: 38628
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040096E5 RID: 38629
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096E6 RID: 38630
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040096E7 RID: 38631
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
