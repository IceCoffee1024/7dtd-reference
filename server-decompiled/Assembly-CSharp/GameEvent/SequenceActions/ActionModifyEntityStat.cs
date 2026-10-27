using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019EB RID: 6635
	[Preserve]
	public class ActionModifyEntityStat : ActionBaseClientAction
	{
		// Token: 0x0600CB13 RID: 51987 RVA: 0x004A3088 File Offset: 0x004A1288
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

		// Token: 0x0600CB14 RID: 51988 RVA: 0x004A31A8 File Offset: 0x004A13A8
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

		// Token: 0x0600CB15 RID: 51989 RVA: 0x004A3238 File Offset: 0x004A1438
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyEntityStat.PropValue, ref this.valueText);
			properties.ParseEnum<ActionModifyEntityStat.StatTypes>(ActionModifyEntityStat.PropStat, ref this.Stat);
			properties.ParseEnum<ActionModifyEntityStat.OperationTypes>(ActionModifyEntityStat.PropOperation, ref this.operationType);
			properties.ParseBool(ActionModifyEntityStat.PropIsPercent, ref this.isPercent);
		}

		// Token: 0x0600CB16 RID: 51990 RVA: 0x004A3290 File Offset: 0x004A1490
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

		// Token: 0x04009997 RID: 39319
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x04009998 RID: 39320
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyEntityStat.StatTypes Stat;

		// Token: 0x04009999 RID: 39321
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyEntityStat.OperationTypes operationType;

		// Token: 0x0400999A RID: 39322
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isPercent;

		// Token: 0x0400999B RID: 39323
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x0400999C RID: 39324
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropStat = "stat";

		// Token: 0x0400999D RID: 39325
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x0400999E RID: 39326
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsPercent = "is_percent";

		// Token: 0x020019EC RID: 6636
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum StatTypes
		{
			// Token: 0x040099A0 RID: 39328
			Health,
			// Token: 0x040099A1 RID: 39329
			Stamina,
			// Token: 0x040099A2 RID: 39330
			Food,
			// Token: 0x040099A3 RID: 39331
			Water,
			// Token: 0x040099A4 RID: 39332
			SightRange
		}

		// Token: 0x020019ED RID: 6637
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040099A6 RID: 39334
			Set,
			// Token: 0x040099A7 RID: 39335
			SetMax,
			// Token: 0x040099A8 RID: 39336
			Add,
			// Token: 0x040099A9 RID: 39337
			Subtract,
			// Token: 0x040099AA RID: 39338
			Multiply
		}
	}
}
