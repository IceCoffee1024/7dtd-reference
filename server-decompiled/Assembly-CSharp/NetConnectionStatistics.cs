using System;
using UnityEngine;

// Token: 0x02000777 RID: 1911
public class NetConnectionStatistics
{
	// Token: 0x0600391A RID: 14618 RVA: 0x00175CE8 File Offset: 0x00173EE8
	public void RegisterReceivedPackage(int _packageType, int _length)
	{
		this.statsPackagePerTypeReceived[_packageType]++;
		this.statsBytesPerTypeReceived[_packageType] += _length;
		this.statsLastPackagesRec.Add(new SNetPackageInfo(_packageType, _length));
	}

	// Token: 0x0600391B RID: 14619 RVA: 0x00175D1E File Offset: 0x00173F1E
	public void RegisterReceivedData(int _packageCount, int _netDataSize)
	{
		this.statsBytesReceived += _netDataSize;
		this.statsPackagesReceived += _packageCount;
	}

	// Token: 0x0600391C RID: 14620 RVA: 0x00175D44 File Offset: 0x00173F44
	public void RegisterSentData(int _packageCount, int _netDataSize)
	{
		this.statsBytesSent += _netDataSize;
		this.statsPackagesSent += _packageCount;
	}

	// Token: 0x0600391D RID: 14621 RVA: 0x00175D6A File Offset: 0x00173F6A
	public void RegisterSentPackage(int _packageType, int _length)
	{
		this.statsPackagePerTypeSent[_packageType]++;
		this.statsBytesPerTypeSent[_packageType] += _length;
		this.statsLastPackagesSent.Add(new SNetPackageInfo(_packageType, _length));
	}

	// Token: 0x0600391E RID: 14622 RVA: 0x00175DA0 File Offset: 0x00173FA0
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

	// Token: 0x0600391F RID: 14623 RVA: 0x00175E5C File Offset: 0x0017405C
	public RingBuffer<SNetPackageInfo> GetLastPackagesSent()
	{
		return this.statsLastPackagesSent;
	}

	// Token: 0x06003920 RID: 14624 RVA: 0x00175E64 File Offset: 0x00174064
	public RingBuffer<SNetPackageInfo> GetLastPackagesReceived()
	{
		return this.statsLastPackagesRec;
	}

	// Token: 0x06003921 RID: 14625 RVA: 0x00175E6C File Offset: 0x0017406C
	public void GetStats(float _interval, out int _bytesPerSecondSent, out int _packagesPerSecondSent, out int _bytesPerSecondReceived, out int _packagesPerSecondReceived)
	{
		_bytesPerSecondSent = this.statsBytesSent;
		_packagesPerSecondSent = this.statsPackagesSent;
		_bytesPerSecondReceived = this.statsBytesReceived;
		_packagesPerSecondReceived = this.statsPackagesReceived;
		this.resetStats();
	}

	// Token: 0x06003922 RID: 14626 RVA: 0x00175E9E File Offset: 0x0017409E
	[PublicizedFrom(EAccessModifier.Private)]
	public void resetStats()
	{
		this.statsBytesReceived = 0;
		this.statsPackagesReceived = 0;
		this.statsBytesSent = 0;
		this.statsPackagesSent = 0;
		this.lastTimeStatsRequested = Time.time;
	}

	// Token: 0x04002EAF RID: 11951
	[PublicizedFrom(EAccessModifier.Private)]
	public float lastTimeStatsRequested;

	// Token: 0x04002EB0 RID: 11952
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsBytesSent;

	// Token: 0x04002EB1 RID: 11953
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsPackagesSent;

	// Token: 0x04002EB2 RID: 11954
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsPackagePerTypeSent = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002EB3 RID: 11955
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsBytesPerTypeSent = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002EB4 RID: 11956
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RingBuffer<SNetPackageInfo> statsLastPackagesSent = new RingBuffer<SNetPackageInfo>(30);

	// Token: 0x04002EB5 RID: 11957
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsBytesReceived;

	// Token: 0x04002EB6 RID: 11958
	[PublicizedFrom(EAccessModifier.Private)]
	public volatile int statsPackagesReceived;

	// Token: 0x04002EB7 RID: 11959
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsPackagePerTypeReceived = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002EB8 RID: 11960
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] statsBytesPerTypeReceived = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04002EB9 RID: 11961
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly RingBuffer<SNetPackageInfo> statsLastPackagesRec = new RingBuffer<SNetPackageInfo>(30);

	// Token: 0x04002EBA RID: 11962
	[PublicizedFrom(EAccessModifier.Private)]
	public int bytesPerSecondSent;

	// Token: 0x04002EBB RID: 11963
	[PublicizedFrom(EAccessModifier.Private)]
	public int packagesPerSecondSent;

	// Token: 0x04002EBC RID: 11964
	[PublicizedFrom(EAccessModifier.Private)]
	public int bytesPerSecondReceived;

	// Token: 0x04002EBD RID: 11965
	[PublicizedFrom(EAccessModifier.Private)]
	public int packagesPerSecondReceived;
}
