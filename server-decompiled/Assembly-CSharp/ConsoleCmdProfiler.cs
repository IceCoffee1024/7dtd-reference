using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000272 RID: 626
[Preserve]
public class ConsoleCmdProfiler : ConsoleCmdAbstract
{
	// Token: 0x06001272 RID: 4722 RVA: 0x00074069 File Offset: 0x00072269
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string[] getCommands()
	{
		return new string[] { "profiler" };
	}

	// Token: 0x170001DE RID: 478
	// (get) Token: 0x06001273 RID: 4723 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool IsExecuteOnClient
	{
		get
		{
			return true;
		}
	}

	// Token: 0x170001DF RID: 479
	// (get) Token: 0x06001274 RID: 4724 RVA: 0x00020119 File Offset: 0x0001E319
	public override bool AllowedInMainMenu
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06001275 RID: 4725 RVA: 0x0003223F File Offset: 0x0003043F
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getHelp()
	{
		return "";
	}

	// Token: 0x06001276 RID: 4726 RVA: 0x0007407C File Offset: 0x0007227C
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

	// Token: 0x06001277 RID: 4727 RVA: 0x0007414B File Offset: 0x0007234B
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitMemoryMetrics()
	{
		if (this.memMetrics != null)
		{
			this.memMetrics.Cleanup();
		}
		this.memMetrics = ProfilerCaptureUtils.CreateMemoryProfiler();
	}

	// Token: 0x06001278 RID: 4728 RVA: 0x0007416B File Offset: 0x0007236B
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogCsv(ProfilingMetricCapture _metrics)
	{
		ThreadManager.StartCoroutine(this.LogCsvNextFrame(_metrics));
	}

	// Token: 0x06001279 RID: 4729 RVA: 0x0007417A File Offset: 0x0007237A
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator LogCsvNextFrame(ProfilingMetricCapture _metrics)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Log.Out(_metrics.GetCsvHeader());
		Log.Out(_metrics.GetLastValueCsv());
		yield break;
	}

	// Token: 0x0600127A RID: 4730 RVA: 0x00074189 File Offset: 0x00072389
	[PublicizedFrom(EAccessModifier.Private)]
	public void LogPretty(ProfilingMetricCapture _metrics)
	{
		ThreadManager.StartCoroutine(this.LogPrettyNextFrame(_metrics));
	}

	// Token: 0x0600127B RID: 4731 RVA: 0x00074198 File Offset: 0x00072398
	[PublicizedFrom(EAccessModifier.Private)]
	public IEnumerator LogPrettyNextFrame(ProfilingMetricCapture _metrics)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Log.Out(_metrics.PrettyPrint());
		yield break;
	}

	// Token: 0x0600127C RID: 4732 RVA: 0x000741A7 File Offset: 0x000723A7
	[PublicizedFrom(EAccessModifier.Protected)]
	public override string getDescription()
	{
		return "Utilities for collection profiling data from a variety of sources";
	}

	// Token: 0x04000D3A RID: 3386
	[PublicizedFrom(EAccessModifier.Private)]
	public ProfilingMetricCapture memMetrics;
}
