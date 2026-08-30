using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D2 RID: 6610
	[Preserve]
	public class ActionTimeChange : BaseAction
	{
		// Token: 0x0600C9E1 RID: 51681 RVA: 0x004A0CBC File Offset: 0x0049EEBC
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

		// Token: 0x0600C9E2 RID: 51682 RVA: 0x004A0E8A File Offset: 0x0049F08A
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionTimeChange.PropTime, ref this.timeText);
			properties.ParseEnum<ActionTimeChange.TimePresets>(ActionTimeChange.PropTimePreset, ref this.timePreset);
		}

		// Token: 0x0600C9E3 RID: 51683 RVA: 0x004A0EB5 File Offset: 0x0049F0B5
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionTimeChange
			{
				timeText = this.timeText,
				timePreset = this.timePreset
			};
		}

		// Token: 0x0400992C RID: 39212
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x0400992D RID: 39213
		[PublicizedFrom(EAccessModifier.Protected)]
		public ActionTimeChange.TimePresets timePreset;

		// Token: 0x0400992E RID: 39214
		public static string PropTimePreset = "time_preset";

		// Token: 0x0400992F RID: 39215
		public static string PropTime = "time";

		// Token: 0x020019D3 RID: 6611
		[PublicizedFrom(EAccessModifier.Protected)]
		public enum TimePresets
		{
			// Token: 0x04009931 RID: 39217
			Current,
			// Token: 0x04009932 RID: 39218
			Morning,
			// Token: 0x04009933 RID: 39219
			Noon,
			// Token: 0x04009934 RID: 39220
			Night,
			// Token: 0x04009935 RID: 39221
			NextMorning,
			// Token: 0x04009936 RID: 39222
			NextNoon,
			// Token: 0x04009937 RID: 39223
			NextNight,
			// Token: 0x04009938 RID: 39224
			HordeNight
		}
	}
}
