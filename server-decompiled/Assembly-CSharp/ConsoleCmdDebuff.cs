using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000206 RID: 518
[Preserve]
public class ConsoleCmdDebuff : ConsoleCmdAbstract
{
	// Token: 0x17000156 RID: 342
	// (get) Token: 0x06000FBE RID: 4030 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000157 RID: 343
	// (get) Token: 0x06000FBF RID: 4031 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000FC0 RID: 4032 RVA: 0x000660A3 File Offset: 0x000642A3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debuff" };
	}

	// Token: 0x06000FC1 RID: 4033 RVA: 0x000660B4 File Offset: 0x000642B4
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!_senderInfo.IsLocalGame)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on clients, use \"debuffplayer\" instead for other players / remote clients");
			return;
		}
		EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		if (!(primaryPlayer != null))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No local player found.");
			return;
		}
		if (_params.Count != 1)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("debuff requires a buff name as the only argument!");
			ConsoleCmdDebuff.PrintActiveBuffNames(primaryPlayer);
			return;
		}
		if (primaryPlayer.Buffs.GetBuff(_params[0]) == null)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Debuff failed: buff \"" + _params[0] + "\" unknown or not active");
			ConsoleCmdDebuff.PrintActiveBuffNames(primaryPlayer);
			return;
		}
		primaryPlayer.Buffs.RemoveBuff(_params[0], -1, true);
	}

	// Token: 0x06000FC2 RID: 4034 RVA: 0x00066170 File Offset: 0x00064370
	public static void PrintActiveBuffNames(EntityPlayer _player)
	{
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Active buffs:");
		foreach (BuffValue buffValue in _player.Buffs.ActiveBuffs)
		{
			if (buffValue != null && buffValue.BuffClass != null)
			{
				BuffClass buffClass = buffValue.BuffClass;
				if (buffClass.Name.Equals(buffClass.LocalizedName))
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output(" - " + buffClass.Name);
				}
				else
				{
					SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[] { " - ", buffClass.Name, " (", buffClass.LocalizedName, ")" }));
				}
			}
		}
	}

	// Token: 0x06000FC3 RID: 4035 RVA: 0x00066258 File Offset: 0x00064458
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Removes a buff from the local player";
	}
}
