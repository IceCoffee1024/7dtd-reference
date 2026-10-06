using System;
using System.IO;

// Token: 0x020003CA RID: 970
public class WaveReader
{
	// Token: 0x06001D0C RID: 7436 RVA: 0x000AF18C File Offset: 0x000AD38C
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
	// (set) Token: 0x06001D0D RID: 7437 RVA: 0x000AF218 File Offset: 0x000AD418
	public int Position
	{
		set
		{
			this.position = this.dataStartPos + value;
		}
	}

	// Token: 0x06001D0E RID: 7438 RVA: 0x000AF228 File Offset: 0x000AD428
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

	// Token: 0x06001D0F RID: 7439 RVA: 0x000AF2B4 File Offset: 0x000AD4B4
	public void Cleanup()
	{
		MemoryPools.poolByte.Free(this.buffer);
		this.buffer = null;
	}

	// Token: 0x040012F7 RID: 4855
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x040012F8 RID: 4856
	[PublicizedFrom(EAccessModifier.Private)]
	public const int bufferSize = 8192;

	// Token: 0x040012F9 RID: 4857
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] buffer;

	// Token: 0x040012FA RID: 4858
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int dataStartPos;

	// Token: 0x040012FB RID: 4859
	[PublicizedFrom(EAccessModifier.Private)]
	public int position;
}
