using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000716 RID: 1814
[Preserve]
public class MinEventActionFadeOutSound : MinEventActionSoundBase
{
	// Token: 0x0600374A RID: 14154 RVA: 0x0016BE3C File Offset: 0x0016A03C
	public override void Execute(MinEventParams _params)
	{
		if ((this.localPlayerOnly && this.targets[0] as EntityPlayerLocal != null) || (!this.localPlayerOnly && this.targets[0] != null))
		{
			string soundGroupForTarget = base.GetSoundGroupForTarget();
			Manager.FadeOut(this.targets[0].entityId, soundGroupForTarget);
		}
	}
}
