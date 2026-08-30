using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x02000BA6 RID: 2982
public class RegionFileRaw : RegionFile
{
	// Token: 0x06005A27 RID: 23079 RVA: 0x00229474 File Offset: 0x00227674
	[PublicizedFrom(EAccessModifier.Private)]
	public RegionFileRaw(string fullFilePath, int rX, int rZ, int paddingBytes, int version)
		: base(fullFilePath, rX, rZ)
	{
		this.paddingBytes = paddingBytes;
		this.version = version;
	}

	// Token: 0x06005A28 RID: 23080 RVA: 0x002294C4 File Offset: 0x002276C4
	public static RegionFileRaw Load(string fullFilePath, int rX, int rZ)
	{
		RegionFileRaw regionFileRaw2;
		using (Stream stream = SdFile.Open(fullFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
		{
			for (int i = 0; i < 3; i++)
			{
				if ((int)RegionFileRaw.FileHeaderMagicBytes[i] != stream.ReadByte())
				{
					throw new Exception("Incorrect header: " + fullFilePath);
				}
			}
			int num = StreamUtils.ReadInt32(stream);
			int num2 = StreamUtils.ReadInt32(stream);
			RegionFileRaw regionFileRaw = new RegionFileRaw(fullFilePath, rX, rZ, num2, num);
			RegionFileRaw.ReadBytes<int>(stream, regionFileRaw.locationHeader, 0, regionFileRaw.locationHeader.Length);
			RegionFileRaw.ReadBytes<uint>(stream, regionFileRaw.timestampHeader, 0, regionFileRaw.timestampHeader.Length);
			regionFileRaw.InitUsedSectors();
			regionFileRaw.Length = stream.Length;
			regionFileRaw2 = regionFileRaw;
		}
		return regionFileRaw2;
	}

	// Token: 0x06005A29 RID: 23081 RVA: 0x00229584 File Offset: 0x00227784
	[PublicizedFrom(EAccessModifier.Private)]
	public void InitUsedSectors()
	{
		lock (this)
		{
			this.usedSectors.Clear();
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					int num;
					int num2;
					this.GetLocationInfo(j, i, out num, out num2);
					if (num > 0 && num2 > 0)
					{
						this.usedSectors.Add(num, num2);
					}
				}
			}
		}
	}

