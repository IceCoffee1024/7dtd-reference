using System;
using System.IO;

// Token: 0x02000BC5 RID: 3013
public class RegionFileV1 : RegionFileSectorBased
{
	// Token: 0x06005A85 RID: 23173 RVA: 0x0022EB28 File Offset: 0x0022CD28
	public RegionFileV1(string _fullFilePath, int _rX, int _rZ, Stream _fileStream, int _version)
		: base(_fullFilePath, _rX, _rZ, _version)
	{
		try
		{
			if (_fileStream == null)
			{
				_fileStream = SdFile.Open(_fullFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
				byte[] array = new byte[8196];
				array[0] = RegionFileSectorBased.FileHeaderMagicBytes[0];
				array[1] = RegionFileSectorBased.FileHeaderMagicBytes[1];
				array[2] = RegionFileSectorBased.FileHeaderMagicBytes[2];
				array[3] = 0;
				_fileStream.Write(array, 0, array.Length);
				_fileStream.Seek(0L, SeekOrigin.Begin);
			}
			_fileStream.Seek(4L, SeekOrigin.Begin);
			_fileStream.Read(this.regionLocationHeader, 0, 4096);
			_fileStream.Read(this.regionTimestampHeader, 0, 4096);
		}
		finally
		{
			if (_fileStream != null)
			{
				_fileStream.Dispose();
			}
		}
	}

	// Token: 0x06005A86 RID: 23174 RVA: 0x0022EBE4 File Offset: 0x0022CDE4
	public override void SaveHeaderData()
	{
		lock (this)
		{
			using (Stream stream = SdFile.Open(this.fullFilePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
			{
				stream.Write(RegionFileSectorBased.FileHeaderMagicBytes, 0, 3);
				stream.WriteByte(0);
				stream.Write(this.regionLocationHeader, 0, 4096);
				stream.Write(this.regionTimestampHeader, 0, 4096);
			}
		}
	}

	// Token: 0x06005A87 RID: 23175 RVA: 0x0022EC78 File Offset: 0x0022CE78
	public override void ReadData(int _cX, int _cZ, Stream _targetStream)
	{
		lock (this)
		{
			short num;
			byte b;
			this.GetLocationInfo(_cX, _cZ, out num, out b);
			using (Stream stream = SdFile.Open(this.fullFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
			{
				stream.Seek((long)(num * 4096 + 4), SeekOrigin.Begin);
				int num2 = StreamUtils.ReadInt32(stream);
				stream.ReadByte();
				long position = _targetStream.Position;
				if (num2 > 0)
				{
					_targetStream.SetLength(0L);
				}
				try
				{
					StreamUtils.StreamCopy(stream, _targetStream, num2, this.tempReadBuffer, true);
				}
				catch (NotSupportedException)
				{
					Log.Error("Chunk length: {0}, pos before: {1}", new object[] { num2, position });
					throw;
				}
			}
		}
	}

	// Token: 0x06005A88 RID: 23176 RVA: 0x0022ED60 File Offset: 0x0022CF60
	public override void WriteData(int _cX, int _cZ, int _dataLength, byte _compression, byte[] _data, bool _saveHeaderToFile)
	{
		lock (this)
		{
			uint num = GameUtils.WorldTimeToTotalMinutes(GameManager.Instance.World.worldTime);
			using (Stream stream = SdFile.Open(this.fullFilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
			{
				stream.ReadByte();
				short num2;
				byte b;
				this.GetLocationInfo(_cX, _cZ, out num2, out b);
				if (num2 == 0)
				{
					num2 = (short)Math.Ceiling((double)stream.Length / 4096.0);
				}
				byte b2 = (byte)Math.Ceiling(_dataLength / 4096m);
				if (b2 <= b)
				{
					this.SetLocationInfo(_cX, _cZ, num2, b2);
					this.SetTimestampInfo(_cX, _cZ, num);
					stream.Seek((long)(num2 * 4096 + 4), SeekOrigin.Begin);
				}
				else if (num2 == (short)Math.Ceiling((double)stream.Length / 4096.0) - (short)b2)
				{
					this.SetLocationInfo(_cX, _cZ, num2, b2);
					this.SetTimestampInfo(_cX, _cZ, num);
					stream.Seek((long)(num2 * 4096 + 4), SeekOrigin.Begin);
				}
				else
				{
					this.SetLocationInfo(_cX, _cZ, (short)Math.Ceiling((double)stream.Length / 4096.0), b2);
					this.SetTimestampInfo(_cX, _cZ, num);
					stream.Seek((long)((short)Math.Ceiling((double)stream.Length / 4096.0) * 4096 + 4), SeekOrigin.Begin);
				}
				StreamUtils.Write(stream, _dataLength);
				stream.WriteByte(0);
				stream.Write(_data, 0, _dataLength);
				for (int i = _dataLength; i < (int)b2 * 4096; i++)
				{
					stream.WriteByte(0);
				}
			}
			if (_saveHeaderToFile)
			{
				this.SaveHeaderData();
			}
		}
	}

	// Token: 0x06005A89 RID: 23177 RVA: 0x000027FC File Offset: 0x000009FC
	public override void OptimizeLayout()
	{
	}

	// Token: 0x06005A8A RID: 23178 RVA: 0x00100DD1 File Offset: 0x000FEFD1
	public override int ChunkCount()
	{
		return -1;
	}

	// Token: 0x0400466D RID: 18029
	[PublicizedFrom(EAccessModifier.Private)]
	public const int FILE_HEADER_SIZE = 4;
}
