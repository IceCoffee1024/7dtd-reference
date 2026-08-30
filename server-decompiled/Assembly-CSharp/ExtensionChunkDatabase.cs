using System;
using System.IO;

// Token: 0x02000C29 RID: 3113
public class ExtensionChunkDatabase : DatabaseWithFixedDS<long, byte[]>
{
	// Token: 0x06005ECB RID: 24267 RVA: 0x002587F8 File Offset: 0x002569F8
	public ExtensionChunkDatabase(int _magicBytes, int AraSizeX, int AraSizeY, int ChunkSize)
		: base(_magicBytes, 4, AraSizeX * AraSizeY / ChunkSize / ChunkSize, ChunkSize * ChunkSize, -1L, -1)
	{
		this.SizeOfDataSet = ChunkSize * ChunkSize;
	}

	// Token: 0x06005ECC RID: 24268 RVA: 0x0025881E File Offset: 0x00256A1E
	[PublicizedFrom(EAccessModifier.Protected)]
	public override long readKey(BinaryReader _br)
	{
		return _br.ReadInt64();
	}

	// Token: 0x06005ECD RID: 24269 RVA: 0x00258826 File Offset: 0x00256A26
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void writeKey(BinaryWriter _bw, long _key)
	{
		_bw.Write(_key);
	}

	// Token: 0x06005ECE RID: 24270 RVA: 0x00258830 File Offset: 0x00256A30
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyFromRead(byte[] _dataRead, byte[] _data)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_data[i] = _dataRead[i];
		}
	}

	// Token: 0x06005ECF RID: 24271 RVA: 0x00258854 File Offset: 0x00256A54
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyToWrite(byte[] _data, byte[] _dataWrite)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_dataWrite[i] = _data[i];
		}
	}

	// Token: 0x06005ED0 RID: 24272 RVA: 0x00258875 File Offset: 0x00256A75
	[PublicizedFrom(EAccessModifier.Protected)]
	public override byte[] allocateDataStorage()
	{
		return new byte[this.SizeOfDataSet];
	}

	// Token: 0x04004A2C RID: 18988
	[PublicizedFrom(EAccessModifier.Private)]
	public int SizeOfDataSet;
}
