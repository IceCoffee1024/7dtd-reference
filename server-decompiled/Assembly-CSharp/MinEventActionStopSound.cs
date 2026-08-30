using System;
using Audio;
using DynamicMusic;
using UnityEngine.Scripting;

// Token: 0x02000713 RID: 1811
[Preserve]
public class MinEventActionStopSound : MinEventActionSoundBase
{
	// Token: 0x06003705 RID: 14085 RVA: 0x0016B428 File Offset: 0x00169628
	public override void Execute(MinEventParams _params)
	{
		string soundGroupForTarget = base.GetSoundGroupForTarget();
		if (this.localPlayerOnly && this.targets[0] as EntityPlayerLocal != null)
		{
			if (!this.loop)
			{
				Manager.Stop(this.targets[0].entityId, soundGroupForTarget);
				return;
			}
			Manager.StopLoopInsidePlayerHead(soundGroupForTarget, this.targets[0].entityId, false);
			if (this.toggleDMS)
			{
				SectionSelector.IsDMSTempDisabled = false;
				return;
			}
		}
		else if (!this.localPlayerOnly && this.targets[0] != null)
		{
			Manager.BroadcastStop(this.targets[0].entityId, soundGroupForTarget);
		}
	}
}
