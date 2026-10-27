using System;
using UnityEngine.Scripting;

namespace GameEvent.SequenceActions
{
	// Token: 0x02001A0E RID: 6670
	[Preserve]
	public class ActionResetSleepers : BaseAction
	{
		// Token: 0x0600CBBD RID: 52157 RVA: 0x004A65C2 File Offset: 0x004A47C2
		public override BaseAction.ActionCompleteStates OnPerformAction()
		{
			GameManager.Instance.World.ResetSleeperVolumes();
			Log.Out("Reset all sleeper volumes");
			return BaseAction.ActionCompleteStates.Complete;
		}

		// Token: 0x0600CBBE RID: 52158 RVA: 0x004A65DE File Offset: 0x004A47DE
		[PublicizedFrom(EAccessModifier.Protected)]
		public override BaseAction CloneChildSettings()
		{
			return new ActionResetSleepers();
		}
	}
}
