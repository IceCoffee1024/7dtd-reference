using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x0200198B RID: 6539
	[Preserve]
	public class ActionResetSleepers : BaseAction
	{
		// Token: 0x0600C7CA RID: 51146 RVA: 0x0049827A File Offset: 0x0049647A
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameManager.Instance.World.ResetSleeperVolumes();
			Log.Out("Reset all sleeper volumes");
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C7CB RID: 51147 RVA: 0x00498296 File Offset: 0x00496496
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetSleepers();
		}
	}
}
