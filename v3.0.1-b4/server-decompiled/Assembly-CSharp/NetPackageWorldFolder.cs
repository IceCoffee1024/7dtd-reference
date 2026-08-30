using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Noemax.GZip;
using UnityEngine;
using UnityEngine.Scripting;

// Token: 0x02000864 RID: 2148
[Preserve]
public class NetPackageWorldFolder : NetPackage
{
	// Token: 0x17000679 RID: 1657
	// (get) Token: 0x06003EA1 RID: 16033 RVA: 0x00010E62 File Offset: 0x0000F062
	public override bool Compress
	{
		get
		{
			return false;
		}
	}

	// Token: 0x1700067A RID: 1658
	// (get) Token: 0x06003EA2 RID: 16034 RVA: 0x0001FF5D File Offset: 0x0001E15D
	public override int Channel
	{
		get
		{
			return 1;
		}
	}

	// Token: 0x06003EA3 RID: 16035 RVA: 0x0018A88A File Offset: 0x00188A8A
	public NetPackageWorldFolder Setup(byte[] _data, int _seqNr, int _totalParts)
	{
		this.data = _data;
		this.seqNr = _seqNr;
		this.totalParts = _totalParts;
		return this;
	}

	// Token: 0x06003EA4 RID: 16036 RVA: 0x0018A8A4 File Offset: 0x00188AA4
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

	// Token: 0x06003EA5 RID: 16037 RVA: 0x0018A8E4 File Offset: 0x00188AE4
	public override void write(PooledBinaryWriter _writer)
	{
		base.write(_writer);
		_writer.Write(this.seqNr);
		_writer.Write(this.totalParts);
		_writer.Write((this.data != null) ? this.data.Length : (-1));
		if (this.data != null)
		{
			_writer.Write(this.data);
		}
	}

