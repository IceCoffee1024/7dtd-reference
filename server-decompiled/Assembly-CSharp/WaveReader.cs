using System;
using System.IO;

// Token: 0x020003C3 RID: 963
public class WaveReader
{
	// Token: 0x06001CF7 RID: 7415 RVA: 0x000AE840 File Offset: 0x000ACA40
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

	// Token: 0x1700037E RID: 894
	// (set) Token: 0x06001CF8 RID: 7416 RVA: 0x000AE8CC File Offset: 0x000ACACC
	public int Position
	{
		set
		{
			this.position = this.dataStartPos + value;
		}
	}

	// Token: 0x06001CF9 RID: 7417 RVA: 0x000AE8DC File Offset: 0x000ACADC
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

	// Token: 0x06001CFA RID: 7418 RVA: 0x000AE968 File Offset: 0x000ACB68
	public void Cleanup()
	{
		MemoryPools.poolByte.Free(this.buffer);
		this.buffer = null;
	}

	// Token: 0x040012E9 RID: 4841
	[PublicizedFrom(EAccessModifier.Private)]
	public string filename;

	// Token: 0x040012EA RID: 4842
	[PublicizedFrom(EAccessModifier.Private)]
	public const int bufferSize = 8192;

	// Token: 0x040012EB RID: 4843
	[PublicizedFrom(EAccessModifier.Private)]
	public byte[] buffer;

	// Token: 0x040012EC RID: 4844
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly int dataStartPos;

	// Token: 0x040012ED RID: 4845
	[PublicizedFrom(EAccessModifier.Private)]
	public int position;
}