	// Token: 0x06005A2A RID: 23082 RVA: 0x00229604 File Offset: 0x00227804
	public static RegionFileRaw New(string fullFilePath, int rX, int rZ, int paddingBytes)
	{
		RegionFileRaw regionFileRaw2;
		using (Stream stream = SdFile.Open(fullFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
		{
			RegionFileRaw regionFileRaw = new RegionFileRaw(fullFilePath, rX, rZ, paddingBytes, 1);
			stream.Write(RegionFileRaw.FileHeaderMagicBytes, 0, 3);
			StreamUtils.Write(stream, 1);
			StreamUtils.Write(stream, paddingBytes);
			if (stream.Position != 11L)
			{
				throw new Exception(string.Format("Unexpected header length written. Expected: {0}, Actual: {1}", 11, stream.Position));
			}
			RegionFileRaw.WriteBytes<int>(stream, regionFileRaw.locationHeader, 0, regionFileRaw.locationHeader.Length);
			RegionFileRaw.WriteBytes<uint>(stream, regionFileRaw.timestampHeader, 0, regionFileRaw.timestampHeader.Length);
			regionFileRaw.Length = stream.Length;
			regionFileRaw2 = regionFileRaw;
		}
		return regionFileRaw2;
	}

	// Token: 0x06005A2B RID: 23083 RVA: 0x002296C4 File Offset: 0x002278C4
	public override void ReadData(int cX, int cZ, Stream targetStream)
	{
		lock (this)
		{
			int num;
			int num2;
			this.GetLocationInfo(cX, cZ, out num, out num2);
			if (num < 779)
			{
				throw new Exception(string.Format("Reading outside of allowed bounds. R={0}/{1}, C={2}/{3}, Off={4}, Len={5}", new object[] { this.regionX, this.regionZ, cX, cZ, num, num2 }));
			}
			using (Stream stream = SdFile.Open(this.fullFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				stream.Seek((long)num, SeekOrigin.Begin);
				int num3 = StreamUtils.ReadInt32(stream);
				if (num3 > 0)
				{
					targetStream.SetLength(0L);
				}
				StreamUtils.StreamCopy(stream, targetStream, num3, this.tempReadBuffer, true);
			}
		}
	}

	// Token: 0x06005A2C RID: 23084 RVA: 0x002297C0 File Offset: 0x002279C0
	public override void WriteData(int cX, int cZ, int dataLength, byte compression, byte[] data, bool saveHeaderToFile)
	{
		lock (this)
		{
			uint num = GameUtils.WorldTimeToTotalMinutes(GameManager.Instance.World.worldTime);
			int num2 = dataLength + 4;
			int num3;
			int num4;
			this.GetLocationInfo(cX, cZ, out num3, out num4);
			if (num3 == 0 || num2 > num4)
			{
				num2 += this.paddingBytes;
				if (num3 > 0)
				{
					this.usedSectors.Remove(num3);
				}
				num3 = this.FindBestFreeSpace(num2);
				this.SetLocationInfo(cX, cZ, num3, num2);
			}
			else
			{
				num2 = num4;
			}
			if (num3 < 779)
			{
				throw new Exception(string.Format("Cannot write to protected offset: R={0}/{1}, C={2}/{3}, Off={4}, Len={5}, DataLen={6}", new object[] { this.regionX, this.regionZ, cX, cZ, num3, num2, dataLength }));
			}
			this.SetTimestampInfo(cX, cZ, num);
			int num5 = num3 + num2;
			using (Stream stream = SdFile.Open(this.fullFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				stream.Seek((long)num3, SeekOrigin.Begin);
				StreamUtils.Write(stream, dataLength);
				stream.Write(data, 0, dataLength);
				int num6 = num2 - 4 - dataLength;
				if (num6 > 0)
				{
					stream.Seek((long)(num6 - 1), SeekOrigin.Current);
					stream.WriteByte(0);
				}
				if ((long)num5 != stream.Position)
				{
					throw new Exception(string.Format("Wrong write end: R={0}/{1}, C={2}/{3}, Off={4}, Len={5}, DataLen={6}, FOEndExp={7}, FOEndFound={8}", new object[] { this.regionX, this.regionZ, cX, cZ, num3, num2, dataLength, num5, stream.Position }));
				}
				base.Length = stream.Length;
			}
			if (saveHeaderToFile)
			{
				this.SaveHeaderData();
			}
		}
	}

	// Token: 0x06005A2D RID: 23085 RVA: 0x002299F0 File Offset: 0x00227BF0
	public override void SaveHeaderData()
	{
		lock (this)
		{
			using (Stream stream = SdFile.Open(this.fullFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				stream.Seek(11L, SeekOrigin.Begin);
				RegionFileRaw.WriteBytes<int>(stream, this.locationHeader, 0, this.locationHeader.Length);
				RegionFileRaw.WriteBytes<uint>(stream, this.timestampHeader, 0, this.timestampHeader.Length);
			}
		}
	}

	// Token: 0x06005A2E RID: 23086 RVA: 0x00229A80 File Offset: 0x00227C80
	public override bool HasChunk(int cX, int cZ)
	{
		int num;
		int num2;
		this.GetLocationInfo(cX, cZ, out num, out num2);
		return num > 0 && num2 > 0;
	}

	// Token: 0x06005A2F RID: 23087 RVA: 0x00229AA4 File Offset: 0x00227CA4
	public override void RemoveChunk(int cX, int cZ)
	{
		lock (this)
		{
			int num;
			int num2;
			this.GetLocationInfo(cX, cZ, out num, out num2);
			if (num > 0 && num2 > 0)
			{
				this.usedSectors.Remove(num);
			}
			this.SetLocationInfo(cX, cZ, 0, 0);
		}
	}

	// Token: 0x06005A30 RID: 23088 RVA: 0x00229B04 File Offset: 0x00227D04
	public override int ChunkCount()
	{
		int num2;
		lock (this)
		{
			int num = 0;
			using (SortedDictionary<int, int>.ValueCollection.Enumerator enumerator = this.usedSectors.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current > 0)
					{
						num++;
					}
				}
			}
			num2 = num;
		}
		return num2;
	}

	// Token: 0x06005A31 RID: 23089 RVA: 0x00229B88 File Offset: 0x00227D88
	public override int GetChunkByteCount(int cX, int cZ)
	{
		int num;
		int num2;
		this.GetLocationInfo(cX, cZ, out num, out num2);
		return num2;
	}

	// Token: 0x06005A32 RID: 23090 RVA: 0x00229BA4 File Offset: 0x00227DA4
	public override void GetTimestampInfo(int cX, int cZ, out uint timeStamp)
	{
		lock (this)
		{
			long offsetFromXz = this.GetOffsetFromXz(cX, cZ);
			timeStamp = this.timestampHeader[(int)(checked((IntPtr)offsetFromXz))];
		}
	}

	// Token: 0x06005A33 RID: 23091 RVA: 0x00229BF0 File Offset: 0x00227DF0
	public override void SetTimestampInfo(int cX, int cZ, uint timeStamp)
	{
		long offsetFromXz = this.GetOffsetFromXz(cX, cZ);
		this.timestampHeader[(int)(checked((IntPtr)offsetFromXz))] = timeStamp;
	}

	// Token: 0x06005A34 RID: 23092 RVA: 0x00229C10 File Offset: 0x00227E10
	public override void OptimizeLayout()
	{
		lock (this)
		{
			this.usedSectors.Clear();
			using (Stream stream = SdFile.Open(this.fullFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				using (PooledExpandableMemoryStream pooledExpandableMemoryStream = MemoryPools.poolMemoryStream.AllocSync(true))
				{
					pooledExpandableMemoryStream.Seek(779L, SeekOrigin.Begin);
					for (int i = 0; i < 8; i++)
					{
						for (int j = 0; j < 8; j++)
						{
							int num;
							int num2;
							this.GetLocationInfo(j, i, out num, out num2);
							if (num > 0 && num2 > 0)
							{
								stream.Seek((long)num, SeekOrigin.Begin);
								int num3 = (int)pooledExpandableMemoryStream.Position;
								StreamUtils.StreamCopy(stream, pooledExpandableMemoryStream, num2, null, true);
								this.SetLocationInfo(j, i, num3, num2);
							}
						}
					}
					stream.Seek(11L, SeekOrigin.Begin);
					RegionFileRaw.WriteBytes<int>(stream, this.locationHeader, 0, this.locationHeader.Length);
					RegionFileRaw.WriteBytes<uint>(stream, this.timestampHeader, 0, this.timestampHeader.Length);
					pooledExpandableMemoryStream.Seek(779L, SeekOrigin.Begin);
					StreamUtils.StreamCopy(pooledExpandableMemoryStream, stream, null, true);
					stream.SetLength(pooledExpandableMemoryStream.Length);
					base.Length = stream.Length;
				}
			}
		}
	}

	// Token: 0x06005A35 RID: 23093 RVA: 0x00229D94 File Offset: 0x00227F94
	[PublicizedFrom(EAccessModifier.Private)]
	public void GetLocationInfo(int cX, int cZ, out int offset, out int length)
	{
		lock (this)
		{
			long num = this.GetOffsetFromXz(cX, cZ) * 2L;
			checked
			{
				offset = this.locationHeader[(int)((IntPtr)num)];
				length = this.locationHeader[(int)((IntPtr)(unchecked(num + 1L)))];
			}
		}
	}

	// Token: 0x06005A36 RID: 23094 RVA: 0x00229DF0 File Offset: 0x00227FF0
	[PublicizedFrom(EAccessModifier.Private)]
	public void SetLocationInfo(int cX, int cZ, int offset, int length)
	{
		lock (this)
		{
			long num = this.GetOffsetFromXz(cX, cZ) * 2L;
			checked
			{
				this.locationHeader[(int)((IntPtr)num)] = offset;
				this.locationHeader[(int)((IntPtr)(unchecked(num + 1L)))] = length;
				if (offset > 0)
				{
					this.usedSectors[offset] = length;
				}
			}
		}
	}

	// Token: 0x06005A37 RID: 23095 RVA: 0x00229E5C File Offset: 0x0022805C
	[PublicizedFrom(EAccessModifier.Private)]
	public long GetOffsetFromXz(int _cX, int _cZ)
	{
		int num = _cX % 8;
		int num2 = _cZ % 8;
		if (_cX < 0)
		{
			num += 7;
		}
		if (_cZ < 0)
		{
			num2 += 7;
		}
		return (long)(num + num2 * 8);
	}

	// Token: 0x06005A38 RID: 23096 RVA: 0x00229E88 File Offset: 0x00228088
	[PublicizedFrom(EAccessModifier.Private)]
	public int FindBestFreeSpace(int requiredLength)
	{
		int num6;
		lock (this)
		{
			int num = 779;
			int num2 = -1;
			int num3 = int.MaxValue;
			foreach (KeyValuePair<int, int> keyValuePair in this.usedSectors)
			{
				int num4 = keyValuePair.Key - num;
				int num5 = num4 - requiredLength;
				if (num5 == 0)
				{
					return num;
				}
				if (num4 > requiredLength && num5 < num3 - requiredLength)
				{
					num2 = num;
					num3 = num4;
				}
				num = keyValuePair.Key + keyValuePair.Value;
			}
			if (num2 > 0)
			{
				num6 = num2;
			}
			else
			{
				num6 = num;
			}
		}
		return num6;
	}

	// Token: 0x06005A39 RID: 23097 RVA: 0x00229F54 File Offset: 0x00228154
	[PublicizedFrom(EAccessModifier.Private)]
	public static void WriteBytes<[IsUnmanaged] T>(Stream stream, T[] buf, int offset, int length) where T : struct, ValueType
	{
		ReadOnlySpan<byte> readOnlySpan = MemoryMarshal.Cast<T, byte>(buf.AsSpan<T>(offset, length));
		stream.Write(readOnlySpan);
	}

	// Token: 0x06005A3A RID: 23098 RVA: 0x00229F7C File Offset: 0x0022817C
	[PublicizedFrom(EAccessModifier.Private)]
	public static void ReadBytes<[IsUnmanaged] T>(Stream stream, T[] buf, int offset, int length) where T : struct, ValueType
	{
		Span<byte> span = MemoryMarshal.Cast<T, byte>(buf.AsSpan<T>(offset, length));
		while (span.Length > 0)
		{
			int num = stream.Read(span);
			if (num <= 0)
			{
				throw new EndOfStreamException();
			}
			Span<byte> span2 = span;
			int num2 = num;
			span = span2.Slice(num2, span2.Length - num2);
		}
	}

	// Token: 0x040045F1 RID: 17905
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CurrentVersion = 1;

	// Token: 0x040045F2 RID: 17906
	[PublicizedFrom(EAccessModifier.Protected)]
	public static readonly byte[] FileHeaderMagicBytes = Encoding.ASCII.GetBytes("7rr");

	// Token: 0x040045F3 RID: 17907
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int FileHeaderMagicBytesLength = 3;

	// Token: 0x040045F4 RID: 17908
	public const int ChunksPerRegionPerDimension = 8;

	// Token: 0x040045F5 RID: 17909
	public const int ChunksPerRegion = 64;

	// Token: 0x040045F6 RID: 17910
	[PublicizedFrom(EAccessModifier.Private)]
	public const int fileHeaderLength = 11;

	// Token: 0x040045F7 RID: 17911
	[PublicizedFrom(EAccessModifier.Private)]
	public const int locationHeaderLength = 128;

	// Token: 0x040045F8 RID: 17912
	[PublicizedFrom(EAccessModifier.Private)]
	public const int timestampHeaderLength = 64;

	// Token: 0x040045F9 RID: 17913
	[PublicizedFrom(EAccessModifier.Private)]
	public const int sectorsStartOffset = 779;

	// Token: 0x040045FA RID: 17914
	[PublicizedFrom(EAccessModifier.Private)]
	public const int reservedBytesPerEntry = 4;

	// Token: 0x040045FB RID: 17915
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int[] locationHeader = new int[128];

	// Token: 0x040045FC RID: 17916
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly uint[] timestampHeader = new uint[64];

	// Token: 0x040045FD RID: 17917
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int paddingBytes;

	// Token: 0x040045FE RID: 17918
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int version;

	// Token: 0x040045FF RID: 17919
	[PublicizedFrom(EAccessModifier.Private)]
	public SortedDictionary<int, int> usedSectors = new SortedDictionary<int, int>();
}
