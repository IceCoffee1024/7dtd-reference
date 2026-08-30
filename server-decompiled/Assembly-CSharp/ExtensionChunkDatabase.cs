using System;
using System.IO;

// Token: 0x02000C3B RID: 3131
public class ExtensionChunkDatabase : DatabaseWithFixedDS<long, byte[]>
{
	// Token: 0x06005F55 RID: 24405 RVA: 0x0025B6A8 File Offset: 0x002598A8
	public ExtensionChunkDatabase(int _magicBytes, int AraSizeX, int AraSizeY, int ChunkSize)
		: base(_magicBytes, 4, AraSizeX * AraSizeY / ChunkSize / ChunkSize, ChunkSize * ChunkSize, -1L, -1)
	{
		this.SizeOfDataSet = ChunkSize * ChunkSize;
	}

	// Token: 0x06005F56 RID: 24406 RVA: 0x0025B6CE File Offset: 0x002598CE
	[PublicizedFrom(EAccessModifier.Protected)]
	public override long readKey(BinaryReader _br)
	{
		return _br.ReadInt64();
	}

	// Token: 0x06005F57 RID: 24407 RVA: 0x0025B6D6 File Offset: 0x002598D6
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void writeKey(BinaryWriter _bw, long _key)
	{
		_bw.Write(_key);
	}

	// Token: 0x06005F58 RID: 24408 RVA: 0x0025B6E0 File Offset: 0x002598E0
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyFromRead(byte[] _dataRead, byte[] _data)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_data[i] = _dataRead[i];
		}
	}

	// Token: 0x06005F59 RID: 24409 RVA: 0x0025B704 File Offset: 0x00259904
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyToWrite(byte[] _data, byte[] _dataWrite)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_dataWrite[i] = _data[i];
		}
	}

	// Token: 0x06005F5A RID: 24410 RVA: 0x0025B725 File Offset: 0x00259925
	[PublicizedFrom(EAccessModifier.Protected)]
	public override byte[] allocateDataStorage()
	{
		return new byte[this.SizeOfDataSet];
	}

	// Token: 0x04004AC9 RID: 19145
	[PublicizedFrom(EAccessModifier.Private)]
	public int SizeOfDataSet;
}
