using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002A3 RID: 675
[Preserve]
public class ConsoleCmdSpectatorMode : ConsoleCmdAbstract
{
	// Token: 0x06001388 RID: 5000 RVA: 0x00077AB4 File Offset: 0x00075CB4
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables spectator mode";
	}

	// Token: 0x06001389 RID: 5001 RVA: 0x00077ABB File Offset: 0x00075CBB
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spectator", "spectatormode", "sm" };
	}

	// Token: 0x17000211 RID: 529
	// (get) Token: 0x0600138A RID: 5002 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600138B RID: 5003 RVA: 0x00077ADC File Offset: 0x00075CDC
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
