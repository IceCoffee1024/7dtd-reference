using System;
using System.IO;
using System.Text;

// Token: 0x02000064 RID: 100
public static class PlatformUserIdentifierExtensions
{
	// Token: 0x060001F8 RID: 504 RVA: 0x000111E8 File Offset: 0x0000F3E8
	public static void ToStream(this PlatformUserIdentifierAbs _instance, Stream _targetStream, bool _inclCustomData = false)
	{
		if (_targetStream == null)
		{
			return;
		}
		if (_instance == null)
		{
			_targetStream.WriteByte(0);
			return;
		}
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(true))
		{
			pooledBinaryWriter.SetBaseStream(_targetStream);
			_instance.ToStream(pooledBinaryWriter, _inclCustomData);
		}
	}

	// Token: 0x060001F9 RID: 505 RVA: 0x0001123C File Offset: 0x0000F43C
	public static void ToStream(this PlatformUserIdentifierAbs _instance, BinaryWriter _writer, bool _inclCustomData = false)
	{
		if (_writer == null)
		{
			return;
		}
		if (_instance == null)
		{
			_writer.Write(0);
			return;
		}
		_writer.Write(1);
		_writer.Write(1);
		_writer.Write(_instance.PlatformIdentifierString);
		_writer.Write(_instance.ReadablePlatformUserIdentifier);
		if (_inclCustomData)
		{
			_instance.WriteCustomData(_writer);
		}
	}

	// Token: 0x060001FA RID: 506 RVA: 0x00011288 File Offset: 0x0000F488
	public static int GetToStreamLength(this PlatformUserIdentifierAbs _instance, Encoding encoding, bool _inclCustomData = false)
	{
		if (_instance == null)
		{
			return 1;
		}
		return 0 + 1 + 1 + _instance.PlatformIdentifierString.GetBinaryWriterLength(encoding) + _instance.ReadablePlatformUserIdentifier.GetBinaryWriterLength(encoding) + _instance.GetCustomDataLengthEstimate();
	}
}
