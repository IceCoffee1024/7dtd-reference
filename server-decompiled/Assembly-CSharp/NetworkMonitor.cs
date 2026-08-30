using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;

// Token: 0x0200144F RID: 5199
public class NetworkMonitor
{
	// Token: 0x0600A341 RID: 41793 RVA: 0x003D0398 File Offset: 0x003CE598
	public NetworkMonitor(int _channel, Transform _parent)
	{
		this.channel = _channel;
		this.parent = _parent;
		this.uiLblOverview = _parent.Find("lblOverview").GetComponent<UILabel>();
		Transform transform;
		this.uiLblRecvPkgCnt = (((transform = _parent.Find("lblRecvPkgCnt")) != null) ? transform.GetComponent<UILabel>() : null);
		this.uiLblSentPkgCnt = (((transform = _parent.Find("lblSentPkgCnt")) != null) ? transform.GetComponent<UILabel>() : null);
		this.uiLblRecvPkgSeq = (((transform = _parent.Find("lblRecvPkgSeq")) != null) ? transform.GetComponent<UILabel>() : null);
		this.uiLblSentPkgSeq = (((transform = _parent.Find("lblSentPkgSeq")) != null) ? transform.GetComponent<UILabel>() : null);
		this.uiTxtRecv = (((transform = _parent.Find("texRecv")) != null) ? transform.GetComponent<UITexture>() : null);
		this.uiTxtSent = (((transform = _parent.Find("texSent")) != null) ? transform.GetComponent<UITexture>() : null);
	}

	// Token: 0x0600A342 RID: 41794 RVA: 0x003D0500 File Offset: 0x003CE700
	[PublicizedFrom(EAccessModifier.Private)]
	public void Init()
	{
		this.initialized = true;
		if (this.uiTxtRecv != null)
		{
			this.graphReceived = new SimpleGraph();
			this.graphReceived.Init(1024, 128, 1f, new float[] { 0.01f, 0.5f, 1f });
			this.uiTxtRecv.mainTexture = this.graphReceived.texture;
		}
		if (this.uiTxtSent != null)
		{
			this.graphSent = new SimpleGraph();
			this.graphSent.Init(1024, 128, 1f, new float[] { 0.01f, 0.5f, 1f });
			this.uiTxtSent.mainTexture = this.graphSent.texture;
		}
	}

	// Token: 0x0600A343 RID: 41795 RVA: 0x003D05C8 File Offset: 0x003CE7C8
	public void Cleanup()
	{
		SimpleGraph simpleGraph = this.graphReceived;
		if (simpleGraph != null)
		{
			simpleGraph.Cleanup();
		}
		SimpleGraph simpleGraph2 = this.graphSent;
		if (simpleGraph2 == null)
		{
			return;
		}
		simpleGraph2.Cleanup();
	}

	// Token: 0x0600A344 RID: 41796 RVA: 0x003D05EC File Offset: 0x003CE7EC
	public void ResetAllNumbers()
	{
		this.bResetNext = true;
		this.sumBRecv = 0;
		this.totalBRecv = 0;
		this.sumBSent = 0;
		this.totalBSent = 0;
		this.sumPRecv = 0;
		this.totalPRecv = 0;
		this.sumPSent = 0;
		this.totalPSent = 0;
	}

	// Token: 0x0600A345 RID: 41797 RVA: 0x003D0638 File Offset: 0x003CE838
	[PublicizedFrom(EAccessModifier.Private)]
	public void updateDataForConnection(INetConnection[] _nc)
	{
		if (this.channel >= _nc.Length)
		{
			return;
		}
		if (_nc[this.channel] != null)
		{
			int num;
			int num2;
			int num3;
			int num4;
			_nc[this.channel].GetStats().GetStats(0f, out num, out num2, out num3, out num4);
			this.totalBytesSent += num;
			this.totalBytesReceived += num3;
			this.totalPackagesSent += num2;
			this.totalPackagesReceived += num4;
			_nc[this.channel].GetStats().GetPackageTypes(this.packagesPerTypeReceived, this.bytesPerTypeReceived, this.packagesPerTypeSent, this.bytesPerTypeSent, this.bResetNext);
			this.recSequence = _nc[this.channel].GetStats().GetLastPackagesReceived();
			this.sentSequence = _nc[this.channel].GetStats().GetLastPackagesSent();
		}
		this.bResetNext = false;
	}

