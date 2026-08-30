using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200021C RID: 540
[Preserve]
public class ConsoleCmdGetOptions : ConsoleCmdAbstract
{
	// Token: 0x06001066 RID: 4198 RVA: 0x000674E8 File Offset: 0x000656E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getoptions" };
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06001067 RID: 4199 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06001068 RID: 4200 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x0600106A RID: 4202 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x0600106B RID: 4203 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x000674F8 File Offset: 0x000656F8
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets game options";
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x000674FF File Offset: 0x000656FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Get all game options on the local game";
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x00067508 File Offset: 0x00065708
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		string text = null;
		if (_params.Count > 0)
		{
			text = _params[0];
		}
		SortedList<string, string> sortedList = new SortedList<string, string>();
		foreach (EnumGamePrefs enumGamePrefs in EnumUtils.Values<EnumGamePrefs>())
		{
			if (enumGamePrefs.ToStringCached<EnumGamePrefs>().StartsWith("option", StringComparison.OrdinalIgnoreCase) && (string.IsNullOrEmpty(text) || enumGamePrefs.ToStringCached<EnumGamePrefs>().ContainsCaseInsensitive(text)))
			{
				sortedList.Add(enumGamePrefs.ToStringCached<EnumGamePrefs>(), string.Format("GamePref.{0} = {1}", enumGamePrefs.ToStringCached<EnumGamePrefs>(), GamePrefs.GetObject(enumGamePrefs)));
			}
		}
		foreach (string text2 in sortedList.Keys)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(sortedList[text2]);
		}
	}
}
