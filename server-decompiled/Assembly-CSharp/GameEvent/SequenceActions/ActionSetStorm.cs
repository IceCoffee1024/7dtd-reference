using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019BE RID: 6590
	[Preserve]
	public class ActionSetStorm : BaseAction
	{
		// Token: 0x0600C989 RID: 51593 RVA: 0x0049F664 File Offset: 0x0049D864
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			float floatValue = GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.timeText, 1f);
			WeatherManager.Instance.SetStorm(null, (int)(floatValue * 1000f));
			WeatherManager.Instance.TriggerUpdate();
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C98A RID: 51594 RVA: 0x0049F6B0 File Offset: 0x0049D8B0
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetStorm.PropTime, ref this.timeText);
		}

		// Token: 0x0600C98B RID: 51595 RVA: 0x0049F6CA File Offset: 0x0049D8CA
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetStorm
			{
				timeText = this.timeText
			};
		}

		// Token: 0x040098D8 RID: 39128
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x040098D9 RID: 39129
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "hours";
	}
}
