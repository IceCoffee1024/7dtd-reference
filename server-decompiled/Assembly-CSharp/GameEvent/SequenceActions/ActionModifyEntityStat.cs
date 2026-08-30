using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198E RID: 6542
	[Preserve]
	public class ActionModifyEntityStat : ActionBaseClientAction
	{
		// Token: 0x0600C8A7 RID: 51367 RVA: 0x0049B0C4 File Offset: 0x004992C4
		public override void OnClientPerform(Entity target)
		{
			EntityAlive entityAlive = target as EntityAlive;
			if (entityAlive != null)
			{
				float floatValue = GameEventManager.GetFloatValue(entityAlive, this.valueText, 0f);
				switch (this.Stat)
				{
				case ActionModifyEntityStat.StatTypes.Health:
					entityAlive.Health = (int)this.GetValue(floatValue, (float)entityAlive.Health, (float)entityAlive.GetMaxHealth());
					return;
				case ActionModifyEntityStat.StatTypes.Stamina:
					entityAlive.Stamina = (float)((int)this.GetValue(floatValue, entityAlive.Stamina, (float)entityAlive.GetMaxStamina()));
					return;
				case ActionModifyEntityStat.StatTypes.Food:
					entityAlive.Stats.Food.Value = (float)((int)this.GetValue(floatValue, (float)((int)entityAlive.Stats.Food.Value), (float)((int)entityAlive.Stats.Food.Max)));
					return;
				case ActionModifyEntityStat.StatTypes.Water:
					entityAlive.Stats.Water.Value = (float)((int)this.GetValue(floatValue, (float)((int)entityAlive.Stats.Water.Value), (float)((int)entityAlive.Stats.Water.Max)));
					return;
				case ActionModifyEntityStat.StatTypes.SightRange:
					entityAlive.sightRangeBase = this.GetValue(floatValue, entityAlive.sightRangeBase, entityAlive.sightRangeBase);
					break;
				default:
					return;
				}
			}
		}

		// Token: 0x0600C8A8 RID: 51368 RVA: 0x0049B1E4 File Offset: 0x004993E4
		[PublicizedFrom(EAccessModifier.Protected)]
		public virtual float GetValue(float value, float original, float max)
		{
			if (this.isPercent)
			{
				switch (this.operationType)
				{
				case ActionModifyEntityStat.OperationTypes.Set:
					return value * max;
				case ActionModifyEntityStat.OperationTypes.SetMax:
					return max;
				case ActionModifyEntityStat.OperationTypes.Add:
					return original / max + value * max;
				case ActionModifyEntityStat.OperationTypes.Subtract:
					return original / max - value * max;
				case ActionModifyEntityStat.OperationTypes.Multiply:
					return original / max * (value * max);
				}
			}
			else
			{
				switch (this.operationType)
				{
				case ActionModifyEntityStat.OperationTypes.Set:
					return value;
				case ActionModifyEntityStat.OperationTypes.SetMax:
					return max;
				case ActionModifyEntityStat.OperationTypes.Add:
					return original + value;
				case ActionModifyEntityStat.OperationTypes.Subtract:
					return original - value;
				case ActionModifyEntityStat.OperationTypes.Multiply:
					return original * value;
				}
			}
			return 0f;
		}

		// Token: 0x0600C8A9 RID: 51369 RVA: 0x0049B274 File Offset: 0x00499474
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyEntityStat.PropValue, ref this.valueText);
			properties.ParseEnum<ActionModifyEntityStat.StatTypes>(ActionModifyEntityStat.PropStat, ref this.Stat);
			properties.ParseEnum<ActionModifyEntityStat.OperationTypes>(ActionModifyEntityStat.PropOperation, ref this.operationType);
			properties.ParseBool(ActionModifyEntityStat.PropIsPercent, ref this.isPercent);
		}

		// Token: 0x0600C8AA RID: 51370 RVA: 0x0049B2CC File Offset: 0x004994CC
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionModifyEntityStat
			{
				Stat = this.Stat,
				valueText = this.valueText,
				operationType = this.operationType,
				isPercent = this.isPercent
			};
		}

		// Token: 0x040097E8 RID: 38888
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040097E9 RID: 38889
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyEntityStat.StatTypes Stat;

		// Token: 0x040097EA RID: 38890
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyEntityStat.OperationTypes operationType;

		// Token: 0x040097EB RID: 38891
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isPercent;

		// Token: 0x040097EC RID: 38892
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x040097ED RID: 38893
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropStat = "stat";

		// Token: 0x040097EE RID: 38894
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x040097EF RID: 38895
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsPercent = "is_percent";

		// Token: 0x0200198F RID: 6543
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum StatTypes
		{
			// Token: 0x040097F1 RID: 38897
			Health,
			// Token: 0x040097F2 RID: 38898
			Stamina,
			// Token: 0x040097F3 RID: 38899
			Food,
			// Token: 0x040097F4 RID: 38900
			Water,
			// Token: 0x040097F5 RID: 38901
			SightRange
		}

		// Token: 0x02001990 RID: 6544
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040097F7 RID: 38903
			Set,
			// Token: 0x040097F8 RID: 38904
			SetMax,
			// Token: 0x040097F9 RID: 38905
			Add,
			// Token: 0x040097FA RID: 38906
			Subtract,
			// Token: 0x040097FB RID: 38907
			Multiply
		}
	}
}
