using System;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;

// Token: 0x0200155F RID: 5471
public class ProfilingMetricCapture
{
	// Token: 0x0600AB30 RID: 43824 RVA: 0x003FE640 File Offset: 0x003FC840
	public void AddDummy(string header)
	{
		this.metrics.Add(new ConstantValueMetric
		{
			Header = header,
			value = 0
		});
	}

	// Token: 0x0600AB31 RID: 43825 RVA: 0x003FE660 File Offset: 0x003FC860
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

	// Token: 0x0600AB32 RID: 43826 RVA: 0x003FE690 File Offset: 0x003FC890
	public void Add(string header, CallbackMetric.GetLastValue callback)
	{
		this.metrics.Add(new CallbackMetric
		{
			callback = callback,
			Header = header
		});
	}

	// Token: 0x0600AB33 RID: 43827 RVA: 0x003FE6B0 File Offset: 0x003FC8B0
	public void Add(IMetric metric)
	{
		this.metrics.Add(metric);
	}

	// Token: 0x0600AB34 RID: 43828 RVA: 0x003FE6C0 File Offset: 0x003FC8C0
	public void Cleanup()
	{
		foreach (IMetric metric in this.metrics)
		{
			metric.Cleanup();
		}
		this.metrics.Clear();
	}

	// Token: 0x0600AB35 RID: 43829 RVA: 0x003FE71C File Offset: 0x003FC91C
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

	// Token: 0x0600AB36 RID: 43830 RVA: 0x003FE794 File Offset: 0x003FC994
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

	// Token: 0x0600AB37 RID: 43831 RVA: 0x003FE808 File Offset: 0x003FCA08
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

	// Token: 0x04007F18 RID: 32536
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IMetric> metrics = new List<IMetric>();

	// Token: 0x04007F19 RID: 32537
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder outputBuilder = new StringBuilder();
}
