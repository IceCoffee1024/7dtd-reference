using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200035D RID: 861
[Preserve]
public class AdminSpeedConsoleCmd : ConsoleCmdAbstract
{
	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x060018C0 RID: 6336 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x060018C1 RID: 6337 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x0008C370 File Offset: 0x0008A570
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

	// Token: 0x060018C3 RID: 6339 RVA: 0x0008C3EF File Offset: 0x0008A5EF
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

	// Token: 0x060018C4 RID: 6340 RVA: 0x0008C408 File Offset: 0x0008A608
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

	// Token: 0x060018C5 RID: 6341 RVA: 0x0008C446 File Offset: 0x0008A646
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[]
		{
			AdminSpeedConsoleCmd.info,
			"as"
		};
	}

	// Token: 0x060018C6 RID: 6342 RVA: 0x0008C45E File Offset: 0x0008A65E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return AdminSpeedConsoleCmd.info;
	}

	// Token: 0x04000FD5 RID: 4053
	[PublicizedFrom(EAccessModifier.Private)]
	public static string info = "AdminSpeed";

	// Token: 0x04000FD6 RID: 4054
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> parameters;
}
