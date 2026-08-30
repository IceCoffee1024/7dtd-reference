using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x0200029C RID: 668
[Preserve]
public class ConsoleCmdSpectatorMode : ConsoleCmdAbstract
{
	// Token: 0x06001379 RID: 4985 RVA: 0x000771A4 File Offset: 0x000753A4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables spectator mode";
	}

	// Token: 0x0600137A RID: 4986 RVA: 0x000771AB File Offset: 0x000753AB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spectator", "spectatormode", "sm" };
	}

	// Token: 0x17000218 RID: 536
	// (get) Token: 0x0600137B RID: 4987 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600137C RID: 4988 RVA: 0x000771CC File Offset: 0x000753CC
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
