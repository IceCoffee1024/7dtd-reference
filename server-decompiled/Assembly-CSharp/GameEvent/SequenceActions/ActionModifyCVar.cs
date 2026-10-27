using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E9 RID: 6633
	[Preserve]
	public class ActionModifyCVar : ActionBaseClientAction
	{
		// Token: 0x0600CB0E RID: 51982 RVA: 0x004A2E98 File Offset: 0x004A1098
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

		// Token: 0x0600CB0F RID: 51983 RVA: 0x004A2FEB File Offset: 0x004A11EB
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyCVar.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyCVar.PropCvar, ref this.cvar);
			properties.ParseEnum<ActionModifyCVar.OperationTypes>(ActionModifyCVar.PropOperation, ref this.operationType);
		}

		// Token: 0x0600CB10 RID: 51984 RVA: 0x004A3027 File Offset: 0x004A1227
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

		// Token: 0x0400998A RID: 39306
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x0400998B RID: 39307
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cvar = "";

		// Token: 0x0400998C RID: 39308
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyCVar.OperationTypes operationType;

		// Token: 0x0400998D RID: 39309
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x0400998E RID: 39310
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCvar = "cvar";

		// Token: 0x0400998F RID: 39311
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x020019EA RID: 6634
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x04009991 RID: 39313
			Set,
			// Token: 0x04009992 RID: 39314
			Add,
			// Token: 0x04009993 RID: 39315
			Subtract,
			// Token: 0x04009994 RID: 39316
			Multiply,
			// Token: 0x04009995 RID: 39317
			PercentAdd,
			// Token: 0x04009996 RID: 39318
			PercentSubtract
		}
	}
}
