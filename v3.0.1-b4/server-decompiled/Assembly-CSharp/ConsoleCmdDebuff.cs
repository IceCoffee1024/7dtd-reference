using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000203 RID: 515
[Preserve]
public class ConsoleCmdDebuff : ConsoleCmdAbstract
{
	// Token: 0x1700015E RID: 350
	// (get) Token: 0x06000FC0 RID: 4032 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700015F RID: 351
	// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000FC2 RID: 4034 RVA: 0x00065B6B File Offset: 0x00063D6B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "debuff" };
	}

	// Token: 0x06000FC3 RID: 4035 RVA: 0x00065B7C File Offset: 0x00063D7C
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

	// Token: 0x06000FC4 RID: 4036 RVA: 0x00065C38 File Offset: 0x00063E38
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

	// Token: 0x06000FC5 RID: 4037 RVA: 0x00065D20 File Offset: 0x00063F20
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Removes a buff from the local player";
	}
}
