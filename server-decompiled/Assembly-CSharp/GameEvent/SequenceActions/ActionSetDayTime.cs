using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B4 RID: 6580
	[Preserve]
	public class ActionSetDayTime : BaseAction
	{
		// Token: 0x0600C961 RID: 51553 RVA: 0x0049EDA8 File Offset: 0x0049CFA8
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

		// Token: 0x0600C962 RID: 51554 RVA: 0x0049EE1D File Offset: 0x0049D01D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(ActionSetDayTime.PropDay, ref this.day);
			properties.ParseInt(ActionSetDayTime.PropHours, ref this.hours);
			properties.ParseInt(ActionSetDayTime.PropMinutes, ref this.minutes);
		}

		// Token: 0x0600C963 RID: 51555 RVA: 0x0049EE59 File Offset: 0x0049D059
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

		// Token: 0x040098AD RID: 39085
		[PublicizedFrom(EAccessModifier.Protected)]
		public int day = -1;

		// Token: 0x040098AE RID: 39086
		[PublicizedFrom(EAccessModifier.Protected)]
		public int hours = -1;

		// Token: 0x040098AF RID: 39087
		[PublicizedFrom(EAccessModifier.Protected)]
		public int minutes = -1;

		// Token: 0x040098B0 RID: 39088
		public static string PropDay = "day";

		// Token: 0x040098B1 RID: 39089
		public static string PropHours = "hours";

		// Token: 0x040098B2 RID: 39090
		public static string PropMinutes = "minutes";
	}
}
