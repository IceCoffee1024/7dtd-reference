using System;
using UnityEngine;

// Token: 0x0200077F RID: 1919
public class NetConnectionStatistics
{
	// Token: 0x06003931 RID: 14641 RVA: 0x00176C80 File Offset: 0x00174E80
	public void RegisterReceivedPackage(int _packageType, int _length)
	{
		this.statsPackagePerTypeReceived[_packageType]++;
		this.statsBytesPerTypeReceived[_packageType] += _length;
		this.statsLastPackagesRec.Add(new SNetPackageInfo(_packageType, _length));
	}

	// Token: 0x06003932 RID: 14642 RVA: 0x00176CB6 File Offset: 0x00174EB6
	public void RegisterReceivedData(int _packageCount, int _netDataSize)
	{
		this.statsBytesReceived += _netDataSize;
		this.statsPackagesReceived += _packageCount;
	}

	// Token: 0x06003933 RID: 14643 RVA: 0x00176CDC File Offset: 0x00174EDC
	public void RegisterSentData(int _packageCount, int _netDataSize)
	{
		this.statsBytesSent += _netDataSize;
		this.statsPackagesSent += _packageCount;
	}

	// Token: 0x06003934 RID: 14644 RVA: 0x00176D02 File Offset: 0x00174F02
	public void RegisterSentPackage(int _packageType, int _length)
	{
		this.statsPackagePerTypeSent[_packageType]++;
		this.statsBytesPerTypeSent[_packageType] += _length;
		this.statsLastPackagesSent.Add(new SNetPackageInfo(_packageType, _length));
	}

	// Token: 0x06003935 RID: 14645 RVA: 0x00176D38 File Offset: 0x00174F38
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

	// Token: 0x06003936 RID: 14646 RVA: 0x00176DF4 File Offset: 0x00174FF4
	public RingBuffer<SNetPackageInfo> GetLastPackagesSent()
	{
		return this.statsLastPackagesSent;
	}

	// Token: 0x06003937 RID: 14647 RVA: 0x00176DFC File Offset: 0x00174FFC
	public RingBuffer<SNetPackageInfo> GetLastPackagesReceived()
	{
		return this.statsLastPackagesRec;
	}

	// Token: 0x06003938 RID: 14648 RVA: 0x00176E04 File Offset: 0x00175004
	public void GetStats(float _interval, out int _bytesPerSecondSent, out int _packagesPerSecondSent, out int _bytesPerSecondReceived, out int _packagesPerSecondReceived)
	{
		_bytesPerSecondSent = this.statsBytesSent;
		_packagesPerSecondSent = this.statsPackagesSent;
		_bytesPerSecondReceived = this.statsBytesReceived;
		_packagesPerSecondReceived = this.statsPackagesReceived;
		this.resetStats();
	}

	// Token: 0x06003939 RID: 14649 RVA: 0x00176E36 File Offset: 0x00175036
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetStats()
	{
		this.statsBytesReceived = 0;
		this.statsPackagesReceived = 0;
		this.statsBytesSent = 0;
		this.statsPackagesSent = 0;
		this.lastTimeStatsRequested = Time.time;
	}

	// Token: 0x04002ECB RID: 11979
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeStatsRequested;

	// Token: 0x04002ECC RID: 11980
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsBytesSent;

	// Token: 0x04002ECD RID: 11981
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsPackagesSent;

	// Token: 0x04002ECE RID: 11982
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsPackagePerTypeSent = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002ECF RID: 11983
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsBytesPerTypeSent = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002ED0 RID: 11984
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RingBuffer<SNetPackageInfo> statsLastPackagesSent = new RingBuffer<SNetPackageInfo>(30);

	// Token: 0x04002ED1 RID: 11985
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsBytesReceived;

	// Token: 0x04002ED2 RID: 11986
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsPackagesReceived;

	// Token: 0x04002ED3 RID: 11987
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsPackagePerTypeReceived = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002ED4 RID: 11988
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsBytesPerTypeReceived = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002ED5 RID: 11989
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RingBuffer<SNetPackageInfo> statsLastPackagesRec = new RingBuffer<SNetPackageInfo>(30);

	// Token: 0x04002ED6 RID: 11990
	[PublicizedFrom(EAccessModifier.Private)]
	public int bytesPerSecondSent;

	// Token: 0x04002ED7 RID: 11991
	[PublicizedFrom(EAccessModifier.Private)]
	public int packagesPerSecondSent;

	// Token: 0x04002ED8 RID: 11992
	[PublicizedFrom(EAccessModifier.Private)]
	public int bytesPerSecondReceived;

	// Token: 0x04002ED9 RID: 11993
	[PublicizedFrom(EAccessModifier.Private)]
	public int packagesPerSecondReceived;
}
