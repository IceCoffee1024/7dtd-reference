using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001974 RID: 6516
	[Preserve]
	public class RequirementVarInt : BaseOperationRequirement
	{
		// Token: 0x0600C8CC RID: 51404 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C8CD RID: 51405 RVA: 0x00496554 File Offset: 0x00494754
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			int num = 0;
			this.Owner.EventVariables.ParseVarInt(this.varName, ref num);
			return num;
		}

		// Token: 0x0600C8CE RID: 51406 RVA: 0x00496581 File Offset: 0x00494781
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetIntValue(target as EntityAlive, this.valueText, 0);
		}

		// Token: 0x0600C8CF RID: 51407 RVA: 0x0049659A File Offset: 0x0049479A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementVarInt.PropVarName, ref this.varName);
			properties.ParseString(RequirementVarInt.PropValue, ref this.valueText);
		}

		// Token: 0x0600C8D0 RID: 51408 RVA: 0x004965C5 File Offset: 0x004947C5
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

		// Token: 0x040096E8 RID: 38632
		[PublicizedFrom(EAccessModifier.Protected)]
		public string varName;

		// Token: 0x040096E9 RID: 38633
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096EA RID: 38634
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropVarName = "var_name";

		// Token: 0x040096EB RID: 38635
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
