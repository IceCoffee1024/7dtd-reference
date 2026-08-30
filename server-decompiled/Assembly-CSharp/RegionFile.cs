using System;
using System.IO;

// Token: 0x02000B88 RID: 2952
public abstract class RegionFile
{
	// Token: 0x1700096A RID: 2410
	// (get) Token: 0x06005960 RID: 22880 RVA: 0x00223A48 File Offset: 0x00221C48
	// (set) Token: 0x06005961 RID: 22881 RVA: 0x00223A50 File Offset: 0x00221C50
	public long Length
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x06005962 RID: 22882 RVA: 0x00223A59 File Offset: 0x00221C59
	public static string ConstructFullFilePath(string dir, int rX, int rZ, string ext)
	{
		return string.Format("{0}/r.{1}.{2}.{3}", new object[] { dir, rX, rZ, ext });
	}

	// Token: 0x06005963 RID: 22883 RVA: 0x00223A85 File Offset: 0x00221C85
	[PublicizedFrom(EAccessModifier.Protected)]
	public RegionFile(string fullFilePath, int rX, int rZ)
	{
		this.regionX = rX;
		this.regionZ = rZ;
		this.fullFilePath = fullFilePath;
	}

	// Token: 0x06005964 RID: 22884 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Close()
	{
	}

	// Token: 0x06005965 RID: 22885 RVA: 0x00223AB2 File Offset: 0x00221CB2
	public void GetPositionAndPath(out int regionX, out int regionZ, out string fullFilePath)
	{
		regionX = this.regionX;
		regionZ = this.regionZ;
		fullFilePath = this.fullFilePath;
	}

	// Token: 0x06005966 RID: 22886
	public abstract void SaveHeaderData();

	// Token: 0x06005967 RID: 22887
	public abstract void GetTimestampInfo(int cX, int cZ, out uint timeStamp);

	// Token: 0x06005968 RID: 22888
	public abstract void SetTimestampInfo(int cX, int cZ, uint timeStamp);

	// Token: 0x06005969 RID: 22889
	public abstract bool HasChunk(int _cX, int _cZ);

	// Token: 0x0600596A RID: 22890
	public abstract int GetChunkByteCount(int _cX, int _cZ);

	// Token: 0x0600596B RID: 22891
	public abstract void ReadData(int cX, int cZ, Stream _targetStream);

	// Token: 0x0600596C RID: 22892
	public abstract void WriteData(int _cX, int _cZ, int _dataLength, byte _compression, byte[] _data, bool _saveHeaderToFile);

	// Token: 0x0600596D RID: 22893
	public abstract void RemoveChunk(int cX, int cZ);

	// Token: 0x0600596E RID: 22894
	public abstract void OptimizeLayout();

	// Token: 0x0600596F RID: 22895
	public abstract int ChunkCount();

	// Token: 0x06005970 RID: 22896 RVA: 0x00223ACC File Offset: 0x00221CCC
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

	// Token: 0x06005971 RID: 22897 RVA: 0x00223B00 File Offset: 0x00221D00
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void FromShort(short number, out byte byte1, out byte byte2)
	{
		byte2 = (byte)(number >> 8);
		byte1 = (byte)(number & 255);
	}

	// Token: 0x0400456B RID: 17771
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] tempReadBuffer = new byte[4096];

	// Token: 0x0400456C RID: 17772
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string fullFilePath;

	// Token: 0x0400456D RID: 17773
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int regionX;

	// Token: 0x0400456E RID: 17774
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int regionZ;
}
