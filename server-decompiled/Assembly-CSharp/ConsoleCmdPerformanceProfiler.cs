using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x0200025A RID: 602
[Preserve]
public class ConsoleCmdPerformanceProfiler : ConsoleCmdAbstract
{
	// Token: 0x060011C3 RID: 4547 RVA: 0x0006FCDD File Offset: 0x0006DEDD
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "performanceprofiler", "pp" };
	}

	// Token: 0x060011C4 RID: 4548 RVA: 0x0006FCF5 File Offset: 0x0006DEF5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Performance Profiling Utility";
	}

	// Token: 0x060011C5 RID: 4549 RVA: 0x0006FCFC File Offset: 0x0006DEFC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Type 'pp' to toggle capture, or 'pp start [fps]' / 'pp stop'.";
	}

	// Token: 0x170001C6 RID: 454
	// (get) Token: 0x060011C6 RID: 4550 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001C7 RID: 455
	// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00061906 File Offset: 0x0005FB06
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001C8 RID: 456
	// (get) Token: 0x060011C8 RID: 4552 RVA: 0x0006064B File Offset: 0x0005E84B
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060011C9 RID: 4553 RVA: 0x0006FD04 File Offset: 0x0006DF04
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			this.Toggle();
			return;
		}
		string text = _params[0].ToLower();
		if (text == "start")
		{
			this.CmdStart(_params);
			return;
		}
		if (!(text == "stop"))
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Unknown sub-command '" + _params[0] + "'. See 'help pp'.");
			return;
		}
		this.CmdStop();
	}

	// Token: 0x060011CA RID: 4554 RVA: 0x0006FD78 File Offset: 0x0006DF78
	[PublicizedFrom(EAccessModifier.Private)]
	public void Toggle()
	{
		if (PerformanceProfiler.IsCapturing())
		{
			Log.Out(string.Format("[PerformanceProfiler] Stopping after {0} frames.", PerformanceProfiler.GetCurrentFrameCount()));
			PerformanceProfiler.StopCapture(true);
			return;
		}
		Log.Out("[PerformanceProfiler] Starting capture (30 FPS target).");
		PerformanceProfiler.StartCapture(null, null, 30);
	}

	// Token: 0x060011CB RID: 4555 RVA: 0x0006FDB8 File Offset: 0x0006DFB8
	[PublicizedFrom(EAccessModifier.Private)]
	public void CmdStart(List<string> p)
	{
		if (PerformanceProfiler.IsCapturing())
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("Capture already running.");
			return;
		}
		int num2;
		int num = ((p.Count > 1 && int.TryParse(p[1], out num2)) ? num2 : 30);
		PerformanceProfiler.StartCapture(null, null, num);
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Capture started at {0} FPS target.", num));
	}

	// Token: 0x060011CC RID: 4556 RVA: 0x0006FE1D File Offset: 0x0006E01D
	[PublicizedFrom(EAccessModifier.Private)]
	public void CmdStop()
	{
		if (!PerformanceProfiler.IsCapturing())
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output("No capture in progress.");
			return;
		}
		SingletonMonoBehaviour<SdtdConsole>.Instance.Output(string.Format("Stopping capture after {0} frames.", PerformanceProfiler.GetCurrentFrameCount()));
		PerformanceProfiler.StopCapture(true);
	}
}
