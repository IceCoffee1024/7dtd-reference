using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Noemax.GZip;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x020007FF RID: 2047
[Preserve]
public class NetPackageLocalization : NetPackage
{
	// Token: 0x17000642 RID: 1602
	// (get) Token: 0x06003C40 RID: 15424 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool Compress
	{
		get
		{
			return false;
		}
	}

	// Token: 0x06003C41 RID: 15425 RVA: 0x00180969 File Offset: 0x0017EB69
	[PublicizedFrom(EAccessModifier.Private)]
	public NetPackageLocalization Setup(byte[] _data, int _seqNr, int _totalParts)
	{
		this.data = _data;
		this.seqNr = _seqNr;
		this.totalParts = _totalParts;
		return this;
	}

	// Token: 0x06003C42 RID: 15426 RVA: 0x00180984 File Offset: 0x0017EB84
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

	// Token: 0x06003C43 RID: 15427 RVA: 0x001809C4 File Offset: 0x0017EBC4
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

	// Token: 0x06003C44 RID: 15428 RVA: 0x00180A1C File Offset: 0x0017EC1C
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

	// Token: 0x06003C45 RID: 15429 RVA: 0x00180B9C File Offset: 0x0017ED9C
	public override int GetLength()
	{
		return 16 + this.data.Length;
	}

	// Token: 0x17000643 RID: 1603
	// (get) Token: 0x06003C46 RID: 15430 RVA: 0x0002EEAC File Offset: 0x0002D0AC
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.ToClient;
		}
	}

	// Token: 0x06003C47 RID: 15431 RVA: 0x00180BA9 File Offset: 0x0017EDA9
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

	// Token: 0x06003C48 RID: 15432 RVA: 0x00180BB8 File Offset: 0x0017EDB8
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

	// Token: 0x06003C49 RID: 15433 RVA: 0x00180CA6 File Offset: 0x0017EEA6
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

	// Token: 0x040030B5 RID: 12469
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x040030B6 RID: 12470
	[PublicizedFrom(EAccessModifier.Private)]
	public int seqNr;

	// Token: 0x040030B7 RID: 12471
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalParts;

	// Token: 0x040030B8 RID: 12472
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime downloadStartTime;

	// Token: 0x040030B9 RID: 12473
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryStream receiveStream;

	// Token: 0x040030BA RID: 12474
	[PublicizedFrom(EAccessModifier.Private)]
	public static WaitForSeconds PACKET_SEND_DELAY = new WaitForSeconds(0.25f);

	// Token: 0x040030BB RID: 12475
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHUNK_SIZE = 131072;

	// Token: 0x040030BC RID: 12476
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<byte[]> dataChunks;

	// Token: 0x040030BD RID: 12477
	[PublicizedFrom(EAccessModifier.Private)]
	public static int cachedDataSize;

	// Token: 0x040030BE RID: 12478
	[PublicizedFrom(EAccessModifier.Private)]
	public static readonly object lockObj = new object();
}
