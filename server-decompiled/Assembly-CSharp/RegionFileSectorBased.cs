using System;
using System.IO;
using System.Text;

// Token: 0x02000BC4 RID: 3012
public abstract class RegionFileSectorBased : RegionFile
{
	// Token: 0x06005A7A RID: 23162 RVA: 0x0022E837 File Offset: 0x0022CA37
	[PublicizedFrom(EAccessModifier.Protected)]
	public RegionFileSectorBased(string _fullFilePath, int _rX, int _rZ, int _version)
		: base(_fullFilePath, _rX, _rZ)
	{
		this.Version = _version;
	}

	// Token: 0x06005A7B RID: 23163 RVA: 0x0022E86C File Offset: 0x0022CA6C
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

	// Token: 0x06005A7C RID: 23164 RVA: 0x0022E90C File Offset: 0x0022CB0C
	public override void GetTimestampInfo(int _cX, int _cZ, out uint _timeStamp)
	{
		lock (this)
		{
			long offsetFromXz = this.GetOffsetFromXz(_cX, _cZ);
			_timeStamp = BitConverter.ToUInt32(this.regionTimestampHeader, (int)offsetFromXz);
		}
	}

	// Token: 0x06005A7D RID: 23165 RVA: 0x0022E95C File Offset: 0x0022CB5C
	public override void SetTimestampInfo(int _cX, int _cZ, uint _timeStamp)
	{
		lock (this)
		{
			long offsetFromXz = this.GetOffsetFromXz(_cX, _cZ);
			Utils.GetBytes(_timeStamp, this.regionTimestampHeader, (int)offsetFromXz);
		}
	}

	// Token: 0x06005A7E RID: 23166 RVA: 0x0022E9A8 File Offset: 0x0022CBA8
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

	// Token: 0x06005A7F RID: 23167 RVA: 0x0022EA14 File Offset: 0x0022CC14
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

	// Token: 0x06005A80 RID: 23168 RVA: 0x0022EA84 File Offset: 0x0022CC84
	public override bool HasChunk(int _cX, int _cZ)
	{
		short num;
		byte b;
		this.GetLocationInfo(_cX, _cZ, out num, out b);
		return num > 0 && b > 0;
	}

	// Token: 0x06005A81 RID: 23169 RVA: 0x0022EAA8 File Offset: 0x0022CCA8
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

	// Token: 0x06005A82 RID: 23170 RVA: 0x0022EAD2 File Offset: 0x0022CCD2
	public override void RemoveChunk(int _cX, int _cZ)
	{
		this.SetLocationInfo(_cX, _cZ, 0, 0);
	}

	// Token: 0x06005A83 RID: 23171 RVA: 0x0022EAE0 File Offset: 0x0022CCE0
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

	// Token: 0x04004664 RID: 18020
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int CURRENT_VERSION = 1;

	// Token: 0x04004665 RID: 18021
	[PublicizedFrom(EAccessModifier.Protected)]
	public static readonly byte[] FileHeaderMagicBytes = Encoding.ASCII.GetBytes("7rg");

	// Token: 0x04004666 RID: 18022
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int FileHeaderMagicBytesLength = 3;

	// Token: 0x04004667 RID: 18023
	public const int ChunksPerRegionPerDimension = 32;

	// Token: 0x04004668 RID: 18024
	public const int ChunksPerRegion = 1024;

	// Token: 0x04004669 RID: 18025
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int SECTOR_SIZE = 4096;

	// Token: 0x0400466A RID: 18026
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] regionLocationHeader = new byte[4096];

	// Token: 0x0400466B RID: 18027
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] regionTimestampHeader = new byte[4096];

	// Token: 0x0400466C RID: 18028
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int Version;
}
