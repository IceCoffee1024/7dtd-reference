using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198E RID: 6542
	[Preserve]
	public class ActionSetDayTime : BaseAction
	{
		// Token: 0x0600C7DA RID: 51162 RVA: 0x00498A50 File Offset: 0x00496C50
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

		// Token: 0x0600C7DB RID: 51163 RVA: 0x00498AC5 File Offset: 0x00496CC5
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(ActionSetDayTime.PropDay, ref this.day);
			properties.ParseInt(ActionSetDayTime.PropHours, ref this.hours);
			properties.ParseInt(ActionSetDayTime.PropMinutes, ref this.minutes);
		}

		// Token: 0x0600C7DC RID: 51164 RVA: 0x00498B01 File Offset: 0x00496D01
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

		// Token: 0x04009784 RID: 38788
		[PublicizedFrom(EAccessModifier.Protected)]
		public int day = -1;

		// Token: 0x04009785 RID: 38789
		[PublicizedFrom(EAccessModifier.Protected)]
		public int hours = -1;

		// Token: 0x04009786 RID: 38790
		[PublicizedFrom(EAccessModifier.Protected)]
		public int minutes = -1;

		// Token: 0x04009787 RID: 38791
		public static string PropDay = "day";

		// Token: 0x04009788 RID: 38792
		public static string PropHours = "hours";

		// Token: 0x04009789 RID: 38793
		public static string PropMinutes = "minutes";
	}
}
