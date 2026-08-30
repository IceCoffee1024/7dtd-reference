using System;
using System.Collections.Generic;
using Platform;
using UnityEngine.Scripting;

// Token: 0x02000251 RID: 593
[Preserve]
public class ConsoleCmdPerformanceProfiler : ConsoleCmdAbstract
{
	// Token: 0x060011B0 RID: 4528 RVA: 0x0006F205 File Offset: 0x0006D405
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "performanceprofiler", "pp" };
	}

	// Token: 0x060011B1 RID: 4529 RVA: 0x0006F21D File Offset: 0x0006D41D
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Performance Profiling Utility";
	}

	// Token: 0x060011B2 RID: 4530 RVA: 0x0006F224 File Offset: 0x0006D424
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "Type 'pp' to toggle capture, or 'pp start [fps]' / 'pp stop'.";
	}

	// Token: 0x170001CD RID: 461
	// (get) Token: 0x060011B3 RID: 4531 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001CE RID: 462
	// (get) Token: 0x060011B4 RID: 4532 RVA: 0x0006110A File Offset: 0x0005F30A
	public override int DefaultPermissionLevel
	{
		get
		{
			return 1000;
		}
	}

	// Token: 0x170001CF RID: 463
	// (get) Token: 0x060011B5 RID: 4533 RVA: 0x0005FE4F File Offset: 0x0005E04F
	public override DeviceFlag AllowedDeviceTypesClient
	{
		get
		{
			return DeviceFlag.StandaloneWindows | DeviceFlag.StandaloneLinux | DeviceFlag.StandaloneOSX | DeviceFlag.XBoxSeriesS | DeviceFlag.XBoxSeriesX | DeviceFlag.PS5;
		}
	}

	// Token: 0x060011B6 RID: 4534 RVA: 0x0006F22C File Offset: 0x0006D42C
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

	// Token: 0x060011B7 RID: 4535 RVA: 0x0006F2A0 File Offset: 0x0006D4A0
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

	// Token: 0x060011B8 RID: 4536 RVA: 0x0006F2E0 File Offset: 0x0006D4E0
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

	// Token: 0x060011B9 RID: 4537 RVA: 0x0006F345 File Offset: 0x0006D545
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
