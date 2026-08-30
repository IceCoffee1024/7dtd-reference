using System;
using System.Collections.Generic;
using System.Diagnostics;

// Token: 0x02000CD5 RID: 3285
public class NetPackageMeasure
{
	// Token: 0x060064D4 RID: 25812 RVA: 0x0027729D File Offset: 0x0027549D
	public NetPackageMeasure(double _windowSizeSeconds)
	{
		this.timeWindowTicks = (long)(_windowSizeSeconds * (double)Stopwatch.Frequency);
		this.timer.Start();
	}

	// Token: 0x060064D5 RID: 25813 RVA: 0x002772D8 File Offset: 0x002754D8
	public void SamplePackages(List<NetPackage> _packages)
	{
		long num = 0L;
		foreach (NetPackage netPackage in _packages)
		{
			num += (long)netPackage.GetLength();
		}
		this.AddSample(num);
	}

	// Token: 0x060064D6 RID: 25814 RVA: 0x00277334 File Offset: 0x00275534
	public void AddSample(long _totalBytes)
	{
		this.samples.AddLast(new NetPackageMeasure.Sample(this.timer.ElapsedTicks, _totalBytes));
		this.totalSent += _totalBytes;
	}

	// Token: 0x060064D7 RID: 25815 RVA: 0x00277364 File Offset: 0x00275564
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

	// Token: 0x04004E6A RID: 20074
	[PublicizedFrom(EAccessModifier.Private)]
	public long timeWindowTicks;

	// Token: 0x04004E6B RID: 20075
	[PublicizedFrom(EAccessModifier.Private)]
	public LinkedList<NetPackageMeasure.Sample> samples = new LinkedList<NetPackageMeasure.Sample>();

	// Token: 0x04004E6C RID: 20076
	[PublicizedFrom(EAccessModifier.Private)]
	public Stopwatch timer = new Stopwatch();

	// Token: 0x04004E6D RID: 20077
	public long totalSent;

	// Token: 0x02000CD6 RID: 3286
	public struct Sample
	{
		// Token: 0x060064D8 RID: 25816 RVA: 0x002773F3 File Offset: 0x002755F3
		public Sample(long _timestamp, long _totalBytesSent)
		{
			this.timestamp = _timestamp;
			this.totalBytesSent = _totalBytesSent;
		}

		// Token: 0x04004E6E RID: 20078
		public long totalBytesSent;

		// Token: 0x04004E6F RID: 20079
		public long timestamp;
	}
}
