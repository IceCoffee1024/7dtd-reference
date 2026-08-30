using System;
using System.IO;
using System.Text;

// Token: 0x02000B95 RID: 2965
public abstract class RegionFileSectorBased : RegionFile
{
	// Token: 0x060059B4 RID: 22964 RVA: 0x002272C3 File Offset: 0x002254C3
	[PublicizedFrom(EAccessModifier.Protected)]
	public RegionFileSectorBased(string _fullFilePath, int _rX, int _rZ, int _version)
		: base(_fullFilePath, _rX, _rZ)
	{
		this.Version = _version;
	}

	// Token: 0x060059B5 RID: 22965 RVA: 0x002272F8 File Offset: 0x002254F8
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

	// Token: 0x060059B6 RID: 22966 RVA: 0x00227398 File Offset: 0x00225598
	public override void GetTimestampInfo(int _cX, int _cZ, out uint _timeStamp)
	{
		lock (this)
		{
			long offsetFromXz = this.GetOffsetFromXz(_cX, _cZ);
			_timeStamp = BitConverter.ToUInt32(this.regionTimestampHeader, (int)offsetFromXz);
		}
	}

	// Token: 0x060059B7 RID: 22967 RVA: 0x002273E8 File Offset: 0x002255E8
	public override void SetTimestampInfo(int _cX, int _cZ, uint _timeStamp)
	{
		lock (this)
		{
			long offsetFromXz = this.GetOffsetFromXz(_cX, _cZ);
			Utils.GetBytes(_timeStamp, this.regionTimestampHeader, (int)offsetFromXz);
		}
	}

	// Token: 0x060059B8 RID: 22968 RVA: 0x00227434 File Offset: 0x00225634
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

	// Token: 0x060059B9 RID: 22969 RVA: 0x002274A0 File Offset: 0x002256A0
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

	// Token: 0x060059BA RID: 22970 RVA: 0x00227510 File Offset: 0x00225710
	public override bool HasChunk(int _cX, int _cZ)
	{
		short num;
		byte b;
		this.GetLocationInfo(_cX, _cZ, out num, out b);
		return num > 0 && b > 0;
	}

	// Token: 0x060059BB RID: 22971 RVA: 0x00227534 File Offset: 0x00225734
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

	// Token: 0x060059BC RID: 22972 RVA: 0x0022755E File Offset: 0x0022575E
	public override void RemoveChunk(int _cX, int _cZ)
	{
		this.SetLocationInfo(_cX, _cZ, 0, 0);
	}

	// Token: 0x060059BD RID: 22973 RVA: 0x0022756C File Offset: 0x0022576C
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

	// Token: 0x04004565 RID: 17765
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int CURRENT_VERSION = 1;

	// Token: 0x04004566 RID: 17766
	[PublicizedFrom(EAccessModifier.Protected)]
	public static readonly byte[] FileHeaderMagicBytes = Encoding.ASCII.GetBytes("7rg");

	// Token: 0x04004567 RID: 17767
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int FileHeaderMagicBytesLength = 3;

	// Token: 0x04004568 RID: 17768
	public const int ChunksPerRegionPerDimension = 32;

	// Token: 0x04004569 RID: 17769
	public const int ChunksPerRegion = 1024;

	// Token: 0x0400456A RID: 17770
	[PublicizedFrom(EAccessModifier.Protected)]
	public const int SECTOR_SIZE = 4096;

	// Token: 0x0400456B RID: 17771
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] regionLocationHeader = new byte[4096];

	// Token: 0x0400456C RID: 17772
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] regionTimestampHeader = new byte[4096];

	// Token: 0x0400456D RID: 17773
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int Version;
}
