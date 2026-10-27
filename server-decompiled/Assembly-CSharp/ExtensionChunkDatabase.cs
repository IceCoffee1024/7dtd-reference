using System;
using System.IO;

// Token: 0x02000C58 RID: 3160
public class ExtensionChunkDatabase : DatabaseWithFixedDS<long, byte[]>
{
	// Token: 0x06005F93 RID: 24467 RVA: 0x0025FECC File Offset: 0x0025E0CC
	public ExtensionChunkDatabase(int _magicBytes, int AraSizeX, int AraSizeY, int ChunkSize)
		: base(_magicBytes, 4, AraSizeX * AraSizeY / ChunkSize / ChunkSize, ChunkSize * ChunkSize, -1L, -1)
	{
		this.SizeOfDataSet = ChunkSize * ChunkSize;
	}

	// Token: 0x06005F94 RID: 24468 RVA: 0x0025FEF2 File Offset: 0x0025E0F2
	[PublicizedFrom(EAccessModifier.Protected)]
	public override long readKey(BinaryReader _br)
	{
		return _br.ReadInt64();
	}

	// Token: 0x06005F95 RID: 24469 RVA: 0x0025FEFA File Offset: 0x0025E0FA
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void writeKey(BinaryWriter _bw, long _key)
	{
		_bw.Write(_key);
	}

	// Token: 0x06005F96 RID: 24470 RVA: 0x0025FF04 File Offset: 0x0025E104
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyFromRead(byte[] _dataRead, byte[] _data)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_data[i] = _dataRead[i];
		}
	}

	// Token: 0x06005F97 RID: 24471 RVA: 0x0025FF28 File Offset: 0x0025E128
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void copyToWrite(byte[] _data, byte[] _dataWrite)
	{
		for (int i = 0; i < _data.Length; i++)
		{
			_dataWrite[i] = _data[i];
		}
	}

	// Token: 0x06005F98 RID: 24472 RVA: 0x0025FF49 File Offset: 0x0025E149
	[PublicizedFrom(EAccessModifier.Protected)]
	public override byte[] allocateDataStorage()
	{
		return new byte[this.SizeOfDataSet];
	}

	// Token: 0x04004B2D RID: 19245
	[PublicizedFrom(EAccessModifier.Private)]
	public int SizeOfDataSet;
}
