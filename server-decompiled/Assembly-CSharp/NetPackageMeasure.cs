using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000CE3 RID: 3299
public class NetPackageMeasure
{
	// Token: 0x060064C7 RID: 25799 RVA: 0x0027A6A1 File Offset: 0x002788A1
	public NetPackageMeasure(double _windowSizeSeconds)
	{
		this.timeWindowTicks = (long)(_windowSizeSeconds * (double)Stopwatch.Frequency);
		this.timer.Start();
	}

	// Token: 0x060064C8 RID: 25800 RVA: 0x0027A6D9 File Offset: 0x002788D9
	public void AddSample(long _totalBytes)
	{
		this.samples.AddLast(new NetPackageMeasure.Sample(this.timer.ElapsedTicks, _totalBytes));
		this.totalSent += _totalBytes;
	}

	// Token: 0x060064C9 RID: 25801 RVA: 0x0027A708 File Offset: 0x00278908
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

	// Token: 0x04004EA0 RID: 20128
	[PublicizedFrom(EAccessModifier.Private)]
	public long timeWindowTicks;

	// Token: 0x04004EA1 RID: 20129
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<NetPackageMeasure.Sample> samples = new LinkedList<NetPackageMeasure.Sample>();

	// Token: 0x04004EA2 RID: 20130
	[PublicizedFrom(EAccessModifier.Private)]
	public Stopwatch timer = new Stopwatch();

	// Token: 0x04004EA3 RID: 20131
	public long totalSent;

	// Token: 0x02000CE4 RID: 3300
	public struct Sample
	{
		// Token: 0x060064CA RID: 25802 RVA: 0x0027A797 File Offset: 0x00278997
		public Sample(long _timestamp, long _totalBytesSent)
		{
			this.timestamp = _timestamp;
			this.totalBytesSent = _totalBytesSent;
		}

		// Token: 0x04004EA4 RID: 20132
		public long totalBytesSent;

		// Token: 0x04004EA5 RID: 20133
		public long timestamp;
	}
}
