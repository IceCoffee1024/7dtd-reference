using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x0200026F RID: 623
[Preserve]
public class ConsoleCmdProfiler : ConsoleCmdAbstract
{
	// Token: 0x06001275 RID: 4725 RVA: 0x00073DB5 File Offset: 0x00071FB5
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "profiler" };
	}

	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06001276 RID: 4726 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06001277 RID: 4727 RVA: 0x00020021 File Offset: 0x0001E221
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x00032147 File Offset: 0x00030347
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x00073DC8 File Offset: 0x00071FC8
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

	// Token: 0x0600127A RID: 4730 RVA: 0x00073E97 File Offset: 0x00072097
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitMemoryMetrics()
	{
		if (this.memMetrics != null)
		{
			this.memMetrics.Cleanup();
		}
		this.memMetrics = ProfilerCaptureUtils.CreateMemoryProfiler();
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x00073EB7 File Offset: 0x000720B7
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogCsv(ProfilingMetricCapture _metrics)
	{
		ThreadManager.StartCoroutine(this.LogCsvNextFrame(_metrics));
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x00073EC6 File Offset: 0x000720C6
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator LogCsvNextFrame(ProfilingMetricCapture _metrics)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Log.Out(_metrics.GetCsvHeader());
		Log.Out(_metrics.GetLastValueCsv());
		yield break;
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x00073ED5 File Offset: 0x000720D5
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogPretty(ProfilingMetricCapture _metrics)
	{
		ThreadManager.StartCoroutine(this.LogPrettyNextFrame(_metrics));
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x00073EE4 File Offset: 0x000720E4
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator LogPrettyNextFrame(ProfilingMetricCapture _metrics)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Log.Out(_metrics.PrettyPrint());
		yield break;
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x00073EF3 File Offset: 0x000720F3
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Utilities for collection profiling data from a variety of sources";
	}

	// Token: 0x04000D41 RID: 3393
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilingMetricCapture memMetrics;
}
