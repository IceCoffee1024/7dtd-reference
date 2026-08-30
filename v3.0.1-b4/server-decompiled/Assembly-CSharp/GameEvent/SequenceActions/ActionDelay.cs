using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001950 RID: 6480
	[Preserve]
	public class ActionDelay : BaseAction
	{
		// Token: 0x0600C6CB RID: 50891 RVA: 0x0049128C File Offset: 0x0048F48C
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

		// Token: 0x0600C6CC RID: 50892 RVA: 0x004912EE File Offset: 0x0048F4EE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.currentTime = -999f;
		}

		// Token: 0x0600C6CD RID: 50893 RVA: 0x004912FB File Offset: 0x0048F4FB
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDelay.PropTime, ref this.delayTimeText);
		}

		// Token: 0x0600C6CE RID: 50894 RVA: 0x00491315 File Offset: 0x0048F515
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDelay
			{
				delayTimeText = this.delayTimeText
			};
		}

		// Token: 0x0400960A RID: 38410
		[PublicizedFrom(EAccessModifier.Protected)]
		public string delayTimeText = "";

		// Token: 0x0400960B RID: 38411
		[PublicizedFrom(EAccessModifier.Protected)]
		public float delayTime = 5f;

		// Token: 0x0400960C RID: 38412
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime = -999f;

		// Token: 0x0400960D RID: 38413
		public static string PropTime = "time";
	}
}
