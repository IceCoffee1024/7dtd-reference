using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200021E RID: 542
[Preserve]
public class ConsoleCmdGetOptions : ConsoleCmdAbstract
{
	// Token: 0x06001071 RID: 4209 RVA: 0x00067A40 File Offset: 0x00065C40
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "getoptions" };
	}

	// Token: 0x1700018D RID: 397
	// (get) Token: 0x06001072 RID: 4210 RVA: 0x00061652 File Offset: 0x0005F852
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x1700018E RID: 398
	// (get) Token: 0x06001073 RID: 4211 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700018F RID: 399
	// (get) Token: 0x06001074 RID: 4212 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000190 RID: 400
	// (get) Token: 0x06001075 RID: 4213 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypes
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x17000191 RID: 401
	// (get) Token: 0x06001076 RID: 4214 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x00067A50 File Offset: 0x00065C50
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Gets game options";
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x00067A57 File Offset: 0x00065C57
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Get all game options on the local game";
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x00067A60 File Offset: 0x00065C60
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
