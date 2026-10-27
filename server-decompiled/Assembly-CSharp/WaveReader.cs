using System;
using System.IO;

// Token: 0x020003CC RID: 972
public class WaveReader
{
	// Token: 0x06001D10 RID: 7440 RVA: 0x000AF39C File Offset: 0x000AD59C
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

	// Token: 0x1700037A RID: 890
	// (set) Token: 0x06001D11 RID: 7441 RVA: 0x000AF428 File Offset: 0x000AD628
	public int Position
	{
		set
		{
			this.position = this.dataStartPos + value;
		}
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x000AF438 File Offset: 0x000AD638
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

	// Token: 0x06001D13 RID: 7443 RVA: 0x000AF4C4 File Offset: 0x000AD6C4
	public void Cleanup()
	{
		MemoryPools.poolByte.Free(this.buffer);
		this.buffer = null;
	}

	// Token: 0x04001307 RID: 4871
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x04001308 RID: 4872
	[PublicizedFrom(EAccessModifier.Private)]
	public const int bufferSize = 8192;

	// Token: 0x04001309 RID: 4873
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] buffer;

	// Token: 0x0400130A RID: 4874
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int dataStartPos;

	// Token: 0x0400130B RID: 4875
	[PublicizedFrom(EAccessModifier.Private)]
	public int position;
}
