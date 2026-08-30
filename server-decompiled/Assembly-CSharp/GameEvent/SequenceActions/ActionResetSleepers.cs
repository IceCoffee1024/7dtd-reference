using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x020019B1 RID: 6577
	[Preserve]
	public class ActionResetSleepers : BaseAction
	{
		// Token: 0x0600C951 RID: 51537 RVA: 0x0049E5D2 File Offset: 0x0049C7D2
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameManager.Instance.World.ResetSleeperVolumes();
			Log.Out("Reset all sleeper volumes");
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600C952 RID: 51538 RVA: 0x0049E5EE File Offset: 0x0049C7EE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetSleepers();
		}
	}
}