	// Token: 0x0600A346 RID: 41798 RVA: 0x003D071C File Offset: 0x003CE91C
	public void Update()
	{
		if (this.Enabled != this.parent.gameObject.activeSelf)
		{
			this.parent.gameObject.SetActive(this.Enabled);
		}
		if (!this.Enabled)
		{
			this.bpsRecv = (this.bpsSent = 0f);
			this.sumPRecv = (this.sumPSent = 0);
			return;
		}
		this.timePassed += Time.deltaTime;
		if (!this.initialized)
		{
			this.Init();
		}
		this.totalBytesSent = 0;
		this.totalBytesReceived = 0;
		this.totalPackagesSent = 0;
		this.totalPackagesReceived = 0;
		Array.Clear(this.packagesPerTypeReceived, 0, this.packagesPerTypeReceived.Length);
		Array.Clear(this.bytesPerTypeReceived, 0, this.bytesPerTypeReceived.Length);
		Array.Clear(this.packagesPerTypeSent, 0, this.packagesPerTypeSent.Length);
		Array.Clear(this.bytesPerTypeSent, 0, this.bytesPerTypeSent.Length);
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			ReadOnlyCollection<ClientInfo> list = SingletonMonoBehaviour<ConnectionManager>.Instance.Clients.List;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					ClientInfo clientInfo = list[i];
					this.updateDataForConnection(clientInfo.netConnection);
				}
			}
		}
		else
		{
			this.updateDataForConnection(SingletonMonoBehaviour<ConnectionManager>.Instance.GetConnectionToServer());
		}
		SimpleGraph simpleGraph = this.graphReceived;
		if (simpleGraph != null)
		{
			simpleGraph.Update((float)this.totalBytesReceived / 1024f, Color.green);
		}
		SimpleGraph simpleGraph2 = this.graphSent;
		if (simpleGraph2 != null)
		{
			simpleGraph2.Update((float)this.totalBytesSent / 1024f, Color.green);
		}
		this.sumBRecv += this.totalBytesReceived;
		this.totalBRecv += this.totalBytesReceived;
		this.sumBSent += this.totalBytesSent;
		this.totalBSent += this.totalBytesSent;
		this.sumPRecv += this.totalPackagesReceived;
		this.totalPRecv += this.totalPackagesReceived;
		this.sumPSent += this.totalPackagesSent;
		this.totalPSent += this.totalPackagesSent;
		if (this.timePassed > 1f)
		{
			this.bpsRecv = (float)this.sumBRecv / this.timePassed;
			this.bpsSent = (float)this.sumBSent / this.timePassed;
			this.sumBSent = (this.sumBRecv = 0);
			this.ppsRecv = (float)this.sumPRecv / this.timePassed;
			this.ppsSent = (float)this.sumPSent / this.timePassed;
			this.sumPRecv = (this.sumPSent = 0);
			this.timePassed = 0f;
		}
		this.uiLblOverview.text = string.Format("Overview Channel {0}:\n Recv: {1:0.00} kB/s {2:0} kB\n Sent: {3:0.00} kB/s {4:0} kB\n Recv: {5:0.0} p/s {6:0} p\n Sent: {7:0.0} p/s {8:0} p\n", new object[]
		{
			this.channel,
			this.bpsRecv / 1024f,
			(float)this.totalBRecv / 1024f,
			this.bpsSent / 1024f,
			(float)this.totalBSent / 1024f,
			this.ppsRecv,
			this.totalPRecv,
			this.ppsSent,
			this.totalPSent
		});
		StringBuilder stringBuilder = new StringBuilder();
		this.updatePackageListText(stringBuilder, this.uiLblRecvPkgCnt, this.packagesPerTypeReceived, this.bytesPerTypeReceived, "Rec");
		this.updatePackageListText(stringBuilder, this.uiLblSentPkgCnt, this.packagesPerTypeSent, this.bytesPerTypeSent, "Sent");
		if (this.uiLblRecvPkgSeq != null)
		{
			this.uiLblRecvPkgSeq.text = this.printPackageSequence(this.recSequence, "Rec last 30:");
		}
		if (this.uiLblSentPkgSeq != null)
		{
			this.uiLblSentPkgSeq.text = this.printPackageSequence(this.sentSequence, "Sent last 30:");
		}
	}

	// Token: 0x0600A347 RID: 41799 RVA: 0x003D0B20 File Offset: 0x003CED20
	[PublicizedFrom(EAccessModifier.Private)]
	public void updatePackageListText(StringBuilder _sb, UILabel _label, int[] _packagesPerType, int[] _bytesPerType, string _caption)
	{
		if (_label != null)
		{
			this.sortList.Clear();
			for (int i = 0; i < _packagesPerType.Length; i++)
			{
				NetworkMonitor.SIdCnt sidCnt = new NetworkMonitor.SIdCnt
				{
					Id = i,
					Cnt = _packagesPerType[i],
					Bytes = _bytesPerType[i]
				};
				this.sortList.Add(sidCnt);
			}
			this.sortList.Sort(this.sorter);
			_sb.Length = 0;
			_sb.Append(_caption);
			_sb.Append(":\n");
			int num = 0;
			while (num < 16 && num < this.sortList.Count && this.sortList[num].Cnt != 0)
			{
				_sb.Append(string.Format("{0}. {1}: {2} - {3} B\n", new object[]
				{
					num + 1,
					NetPackageManager.GetPackageName(this.sortList[num].Id),
					this.sortList[num].Cnt,
					this.sortList[num].Bytes
				}));
				num++;
			}
			_label.text = _sb.ToString();
		}
	}

	// Token: 0x0600A348 RID: 41800 RVA: 0x003D0C64 File Offset: 0x003CEE64
	[PublicizedFrom(EAccessModifier.Private)]
	public string printPackageSequence(RingBuffer<SNetPackageInfo> _sequence, string _addInfo)
	{
		if (_sequence == null)
		{
			return _addInfo;
		}
		StringBuilder stringBuilder = new StringBuilder();
		_sequence.SetToLast();
		ulong tick = _sequence.Peek().Tick;
		stringBuilder.Append(_addInfo);
		stringBuilder.Append(" ");
		stringBuilder.Append(tick.ToString());
		stringBuilder.Append("\n");
		SNetPackageInfo? snetPackageInfo = null;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		while (num3 < 30 && num4 < _sequence.Count - 1)
		{
			SNetPackageInfo prev = _sequence.GetPrev();
			if (snetPackageInfo != null)
			{
				if (snetPackageInfo.Value.Tick == prev.Tick && snetPackageInfo.Value.Id == prev.Id)
				{
					num++;
				}
				else
				{
					stringBuilder.Append(this.formatPackage(tick, snetPackageInfo.Value, num, num2));
					num = 0;
					num2 = 0;
					num3++;
				}
			}
			snetPackageInfo = new SNetPackageInfo?(prev);
			num2 += prev.Size;
			num4++;
		}
		if (snetPackageInfo != null)
		{
			stringBuilder.Append(this.formatPackage(tick, snetPackageInfo.Value, num, num2));
		}
		return stringBuilder.ToString();
	}

	// Token: 0x0600A349 RID: 41801 RVA: 0x003D0D8C File Offset: 0x003CEF8C
	[PublicizedFrom(EAccessModifier.Private)]
	public string formatPackage(ulong _baseTick, SNetPackageInfo _lastPackage, int _lastPackageCnt, int _size)
	{
		return string.Format(" {0:000} {1}{2} {3} B\n", new object[]
		{
			_baseTick - _lastPackage.Tick,
			(_lastPackageCnt > 0) ? ((_lastPackageCnt + 1).ToString() + "x ") : string.Empty,
			NetPackageManager.GetPackageName(_lastPackage.Id),
			_size
		});
	}

	// Token: 0x04007A4B RID: 31307
	public bool Enabled;

	// Token: 0x04007A4C RID: 31308
	[PublicizedFrom(EAccessModifier.Private)]
	public bool initialized;

	// Token: 0x04007A4D RID: 31309
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly UILabel uiLblOverview;

	// Token: 0x04007A4E RID: 31310
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly UILabel uiLblRecvPkgCnt;

	// Token: 0x04007A4F RID: 31311
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly UILabel uiLblSentPkgCnt;

	// Token: 0x04007A50 RID: 31312
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly UILabel uiLblRecvPkgSeq;

	// Token: 0x04007A51 RID: 31313
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly UILabel uiLblSentPkgSeq;

	// Token: 0x04007A52 RID: 31314
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly UITexture uiTxtRecv;

	// Token: 0x04007A53 RID: 31315
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly UITexture uiTxtSent;

	// Token: 0x04007A54 RID: 31316
	[PublicizedFrom(EAccessModifier.Private)]
	public SimpleGraph graphReceived;

	// Token: 0x04007A55 RID: 31317
	[PublicizedFrom(EAccessModifier.Private)]
	public SimpleGraph graphSent;

	// Token: 0x04007A56 RID: 31318
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] packagesPerTypeReceived = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04007A57 RID: 31319
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] packagesPerTypeSent = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04007A58 RID: 31320
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] bytesPerTypeReceived = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04007A59 RID: 31321
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] bytesPerTypeSent = new int[NetPackageManager.KnownPackageCount];

	// Token: 0x04007A5A RID: 31322
	[PublicizedFrom(EAccessModifier.Private)]
	public bool bResetNext;

	// Token: 0x04007A5B RID: 31323
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalBytesSent;

	// Token: 0x04007A5C RID: 31324
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalBytesReceived;

	// Token: 0x04007A5D RID: 31325
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalPackagesSent;

	// Token: 0x04007A5E RID: 31326
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalPackagesReceived;

	// Token: 0x04007A5F RID: 31327
	[PublicizedFrom(EAccessModifier.Private)]
	public RingBuffer<SNetPackageInfo> recSequence;

	// Token: 0x04007A60 RID: 31328
	[PublicizedFrom(EAccessModifier.Private)]
	public RingBuffer<SNetPackageInfo> sentSequence;

	// Token: 0x04007A61 RID: 31329
	[PublicizedFrom(EAccessModifier.Private)]
	public float timePassed;

	// Token: 0x04007A62 RID: 31330
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly List<NetworkMonitor.SIdCnt> sortList = new List<NetworkMonitor.SIdCnt>();

	// Token: 0x04007A63 RID: 31331
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly NetworkMonitor.SIdCntSorter sorter = new NetworkMonitor.SIdCntSorter();

	// Token: 0x04007A64 RID: 31332
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly Transform parent;

	// Token: 0x04007A65 RID: 31333
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int channel;

	// Token: 0x04007A66 RID: 31334
	[PublicizedFrom(EAccessModifier.Private)]
	public int sumBRecv;

	// Token: 0x04007A67 RID: 31335
	[PublicizedFrom(EAccessModifier.Private)]
	public int sumBSent;

	// Token: 0x04007A68 RID: 31336
	[PublicizedFrom(EAccessModifier.Private)]
	public int sumPRecv;

	// Token: 0x04007A69 RID: 31337
	[PublicizedFrom(EAccessModifier.Private)]
	public int sumPSent;

	// Token: 0x04007A6A RID: 31338
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalBRecv;

	// Token: 0x04007A6B RID: 31339
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalBSent;

	// Token: 0x04007A6C RID: 31340
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalPRecv;

	// Token: 0x04007A6D RID: 31341
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalPSent;

	// Token: 0x04007A6E RID: 31342
	[PublicizedFrom(EAccessModifier.Private)]
	public float bpsRecv;

	// Token: 0x04007A6F RID: 31343
	[PublicizedFrom(EAccessModifier.Private)]
	public float bpsSent;

	// Token: 0x04007A70 RID: 31344
	[PublicizedFrom(EAccessModifier.Private)]
	public float ppsRecv;

	// Token: 0x04007A71 RID: 31345
	[PublicizedFrom(EAccessModifier.Private)]
	public float ppsSent;

	// Token: 0x04007A72 RID: 31346
	[PublicizedFrom(EAccessModifier.Private)]
	public const int maxDisplayedPackageTypes = 16;

	// Token: 0x02001450 RID: 5200
	[PublicizedFrom(EAccessModifier.Private)]
	public struct SIdCnt
	{
		// Token: 0x04007A73 RID: 31347
		public int Id;

		// Token: 0x04007A74 RID: 31348
		public int Cnt;

		// Token: 0x04007A75 RID: 31349
		public int Bytes;
	}

	// Token: 0x02001451 RID: 5201
	[PublicizedFrom(EAccessModifier.Private)]
	public class SIdCntSorter : IComparer<NetworkMonitor.SIdCnt>
	{
		// Token: 0x0600A34A RID: 41802 RVA: 0x003D0DF4 File Offset: 0x003CEFF4
		public int Compare(NetworkMonitor.SIdCnt _obj1, NetworkMonitor.SIdCnt _obj2)
		{
			return _obj2.Cnt - _obj1.Cnt;
		}
	}
}
