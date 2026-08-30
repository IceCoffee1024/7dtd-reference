using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001976 RID: 6518
	[Preserve]
	public class ActionDelay : BaseAction
	{
		// Token: 0x0600C852 RID: 51282 RVA: 0x004975D8 File Offset: 0x004957D8
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

		// Token: 0x0600C853 RID: 51283 RVA: 0x0049763A File Offset: 0x0049583A
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.currentTime = -999f;
		}

		// Token: 0x0600C854 RID: 51284 RVA: 0x00497647 File Offset: 0x00495847
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDelay.PropTime, ref this.delayTimeText);
		}

		// Token: 0x0600C855 RID: 51285 RVA: 0x00497661 File Offset: 0x00495861
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDelay
			{
				delayTimeText = this.delayTimeText
			};
		}

		// Token: 0x04009733 RID: 38707
		[PublicizedFrom(EAccessModifier.Protected)]
		public string delayTimeText = "";

		// Token: 0x04009734 RID: 38708
		[PublicizedFrom(EAccessModifier.Protected)]
		public float delayTime = 5f;

		// Token: 0x04009735 RID: 38709
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime = -999f;

		// Token: 0x04009736 RID: 38710
		public static string PropTime = "time";
	}
}
