using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000274 RID: 628
[Preserve]
public class ConsoleCmdProfiler : ConsoleCmdAbstract
{
	// Token: 0x06001276 RID: 4726 RVA: 0x00074239 File Offset: 0x00072439
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "profiler" };
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06001277 RID: 4727 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06001278 RID: 4728 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x000321FF File Offset: 0x000303FF
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x0007424C File Offset: 0x0007244C
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

	// Token: 0x0600127B RID: 4731 RVA: 0x0007431B File Offset: 0x0007251B
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitMemoryMetrics()
	{
		if (this.memMetrics != null)
		{
			this.memMetrics.Cleanup();
		}
		this.memMetrics = ProfilerCaptureUtils.CreateMemoryProfiler();
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x0007433B File Offset: 0x0007253B
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogCsv(ProfilingMetricCapture _metrics)
	{
		ThreadManager.StartCoroutine(this.LogCsvNextFrame(_metrics));
	}

	// Token: 0x0600127D RID: 4733 RVA: 0x0007434A File Offset: 0x0007254A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator LogCsvNextFrame(ProfilingMetricCapture _metrics)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Log.Out(_metrics.GetCsvHeader());
		Log.Out(_metrics.GetLastValueCsv());
		yield break;
	}

	// Token: 0x0600127E RID: 4734 RVA: 0x00074359 File Offset: 0x00072559
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogPretty(ProfilingMetricCapture _metrics)
	{
		ThreadManager.StartCoroutine(this.LogPrettyNextFrame(_metrics));
	}

	// Token: 0x0600127F RID: 4735 RVA: 0x00074368 File Offset: 0x00072568
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator LogPrettyNextFrame(ProfilingMetricCapture _metrics)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Log.Out(_metrics.PrettyPrint());
		yield break;
	}

	// Token: 0x06001280 RID: 4736 RVA: 0x00074377 File Offset: 0x00072577
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Utilities for collection profiling data from a variety of sources";
	}

	// Token: 0x04000D3B RID: 3387
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilingMetricCapture memMetrics;
}
