using System;
using System.IO;

// Token: 0x020003C7 RID: 967
public class WaveReader
{
	// Token: 0x06001D14 RID: 7444 RVA: 0x000AEF80 File Offset: 0x000AD180
	public WaveReader(string _fileName)
	{
		this.filename = _fileName;
		this.buffer = MemoryPools.poolByte.Alloc(8192);
		using (BinaryReader binaryReader = new BinaryReader(SdFile.Open(this.filename, FileMode.Open, FileAccess.Read, FileShare.Read)))
		{
			string text = new string(binaryReader.ReadChars(88));
			this.position = (this.dataStartPos = text.IndexOf("data") + 8);
		}
	}

	// Token: 0x17000382 RID: 898
	// (set) Token: 0x06001D15 RID: 7445 RVA: 0x000AF00C File Offset: 0x000AD20C
	public int Position
	{
		set
		{
			this.position = this.dataStartPos + value;
		}
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x000AF01C File Offset: 0x000AD21C
	public void Read(float[] data, int count)
	{
		using (BinaryReader binaryReader = new BinaryReader(SdFile.Open(this.filename, FileMode.Open, FileAccess.Read, FileShare.Read)))
		{
			binaryReader.BaseStream.Position = (long)this.position;
			binaryReader.Read(this.buffer, 0, 8192);
			for (int i = 0; i < data.Length; i++)
			{
				short num = BitConverter.ToInt16(this.buffer, 2 * i);
				data[i] = (float)num / 32767f;
			}
		}
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x000AF0A8 File Offset: 0x000AD2A8
	public void Cleanup()
	{
		MemoryPools.poolByte.Free(this.buffer);
		this.buffer = null;
	}

	// Token: 0x040012FE RID: 4862
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x040012FF RID: 4863
	[PublicizedFrom(EAccessModifier.Private)]
	public const int bufferSize = 8192;

	// Token: 0x04001300 RID: 4864
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] buffer;

	// Token: 0x04001301 RID: 4865
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int dataStartPos;

	// Token: 0x04001302 RID: 4866
	[PublicizedFrom(EAccessModifier.Private)]
	public int position;
}
