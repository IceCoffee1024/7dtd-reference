using System;
using System.IO;

// Token: 0x02000B76 RID: 2934
public abstract class RegionFile
{
	// Token: 0x17000958 RID: 2392
	// (get) Token: 0x060058D8 RID: 22744 RVA: 0x00220D2C File Offset: 0x0021EF2C
	// (set) Token: 0x060058D9 RID: 22745 RVA: 0x00220D34 File Offset: 0x0021EF34
	public long Length
	{
		get; [PublicizedFrom(EAccessModifier.Protected)]
		set;
	}

	// Token: 0x060058DA RID: 22746 RVA: 0x00220D3D File Offset: 0x0021EF3D
	public static string ConstructFullFilePath(string dir, int rX, int rZ, string ext)
	{
		return string.Format("{0}/r.{1}.{2}.{3}", new object[] { dir, rX, rZ, ext });
	}

	// Token: 0x060058DB RID: 22747 RVA: 0x00220D69 File Offset: 0x0021EF69
	[PublicizedFrom(EAccessModifier.Protected)]
	public RegionFile(string fullFilePath, int rX, int rZ)
	{
		this.regionX = rX;
		this.regionZ = rZ;
		this.fullFilePath = fullFilePath;
	}

	// Token: 0x060058DC RID: 22748 RVA: 0x000027FC File Offset: 0x000009FC
	public virtual void Close()
	{
	}

	// Token: 0x060058DD RID: 22749 RVA: 0x00220D96 File Offset: 0x0021EF96
	public void GetPositionAndPath(out int regionX, out int regionZ, out string fullFilePath)
	{
		regionX = this.regionX;
		regionZ = this.regionZ;
		fullFilePath = this.fullFilePath;
	}

	// Token: 0x060058DE RID: 22750
	public abstract void SaveHeaderData();

	// Token: 0x060058DF RID: 22751
	public abstract void GetTimestampInfo(int cX, int cZ, out uint timeStamp);

	// Token: 0x060058E0 RID: 22752
	public abstract void SetTimestampInfo(int cX, int cZ, uint timeStamp);

	// Token: 0x060058E1 RID: 22753
	public abstract bool HasChunk(int _cX, int _cZ);

	// Token: 0x060058E2 RID: 22754
	public abstract int GetChunkByteCount(int _cX, int _cZ);

	// Token: 0x060058E3 RID: 22755
	public abstract void ReadData(int cX, int cZ, Stream _targetStream);

	// Token: 0x060058E4 RID: 22756
	public abstract void WriteData(int _cX, int _cZ, int _dataLength, byte _compression, byte[] _data, bool _saveHeaderToFile);

	// Token: 0x060058E5 RID: 22757
	public abstract void RemoveChunk(int cX, int cZ);

	// Token: 0x060058E6 RID: 22758
	public abstract void OptimizeLayout();

	// Token: 0x060058E7 RID: 22759
	public abstract int ChunkCount();

	// Token: 0x060058E8 RID: 22760 RVA: 0x00220DB0 File Offset: 0x0021EFB0
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

	// Token: 0x060058E9 RID: 22761 RVA: 0x00220DE4 File Offset: 0x0021EFE4
	[PublicizedFrom(EAccessModifier.Protected)]
	public static void FromShort(short number, out byte byte1, out byte byte2)
	{
		byte2 = (byte)(number >> 8);
		byte1 = (byte)(number & 255);
	}

	// Token: 0x040044D0 RID: 17616
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly byte[] tempReadBuffer = new byte[4096];

	// Token: 0x040044D1 RID: 17617
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly string fullFilePath;

	// Token: 0x040044D2 RID: 17618
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int regionX;

	// Token: 0x040044D3 RID: 17619
	[PublicizedFrom(EAccessModifier.Protected)]
	public readonly int regionZ;
}
