using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x020019A4 RID: 6564
	[Preserve]
	public class RequirementVarFloat : BaseOperationRequirement
	{
		// Token: 0x0600C9D9 RID: 51673 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C9DA RID: 51674 RVA: 0x00499E84 File Offset: 0x00498084
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			int num = 0;
			this.Owner.EventVariables.ParseVarInt(this.varName, ref num);
			return num;
		}

		// Token: 0x0600C9DB RID: 51675 RVA: 0x00499EB1 File Offset: 0x004980B1
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C9DC RID: 51676 RVA: 0x00499ECE File Offset: 0x004980CE
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarFloat.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarFloat.PropValue, ref this.valueText);
		}

		// Token: 0x0600C9DD RID: 51677 RVA: 0x00499EF9 File Offset: 0x004980F9
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

		// Token: 0x040097CD RID: 38861
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040097CE RID: 38862
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040097CF RID: 38863
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040097D0 RID: 38864
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
