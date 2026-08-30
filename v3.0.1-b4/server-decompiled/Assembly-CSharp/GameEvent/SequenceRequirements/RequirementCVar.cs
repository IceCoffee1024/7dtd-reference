using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x020018FE RID: 6398
	[Preserve]
	public class RequirementCVar : BaseOperationRequirement
	{
		// Token: 0x0600C536 RID: 50486 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C537 RID: 50487 RVA: 0x0048A608 File Offset: 0x00488808
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			return (entityAlive != null) ? entityAlive.Buffs.GetCustomVar(this.cvar) : 0f;
		}

		// Token: 0x0600C538 RID: 50488 RVA: 0x0048A63C File Offset: 0x0048883C
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C539 RID: 50489 RVA: 0x0048A659 File Offset: 0x00488859
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementCVar.PropCvar, ref this.cvar);
			properties.ParseString(RequirementCVar.PropValue, ref this.valueText);
		}

		// Token: 0x0600C53A RID: 50490 RVA: 0x0048A684 File Offset: 0x00488884
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

		// Token: 0x040094A4 RID: 38052
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cvar = "";

		// Token: 0x040094A5 RID: 38053
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040094A6 RID: 38054
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCvar = "cvar";

		// Token: 0x040094A7 RID: 38055
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
