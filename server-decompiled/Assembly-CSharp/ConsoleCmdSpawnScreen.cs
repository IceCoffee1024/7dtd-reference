using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020002A2 RID: 674
[Preserve]
public class ConsoleCmdSpawnScreen : ConsoleCmdAbstract
{
	// Token: 0x06001383 RID: 4995 RVA: 0x000779F4 File Offset: 0x00075BF4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "SpawnScreen" };
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x00077A04 File Offset: 0x00075C04
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Display SpawnScreen";
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x00077A0B File Offset: 0x00075C0B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "SpawnScreen on/off";
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x00077A14 File Offset: 0x00075C14
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		foreach (EntityPlayerLocal entityPlayerLocal in GameManager.Instance.World.GetLocalPlayers())
		{
			if (entityPlayerLocal)
			{
				entityPlayerLocal.spawnInTime = Time.time;
				entityPlayerLocal.bPlayingSpawnIn = true;
				if (_params.Count > 0)
				{
					int num = 0;
					if (int.TryParse(_params[0], out num))
					{
						EntityPlayerLocal.spawnInEffectSpeed = (float)num;
					}
				}
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output("SpawnEffect initiated...");
			}
		}
	}
}
