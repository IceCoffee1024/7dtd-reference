using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A1B RID: 6683
	[Preserve]
	public class ActionSetStorm : BaseAction
	{
		// Token: 0x0600CBF5 RID: 52213 RVA: 0x004A7650 File Offset: 0x004A5850
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			float floatValue = GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.timeText, 1f);
			WeatherManager.Instance.SetStorm(null, (int)(floatValue * 1000f));
			WeatherManager.Instance.TriggerUpdate();
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CBF6 RID: 52214 RVA: 0x004A769C File Offset: 0x004A589C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetStorm.PropTime, ref this.timeText);
		}

		// Token: 0x0600CBF7 RID: 52215 RVA: 0x004A76B6 File Offset: 0x004A58B6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetStorm
			{
				timeText = this.timeText
			};
		}

		// Token: 0x04009A87 RID: 39559
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x04009A88 RID: 39560
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "hours";
	}
}
