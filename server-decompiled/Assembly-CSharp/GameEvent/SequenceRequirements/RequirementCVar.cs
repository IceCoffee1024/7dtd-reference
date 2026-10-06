using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001950 RID: 6480
	[Preserve]
	public class RequirementCVar : BaseOperationRequirement
	{
		// Token: 0x0600C815 RID: 51221 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C816 RID: 51222 RVA: 0x00494E40 File Offset: 0x00493040
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			return (entityAlive != null) ? entityAlive.Buffs.GetCustomVar(this.cvar) : 0f;
		}

		// Token: 0x0600C817 RID: 51223 RVA: 0x00494E74 File Offset: 0x00493074
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C818 RID: 51224 RVA: 0x00494E91 File Offset: 0x00493091
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementCVar.PropCvar, ref this.cvar);
			properties.ParseString(RequirementCVar.PropValue, ref this.valueText);
		}

		// Token: 0x0600C819 RID: 51225 RVA: 0x00494EBC File Offset: 0x004930BC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseRequirement CloneChildSettings()
		{
			return new RequirementCVar
			{
				Invert = this.Invert,
				operation = this.operation,
				cvar = this.cvar,
				valueText = this.valueText
			};
		}

		// Token: 0x04009693 RID: 38547
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cvar = "";

		// Token: 0x04009694 RID: 38548
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x04009695 RID: 38549
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCvar = "cvar";

		// Token: 0x04009696 RID: 38550
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
