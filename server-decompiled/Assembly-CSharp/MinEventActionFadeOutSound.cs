using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x0200071E RID: 1822
[Preserve]
public class MinEventActionFadeOutSound : MinEventActionSoundBase
{
	// Token: 0x06003761 RID: 14177 RVA: 0x0016CDD4 File Offset: 0x0016AFD4
	public override void Execute(MinEventParams _params)
	{
		if ((this.localPlayerOnly && this.targets[0] as EntityPlayerLocal != null) || (!this.localPlayerOnly && this.targets[0] != null))
		{
			string soundGroupForTarget = base.GetSoundGroupForTarget();
			Manager.FadeOut(this.targets[0].entityId, soundGroupForTarget);
		}
	}
}
