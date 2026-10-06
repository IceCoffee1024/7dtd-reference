using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B8 RID: 6584
	[Preserve]
	public class ActionModifyCVar : ActionBaseClientAction
	{
		// Token: 0x0600C9FA RID: 51706 RVA: 0x0049F398 File Offset: 0x0049D598
		public override void OnClientPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				float floatValue = GameEventManager.GetFloatValue(entityAlive, this.valueText, 0f);
				switch (this.operationType)
				{
				case ActionModifyCVar.OperationTypes.Set:
					entityAlive.Buffs.SetCustomVar(this.cvar, floatValue, true, CVarOperation.set, false);
					return;
				case ActionModifyCVar.OperationTypes.Add:
					entityAlive.Buffs.SetCustomVar(this.cvar, entityAlive.Buffs.GetCustomVar(this.cvar) + floatValue, true, CVarOperation.set, false);
					return;
				case ActionModifyCVar.OperationTypes.Subtract:
					entityAlive.Buffs.SetCustomVar(this.cvar, entityAlive.Buffs.GetCustomVar(this.cvar) - floatValue, true, CVarOperation.set, false);
					return;
				case ActionModifyCVar.OperationTypes.Multiply:
					entityAlive.Buffs.SetCustomVar(this.cvar, entityAlive.Buffs.GetCustomVar(this.cvar) * floatValue, true, CVarOperation.set, false);
					return;
				case ActionModifyCVar.OperationTypes.PercentAdd:
					entityAlive.Buffs.SetCustomVar(this.cvar, entityAlive.Buffs.GetCustomVar(this.cvar) + entityAlive.Buffs.GetCustomVar(this.cvar) * floatValue, true, CVarOperation.set, false);
					return;
				case ActionModifyCVar.OperationTypes.PercentSubtract:
					entityAlive.Buffs.SetCustomVar(this.cvar, entityAlive.Buffs.GetCustomVar(this.cvar) - entityAlive.Buffs.GetCustomVar(this.cvar) * floatValue, true, CVarOperation.set, false);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600C9FB RID: 51707 RVA: 0x0049F4EB File Offset: 0x0049D6EB
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyCVar.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyCVar.PropCvar, ref this.cvar);
			properties.ParseEnum<ActionModifyCVar.OperationTypes>(ActionModifyCVar.PropOperation, ref this.operationType);
		}

		// Token: 0x0600C9FC RID: 51708 RVA: 0x0049F527 File Offset: 0x0049D727
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

		// Token: 0x040098A1 RID: 39073
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040098A2 RID: 39074
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cvar = "";

		// Token: 0x040098A3 RID: 39075
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyCVar.OperationTypes operationType;

		// Token: 0x040098A4 RID: 39076
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x040098A5 RID: 39077
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCvar = "cvar";

		// Token: 0x040098A6 RID: 39078
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x020019B9 RID: 6585
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040098A8 RID: 39080
			Set,
			// Token: 0x040098A9 RID: 39081
			Add,
			// Token: 0x040098AA RID: 39082
			Subtract,
			// Token: 0x040098AB RID: 39083
			Multiply,
			// Token: 0x040098AC RID: 39084
			PercentAdd,
			// Token: 0x040098AD RID: 39085
			PercentSubtract
		}
	}
}
