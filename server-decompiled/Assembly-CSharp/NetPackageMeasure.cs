using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000CEE RID: 3310
public class NetPackageMeasure
{
	// Token: 0x060064EC RID: 25836 RVA: 0x0027BBC9 File Offset: 0x00279DC9
	public NetPackageMeasure(double _windowSizeSeconds)
	{
		this.timeWindowTicks = (long)(_windowSizeSeconds * (double)Stopwatch.Frequency);
		this.timer.Start();
	}

	// Token: 0x060064ED RID: 25837 RVA: 0x0027BC01 File Offset: 0x00279E01
	public void AddSample(long _totalBytes)
	{
		this.samples.AddLast(new NetPackageMeasure.Sample(this.timer.ElapsedTicks, _totalBytes));
		this.totalSent += _totalBytes;
	}

	// Token: 0x060064EE RID: 25838 RVA: 0x0027BC30 File Offset: 0x00279E30
	public void RecalculateTotals()
	{
		this.timer.Stop();
		while (this.samples.First != null && Math.Abs(this.timer.ElapsedTicks - this.samples.First.Value.timestamp) > this.timeWindowTicks)
		{
			NetPackageMeasure.Sample value = this.samples.First.Value;
			this.totalSent -= value.totalBytesSent;
			this.samples.RemoveFirst();
		}
		this.timer.Start();
	}

	// Token: 0x04004EC5 RID: 20165
	[PublicizedFrom(EAccessModifier.Private)]
	public long timeWindowTicks;

	// Token: 0x04004EC6 RID: 20166
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<NetPackageMeasure.Sample> samples = new LinkedList<NetPackageMeasure.Sample>();

	// Token: 0x04004EC7 RID: 20167
	[PublicizedFrom(EAccessModifier.Private)]
	public Stopwatch timer = new Stopwatch();

	// Token: 0x04004EC8 RID: 20168
	public long totalSent;

	// Token: 0x02000CEF RID: 3311
	public struct Sample
	{
		// Token: 0x060064EF RID: 25839 RVA: 0x0027BCBF File Offset: 0x00279EBF
		public Sample(long _timestamp, long _totalBytesSent)
		{
			this.timestamp = _timestamp;
			this.totalBytesSent = _totalBytesSent;
		}

		// Token: 0x04004EC9 RID: 20169
		public long totalBytesSent;

		// Token: 0x04004ECA RID: 20170
		public long timestamp;
	}
}
