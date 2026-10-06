using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019E0 RID: 6624
	[Preserve]
	public class ActionSetDayTime : BaseAction
	{
		// Token: 0x0600CAB9 RID: 51897 RVA: 0x004A3298 File Offset: 0x004A1498
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

		// Token: 0x0600CABA RID: 51898 RVA: 0x004A330D File Offset: 0x004A150D
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseInt(ActionSetDayTime.PropDay, ref this.day);
			properties.ParseInt(ActionSetDayTime.PropHours, ref this.hours);
			properties.ParseInt(ActionSetDayTime.PropMinutes, ref this.minutes);
		}

		// Token: 0x0600CABB RID: 51899 RVA: 0x004A3349 File Offset: 0x004A1549
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

		// Token: 0x04009973 RID: 39283
		[PublicizedFrom(EAccessModifier.Protected)]
		public int day = -1;

		// Token: 0x04009974 RID: 39284
		[PublicizedFrom(EAccessModifier.Protected)]
		public int hours = -1;

		// Token: 0x04009975 RID: 39285
		[PublicizedFrom(EAccessModifier.Protected)]
		public int minutes = -1;

		// Token: 0x04009976 RID: 39286
		public static string PropDay = "day";

		// Token: 0x04009977 RID: 39287
		public static string PropHours = "hours";

		// Token: 0x04009978 RID: 39288
		public static string PropMinutes = "minutes";
	}
}
