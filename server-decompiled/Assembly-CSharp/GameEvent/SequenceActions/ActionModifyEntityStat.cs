using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BA RID: 6586
	[Preserve]
	public class ActionModifyEntityStat : ActionBaseClientAction
	{
		// Token: 0x0600C9FF RID: 51711 RVA: 0x0049F588 File Offset: 0x0049D788
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

		// Token: 0x0600CA00 RID: 51712 RVA: 0x0049F6A8 File Offset: 0x0049D8A8
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

		// Token: 0x0600CA01 RID: 51713 RVA: 0x0049F738 File Offset: 0x0049D938
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionModifyEntityStat.PropValue, ref this.valueText);
			properties.ParseEnum<ActionModifyEntityStat.StatTypes>(ActionModifyEntityStat.PropStat, ref this.Stat);
			properties.ParseEnum<ActionModifyEntityStat.OperationTypes>(ActionModifyEntityStat.PropOperation, ref this.operationType);
			properties.ParseBool(ActionModifyEntityStat.PropIsPercent, ref this.isPercent);
		}

		// Token: 0x0600CA02 RID: 51714 RVA: 0x0049F790 File Offset: 0x0049D990
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

		// Token: 0x040098AE RID: 39086
		[PublicizedFrom(EAccessModifier.Protected)]
		public string valueText;

		// Token: 0x040098AF RID: 39087
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyEntityStat.StatTypes Stat;

		// Token: 0x040098B0 RID: 39088
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionModifyEntityStat.OperationTypes operationType;

		// Token: 0x040098B1 RID: 39089
		[PublicizedFrom(EAccessModifier.Protected)]
		public bool isPercent;

		// Token: 0x040098B2 RID: 39090
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropValue = "value";

		// Token: 0x040098B3 RID: 39091
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropStat = "stat";

		// Token: 0x040098B4 RID: 39092
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropOperation = "operation";

		// Token: 0x040098B5 RID: 39093
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropIsPercent = "is_percent";

		// Token: 0x020019BB RID: 6587
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum StatTypes
		{
			// Token: 0x040098B7 RID: 39095
			Health,
			// Token: 0x040098B8 RID: 39096
			Stamina,
			// Token: 0x040098B9 RID: 39097
			Food,
			// Token: 0x040098BA RID: 39098
			Water,
			// Token: 0x040098BB RID: 39099
			SightRange
		}

		// Token: 0x020019BC RID: 6588
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum OperationTypes
		{
			// Token: 0x040098BD RID: 39101
			Set,
			// Token: 0x040098BE RID: 39102
			SetMax,
			// Token: 0x040098BF RID: 39103
			Add,
			// Token: 0x040098C0 RID: 39104
			Subtract,
			// Token: 0x040098C1 RID: 39105
			Multiply
		}
	}
}
