using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001966 RID: 6502
	[Preserve]
	public class ActionModifyCVar : ActionBaseClientAction
	{
		// Token: 0x0600C71B RID: 50971 RVA: 0x00494B98 File Offset: 0x00492D98
		public override void OnClientPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				float floatValue = GameEventManager.GetFloatValue(entityAlive, this.valueText, 0f);
				switch (this.operationType)
				{
				case ActionModifyCVar.OperationTypes.Set:
					entityAlive.Buffs.SetCustomVar(this.cvar, floatValue, true, CVarOperation.set);
					return;
				case ActionModifyCVar.OperationTypes.Add:
					entityAlive.Buffs.SetCustomVar(this.cvar, entityAlive.Buffs.GetCustomVar(this.cvar) + floatValue, true, CVarOperation.set);
					return;
				case ActionModifyCVar.OperationTypes.Subtract:
					entityAlive.Buffs.SetCustomVar(this.cvar, entityAlive.Buffs.GetCustomVar(this.cvar) - floatValue, true, CVarOperation.set);
					return;
				case ActionModifyCVar.OperationTypes.Multiply:
					entityAlive.Buffs.SetCustomVar(this.cvar, entityAlive.Buffs.GetCustomVar(this.cvar) * floatValue, true, CVarOperation.set);
					return;
				case ActionModifyCVar.OperationTypes.PercentAdd:
					entityAlive.Buffs.SetCustomVar(this.cvar, entityAlive.Buffs.GetCustomVar(this.cvar) + entityAlive.Buffs.GetCustomVar(this.cvar) * floatValue, true, CVarOperation.set);
					return;
				case ActionModifyCVar.OperationTypes.PercentSubtract:
					entityAlive.Buffs.SetCustomVar(this.cvar, entityAlive.Buffs.GetCustomVar(this.cvar) - entityAlive.Buffs.GetCustomVar(this.cvar) * floatValue, true, CVarOperation.set);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600C71C RID: 50972 RVA: 0x00494CE5 File Offset: 0x00492EE5
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyCVar.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyCVar.PropCvar, ref this.cvar);
			properties.ParseEnum<ActionModifyCVar.OperationTypes>(ActionModifyCVar.PropOperation, ref this.operationType);
		}

		// Token: 0x0600C71D RID: 50973 RVA: 0x00494D21 File Offset: 0x00492F21
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionModifyCVar
			{
				cvar = this.cvar,
				valueText = this.valueText,
				operationType = this.operationType
			};
		}

		// Token: 0x040096B2 RID: 38578
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096B3 RID: 38579
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cvar = "";

		// Token: 0x040096B4 RID: 38580
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyCVar.OperationTypes operationType;

		// Token: 0x040096B5 RID: 38581
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x040096B6 RID: 38582
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCvar = "cvar";

		// Token: 0x040096B7 RID: 38583
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x02001967 RID: 6503
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040096B9 RID: 38585
			Set,
			// Token: 0x040096BA RID: 38586
			Add,
			// Token: 0x040096BB RID: 38587
			Subtract,
			// Token: 0x040096BC RID: 38588
			Multiply,
			// Token: 0x040096BD RID: 38589
			PercentAdd,
			// Token: 0x040096BE RID: 38590
			PercentSubtract
		}
	}
}
