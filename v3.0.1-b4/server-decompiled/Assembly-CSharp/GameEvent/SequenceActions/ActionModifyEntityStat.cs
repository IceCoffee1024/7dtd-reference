using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001968 RID: 6504
	[Preserve]
	public class ActionModifyEntityStat : ActionBaseClientAction
	{
		// Token: 0x0600C720 RID: 50976 RVA: 0x00494D80 File Offset: 0x00492F80
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

		// Token: 0x0600C721 RID: 50977 RVA: 0x00494EA0 File Offset: 0x004930A0
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

		// Token: 0x0600C722 RID: 50978 RVA: 0x00494F30 File Offset: 0x00493130
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyEntityStat.PropValue, ref this.valueText);
			properties.ParseEnum<ActionModifyEntityStat.StatTypes>(ActionModifyEntityStat.PropStat, ref this.Stat);
			properties.ParseEnum<ActionModifyEntityStat.OperationTypes>(ActionModifyEntityStat.PropOperation, ref this.operationType);
			properties.ParseBool(ActionModifyEntityStat.PropIsPercent, ref this.isPercent);
		}

		// Token: 0x0600C723 RID: 50979 RVA: 0x00494F88 File Offset: 0x00493188
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

		// Token: 0x040096BF RID: 38591
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040096C0 RID: 38592
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyEntityStat.StatTypes Stat;

		// Token: 0x040096C1 RID: 38593
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyEntityStat.OperationTypes operationType;

		// Token: 0x040096C2 RID: 38594
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isPercent;

		// Token: 0x040096C3 RID: 38595
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x040096C4 RID: 38596
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropStat = "stat";

		// Token: 0x040096C5 RID: 38597
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x040096C6 RID: 38598
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsPercent = "is_percent";

		// Token: 0x02001969 RID: 6505
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum StatTypes
		{
			// Token: 0x040096C8 RID: 38600
			Health,
			// Token: 0x040096C9 RID: 38601
			Stamina,
			// Token: 0x040096CA RID: 38602
			Food,
			// Token: 0x040096CB RID: 38603
			Water,
			// Token: 0x040096CC RID: 38604
			SightRange
		}

		// Token: 0x0200196A RID: 6506
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040096CE RID: 38606
			Set,
			// Token: 0x040096CF RID: 38607
			SetMax,
			// Token: 0x040096D0 RID: 38608
			Add,
			// Token: 0x040096D1 RID: 38609
			Subtract,
			// Token: 0x040096D2 RID: 38610
			Multiply
		}
	}
}
