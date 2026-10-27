using System;
using System.IO;

// Token: 0x02000BA5 RID: 2981
public abstract class RegionFile
{
	// Token: 0x17000992 RID: 2450
	// (get) Token: 0x0600599F RID: 22943 RVA: 0x00228230 File Offset: 0x00226430
	// (set) Token: 0x060059A0 RID: 22944 RVA: 0x00228238 File Offset: 0x00226438
	public long Length
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x060059A1 RID: 22945 RVA: 0x00228241 File Offset: 0x00226441
	public static string ConstructFullFilePath(string dir, int rX, int rZ, string ext)
	{
		return string.Format("{0}/r.{1}.{2}.{3}", new object[] { dir, rX, rZ, ext });
	}

	// Token: 0x060059A2 RID: 22946 RVA: 0x0022826D File Offset: 0x0022646D
	[PublicizedFrom(EAccessModifier.Protected)]
	public RegionFile(string fullFilePath, int rX, int rZ)
	{
		this.regionX = rX;
		this.regionZ = rZ;
		this.fullFilePath = fullFilePath;
	}

	// Token: 0x060059A3 RID: 22947 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Close()
	{
	}

	// Token: 0x060059A4 RID: 22948 RVA: 0x0022829A File Offset: 0x0022649A
	public void GetPositionAndPath(out int regionX, out int regionZ, out string fullFilePath)
	{
		regionX = this.regionX;
		regionZ = this.regionZ;
		fullFilePath = this.fullFilePath;
	}

	// Token: 0x060059A5 RID: 22949
	public abstract void SaveHeaderData();

	// Token: 0x060059A6 RID: 22950
	public abstract void GetTimestampInfo(int cX, int cZ, out uint timeStamp);

	// Token: 0x060059A7 RID: 22951
	public abstract void SetTimestampInfo(int cX, int cZ, uint timeStamp);

	// Token: 0x060059A8 RID: 22952
	public abstract bool HasChunk(int _cX, int _cZ);

	// Token: 0x060059A9 RID: 22953
	public abstract int GetChunkByteCount(int _cX, int _cZ);

	// Token: 0x060059AA RID: 22954
	public abstract void ReadData(int cX, int cZ, Stream _targetStream);

	// Token: 0x060059AB RID: 22955
	public abstract void WriteData(int _cX, int _cZ, int _dataLength, byte _compression, byte[] _data, bool _saveHeaderToFile);

	// Token: 0x060059AC RID: 22956
	public abstract void RemoveChunk(int cX, int cZ);

	// Token: 0x060059AD RID: 22957
	public abstract void OptimizeLayout();

	// Token: 0x060059AE RID: 22958
	public abstract int ChunkCount();

	// Token: 0x060059AF RID: 22959 RVA: 0x002282B4 File Offset: 0x002264B4
	[PublicizedFrom(EAccessModifier.Protected)]
	public static short ToShort(short byte1, short byte2)
	{
		int num = ((int)byte2 << 8) + (int)byte1;
		short num2;
		try
		{
			num2 = Convert.ToInt16(num);
		}
		catch
		{
			num2 = 0;
		}
		return num2;
	}

	// Token: 0x060059B0 RID: 22960 RVA: 0x002282E8 File Offset: 0x002264E8
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void FromShort(short number, out byte byte1, out byte byte2)
	{
		byte2 = (byte)(number >> 8);
		byte1 = (byte)(number & 255);
	}

	// Token: 0x040045CE RID: 17870
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] tempReadBuffer = new byte[4096];

	// Token: 0x040045CF RID: 17871
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string fullFilePath;

	// Token: 0x040045D0 RID: 17872
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int regionX;

	// Token: 0x040045D1 RID: 17873
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int regionZ;
}
