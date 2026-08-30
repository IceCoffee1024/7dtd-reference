using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000CC3 RID: 3267
public class NetPackageMeasure
{
	// Token: 0x06006448 RID: 25672 RVA: 0x002743A1 File Offset: 0x002725A1
	public NetPackageMeasure(double _windowSizeSeconds)
	{
		this.timeWindowTicks = (long)(_windowSizeSeconds * (double)Stopwatch.Frequency);
		this.timer.Start();
	}

	// Token: 0x06006449 RID: 25673 RVA: 0x002743DC File Offset: 0x002725DC
	public void SamplePackages(List<NetPackage> _packages)
	{
		long num = 0L;
		foreach (NetPackage netPackage in _packages)
		{
			num += (long)netPackage.GetLength();
		}
		this.AddSample(num);
	}

	// Token: 0x0600644A RID: 25674 RVA: 0x00274438 File Offset: 0x00272638
	public void AddSample(long _totalBytes)
	{
		this.samples.AddLast(new NetPackageMeasure.Sample(this.timer.ElapsedTicks, _totalBytes));
		this.totalSent += _totalBytes;
	}

	// Token: 0x0600644B RID: 25675 RVA: 0x00274468 File Offset: 0x00272668
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

	// Token: 0x04004DCC RID: 19916
	[PublicizedFrom(EAccessModifier.Private)]
	public long timeWindowTicks;

	// Token: 0x04004DCD RID: 19917
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<NetPackageMeasure.Sample> samples = new LinkedList<NetPackageMeasure.Sample>();

	// Token: 0x04004DCE RID: 19918
	[PublicizedFrom(EAccessModifier.Private)]
	public Stopwatch timer = new Stopwatch();

	// Token: 0x04004DCF RID: 19919
	public long totalSent;

	// Token: 0x02000CC4 RID: 3268
	public struct Sample
	{
		// Token: 0x0600644C RID: 25676 RVA: 0x002744F7 File Offset: 0x002726F7
		public Sample(long _timestamp, long _totalBytesSent)
		{
			this.timestamp = _timestamp;
			this.totalBytesSent = _totalBytesSent;
		}

		// Token: 0x04004DD0 RID: 19920
		public long totalBytesSent;

		// Token: 0x04004DD1 RID: 19921
		public long timestamp;
	}
}
