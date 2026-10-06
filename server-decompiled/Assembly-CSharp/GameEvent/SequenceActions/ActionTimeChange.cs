using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019FE RID: 6654
	[Preserve]
	public class ActionTimeChange : BaseAction
	{
		// Token: 0x0600CB39 RID: 52025 RVA: 0x004A5160 File Offset: 0x004A3360
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

		// Token: 0x0600CB3A RID: 52026 RVA: 0x004A532E File Offset: 0x004A352E
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTimeChange.PropTime, ref this.timeText);
			properties.ParseEnum<ActionTimeChange.TimePresets>(ActionTimeChange.PropTimePreset, ref this.timePreset);
		}

		// Token: 0x0600CB3B RID: 52027 RVA: 0x004A5359 File Offset: 0x004A3559
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTimeChange
			{
				timeText = this.timeText,
				timePreset = this.timePreset
			};
		}

		// Token: 0x040099F2 RID: 39410
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x040099F3 RID: 39411
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTimeChange.TimePresets timePreset;

		// Token: 0x040099F4 RID: 39412
		public static string PropTimePreset = "time_preset";

		// Token: 0x040099F5 RID: 39413
		public static string PropTime = "time";

		// Token: 0x020019FF RID: 6655
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TimePresets
		{
			// Token: 0x040099F7 RID: 39415
			Current,
			// Token: 0x040099F8 RID: 39416
			Morning,
			// Token: 0x040099F9 RID: 39417
			Noon,
			// Token: 0x040099FA RID: 39418
			Night,
			// Token: 0x040099FB RID: 39419
			NextMorning,
			// Token: 0x040099FC RID: 39420
			NextNoon,
			// Token: 0x040099FD RID: 39421
			NextNight,
			// Token: 0x040099FE RID: 39422
			HordeNight
		}
	}
}
