using System;
using System.Collections.Generic;
using UnityEngine.Scripting;

// Token: 0x020002A0 RID: 672
[Preserve]
public class ConsoleCmdSpectatorMode : ConsoleCmdAbstract
{
	// Token: 0x0600138B RID: 5003 RVA: 0x00077800 File Offset: 0x00075A00
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "enables/disables spectator mode";
	}

	// Token: 0x0600138C RID: 5004 RVA: 0x00077807 File Offset: 0x00075A07
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "spectator", "spectatormode", "sm" };
	}

	// Token: 0x17000219 RID: 537
	// (get) Token: 0x0600138D RID: 5005 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x0600138E RID: 5006 RVA: 0x00077828 File Offset: 0x00075A28
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
