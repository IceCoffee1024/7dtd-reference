using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000354 RID: 852
[Preserve]
public class AdminSpeedConsoleCmd : ConsoleCmdAbstract
{
	// Token: 0x170002E9 RID: 745
	// (get) Token: 0x060018A4 RID: 6308 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170002EA RID: 746
	// (get) Token: 0x060018A5 RID: 6309 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060018A6 RID: 6310 RVA: 0x0008B77C File Offset: 0x0008997C
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

	// Token: 0x060018A7 RID: 6311 RVA: 0x0008B7FB File Offset: 0x000899FB
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

	// Token: 0x060018A8 RID: 6312 RVA: 0x0008B814 File Offset: 0x00089A14
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

	// Token: 0x060018A9 RID: 6313 RVA: 0x0008B852 File Offset: 0x00089A52
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[]
		{
			AdminSpeedConsoleCmd.info,
			"as"
		};
	}

	// Token: 0x060018AA RID: 6314 RVA: 0x0008B86A File Offset: 0x00089A6A
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return AdminSpeedConsoleCmd.info;
	}

	// Token: 0x04000FB7 RID: 4023
	[PublicizedFrom(EAccessModifier.Private)]
	public static string info = "AdminSpeed";

	// Token: 0x04000FB8 RID: 4024
	[PublicizedFrom(EAccessModifier.Private)]
	public List<string> parameters;
}
