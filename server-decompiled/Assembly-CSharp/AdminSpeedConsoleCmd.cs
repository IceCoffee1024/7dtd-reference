using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200035B RID: 859
[Preserve]
public class AdminSpeedConsoleCmd : ConsoleCmdAbstract
{
	// Token: 0x170002E5 RID: 741
	// (get) Token: 0x060018BC RID: 6332 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x060018BD RID: 6333 RVA: 0x00060387 File Offset: 0x0005E587
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060018BE RID: 6334 RVA: 0x0008C13C File Offset: 0x0008A33C
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

	// Token: 0x060018BF RID: 6335 RVA: 0x0008C1BB File Offset: 0x0008A3BB
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

	// Token: 0x060018C0 RID: 6336 RVA: 0x0008C1D4 File Offset: 0x0008A3D4
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

	// Token: 0x060018C1 RID: 6337 RVA: 0x0008C212 File Offset: 0x0008A412
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[]
		{
			AdminSpeedConsoleCmd.info,
			"as"
		};
	}

	// Token: 0x060018C2 RID: 6338 RVA: 0x0008C22A File Offset: 0x0008A42A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return AdminSpeedConsoleCmd.info;
	}

	// Token: 0x04000FC5 RID: 4037
	[PublicizedFrom(EAccessModifier.Private)]
	public static string info = "AdminSpeed";

	// Token: 0x04000FC6 RID: 4038
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> parameters;
}
