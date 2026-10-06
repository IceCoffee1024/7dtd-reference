using System;
using System.IO;

// Token: 0x02000B9A RID: 2970
public abstract class RegionFile
{
	// Token: 0x1700098C RID: 2444
	// (get) Token: 0x0600597A RID: 22906 RVA: 0x00226CD4 File Offset: 0x00224ED4
	// (set) Token: 0x0600597B RID: 22907 RVA: 0x00226CDC File Offset: 0x00224EDC
	public long Length
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x0600597C RID: 22908 RVA: 0x00226CE5 File Offset: 0x00224EE5
	public static string ConstructFullFilePath(string dir, int rX, int rZ, string ext)
	{
		return string.Format("{0}/r.{1}.{2}.{3}", new object[] { dir, rX, rZ, ext });
	}

	// Token: 0x0600597D RID: 22909 RVA: 0x00226D11 File Offset: 0x00224F11
	[PublicizedFrom(EAccessModifier.Protected)]
	public RegionFile(string fullFilePath, int rX, int rZ)
	{
		this.regionX = rX;
		this.regionZ = rZ;
		this.fullFilePath = fullFilePath;
	}

	// Token: 0x0600597E RID: 22910 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Close()
	{
	}

	// Token: 0x0600597F RID: 22911 RVA: 0x00226D3E File Offset: 0x00224F3E
	public void GetPositionAndPath(out int regionX, out int regionZ, out string fullFilePath)
	{
		regionX = this.regionX;
		regionZ = this.regionZ;
		fullFilePath = this.fullFilePath;
	}

	// Token: 0x06005980 RID: 22912
	public abstract void SaveHeaderData();

	// Token: 0x06005981 RID: 22913
	public abstract void GetTimestampInfo(int cX, int cZ, out uint timeStamp);

	// Token: 0x06005982 RID: 22914
	public abstract void SetTimestampInfo(int cX, int cZ, uint timeStamp);

	// Token: 0x06005983 RID: 22915
	public abstract bool HasChunk(int _cX, int _cZ);

	// Token: 0x06005984 RID: 22916
	public abstract int GetChunkByteCount(int _cX, int _cZ);

	// Token: 0x06005985 RID: 22917
	public abstract void ReadData(int cX, int cZ, Stream _targetStream);

	// Token: 0x06005986 RID: 22918
	public abstract void WriteData(int _cX, int _cZ, int _dataLength, byte _compression, byte[] _data, bool _saveHeaderToFile);

	// Token: 0x06005987 RID: 22919
	public abstract void RemoveChunk(int cX, int cZ);

	// Token: 0x06005988 RID: 22920
	public abstract void OptimizeLayout();

	// Token: 0x06005989 RID: 22921
	public abstract int ChunkCount();

	// Token: 0x0600598A RID: 22922 RVA: 0x00226D58 File Offset: 0x00224F58
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

	// Token: 0x0600598B RID: 22923 RVA: 0x00226D8C File Offset: 0x00224F8C
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void FromShort(short number, out byte byte1, out byte byte2)
	{
		byte2 = (byte)(number >> 8);
		byte1 = (byte)(number & 255);
	}

	// Token: 0x040045A9 RID: 17833
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] tempReadBuffer = new byte[4096];

	// Token: 0x040045AA RID: 17834
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string fullFilePath;

	// Token: 0x040045AB RID: 17835
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int regionX;

	// Token: 0x040045AC RID: 17836
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int regionZ;
}
