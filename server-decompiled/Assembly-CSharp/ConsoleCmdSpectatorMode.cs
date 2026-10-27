using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002A5 RID: 677
[Preserve]
public class ConsoleCmdSpectatorMode : ConsoleCmdAbstract
{
	// Token: 0x0600138C RID: 5004 RVA: 0x00077C84 File Offset: 0x00075E84
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables spectator mode";
	}

	// Token: 0x0600138D RID: 5005 RVA: 0x00077C8B File Offset: 0x00075E8B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spectator", "spectatormode", "sm" };
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x0600138E RID: 5006 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600138F RID: 5007 RVA: 0x00077CAC File Offset: 0x00075EAC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (GameManager.IsDedicatedServer)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Cannot execute spectatormode.");
		}
		if (_params.Count != 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Invalid arguments");
			return;
		}
		EntityPlayer player = XUiM_Player.GetPlayer();
		if (player != null)
		{
			player.IsSpectator = !player.IsSpectator;
			player.bPlayerStatsChanged = true;
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Spectator Mode: " + player.IsSpectator.ToString());
		}
	}
}
