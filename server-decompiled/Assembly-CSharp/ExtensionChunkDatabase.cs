using System;
using System.IO;

// Token: 0x02000C4D RID: 3149
public class ExtensionChunkDatabase : DatabaseWithFixedDS<long, byte[]>
{
	// Token: 0x06005F6E RID: 24430 RVA: 0x0025E9A4 File Offset: 0x0025CBA4
	public ExtensionChunkDatabase(int _magicBytes, int AraSizeX, int AraSizeY, int ChunkSize)
		: base(_magicBytes, 4, AraSizeX * AraSizeY / ChunkSize / ChunkSize, ChunkSize * ChunkSize, -1L, -1)
	{
		this.SizeOfDataSet = ChunkSize * ChunkSize;
	}

	// Token: 0x06005F6F RID: 24431 RVA: 0x0025E9CA File Offset: 0x0025CBCA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override long readKey(BinaryReader _br)
	{
		return _br.ReadInt64();
	}

	// Token: 0x06005F70 RID: 24432 RVA: 0x0025E9D2 File Offset: 0x0025CBD2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void writeKey(BinaryWriter _bw, long _key)
	{
		_bw.Write(_key);
	}

	// Token: 0x06005F71 RID: 24433 RVA: 0x0025E9DC File Offset: 0x0025CBDC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyFromRead(byte[] _dataRead, byte[] _data)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_data[i] = _dataRead[i];
		}
	}

	// Token: 0x06005F72 RID: 24434 RVA: 0x0025EA00 File Offset: 0x0025CC00
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyToWrite(byte[] _data, byte[] _dataWrite)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_dataWrite[i] = _data[i];
		}
	}

	// Token: 0x06005F73 RID: 24435 RVA: 0x0025EA21 File Offset: 0x0025CC21
	[PublicizedFrom(EAccessModifier.Protected)]
	public override byte[] allocateDataStorage()
	{
		return new byte[this.SizeOfDataSet];
	}

	// Token: 0x04004B08 RID: 19208
	[PublicizedFrom(EAccessModifier.Private)]
	public int SizeOfDataSet;
}
