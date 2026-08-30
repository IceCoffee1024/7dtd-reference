using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000358 RID: 856
[Preserve]
public class AdminSpeedConsoleCmd : ConsoleCmdAbstract
{
	// Token: 0x170002ED RID: 749
	// (get) Token: 0x060018C1 RID: 6337 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002EE RID: 750
	// (get) Token: 0x060018C2 RID: 6338 RVA: 0x00060397 File Offset: 0x0005E597
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060018C3 RID: 6339 RVA: 0x0008BEBC File Offset: 0x0008A0BC
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		this.parameters = _params;
		string text = ((_params.Count == 0) ? "nope" : _params[0].ToLower());
		EntityPlayerLocal primaryPlayer = GameManager.Instance.World.GetPrimaryPlayer();
		float num = 0f;
		if (!float.TryParse(text, out num))
		{
			num = (float)((primaryPlayer.GodModeSpeedModifier == 15f) ? 5 : 15);
		}
		primaryPlayer.GodModeSpeedModifier = num;
		Log.Out("Admin speed: " + num.ToString());
	}

	// Token: 0x060018C4 RID: 6340 RVA: 0x0008BF3B File Offset: 0x0008A13B
	[PublicizedFrom(EAccessModifier.Private)]
	public string GetParam(List<string> _params, int index)
	{
		if (_params == null)
		{
			return null;
		}
		if (index >= _params.Count)
		{
			return null;
		}
		return _params[index];
	}

	// Token: 0x060018C5 RID: 6341 RVA: 0x0008BF54 File Offset: 0x0008A154
	[PublicizedFrom(EAccessModifier.Private)]
	public int GetParamAsInt(int index)
	{
		if (this.parameters == null)
		{
			return -1;
		}
		if (index >= this.parameters.Count)
		{
			return -1;
		}
		int num = -1;
		int.TryParse(this.parameters[index], out num);
		return num;
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x0008BF92 File Offset: 0x0008A192
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[]
		{
			AdminSpeedConsoleCmd.info,
			"as"
		};
	}

	// Token: 0x060018C7 RID: 6343 RVA: 0x0008BFAA File Offset: 0x0008A1AA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return AdminSpeedConsoleCmd.info;
	}

	// Token: 0x04000FCC RID: 4044
	[PublicizedFrom(EAccessModifier.Private)]
	public static string info = "AdminSpeed";

	// Token: 0x04000FCD RID: 4045
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> parameters;
}
