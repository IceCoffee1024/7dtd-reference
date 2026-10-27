using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001981 RID: 6529
	[Preserve]
	public class RequirementCVar : BaseOperationRequirement
	{
		// Token: 0x0600C929 RID: 51497 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C92A RID: 51498 RVA: 0x00498834 File Offset: 0x00496A34
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			return (entityAlive != null) ? entityAlive.Buffs.GetCustomVar(this.cvar) : 0f;
		}

		// Token: 0x0600C92B RID: 51499 RVA: 0x00498868 File Offset: 0x00496A68
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C92C RID: 51500 RVA: 0x00498885 File Offset: 0x00496A85
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementCVar.PropCvar, ref this.cvar);
			properties.ParseString(RequirementCVar.PropValue, ref this.valueText);
		}

		// Token: 0x0600C92D RID: 51501 RVA: 0x004988B0 File Offset: 0x00496AB0
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

		// Token: 0x0400977C RID: 38780
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cvar = "";

		// Token: 0x0400977D RID: 38781
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x0400977E RID: 38782
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCvar = "cvar";

		// Token: 0x0400977F RID: 38783
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
