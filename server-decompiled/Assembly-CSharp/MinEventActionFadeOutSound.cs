using System;
using Audio;
using UnityEngine.Scripting;

// Token: 0x02000714 RID: 1812
[Preserve]
public class MinEventActionFadeOutSound : MinEventActionSoundBase
{
	// Token: 0x06003707 RID: 14087 RVA: 0x0016B4D8 File Offset: 0x001696D8
	public override void Execute(MinEventParams _params)
	{
		if ((this.localPlayerOnly && this.targets[0] as EntityPlayerLocal != null) || (!this.localPlayerOnly && this.targets[0] != null))
		{
			string soundGroupForTarget = base.GetSoundGroupForTarget();
			Manager.FadeOut(this.targets[0].entityId, soundGroupForTarget);
		}
	}
}
