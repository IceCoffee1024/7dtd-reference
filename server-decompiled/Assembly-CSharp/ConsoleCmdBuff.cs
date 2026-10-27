using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020001F0 RID: 496
[Preserve]
public class ConsoleCmdBuff : ConsoleCmdAbstract
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000F2F RID: 3887 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000F30 RID: 3888 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000F31 RID: 3889 RVA: 0x0006376B File Offset: 0x0006196B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "buff" };
	}

	// Token: 0x06000F32 RID: 3890 RVA: 0x0006377C File Offset: 0x0006197C
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

	// Token: 0x06000F33 RID: 3891 RVA: 0x000638A3 File Offset: 0x00061AA3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Applies a buff to the local player";
	}

	// Token: 0x06000F34 RID: 3892 RVA: 0x000638AC File Offset: 0x00061AAC
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
