using System;
using UnityEngine;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019A2 RID: 6562
	[Preserve]
	public class ActionDelay : BaseAction
	{
		// Token: 0x0600C9AA RID: 51626 RVA: 0x0049BAB4 File Offset: 0x00499CB4
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

		// Token: 0x0600C9AB RID: 51627 RVA: 0x0049BB16 File Offset: 0x00499D16
		[PublicizedFrom(EAccessModifier.Protected)]
		public override void OnReset()
		{
			this.currentTime = -999f;
		}

		// Token: 0x0600C9AC RID: 51628 RVA: 0x0049BB23 File Offset: 0x00499D23
		public override void ParseProperties(DynamicProperties properties)
		{
			base.ParseProperties(properties);
			properties.ParseString(ActionDelay.PropTime, ref this.delayTimeText);
		}

		// Token: 0x0600C9AD RID: 51629 RVA: 0x0049BB3D File Offset: 0x00499D3D
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionDelay
			{
				delayTimeText = this.delayTimeText
			};
		}

		// Token: 0x040097F9 RID: 38905
		[PublicizedFrom(EAccessModifier.Protected)]
		public string delayTimeText = "";

		// Token: 0x040097FA RID: 38906
		[PublicizedFrom(EAccessModifier.Protected)]
		public float delayTime = 5f;

		// Token: 0x040097FB RID: 38907
		[PublicizedFrom(EAccessModifier.Protected)]
		public float currentTime = -999f;

		// Token: 0x040097FC RID: 38908
		public static string PropTime = "time";
	}
}
