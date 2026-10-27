using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x020019A5 RID: 6565
	[Preserve]
	public class RequirementVarInt : BaseOperationRequirement
	{
		// Token: 0x0600C9E0 RID: 51680 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C9E1 RID: 51681 RVA: 0x00499F48 File Offset: 0x00498148
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			int num = 0;
			this.Owner.EventVariables.ParseVarInt(this.varName, ref num);
			return num;
		}

		// Token: 0x0600C9E2 RID: 51682 RVA: 0x00499F75 File Offset: 0x00498175
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C9E3 RID: 51683 RVA: 0x00499F8E File Offset: 0x0049818E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarInt.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C9E4 RID: 51684 RVA: 0x00499FB9 File Offset: 0x004981B9
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

		// Token: 0x040097D1 RID: 38865
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040097D2 RID: 38866
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040097D3 RID: 38867
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040097D4 RID: 38868
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
