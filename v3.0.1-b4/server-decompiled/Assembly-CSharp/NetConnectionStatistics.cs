using System;
using UnityEngine;

// Token: 0x0200076B RID: 1899
public class NetConnectionStatistics
{
	// Token: 0x06003880 RID: 14464 RVA: 0x001732B0 File Offset: 0x001714B0
	public void RegisterReceivedPackage(int _packageType, int _length)
	{
		this.statsPackagePerTypeReceived[_packageType]++;
		this.statsBytesPerTypeReceived[_packageType] += _length;
		this.statsLastPackagesRec.Add(new SNetPackageInfo(_packageType, _length));
	}

	// Token: 0x06003881 RID: 14465 RVA: 0x001732E6 File Offset: 0x001714E6
	public void RegisterReceivedData(int _packageCount, int _netDataSize)
	{
		this.statsBytesReceived += _netDataSize;
		this.statsPackagesReceived += _packageCount;
	}

	// Token: 0x06003882 RID: 14466 RVA: 0x0017330C File Offset: 0x0017150C
	public void RegisterSentData(int _packageCount, int _netDataSize)
	{
		this.statsBytesSent += _netDataSize;
		this.statsPackagesSent += _packageCount;
	}

	// Token: 0x06003883 RID: 14467 RVA: 0x00173332 File Offset: 0x00171532
	public void RegisterSentPackage(int _packageType, int _length)
	{
		this.statsPackagePerTypeSent[_packageType]++;
		this.statsBytesPerTypeSent[_packageType] += _length;
		this.statsLastPackagesSent.Add(new SNetPackageInfo(_packageType, _length));
	}

	// Token: 0x06003884 RID: 14468 RVA: 0x00173368 File Offset: 0x00171568
	public void GetPackageTypes(int[] _packagesPerTypeReceived, int[] _bytesPerTypeReceived, int[] _packagesPerTypeSent, int[] _bytesPerTypeSent, bool _reset)
	{
		for (int i = 0; i < _packagesPerTypeReceived.Length; i++)
		{
			_packagesPerTypeReceived[i] += this.statsPackagePerTypeReceived[i];
			_packagesPerTypeSent[i] += this.statsPackagePerTypeSent[i];
			_bytesPerTypeReceived[i] += this.statsBytesPerTypeReceived[i];
			_bytesPerTypeSent[i] += this.statsBytesPerTypeSent[i];
		}
		if (_reset)
		{
			Array.Clear(this.statsPackagePerTypeReceived, 0, this.statsPackagePerTypeReceived.Length);
			Array.Clear(this.statsPackagePerTypeSent, 0, this.statsPackagePerTypeSent.Length);
			Array.Clear(this.statsBytesPerTypeReceived, 0, this.statsBytesPerTypeReceived.Length);
			Array.Clear(this.statsBytesPerTypeSent, 0, this.statsBytesPerTypeSent.Length);
		}
	}

	// Token: 0x06003885 RID: 14469 RVA: 0x00173424 File Offset: 0x00171624
	public RingBuffer<SNetPackageInfo> GetLastPackagesSent()
	{
		return this.statsLastPackagesSent;
	}

	// Token: 0x06003886 RID: 14470 RVA: 0x0017342C File Offset: 0x0017162C
	public RingBuffer<SNetPackageInfo> GetLastPackagesReceived()
	{
		return this.statsLastPackagesRec;
	}

	// Token: 0x06003887 RID: 14471 RVA: 0x00173434 File Offset: 0x00171634
	public void GetStats(float _interval, out int _bytesPerSecondSent, out int _packagesPerSecondSent, out int _bytesPerSecondReceived, out int _packagesPerSecondReceived)
	{
		_bytesPerSecondSent = this.statsBytesSent;
		_packagesPerSecondSent = this.statsPackagesSent;
		_bytesPerSecondReceived = this.statsBytesReceived;
		_packagesPerSecondReceived = this.statsPackagesReceived;
		this.resetStats();
	}

	// Token: 0x06003888 RID: 14472 RVA: 0x00173466 File Offset: 0x00171666
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetStats()
	{
		this.statsBytesReceived = 0;
		this.statsPackagesReceived = 0;
		this.statsBytesSent = 0;
		this.statsPackagesSent = 0;
		this.lastTimeStatsRequested = Time.time;
	}

	// Token: 0x04002E3C RID: 11836
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeStatsRequested;

	// Token: 0x04002E3D RID: 11837
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsBytesSent;

	// Token: 0x04002E3E RID: 11838
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsPackagesSent;

	// Token: 0x04002E3F RID: 11839
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsPackagePerTypeSent = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002E40 RID: 11840
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsBytesPerTypeSent = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002E41 RID: 11841
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RingBuffer<SNetPackageInfo> statsLastPackagesSent = new RingBuffer<SNetPackageInfo>(30);

	// Token: 0x04002E42 RID: 11842
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsBytesReceived;

	// Token: 0x04002E43 RID: 11843
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsPackagesReceived;

	// Token: 0x04002E44 RID: 11844
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsPackagePerTypeReceived = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002E45 RID: 11845
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsBytesPerTypeReceived = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002E46 RID: 11846
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RingBuffer<SNetPackageInfo> statsLastPackagesRec = new RingBuffer<SNetPackageInfo>(30);

	// Token: 0x04002E47 RID: 11847
	[PublicizedFrom(EAccessModifier.Private)]
	public int bytesPerSecondSent;

	// Token: 0x04002E48 RID: 11848
	[PublicizedFrom(EAccessModifier.Private)]
	public int packagesPerSecondSent;

	// Token: 0x04002E49 RID: 11849
	[PublicizedFrom(EAccessModifier.Private)]
	public int bytesPerSecondReceived;

	// Token: 0x04002E4A RID: 11850
	[PublicizedFrom(EAccessModifier.Private)]
	public int packagesPerSecondReceived;
}
