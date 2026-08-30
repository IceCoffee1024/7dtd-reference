using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceRequirements
{
	// Token: 0x02001924 RID: 6436
	[Preserve]
	public class RequirementCVar : BaseOperationRequirement
	{
		// Token: 0x0600C6BD RID: 50877 RVA: 0x000027FC File Offset: 0x000009FC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnInit()
		{
		}

		// Token: 0x0600C6BE RID: 50878 RVA: 0x00490954 File Offset: 0x0048EB54
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object LeftSide(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			return (entityAlive != null) ? entityAlive.Buffs.GetCustomVar(this.cvar) : 0f;
		}

		// Token: 0x0600C6BF RID: 50879 RVA: 0x00490988 File Offset: 0x0048EB88
		[PublicizedFrom(EAccessModifier.Protected)]
		public override object RightSide(Entity target)
		{
			return GameEventManager.GetFloatValue(target as EntityAlive, this.valueText, 0f);
		}

		// Token: 0x0600C6C0 RID: 50880 RVA: 0x004909A5 File Offset: 0x0048EBA5
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(RequirementCVar.PropCvar, ref this.cvar);
			properties.ParseString(RequirementCVar.PropValue, ref this.valueText);
		}

		// Token: 0x0600C6C1 RID: 50881 RVA: 0x004909D0 File Offset: 0x0048EBD0
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

		// Token: 0x040095CD RID: 38349
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cvar = "";

		// Token: 0x040095CE RID: 38350
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040095CF RID: 38351
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCvar = "cvar";

		// Token: 0x040095D0 RID: 38352
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";
	}
}
