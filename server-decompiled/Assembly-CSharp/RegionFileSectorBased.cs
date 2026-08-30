using System;
using System.IO;
using System.Text;

// Token: 0x02000BA7 RID: 2983
public abstract class RegionFileSectorBased : RegionFile
{
	// Token: 0x06005A3C RID: 23100 RVA: 0x00229FDF File Offset: 0x002281DF
	[PublicizedFrom(EAccessModifier.Protected)]
	public RegionFileSectorBased(string _fullFilePath, int _rX, int _rZ, int _version)
		: base(_fullFilePath, _rX, _rZ)
	{
		this.Version = _version;
	}

	// Token: 0x06005A3D RID: 23101 RVA: 0x0022A014 File Offset: 0x00228214
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

	// Token: 0x06005A3E RID: 23102 RVA: 0x0022A0B4 File Offset: 0x002282B4
	public override void GetTimestampInfo(int _cX, int _cZ, out uint _timeStamp)
	{
		lock (this)
		{
			long offsetFromXz = this.GetOffsetFromXz(_cX, _cZ);
			_timeStamp = BitConverter.ToUInt32(this.regionTimestampHeader, (int)offsetFromXz);
		}
	}

	// Token: 0x06005A3F RID: 23103 RVA: 0x0022A104 File Offset: 0x00228304
	public override void SetTimestampInfo(int _cX, int _cZ, uint _timeStamp)
	{
		lock (this)
		{
			long offsetFromXz = this.GetOffsetFromXz(_cX, _cZ);
			Utils.GetBytes(_timeStamp, this.regionTimestampHeader, (int)offsetFromXz);
		}
	}

	// Token: 0x06005A40 RID: 23104 RVA: 0x0022A150 File Offset: 0x00228350
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

	// Token: 0x06005A41 RID: 23105 RVA: 0x0022A1BC File Offset: 0x002283BC
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

	// Token: 0x06005A42 RID: 23106 RVA: 0x0022A22C File Offset: 0x0022842C
	public override bool HasChunk(int _cX, int _cZ)
	{
		short num;
		byte b;
		this.GetLocationInfo(_cX, _cZ, out num, out b);
		return num > 0 && b > 0;
	}

	// Token: 0x06005A43 RID: 23107 RVA: 0x0022A250 File Offset: 0x00228450
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

	// Token: 0x06005A44 RID: 23108 RVA: 0x0022A27A File Offset: 0x0022847A
	public override void RemoveChunk(int _cX, int _cZ)
	{
		this.SetLocationInfo(_cX, _cZ, 0, 0);
	}

	// Token: 0x06005A45 RID: 23109 RVA: 0x0022A288 File Offset: 0x00228488
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

	// Token: 0x04004600 RID: 17920
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int CURRENT_VERSION = 1;

	// Token: 0x04004601 RID: 17921
	[PublicizedFrom(EAccessModifier.Protected)]
	public static readonly byte[] FileHeaderMagicBytes = Encoding.ASCII.GetBytes("7rg");

	// Token: 0x04004602 RID: 17922
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int FileHeaderMagicBytesLength = 3;

	// Token: 0x04004603 RID: 17923
	public const int ChunksPerRegionPerDimension = 32;

	// Token: 0x04004604 RID: 17924
	public const int ChunksPerRegion = 1024;

	// Token: 0x04004605 RID: 17925
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int SECTOR_SIZE = 4096;

	// Token: 0x04004606 RID: 17926
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] regionLocationHeader = new byte[4096];

	// Token: 0x04004607 RID: 17927
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] regionTimestampHeader = new byte[4096];

	// Token: 0x04004608 RID: 17928
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int Version;
}
