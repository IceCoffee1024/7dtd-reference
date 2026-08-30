using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200026B RID: 619
[Preserve]
public class ConsoleCmdProfiler : ConsoleCmdAbstract
{
	// Token: 0x06001263 RID: 4707 RVA: 0x00073759 File Offset: 0x00071959
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "profiler" };
	}

	// Token: 0x170001E5 RID: 485
	// (get) Token: 0x06001264 RID: 4708 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06001265 RID: 4709 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x00031E8B File Offset: 0x0003008B
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x06001267 RID: 4711 RVA: 0x0007376C File Offset: 0x0007196C
	public override void Execute(List<string> _params, CommandSenderInfo _senderInfo)
	{
		if (_params.Count == 0)
		{
			SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			return;
		}
		string text = _params[0];
		if (!(text == "listrawmetrics"))
		{
			if (!(text == "mem"))
			{
				SingletonMonoBehaviour<SdtdConsole>.Instance.Output(this.GetHelp());
			}
			else
			{
				if (this.memMetrics == null)
				{
					this.InitMemoryMetrics();
				}
				if (_params.Count == 1)
				{
					this.LogPretty(this.memMetrics);
					return;
				}
				if (_params.Count == 2)
				{
					string text2 = _params[1];
					if (text2 == "csv")
					{
						this.LogCsv(this.memMetrics);
						return;
					}
					if (!(text2 == "pretty"))
					{
						return;
					}
					this.LogPretty(this.memMetrics);
					return;
				}
			}
			return;
		}
		Log.Out(ProfilerUtils.GetAvailableMetricsCsv());
	}

	// Token: 0x06001268 RID: 4712 RVA: 0x0007383B File Offset: 0x00071A3B
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitMemoryMetrics()
	{
		if (this.memMetrics != null)
		{
			this.memMetrics.Cleanup();
		}
		this.memMetrics = ProfilerCaptureUtils.CreateMemoryProfiler();
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x0007385B File Offset: 0x00071A5B
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogCsv(ProfilingMetricCapture _metrics)
	{
		ThreadManager.StartCoroutine(this.LogCsvNextFrame(_metrics));
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x0007386A File Offset: 0x00071A6A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator LogCsvNextFrame(ProfilingMetricCapture _metrics)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Log.Out(_metrics.GetCsvHeader());
		Log.Out(_metrics.GetLastValueCsv());
		yield break;
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x00073879 File Offset: 0x00071A79
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogPretty(ProfilingMetricCapture _metrics)
	{
		ThreadManager.StartCoroutine(this.LogPrettyNextFrame(_metrics));
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x00073888 File Offset: 0x00071A88
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator LogPrettyNextFrame(ProfilingMetricCapture _metrics)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Log.Out(_metrics.PrettyPrint());
		yield break;
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x00073897 File Offset: 0x00071A97
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Utilities for collection profiling data from a variety of sources";
	}

	// Token: 0x04000D30 RID: 3376
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilingMetricCapture memMetrics;
}
