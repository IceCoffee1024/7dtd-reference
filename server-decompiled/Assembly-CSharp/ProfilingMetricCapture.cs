using System;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;

// Token: 0x0200154D RID: 5453
public class ProfilingMetricCapture
{
	// Token: 0x0600AAD8 RID: 43736 RVA: 0x003FC23C File Offset: 0x003FA43C
	public void AddDummy(string header)
	{
		this.metrics.Add(new ConstantValueMetric
		{
			Header = header,
			value = 0
		});
	}

	// Token: 0x0600AAD9 RID: 43737 RVA: 0x003FC25C File Offset: 0x003FA45C
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

	// Token: 0x0600AADA RID: 43738 RVA: 0x003FC28C File Offset: 0x003FA48C
	public void Add(string header, CallbackMetric.GetLastValue callback)
	{
		this.metrics.Add(new CallbackMetric
		{
			callback = callback,
			Header = header
		});
	}

	// Token: 0x0600AADB RID: 43739 RVA: 0x003FC2AC File Offset: 0x003FA4AC
	public void Add(IMetric metric)
	{
		this.metrics.Add(metric);
	}

	// Token: 0x0600AADC RID: 43740 RVA: 0x003FC2BC File Offset: 0x003FA4BC
	public void Cleanup()
	{
		foreach (IMetric metric in this.metrics)
		{
			metric.Cleanup();
		}
		this.metrics.Clear();
	}

	// Token: 0x0600AADD RID: 43741 RVA: 0x003FC318 File Offset: 0x003FA518
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

	// Token: 0x0600AADE RID: 43742 RVA: 0x003FC390 File Offset: 0x003FA590
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

	// Token: 0x0600AADF RID: 43743 RVA: 0x003FC404 File Offset: 0x003FA604
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

	// Token: 0x04007ED7 RID: 32471
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IMetric> metrics = new List<IMetric>();

	// Token: 0x04007ED8 RID: 32472
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder outputBuilder = new StringBuilder();
}
