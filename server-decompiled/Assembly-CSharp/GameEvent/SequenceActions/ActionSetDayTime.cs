using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A11 RID: 6673
	[Preserve]
	public class ActionSetDayTime : BaseAction
	{
		// Token: 0x0600CBCD RID: 52173 RVA: 0x004A6D98 File Offset: 0x004A4F98
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			World world = GameManager.Instance.World;
			ulong worldTime = world.worldTime;
			int num = GameUtils.WorldTimeToDays(worldTime);
			int num2 = GameUtils.WorldTimeToHours(worldTime);
			int num3 = GameUtils.WorldTimeToMinutes(worldTime);
			ulong num4 = GameUtils.DayTimeToWorldTime((this.day < 1) ? num : this.day, (this.hours < 0) ? num2 : this.hours, (this.minutes < 0) ? num3 : this.minutes);
			world.SetTimeJump(num4, true);
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CBCE RID: 52174 RVA: 0x004A6E0D File Offset: 0x004A500D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(ActionSetDayTime.PropDay, ref this.day);
			properties.ParseInt(ActionSetDayTime.PropHours, ref this.hours);
			properties.ParseInt(ActionSetDayTime.PropMinutes, ref this.minutes);
		}

		// Token: 0x0600CBCF RID: 52175 RVA: 0x004A6E49 File Offset: 0x004A5049
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetDayTime
			{
				day = this.day,
				hours = this.hours,
				minutes = this.minutes
			};
		}

		// Token: 0x04009A5C RID: 39516
		[PublicizedFrom(EAccessModifier.Protected)]
		public int day = -1;

		// Token: 0x04009A5D RID: 39517
		[PublicizedFrom(EAccessModifier.Protected)]
		public int hours = -1;

		// Token: 0x04009A5E RID: 39518
		[PublicizedFrom(EAccessModifier.Protected)]
		public int minutes = -1;

		// Token: 0x04009A5F RID: 39519
		public static string PropDay = "day";

		// Token: 0x04009A60 RID: 39520
		public static string PropHours = "hours";

		// Token: 0x04009A61 RID: 39521
		public static string PropMinutes = "minutes";
	}
}
