using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019EA RID: 6634
	[Preserve]
	public class ActionSetStorm : BaseAction
	{
		// Token: 0x0600CAE1 RID: 51937 RVA: 0x004A3B50 File Offset: 0x004A1D50
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			float floatValue = GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.timeText, 1f);
			WeatherManager.Instance.SetStorm(null, (int)(floatValue * 1000f));
			WeatherManager.Instance.TriggerUpdate();
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAE2 RID: 51938 RVA: 0x004A3B9C File Offset: 0x004A1D9C
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionSetStorm.PropTime, ref this.timeText);
		}

		// Token: 0x0600CAE3 RID: 51939 RVA: 0x004A3BB6 File Offset: 0x004A1DB6
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionSetStorm
			{
				timeText = this.timeText
			};
		}

		// Token: 0x0400999E RID: 39326
		[PublicizedFrom(EAccessModifier.Protected)]
		public string timeText;

		// Token: 0x0400999F RID: 39327
		[PublicizedFrom(EAccessModifier.Protected)]
		public static string PropTime = "hours";
	}
}
