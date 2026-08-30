using System;
using System.Collections.Generic;
using System.Text;
using Unity.Profiling;

// Token: 0x02001510 RID: 5392
public class ProfilingMetricCapture
{
	// Token: 0x0600A928 RID: 43304 RVA: 0x003F20A8 File Offset: 0x003F02A8
	public void AddDummy(string header)
	{
		this.metrics.Add(new ConstantValueMetric
		{
			Header = header,
			value = 0
		});
	}

	// Token: 0x0600A929 RID: 43305 RVA: 0x003F20C8 File Offset: 0x003F02C8
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

	// Token: 0x0600A92A RID: 43306 RVA: 0x003F20F8 File Offset: 0x003F02F8
	public void Add(string header, CallbackMetric.GetLastValue callback)
	{
		this.metrics.Add(new CallbackMetric
		{
			callback = callback,
			Header = header
		});
	}

	// Token: 0x0600A92B RID: 43307 RVA: 0x003F2118 File Offset: 0x003F0318
	public void Add(IMetric metric)
	{
		this.metrics.Add(metric);
	}

	// Token: 0x0600A92C RID: 43308 RVA: 0x003F2128 File Offset: 0x003F0328
	public void Cleanup()
	{
		foreach (IMetric metric in this.metrics)
		{
			metric.Cleanup();
		}
		this.metrics.Clear();
	}

	// Token: 0x0600A92D RID: 43309 RVA: 0x003F2184 File Offset: 0x003F0384
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

	// Token: 0x0600A92E RID: 43310 RVA: 0x003F21FC File Offset: 0x003F03FC
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

	// Token: 0x0600A92F RID: 43311 RVA: 0x003F2270 File Offset: 0x003F0470
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

	// Token: 0x04007D7B RID: 32123
	[PublicizedFrom(EAccessModifier.Private)]
	public List<IMetric> metrics = new List<IMetric>();

	// Token: 0x04007D7C RID: 32124
	[PublicizedFrom(EAccessModifier.Private)]
	public StringBuilder outputBuilder = new StringBuilder();
}
