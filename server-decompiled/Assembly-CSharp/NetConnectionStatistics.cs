using System;
using UnityEngine;

// Token: 0x02000775 RID: 1909
public class NetConnectionStatistics
{
	// Token: 0x060038D8 RID: 14552 RVA: 0x00175530 File Offset: 0x00173730
	public void RegisterReceivedPackage(int _packageType, int _length)
	{
		this.statsPackagePerTypeReceived[_packageType]++;
		this.statsBytesPerTypeReceived[_packageType] += _length;
		this.statsLastPackagesRec.Add(new SNetPackageInfo(_packageType, _length));
	}

	// Token: 0x060038D9 RID: 14553 RVA: 0x00175566 File Offset: 0x00173766
	public void RegisterReceivedData(int _packageCount, int _netDataSize)
	{
		this.statsBytesReceived += _netDataSize;
		this.statsPackagesReceived += _packageCount;
	}

	// Token: 0x060038DA RID: 14554 RVA: 0x0017558C File Offset: 0x0017378C
	public void RegisterSentData(int _packageCount, int _netDataSize)
	{
		this.statsBytesSent += _netDataSize;
		this.statsPackagesSent += _packageCount;
	}

	// Token: 0x060038DB RID: 14555 RVA: 0x001755B2 File Offset: 0x001737B2
	public void RegisterSentPackage(int _packageType, int _length)
	{
		this.statsPackagePerTypeSent[_packageType]++;
		this.statsBytesPerTypeSent[_packageType] += _length;
		this.statsLastPackagesSent.Add(new SNetPackageInfo(_packageType, _length));
	}

	// Token: 0x060038DC RID: 14556 RVA: 0x001755E8 File Offset: 0x001737E8
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

	// Token: 0x060038DD RID: 14557 RVA: 0x001756A4 File Offset: 0x001738A4
	public RingBuffer<SNetPackageInfo> GetLastPackagesSent()
	{
		return this.statsLastPackagesSent;
	}

	// Token: 0x060038DE RID: 14558 RVA: 0x001756AC File Offset: 0x001738AC
	public RingBuffer<SNetPackageInfo> GetLastPackagesReceived()
	{
		return this.statsLastPackagesRec;
	}

	// Token: 0x060038DF RID: 14559 RVA: 0x001756B4 File Offset: 0x001738B4
	public void GetStats(float _interval, out int _bytesPerSecondSent, out int _packagesPerSecondSent, out int _bytesPerSecondReceived, out int _packagesPerSecondReceived)
	{
		_bytesPerSecondSent = this.statsBytesSent;
		_packagesPerSecondSent = this.statsPackagesSent;
		_bytesPerSecondReceived = this.statsBytesReceived;
		_packagesPerSecondReceived = this.statsPackagesReceived;
		this.resetStats();
	}

	// Token: 0x060038E0 RID: 14560 RVA: 0x001756E6 File Offset: 0x001738E6
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetStats()
	{
		this.statsBytesReceived = 0;
		this.statsPackagesReceived = 0;
		this.statsBytesSent = 0;
		this.statsPackagesSent = 0;
		this.lastTimeStatsRequested = Time.time;
	}

	// Token: 0x04002EB5 RID: 11957
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeStatsRequested;

	// Token: 0x04002EB6 RID: 11958
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsBytesSent;

	// Token: 0x04002EB7 RID: 11959
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsPackagesSent;

	// Token: 0x04002EB8 RID: 11960
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsPackagePerTypeSent = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002EB9 RID: 11961
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsBytesPerTypeSent = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002EBA RID: 11962
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RingBuffer<SNetPackageInfo> statsLastPackagesSent = new RingBuffer<SNetPackageInfo>(30);

	// Token: 0x04002EBB RID: 11963
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsBytesReceived;

	// Token: 0x04002EBC RID: 11964
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsPackagesReceived;

	// Token: 0x04002EBD RID: 11965
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsPackagePerTypeReceived = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002EBE RID: 11966
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsBytesPerTypeReceived = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002EBF RID: 11967
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RingBuffer<SNetPackageInfo> statsLastPackagesRec = new RingBuffer<SNetPackageInfo>(30);

	// Token: 0x04002EC0 RID: 11968
	[PublicizedFrom(EAccessModifier.Private)]
	public int bytesPerSecondSent;

	// Token: 0x04002EC1 RID: 11969
	[PublicizedFrom(EAccessModifier.Private)]
	public int packagesPerSecondSent;

	// Token: 0x04002EC2 RID: 11970
	[PublicizedFrom(EAccessModifier.Private)]
	public int bytesPerSecondReceived;

	// Token: 0x04002EC3 RID: 11971
	[PublicizedFrom(EAccessModifier.Private)]
	public int packagesPerSecondReceived;
}
