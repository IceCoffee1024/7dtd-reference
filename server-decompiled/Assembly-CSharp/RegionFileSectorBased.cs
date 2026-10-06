using System;
using System.IO;
using System.Text;

// Token: 0x02000BB9 RID: 3001
public abstract class RegionFileSectorBased : RegionFile
{
	// Token: 0x06005A55 RID: 23125 RVA: 0x0022D2DB File Offset: 0x0022B4DB
	[PublicizedFrom(EAccessModifier.Protected)]
	public RegionFileSectorBased(string _fullFilePath, int _rX, int _rZ, int _version)
		: base(_fullFilePath, _rX, _rZ)
	{
		this.Version = _version;
	}

	// Token: 0x06005A56 RID: 23126 RVA: 0x0022D310 File Offset: 0x0022B510
	public static RegionFile Get(string dir, int rX, int rZ, string ext)
	{
		string text = RegionFile.ConstructFullFilePath(dir, rX, rZ, ext);
		if (!SdFile.Exists(text))
		{
			SdFile.Create(text).Close();
			return new RegionFileV2(text, rX, rZ, null, 1);
		}
		Stream stream = SdFile.Open(text, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
		byte[] array = new byte[3];
		stream.Read(array, 0, 3);
		for (int i = 0; i < 3; i++)
		{
			if (array[i] != RegionFileSectorBased.FileHeaderMagicBytes[i])
			{
				throw new Exception("Incorrect region file header! " + text);
			}
		}
		int num = (int)((byte)stream.ReadByte());
		if (num < 1)
		{
			return new RegionFileV1(text, rX, rZ, stream, num);
		}
		return new RegionFileV2(text, rX, rZ, stream, num);
	}

	// Token: 0x06005A57 RID: 23127 RVA: 0x0022D3B0 File Offset: 0x0022B5B0
	public override void GetTimestampInfo(int _cX, int _cZ, out uint _timeStamp)
	{
		lock (this)
		{
			long offsetFromXz = this.GetOffsetFromXz(_cX, _cZ);
			_timeStamp = BitConverter.ToUInt32(this.regionTimestampHeader, (int)offsetFromXz);
		}
	}

	// Token: 0x06005A58 RID: 23128 RVA: 0x0022D400 File Offset: 0x0022B600
	public override void SetTimestampInfo(int _cX, int _cZ, uint _timeStamp)
	{
		lock (this)
		{
			long offsetFromXz = this.GetOffsetFromXz(_cX, _cZ);
			Utils.GetBytes(_timeStamp, this.regionTimestampHeader, (int)offsetFromXz);
		}
	}

	// Token: 0x06005A59 RID: 23129 RVA: 0x0022D44C File Offset: 0x0022B64C
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void GetLocationInfo(int _cX, int _cZ, out short _sectorOffset, out byte _sectorLength)
	{
		checked
		{
			lock (this)
			{
				long offsetFromXz = this.GetOffsetFromXz(_cX, _cZ);
				_sectorOffset = RegionFile.ToShort((short)this.regionLocationHeader[(int)((IntPtr)offsetFromXz)], (short)this.regionLocationHeader[(int)((IntPtr)(unchecked(offsetFromXz + 1L)))]);
				_sectorLength = this.regionLocationHeader[(int)((IntPtr)(unchecked(offsetFromXz + 3L)))];
			}
		}
	}

	// Token: 0x06005A5A RID: 23130 RVA: 0x0022D4B8 File Offset: 0x0022B6B8
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual void SetLocationInfo(int _cX, int _cZ, short _sectorOffset, byte _sectorLength)
	{
		checked
		{
			lock (this)
			{
				long offsetFromXz = this.GetOffsetFromXz(_cX, _cZ);
				RegionFile.FromShort(_sectorOffset, out this.regionLocationHeader[(int)((IntPtr)offsetFromXz)], out this.regionLocationHeader[(int)((IntPtr)(unchecked(offsetFromXz + 1L)))]);
				this.regionLocationHeader[(int)((IntPtr)(unchecked(offsetFromXz + 3L)))] = _sectorLength;
			}
		}
	}

	// Token: 0x06005A5B RID: 23131 RVA: 0x0022D528 File Offset: 0x0022B728
	public override bool HasChunk(int _cX, int _cZ)
	{
		short num;
		byte b;
		this.GetLocationInfo(_cX, _cZ, out num, out b);
		return num > 0 && b > 0;
	}

	// Token: 0x06005A5C RID: 23132 RVA: 0x0022D54C File Offset: 0x0022B74C
	public override int GetChunkByteCount(int _cX, int _cZ)
	{
		short num;
		byte b;
		this.GetLocationInfo(_cX, _cZ, out num, out b);
		if (num <= 0 || b <= 0)
		{
			return 0;
		}
		return (int)b * 4096;
	}

	// Token: 0x06005A5D RID: 23133 RVA: 0x0022D576 File Offset: 0x0022B776
	public override void RemoveChunk(int _cX, int _cZ)
	{
		this.SetLocationInfo(_cX, _cZ, 0, 0);
	}

	// Token: 0x06005A5E RID: 23134 RVA: 0x0022D584 File Offset: 0x0022B784
	[PublicizedFrom(EAccessModifier.Protected)]
	public virtual long GetOffsetFromXz(int _cX, int _cZ)
	{
		int num = _cX % 32;
		int num2 = _cZ % 32;
		if (_cX < 0)
		{
			num += 31;
		}
		if (_cZ < 0)
		{
			num2 += 31;
		}
		return (long)(4 * (num + num2 * 32));
	}

	// Token: 0x0400463F RID: 17983
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int CURRENT_VERSION = 1;

	// Token: 0x04004640 RID: 17984
	[PublicizedFrom(EAccessModifier.Protected)]
	public static readonly byte[] FileHeaderMagicBytes = Encoding.ASCII.GetBytes("7rg");

	// Token: 0x04004641 RID: 17985
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int FileHeaderMagicBytesLength = 3;

	// Token: 0x04004642 RID: 17986
	public const int ChunksPerRegionPerDimension = 32;

	// Token: 0x04004643 RID: 17987
	public const int ChunksPerRegion = 1024;

	// Token: 0x04004644 RID: 17988
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int SECTOR_SIZE = 4096;

	// Token: 0x04004645 RID: 17989
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] regionLocationHeader = new byte[4096];

	// Token: 0x04004646 RID: 17990
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] regionTimestampHeader = new byte[4096];

	// Token: 0x04004647 RID: 17991
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int Version;
}
