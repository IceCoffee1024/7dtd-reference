using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x020001EE RID: 494
[Preserve]
public class ConsoleCmdBuff : ConsoleCmdAbstract
{
	// Token: 0x1700013A RID: 314
	// (get) Token: 0x06000F2B RID: 3883 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700013B RID: 315
	// (get) Token: 0x06000F2C RID: 3884 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06000F2D RID: 3885 RVA: 0x000634A7 File Offset: 0x000616A7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "buff" };
	}

	// Token: 0x06000F2E RID: 3886 RVA: 0x000634B8 File Offset: 0x000616B8
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

	// Token: 0x06000F2F RID: 3887 RVA: 0x000635DF File Offset: 0x000617DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Applies a buff to the local player";
	}

	// Token: 0x06000F30 RID: 3888 RVA: 0x000635E8 File Offset: 0x000617E8
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
