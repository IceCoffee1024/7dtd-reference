using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200021F RID: 543
[Preserve]
public class ConsoleCmdGetOptions : ConsoleCmdAbstract
{
	// Token: 0x06001064 RID: 4196 RVA: 0x00067A30 File Offset: 0x00065C30
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getoptions" };
	}

	// Token: 0x17000185 RID: 389
	// (get) Token: 0x06001065 RID: 4197 RVA: 0x00061642 File Offset: 0x0005F842
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x17000186 RID: 390
	// (get) Token: 0x06001066 RID: 4198 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000187 RID: 391
	// (get) Token: 0x06001067 RID: 4199 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000188 RID: 392
	// (get) Token: 0x06001068 RID: 4200 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000189 RID: 393
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x00067A40 File Offset: 0x00065C40
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets game options";
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x00067A47 File Offset: 0x00065C47
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Get all game options on the local game";
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x00067A50 File Offset: 0x00065C50
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
