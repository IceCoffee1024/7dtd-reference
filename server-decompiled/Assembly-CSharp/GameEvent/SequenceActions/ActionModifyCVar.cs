using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198C RID: 6540
	[Preserve]
	public class ActionModifyCVar : ActionBaseClientAction
	{
		// Token: 0x0600C8A2 RID: 51362 RVA: 0x0049AED4 File Offset: 0x004990D4
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

		// Token: 0x0600C8A3 RID: 51363 RVA: 0x0049B027 File Offset: 0x00499227
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyCVar.PropValue, ref this.valueText);
			properties.ParseString(ActionModifyCVar.PropCvar, ref this.cvar);
			properties.ParseEnum<ActionModifyCVar.OperationTypes>(ActionModifyCVar.PropOperation, ref this.operationType);
		}

		// Token: 0x0600C8A4 RID: 51364 RVA: 0x0049B063 File Offset: 0x00499263
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

		// Token: 0x040097DB RID: 38875
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040097DC RID: 38876
		[PublicizedFrom(EAccessModifier.Protected)]
		public string cvar = "";

		// Token: 0x040097DD RID: 38877
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyCVar.OperationTypes operationType;

		// Token: 0x040097DE RID: 38878
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x040097DF RID: 38879
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropCvar = "cvar";

		// Token: 0x040097E0 RID: 38880
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x0200198D RID: 6541
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040097E2 RID: 38882
			Set,
			// Token: 0x040097E3 RID: 38883
			Add,
			// Token: 0x040097E4 RID: 38884
			Subtract,
			// Token: 0x040097E5 RID: 38885
			Multiply,
			// Token: 0x040097E6 RID: 38886
			PercentAdd,
			// Token: 0x040097E7 RID: 38887
			PercentSubtract
		}
	}
}
