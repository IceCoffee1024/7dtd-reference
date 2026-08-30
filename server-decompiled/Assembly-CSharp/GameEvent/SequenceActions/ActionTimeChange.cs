using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019AC RID: 6572
	[Preserve]
	public class ActionTimeChange : BaseAction
	{
		// Token: 0x0600C85A RID: 51290 RVA: 0x0049A964 File Offset: 0x00498B64
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			World world = GameManager.Instance.World;
			ulong num = world.worldTime;
			ulong num2 = world.worldTime;
			switch (this.timePreset)
			{
			case ActionTimeChange.TimePresets.Current:
				num = world.worldTime;
				break;
			case ActionTimeChange.TimePresets.Morning:
				num = GameUtils.DayTimeToWorldTime(GameUtils.WorldTimeToDays(world.worldTime), (int)SkyManager.GetDuskTime(), 0);
				break;
			case ActionTimeChange.TimePresets.Noon:
				num = GameUtils.DayTimeToWorldTime(GameUtils.WorldTimeToDays(world.worldTime), 12, 0);
				break;
			case ActionTimeChange.TimePresets.Night:
				num = GameUtils.DayTimeToWorldTime(GameUtils.WorldTimeToDays(world.worldTime), 21, 45);
				break;
			case ActionTimeChange.TimePresets.NextMorning:
			{
				ulong worldTime = world.worldTime;
				int num3 = GameUtils.WorldTimeToDays(worldTime);
				int num4 = GameUtils.WorldTimeToHours(worldTime);
				int num5 = (int)SkyManager.GetDawnTime();
				if (num4 < num5)
				{
					num = GameUtils.DayTimeToWorldTime(num3, num5, 0);
				}
				else
				{
					num = GameUtils.DayTimeToWorldTime(num3 + 1, num5, 0);
				}
				break;
			}
			case ActionTimeChange.TimePresets.NextNoon:
			{
				ulong worldTime2 = world.worldTime;
				int num6 = GameUtils.WorldTimeToDays(worldTime2);
				if (GameUtils.WorldTimeToHours(worldTime2) < 12)
				{
					num = GameUtils.DayTimeToWorldTime(num6, 12, 0);
				}
				else
				{
					num = GameUtils.DayTimeToWorldTime(num6 + 1, 12, 0);
				}
				break;
			}
			case ActionTimeChange.TimePresets.NextNight:
			{
				ulong worldTime3 = world.worldTime;
				int num7 = GameUtils.WorldTimeToDays(worldTime3);
				if (GameUtils.WorldTimeToHours(worldTime3) < 22)
				{
					num = GameUtils.DayTimeToWorldTime(num7, 22, 0);
				}
				else
				{
					num = GameUtils.DayTimeToWorldTime(num7 + 1, 22, 0);
				}
				break;
			}
			case ActionTimeChange.TimePresets.HordeNight:
				num = GameUtils.DayTimeToWorldTime(GameStats.GetInt(EnumGameStats.BloodMoonDay), 21, 45);
				break;
			}
			int num8 = GameEventManager.GetIntValue(base.Owner.Target as EntityAlive, this.timeText, 60) * 1000 / 60;
			if (num8 < 0)
			{
				num2 = num + (ulong)((long)num8);
				if (num2 > world.worldTime)
				{
					num2 = 0UL;
				}
			}
			else if (num8 > 0)
			{
				num2 = num + (ulong)((long)num8);
				if (num2 < num)
				{
					num2 = num;
				}
			}
			else
			{
				num2 = num;
			}
			if (num2 != world.worldTime)
			{
				world.SetTimeJump(num2, true);
			}
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C85B RID: 51291 RVA: 0x0049AB32 File Offset: 0x00498D32
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTimeChange.PropTime, ref this.timeText);
			properties.ParseEnum<ActionTimeChange.TimePresets>(ActionTimeChange.PropTimePreset, ref this.timePreset);
		}

		// Token: 0x0600C85C RID: 51292 RVA: 0x0049AB5D File Offset: 0x00498D5D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTimeChange
			{
				timeText = this.timeText,
				timePreset = this.timePreset
			};
		}

		// Token: 0x04009803 RID: 38915
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x04009804 RID: 38916
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTimeChange.TimePresets timePreset;

		// Token: 0x04009805 RID: 38917
		public static string PropTimePreset = "time_preset";

		// Token: 0x04009806 RID: 38918
		public static string PropTime = "time";

		// Token: 0x020019AD RID: 6573
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TimePresets
		{
			// Token: 0x04009808 RID: 38920
			Current,
			// Token: 0x04009809 RID: 38921
			Morning,
			// Token: 0x0400980A RID: 38922
			Noon,
			// Token: 0x0400980B RID: 38923
			Night,
			// Token: 0x0400980C RID: 38924
			NextMorning,
			// Token: 0x0400980D RID: 38925
			NextNoon,
			// Token: 0x0400980E RID: 38926
			NextNight,
			// Token: 0x0400980F RID: 38927
			HordeNight
		}
	}
}
