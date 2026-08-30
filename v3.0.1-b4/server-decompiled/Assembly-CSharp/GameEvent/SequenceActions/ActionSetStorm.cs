using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001998 RID: 6552
	[Preserve]
	public class ActionSetStorm : BaseAction
	{
		// Token: 0x0600C802 RID: 51202 RVA: 0x0049930C File Offset: 0x0049750C
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			float floatValue = GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.timeText, 1f);
			WeatherManager.Instance.SetStorm(null, (int)(floatValue * 1000f));
			WeatherManager.Instance.TriggerUpdate();
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C803 RID: 51203 RVA: 0x00499358 File Offset: 0x00497558
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetStorm.PropTime, ref this.timeText);
		}

		// Token: 0x0600C804 RID: 51204 RVA: 0x00499372 File Offset: 0x00497572
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetStorm
			{
				timeText = this.timeText
			};
		}

		// Token: 0x040097AF RID: 38831
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x040097B0 RID: 38832
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "hours";
	}
}
