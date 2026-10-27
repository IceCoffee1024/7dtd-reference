using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Noemax.GZip;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000813 RID: 2067
[Preserve]
public class NetPackageLocalization : NetPackage
{
	// Token: 0x17000674 RID: 1652
	// (get) Token: 0x06003C99 RID: 15513 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool Compress
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003C9A RID: 15514 RVA: 0x00183E50 File Offset: 0x00182050
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageLocalization Setup(byte[] _data, int _seqNr, int _totalParts)
	{
		this.data = _data;
		this.seqNr = _seqNr;
		this.totalParts = _totalParts;
		return this;
	}

	// Token: 0x06003C9B RID: 15515 RVA: 0x00183E68 File Offset: 0x00182068
	public override void read(PooledBinaryReader _reader)
	{
		this.seqNr = _reader.ReadInt32();
		this.totalParts = _reader.ReadInt32();
		int num = _reader.ReadInt32();
		if (num >= 0)
		{
			this.data = _reader.ReadBytes(num);
		}
	}

	// Token: 0x06003C9C RID: 15516 RVA: 0x00183EA8 File Offset: 0x001820A8
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.seqNr);
		_writer.Write(this.totalParts);
		byte[] array = this.data;
		_writer.Write((array != null) ? array.Length : (-1));
		if (this.data != null)
		{
			_writer.Write(this.data);
		}
	}

	// Token: 0x06003C9D RID: 15517 RVA: 0x00183F00 File Offset: 0x00182100
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		DateTime now = DateTime.Now;
		if (this.seqNr == 0)
		{
			NetPackageLocalization.receiveStream = new MemoryStream();
			NetPackageLocalization.downloadStartTime = now;
		}
		float num = (float)(this.seqNr + 1) / (float)this.totalParts;
		TimeSpan timeSpan = TimeSpan.FromSeconds((now - NetPackageLocalization.downloadStartTime).TotalSeconds / (double)num * (double)(1f - num));
		if ((double)num > 0.05)
		{
			XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, string.Format(Localization.Get("uiLoadDownloadingLocalizationEstimate", false, null), num, (int)timeSpan.TotalMinutes, timeSpan.Seconds), true);
		}
		else
		{
			XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, string.Format(Localization.Get("uiLoadDownloadingLocalization", false, null), num), true);
		}
		NetPackageLocalization.receiveStream.Write(this.data, 0, this.data.Length);
		if (this.seqNr != this.totalParts - 1)
		{
			return;
		}
		NetPackageLocalization.receiveStream.Position = 0L;
		using (DeflateInputStream deflateInputStream = new DeflateInputStream(NetPackageLocalization.receiveStream))
		{
			using (MemoryStream memoryStream = new MemoryStream())
			{
				StreamUtils.StreamCopy(deflateInputStream, memoryStream, null, true);
				Localization.LoadServerPatchDictionary(memoryStream.ToArray());
				NetPackageLocalization.receiveStream = null;
				XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, Localization.Get("uiLoadWaitingForConfigs", false, null), true);
			}
		}
	}

	// Token: 0x17000675 RID: 1653
	// (get) Token: 0x06003C9E RID: 15518 RVA: 0x0002F260 File Offset: 0x0002D460
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C9F RID: 15519 RVA: 0x00184080 File Offset: 0x00182280
	public static IEnumerator StartSendingPacketsToClient(ClientInfo _cInfo)
	{
		byte[] patchedData = Localization.PatchedData;
		if (NetPackageLocalization.dataChunks == null || NetPackageLocalization.cachedDataSize != patchedData.Length)
		{
			object obj = NetPackageLocalization.lockObj;
			lock (obj)
			{
				if (NetPackageLocalization.dataChunks == null || NetPackageLocalization.cachedDataSize != patchedData.Length)
				{
					NetPackageLocalization.prepareDataPackets(patchedData);
				}
			}
		}
		yield return NetPackageLocalization.sendPacketsToClient(_cInfo);
		yield break;
	}

	// Token: 0x06003CA0 RID: 15520 RVA: 0x00184090 File Offset: 0x00182290
	[PublicizedFrom(EAccessModifier.Private)]
	public static void prepareDataPackets(byte[] _patchedData)
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.ServerMaxWorldTransferSpeedKiBs);
		if (@int > 0)
		{
			NetPackageLocalization.PACKET_SEND_DELAY = new WaitForSeconds(131072f / (float)(@int * 1024));
		}
		Log.Out("Preparing Localization chunks for clients");
		long num = (long)(_patchedData.Length / 131072);
		long num2 = (long)(_patchedData.Length % 131072);
		if (num2 == 0L)
		{
			num2 = 131072L;
		}
		else
		{
			num += 1L;
		}
		List<byte[]> list = new List<byte[]>();
		int num3 = 0;
		while ((long)num3 < num)
		{
			long num4 = 131072L;
			if ((long)num3 == num - 1L)
			{
				num4 = num2;
			}
			byte[] array = new byte[num4];
			Array.Copy(_patchedData, (long)(num3 * 131072), array, 0L, num4);
			list.Add(array);
			num3++;
		}
		Log.Out("Localization size: {0} B, chunk count: {1}", new object[] { _patchedData.Length, list.Count });
		NetPackageLocalization.cachedDataSize = _patchedData.Length;
		NetPackageLocalization.dataChunks = list;
	}

	// Token: 0x06003CA1 RID: 15521 RVA: 0x0018417E File Offset: 0x0018237E
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator sendPacketsToClient(ClientInfo _cInfo)
	{
		while (NetPackageLocalization.dataChunks == null)
		{
			yield return null;
		}
		string cInfoString = _cInfo.ToString();
		Log.Out("Starting to send Localization to " + cInfoString + "...");
		int num;
		for (int i = 0; i < NetPackageLocalization.dataChunks.Count; i = num + 1)
		{
			_cInfo.SendPackage(NetPackageManager.GetPackage<NetPackageLocalization>().Setup(NetPackageLocalization.dataChunks[i], i, NetPackageLocalization.dataChunks.Count));
			yield return NetPackageLocalization.PACKET_SEND_DELAY;
			num = i;
		}
		Log.Out("Sending Localization to " + cInfoString + " done");
		yield break;
	}

	// Token: 0x04003142 RID: 12610
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x04003143 RID: 12611
	[PublicizedFrom(EAccessModifier.Private)]
	public int seqNr;

	// Token: 0x04003144 RID: 12612
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalParts;

	// Token: 0x04003145 RID: 12613
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime downloadStartTime;

	// Token: 0x04003146 RID: 12614
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryStream receiveStream;

	// Token: 0x04003147 RID: 12615
	[PublicizedFrom(EAccessModifier.Private)]
	public static WaitForSeconds PACKET_SEND_DELAY = new WaitForSeconds(0.25f);

	// Token: 0x04003148 RID: 12616
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHUNK_SIZE = 131072;

	// Token: 0x04003149 RID: 12617
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<byte[]> dataChunks;

	// Token: 0x0400314A RID: 12618
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cachedDataSize;

	// Token: 0x0400314B RID: 12619
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObj = new object();
}
