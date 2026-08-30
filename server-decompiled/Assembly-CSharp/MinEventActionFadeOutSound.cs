using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x0200070A RID: 1802
[Preserve]
public class MinEventActionFadeOutSound : MinEventActionSoundBase
{
	// Token: 0x060036B1 RID: 14001 RVA: 0x00169300 File Offset: 0x00167500
	public override void Execute(MinEventParams _params)
	{
		if ((this.localPlayerOnly && this.targets[0] as EntityPlayerLocal != null) || (!this.localPlayerOnly && this.targets[0] != null))
		{
			string soundGroupForTarget = base.GetSoundGroupForTarget();
			Manager.FadeOut(this.targets[0].entityId, soundGroupForTarget);
		}
	}
}
