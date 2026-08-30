using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200029B RID: 667
[Preserve]
public class ConsoleCmdSpawnScreen : ConsoleCmdAbstract
{
	// Token: 0x06001374 RID: 4980 RVA: 0x000770E4 File Offset: 0x000752E4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "SpawnScreen" };
	}

	// Token: 0x06001375 RID: 4981 RVA: 0x000770F4 File Offset: 0x000752F4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Display SpawnScreen";
	}

	// Token: 0x06001376 RID: 4982 RVA: 0x000770FB File Offset: 0x000752FB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "SpawnScreen on/off";
	}

	// Token: 0x06001377 RID: 4983 RVA: 0x00077104 File Offset: 0x00075304
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
