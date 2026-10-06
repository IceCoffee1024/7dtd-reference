using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019DD RID: 6621
	[Preserve]
	public class ActionResetSleepers : BaseAction
	{
		// Token: 0x0600CAA9 RID: 51881 RVA: 0x004A2AC2 File Offset: 0x004A0CC2
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameManager.Instance.World.ResetSleeperVolumes();
			Log.Out("Reset all sleeper volumes");
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CAAA RID: 51882 RVA: 0x004A2ADE File Offset: 0x004A0CDE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetSleepers();
		}
	}
}
