using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020001EB RID: 491
[Preserve]
public class ConsoleCmdBuff : ConsoleCmdAbstract
{
	// Token: 0x17000142 RID: 322
	// (get) Token: 0x06000F2D RID: 3885 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000143 RID: 323
	// (get) Token: 0x06000F2E RID: 3886 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000F2F RID: 3887 RVA: 0x00062F6F File Offset: 0x0006116F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "buff" };
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x00062F80 File Offset: 0x00061180
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (!_senderInfo.IsLocalGame)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Command can only be used on clients, use \"buffplayer\" instead for other players / remote clients");
			return;
		}
		if (_params.Count == 1)
		{
			EntityPlayer primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
			if (primaryPlayer != null)
			{
				EntityBuffs.BuffStatus buffStatus = primaryPlayer.Buffs.AddBuff(_params[0], -1, true, false, -1f);
				if (buffStatus != EntityBuffs.BuffStatus.Added)
				{
					switch (buffStatus)
					{
					case EntityBuffs.BuffStatus.FailedInvalidName:
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Buff failed: buff \"" + _params[0] + "\" unknown");
						ConsoleCmdBuff.PrintAvailableBuffNames();
						return;
					case EntityBuffs.BuffStatus.FailedImmune:
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Buff failed: entity is immune to \"" + _params[0] + "\"");
						return;
					case EntityBuffs.BuffStatus.FailedFriendlyFire:
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Buff failed: entity is friendly");
						return;
					case EntityBuffs.BuffStatus.FailedEditor:
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Buff failed: buff " + _params[0] + " not allowed in editor");
						return;
					case EntityBuffs.BuffStatus.FailedGameStat:
						SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Buff failed: missing required game stat.");
						return;
					default:
						return;
					}
				}
			}
		}
		else
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("buff requires a buff name as the only argument!");
			ConsoleCmdBuff.PrintAvailableBuffNames();
		}
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x000630A7 File Offset: 0x000612A7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Applies a buff to the local player";
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x000630B0 File Offset: 0x000612B0
	public static void PrintAvailableBuffNames()
	{
		SortedDictionary<string, BuffClass> sortedDictionary = new SortedDictionary<string, BuffClass>(BuffManager.Buffs, StringComparer.OrdinalIgnoreCase);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Available buffs:");
		foreach (KeyValuePair<string, BuffClass> keyValuePair in sortedDictionary)
		{
			if (keyValuePair.Key.Equals(keyValuePair.Value.LocalizedName))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(" - " + keyValuePair.Key);
			}
			else
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Concat(new string[]
				{
					" - ",
					keyValuePair.Key,
					" (",
					keyValuePair.Value.LocalizedName,
					")"
				}));
			}
		}
	}
}
