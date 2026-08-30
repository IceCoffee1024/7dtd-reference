using System;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;

// Token: 0x0200152B RID: 5419
public class ProfilingMetricCapture
{
	// Token: 0x0600AA25 RID: 43557 RVA: 0x003F73E8 File Offset: 0x003F55E8
	public void AddDummy(string header)
	{
		this.metrics.Add(new ConstantValueMetric
		{
			Header = header,
			value = 0
		});
	}

	// Token: 0x0600AA26 RID: 43558 RVA: 0x003F7408 File Offset: 0x003F5608
	public void Add(string header, ProfilerRecorder recorder)
	{
		if (!recorder.IsRunning)
		{
			recorder.Start();
		}
		this.metrics.Add(new ProfilerRecorderMetric
		{
			recorder = recorder,
			Header = header
		});
	}

	// Token: 0x0600AA27 RID: 43559 RVA: 0x003F7438 File Offset: 0x003F5638
	public void Add(string header, CallbackMetric.GetLastValue callback)
	{
		this.metrics.Add(new CallbackMetric
		{
			callback = callback,
			Header = header
		});
	}

	// Token: 0x0600AA28 RID: 43560 RVA: 0x003F7458 File Offset: 0x003F5658
	public void Add(IMetric metric)
	{
		this.metrics.Add(metric);
	}

	// Token: 0x0600AA29 RID: 43561 RVA: 0x003F7468 File Offset: 0x003F5668
	public void Cleanup()
	{
		foreach (IMetric metric in this.metrics)
		{
			metric.Cleanup();
		}
		this.metrics.Clear();
	}

	// Token: 0x0600AA2A RID: 43562 RVA: 0x003F74C4 File Offset: 0x003F56C4
	public string GetCsvHeader()
	{
		for (int i = 0; i < this.metrics.Count; i++)
		{
			this.outputBuilder.Append(this.metrics[i].Header);
			if (i < this.metrics.Count - 1)
			{
				this.outputBuilder.Append(",");
			}
		}
		string text = this.outputBuilder.ToString();
		this.outputBuilder.Clear();
		return text;
	}

	// Token: 0x0600AA2B RID: 43563 RVA: 0x003F753C File Offset: 0x003F573C
	public string GetLastValueCsv()
	{
		for (int i = 0; i < this.metrics.Count; i++)
		{
			this.metrics[i].AppendLastValue(this.outputBuilder);
			if (i < this.metrics.Count - 1)
			{
				this.outputBuilder.Append(",");
			}
		}
		string text = this.outputBuilder.ToString();
		this.outputBuilder.Clear();
		return text;
	}

	// Token: 0x0600AA2C RID: 43564 RVA: 0x003F75B0 File Offset: 0x003F57B0
	public string PrettyPrint()
	{
		for (int i = 0; i < this.metrics.Count; i++)
		{
			this.outputBuilder.AppendFormat("{0}: ", this.metrics[i].Header);
			this.metrics[i].AppendLastValue(this.outputBuilder);
			this.outputBuilder.AppendLine();
		}
		string text = this.outputBuilder.ToString();
		this.outputBuilder.Clear();
		return text;
	}

	// Token: 0x04007E54 RID: 32340
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IMetric> metrics = new List<IMetric>();

	// Token: 0x04007E55 RID: 32341
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder outputBuilder = new StringBuilder();
}
