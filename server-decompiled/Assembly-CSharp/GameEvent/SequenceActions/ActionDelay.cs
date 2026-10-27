using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019D3 RID: 6611
	[Preserve]
	public class ActionDelay : BaseAction
	{
		// Token: 0x0600CABE RID: 51902 RVA: 0x0049F5B4 File Offset: 0x0049D7B4
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			if (this.currentTime == -999f)
			{
				this.currentTime = GameEventManager.GetFloatValue(base.Owner.Target as EntityAlive, this.delayTimeText, 5f);
			}
			this.currentTime -= Time.deltaTime;
			if (this.currentTime <= 0f)
			{
				return BaseAction.ActionCompleteStates.Complete;
			}
			return BaseAction.ActionCompleteStates.InComplete;
		}

		// Token: 0x0600CABF RID: 51903 RVA: 0x0049F616 File Offset: 0x0049D816
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.currentTime = -999f;
		}

		// Token: 0x0600CAC0 RID: 51904 RVA: 0x0049F623 File Offset: 0x0049D823
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDelay.PropTime, ref this.delayTimeText);
		}

		// Token: 0x0600CAC1 RID: 51905 RVA: 0x0049F63D File Offset: 0x0049D83D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDelay
			{
				delayTimeText = this.delayTimeText
			};
		}

		// Token: 0x040098E2 RID: 39138
		[PublicizedFrom(EAccessModifier.Protected)]
		public string delayTimeText = "";

		// Token: 0x040098E3 RID: 39139
		[PublicizedFrom(EAccessModifier.Protected)]
		public float delayTime = 5f;

		// Token: 0x040098E4 RID: 39140
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime = -999f;

		// Token: 0x040098E5 RID: 39141
		public static string PropTime = "time";
	}
}