	// Token: 0x06003EA6 RID: 16038 RVA: 0x0018A940 File Offset: 0x00188B40
	public override void ProcessPackage(World _world, GameManager _callbacks)
	{
		if (SingletonMonoBehaviour<ConnectionManager>.Instance.IsServer)
		{
			NetPackageWorldFolder.StartSendingPacketsToClient(base.Sender);
			return;
		}
		DateTime now = DateTime.Now;
		if (NetPackageWorldFolder.downloadStartTime > now)
		{
			NetPackageWorldFolder.downloadStartTime = now;
		}
		float num = (float)(this.seqNr + 1) / (float)this.totalParts;
		TimeSpan timeSpan = TimeSpan.FromSeconds((now - NetPackageWorldFolder.downloadStartTime).TotalSeconds / (double)num * (double)(1f - num));
		if ((double)num > 0.05)
		{
			XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, string.Format(Localization.Get("uiLoadDownloadingWorldEstimate", false, null), num, (int)timeSpan.TotalMinutes, timeSpan.Seconds), true);
		}
		else
		{
			XUiC_ProgressWindow.SetText(LocalPlayerUI.primaryUI, string.Format(Localization.Get("uiLoadDownloadingWorld", false, null), num), true);
		}
		NetPackageWorldFolder.ReceiveStream.Write(this.data, 0, this.data.Length);
		if (this.seqNr == this.totalParts - 1)
		{
			ThreadManager.StartCoroutine(NetPackageWorldFolder.uncompressWorld());
		}
	}

	// Token: 0x06003EA7 RID: 16039 RVA: 0x0018AA55 File Offset: 0x00188C55
	public override int GetLength()
	{
		return 16 + ((this.data != null) ? this.data.Length : 0);
	}

	// Token: 0x1700067B RID: 1659
	// (get) Token: 0x06003EA8 RID: 16040 RVA: 0x00010E62 File Offset: 0x0000F062
	public override NetPackageDirection PackageDirection
	{
		get
		{
			return NetPackageDirection.Both;
		}
	}

	// Token: 0x06003EA9 RID: 16041 RVA: 0x0018AA6D File Offset: 0x00188C6D
	public static IEnumerator TestWorldValid(string _locationPath, Dictionary<string, uint> _worldFileHashes, Action<bool> _resultCallback)
	{
		byte[] buffer = new byte[8192];
		using (Dictionary<string, uint>.Enumerator enumerator = _worldFileHashes.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				NetPackageWorldFolder.<>c__DisplayClass17_0 CS$<>8__locals1 = new NetPackageWorldFolder.<>c__DisplayClass17_0();
				CS$<>8__locals1.hashEntry = enumerator.Current;
				string text = _locationPath + "/" + CS$<>8__locals1.hashEntry.Key;
				if (!SdFile.Exists(text))
				{
					Log.Out("World file {0} does not exist", new object[] { CS$<>8__locals1.hashEntry.Key });
					_resultCallback(false);
					yield break;
				}
				CS$<>8__locals1.validHash = true;
				yield return IOUtils.CalcCrcCoroutine(text, delegate(uint _crc)
				{
					if (_crc != CS$<>8__locals1.hashEntry.Value)
					{
						CS$<>8__locals1.validHash = false;
						Log.Out("World file {0} is different than server's version: received {1:X8}, calculated {2:X8}", new object[]
						{
							CS$<>8__locals1.hashEntry.Key,
							CS$<>8__locals1.hashEntry.Value,
							_crc
						});
						return;
					}
					CS$<>8__locals1.validHash = true;
				}, 15, buffer);
				if (!CS$<>8__locals1.validHash)
				{
					_resultCallback(false);
					yield break;
				}
				CS$<>8__locals1 = null;
			}
		}
		Dictionary<string, uint>.Enumerator enumerator = default(Dictionary<string, uint>.Enumerator);
		_resultCallback(true);
		yield break;
		yield break;
	}

	// Token: 0x06003EAA RID: 16042 RVA: 0x0018AA8A File Offset: 0x00188C8A
	public static IEnumerator RequestWorld()
	{
		NetPackageWorldFolder.ReceiveStream = new MemoryStream();
		NetPackageWorldFolder.WorldReceivedAndUncompressed = false;
		NetPackageWorldFolder.downloadStartTime = DateTime.MaxValue;
		SingletonMonoBehaviour<ConnectionManager>.Instance.SendToServer(NetPackageManager.GetPackage<NetPackageWorldFolder>(), false);
		while (!NetPackageWorldFolder.WorldReceivedAndUncompressed && SingletonMonoBehaviour<ConnectionManager>.Instance.IsConnected)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003EAB RID: 16043 RVA: 0x0018AA92 File Offset: 0x00188C92
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator uncompressWorld()
	{
		string worldFolder = GameIO.GetSaveGameLocalDir() + "/World";
		NetPackageWorldFolder.ReceiveStream.Position = 0L;
		DeflateInputStream zipStream = new DeflateInputStream(NetPackageWorldFolder.ReceiveStream);
		BinaryReader reader = new BinaryReader(zipStream);
		SdDirectory.CreateDirectory(worldFolder);
		int fileCount = reader.ReadInt32();
		MicroStopwatch mswCompressing = new MicroStopwatch();
		byte[] buffer = MemoryPools.poolByte.Alloc(4096);
		yield return null;
		int num3;
		for (int i = 0; i < fileCount; i = num3 + 1)
		{
			string text = reader.ReadString();
			long fileSize = reader.ReadInt64();
			if (text.StartsWith('.') || text.IndexOfAny(GameIO.ResourcePathSeparators) >= 0)
			{
				Log.Warning("Received world files contains file with parent path specifier or path separator: " + text);
				bool bWhileCopying = true;
				while (bWhileCopying)
				{
					if (fileSize <= 0L)
					{
						break;
					}
					int num = zipStream.Read(buffer, 0, (int)Math.Min(fileSize, (long)buffer.Length));
					if (num > 0)
					{
						fileSize -= (long)num;
					}
					else
					{
						bWhileCopying = false;
					}
					if (bWhileCopying && mswCompressing.ElapsedMilliseconds > (long)NetPackageWorldFolder.MaxTimePerFrame)
					{
						yield return null;
						mswCompressing.ResetAndRestart();
					}
				}
			}
			else
			{
				Stream fs = SdFile.Create(worldFolder + "/" + text);
				mswCompressing.ResetAndRestart();
				if (text.StartsWith("dtm", StringComparison.OrdinalIgnoreCase) && text.EndsWith(".raw", StringComparison.OrdinalIgnoreCase))
				{
					yield return ThreadManager.StartCoroutine(NetPackageWorldFolder.readDtmDelta(fs, zipStream, mswCompressing, fileSize));
				}
				else
				{
					bool bWhileCopying = true;
					while (bWhileCopying && fileSize > 0L)
					{
						int num2 = zipStream.Read(buffer, 0, (int)Math.Min(fileSize, (long)buffer.Length));
						if (num2 > 0)
						{
							fs.Write(buffer, 0, num2);
							fileSize -= (long)num2;
						}
						else
						{
							bWhileCopying = false;
						}
						if (bWhileCopying && mswCompressing.ElapsedMilliseconds > (long)NetPackageWorldFolder.MaxTimePerFrame)
						{
							yield return null;
							mswCompressing.ResetAndRestart();
						}
					}
				}
				fs.Dispose();
				yield return null;
				fs = null;
			}
			num3 = i;
		}
		MemoryPools.poolByte.Free(buffer);
		zipStream.Dispose();
		yield return null;
		SdFile.WriteAllBytes(worldFolder + "/completed", new byte[0]);
		NetPackageWorldFolder.WorldReceivedAndUncompressed = true;
		yield break;
	}

	// Token: 0x06003EAC RID: 16044 RVA: 0x0018AA9A File Offset: 0x00188C9A
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator readDtmDelta(Stream _fs, DeflateInputStream _zipStream, MicroStopwatch _mswCompressing, long _fileSize)
	{
		int w = (int)Math.Sqrt((double)(_fileSize / 2L));
		int h = w;
		int lineBytes = w * 2;
		byte[] readLineData = new byte[lineBytes];
		byte[] writeLineData = new byte[lineBytes];
		MemoryStream readStream = new MemoryStream(readLineData);
		MemoryStream writeStream = new MemoryStream(writeLineData);
		int num4;
		for (int z = 0; z < h; z = num4 + 1)
		{
			_zipStream.Read(readLineData, 0, lineBytes);
			readStream.Position = 0L;
			writeStream.Position = 0L;
			int num = (int)StreamUtils.ReadUInt16(readStream);
			StreamUtils.Write(writeStream, (ushort)num);
			for (int i = 1; i < w; i++)
			{
				int num2 = (int)StreamUtils.ReadInt16(readStream);
				int num3 = num + num2;
				num = num3;
				if (num3 < 0 || num3 > 65535)
				{
					Log.Out("Current out of range: " + num3.ToString());
				}
				StreamUtils.Write(writeStream, (ushort)num3);
			}
			_fs.Write(writeLineData, 0, lineBytes);
			if (_mswCompressing.ElapsedMilliseconds > (long)NetPackageWorldFolder.MaxTimePerFrame)
			{
				yield return null;
				_mswCompressing.ResetAndRestart();
			}
			num4 = z;
		}
		yield break;
	}

	// Token: 0x1700067C RID: 1660
	// (get) Token: 0x06003EAD RID: 16045 RVA: 0x0018AABE File Offset: 0x00188CBE
	public static int MaxTimePerFrame
	{
		[PublicizedFrom(EAccessModifier.Private)]
		get
		{
			if (!GameManager.IsDedicatedServer)
			{
				return 5;
			}
			return 40;
		}
	}

	// Token: 0x06003EAE RID: 16046 RVA: 0x0018AACC File Offset: 0x00188CCC
	[PublicizedFrom(EAccessModifier.Private)]
	public static void StartSendingPacketsToClient(ClientInfo _cInfo)
	{
		string @string = GamePrefs.GetString(EnumGamePrefs.GameWorld);
		if (NetPackageWorldFolder.CompressedWorldDataChunks == null || @string != NetPackageWorldFolder.CachedWorldName)
		{
			NetPackageWorldFolder.CompressedWorldDataChunks = null;
			NetPackageWorldFolder.CachedWorldName = null;
			object lockObj = NetPackageWorldFolder.LockObj;
			lock (lockObj)
			{
				if (NetPackageWorldFolder.PreparationCoroutine == null)
				{
					NetPackageWorldFolder.PreparationCoroutine = NetPackageWorldFolder.prepareWorldFolderData();
					ThreadManager.StartCoroutine(NetPackageWorldFolder.PreparationCoroutine);
				}
			}
		}
		ThreadManager.StartCoroutine(NetPackageWorldFolder.sendPacketsToClient(_cInfo));
	}

	// Token: 0x06003EAF RID: 16047 RVA: 0x0018AB58 File Offset: 0x00188D58
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator prepareWorldFolderData()
	{
		int @int = GamePrefs.GetInt(EnumGamePrefs.ServerMaxWorldTransferSpeedKiBs);
		if (@int > 0)
		{
			NetPackageWorldFolder.PACKET_SEND_DELAY = new WaitForSeconds(65536f / (float)(@int * 1024));
		}
		Log.Out("Preparing World chunks for clients");
		MemoryStream memStream = new MemoryStream();
		DeflateOutputStream zipStream = new DeflateOutputStream(memStream, 3);
		BinaryWriter writer = new BinaryWriter(zipStream);
		string worldFolder = PathAbstractions.Contextual.FindActiveWorldLocation().FullPath;
		List<string> worldFiles = GameUtils.GetWorldFilesToTransmitToClient(worldFolder);
		yield return null;
		byte[] buffer = MemoryPools.poolByte.Alloc(4096);
		MicroStopwatch mswCompressing = new MicroStopwatch();
		writer.Write(worldFiles.Count);
		int num2;
		for (int i = 0; i < worldFiles.Count; i = num2 + 1)
		{
			string text = worldFiles[i];
			string text2 = worldFolder + "/" + text;
			Stream fs = SdFile.OpenRead(text2);
			long length = fs.Length;
			writer.Write(text);
			writer.Write(length);
			mswCompressing.ResetAndRestart();
			if (text.StartsWith("dtm", StringComparison.OrdinalIgnoreCase) && text.EndsWith(".raw", StringComparison.OrdinalIgnoreCase))
			{
				yield return ThreadManager.StartCoroutine(NetPackageWorldFolder.writeDtmDelta(fs, zipStream, mswCompressing));
			}
			else
			{
				bool bWhileCopying = true;
				while (bWhileCopying)
				{
					int num = fs.Read(buffer, 0, buffer.Length);
					if (num > 0)
					{
						zipStream.Write(buffer, 0, num);
					}
					else
					{
						bWhileCopying = false;
					}
					if (bWhileCopying && mswCompressing.ElapsedMilliseconds > (long)NetPackageWorldFolder.MaxTimePerFrame)
					{
						yield return null;
						mswCompressing.ResetAndRestart();
					}
				}
			}
			fs.Dispose();
			yield return null;
			fs = null;
			num2 = i;
		}
		MemoryPools.poolByte.Free(buffer);
		buffer = null;
		mswCompressing = null;
		zipStream.Flush();
		memStream.Position = 0L;
		yield return null;
		long num3 = memStream.Length / 65536L;
		long num4 = memStream.Length % 65536L;
		if (num4 == 0L)
		{
			num4 = 65536L;
		}
		else
		{
			num3 += 1L;
		}
		List<byte[]> list = new List<byte[]>();
		int num5 = 0;
		while ((long)num5 < num3)
		{
			long num6 = 65536L;
			if ((long)num5 == num3 - 1L)
			{
				num6 = num4;
			}
			byte[] array = new byte[num6];
			memStream.Read(array, 0, (int)num6);
			list.Add(array);
			num5++;
		}
		if (memStream.Position != memStream.Length)
		{
			Log.Out("Wrong memStream Position after creating arrays: pos={0}, len={1}", new object[] { memStream.Position, memStream.Length });
		}
		Log.Out("World chunks size: {0} B, chunk count: {1}", new object[] { memStream.Length, list.Count });
		zipStream.Dispose();
		object lockObj = NetPackageWorldFolder.LockObj;
		lock (lockObj)
		{
			NetPackageWorldFolder.CachedWorldName = GamePrefs.GetString(EnumGamePrefs.GameWorld);
			NetPackageWorldFolder.CompressedWorldDataChunks = list;
			NetPackageWorldFolder.PreparationCoroutine = null;
			yield break;
		}
		yield break;
	}

	// Token: 0x06003EB0 RID: 16048 RVA: 0x0018AB60 File Offset: 0x00188D60
	public static IEnumerator writeDtmDelta(Stream _sourceStream, Stream _targetStream, MicroStopwatch _mswCompressing)
	{
		int w = (int)Math.Sqrt((double)(_sourceStream.Length / 2L));
		int h = w;
		int lineBytes = w * 2;
		byte[] readLineData = new byte[lineBytes];
		byte[] writeLineData = new byte[lineBytes];
		MemoryStream readStream = new MemoryStream(readLineData);
		MemoryStream writeStream = new MemoryStream(writeLineData);
		int num4;
		for (int z = 0; z < h; z = num4 + 1)
		{
			readStream.Position = 0L;
			writeStream.Position = 0L;
			_sourceStream.Read(readLineData, 0, lineBytes);
			int num = (int)StreamUtils.ReadUInt16(readStream);
			StreamUtils.Write(writeStream, (ushort)num);
			for (int i = 1; i < w; i++)
			{
				ushort num2 = StreamUtils.ReadUInt16(readStream);
				int num3 = (int)num2 - num;
				num = (int)num2;
				if (num3 < -32768 || num3 > 32767)
				{
					Log.Out("Delta out of range: " + num3.ToString());
				}
				StreamUtils.Write(writeStream, (short)num3);
			}
			_targetStream.Write(writeLineData, 0, lineBytes);
			if (_mswCompressing != null && _mswCompressing.ElapsedMilliseconds > (long)NetPackageWorldFolder.MaxTimePerFrame)
			{
				yield return null;
				_mswCompressing.ResetAndRestart();
			}
			num4 = z;
		}
		yield break;
	}

	// Token: 0x06003EB1 RID: 16049 RVA: 0x0018AB7D File Offset: 0x00188D7D
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator sendPacketsToClient(ClientInfo _cInfo)
	{
		while (NetPackageWorldFolder.CompressedWorldDataChunks == null)
		{
			yield return null;
		}
		string cInfoString = _cInfo.ToString();
		Log.Out("Starting to send world to " + cInfoString + "...");
		int num;
		for (int i = 0; i < NetPackageWorldFolder.CompressedWorldDataChunks.Count; i = num + 1)
		{
			_cInfo.SendPackage(NetPackageManager.GetPackage<NetPackageWorldFolder>().Setup(NetPackageWorldFolder.CompressedWorldDataChunks[i], i, NetPackageWorldFolder.CompressedWorldDataChunks.Count));
			yield return NetPackageWorldFolder.PACKET_SEND_DELAY;
			num = i;
		}
		Log.Out("Sending world to " + cInfoString + " done");
		yield break;
	}

	// Token: 0x0400325C RID: 12892
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] data;

	// Token: 0x0400325D RID: 12893
	[PublicizedFrom(EAccessModifier.Private)]
	public int seqNr;

	// Token: 0x0400325E RID: 12894
	[PublicizedFrom(EAccessModifier.Private)]
	public int totalParts;

	// Token: 0x0400325F RID: 12895
	[PublicizedFrom(EAccessModifier.Private)]
	public static DateTime downloadStartTime;

	// Token: 0x04003260 RID: 12896
	[PublicizedFrom(EAccessModifier.Private)]
	public static MemoryStream ReceiveStream;

	// Token: 0x04003261 RID: 12897
	[PublicizedFrom(EAccessModifier.Private)]
	public static bool WorldReceivedAndUncompressed;

	// Token: 0x04003262 RID: 12898
	[PublicizedFrom(EAccessModifier.Private)]
	public static WaitForSeconds PACKET_SEND_DELAY = new WaitForSeconds(0.25f);

	// Token: 0x04003263 RID: 12899
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHUNK_SIZE = 65536;

	// Token: 0x04003264 RID: 12900
	[PublicizedFrom(EAccessModifier.Private)]
	public const int DTM_BITMASK = 65520;

	// Token: 0x04003265 RID: 12901
	[PublicizedFrom(EAccessModifier.Private)]
	public static List<byte[]> CompressedWorldDataChunks;

	// Token: 0x04003266 RID: 12902
	[PublicizedFrom(EAccessModifier.Private)]
	public static string CachedWorldName;

	// Token: 0x04003267 RID: 12903
	[PublicizedFrom(EAccessModifier.Private)]
	public static object LockObj = new object();

	// Token: 0x04003268 RID: 12904
	[PublicizedFrom(EAccessModifier.Private)]
	public static IEnumerator PreparationCoroutine;
}
