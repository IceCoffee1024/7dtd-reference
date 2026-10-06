using System;
using System.IO;
using UnityEngine;

// Token: 0x020014D1 RID: 5329
public static class StreamUtils
{
	// Token: 0x0600A6AD RID: 42669 RVA: 0x003E0EA4 File Offset: 0x003DF0A4
	public static long ReadInt64(Stream clientStream)
	{
		return (long)(clientStream.ReadByte() & 255) | ((long)(clientStream.ReadByte() & 255) << 8) | ((long)(clientStream.ReadByte() & 255) << 16) | ((long)(clientStream.ReadByte() & 255) << 24) | ((long)(clientStream.ReadByte() & 255) << 32) | ((long)(clientStream.ReadByte() & 255) << 40) | ((long)(clientStream.ReadByte() & 255) << 48) | ((long)(clientStream.ReadByte() & 255) << 56);
	}

	// Token: 0x0600A6AE RID: 42670 RVA: 0x003E0F34 File Offset: 0x003DF134
	public static void Write(Stream clientStream, long v)
	{
		clientStream.WriteByte((byte)(v & 255L));
		clientStream.WriteByte((byte)((v >> 8) & 255L));
		clientStream.WriteByte((byte)((v >> 16) & 255L));
		clientStream.WriteByte((byte)((v >> 24) & 255L));
		clientStream.WriteByte((byte)((v >> 32) & 255L));
		clientStream.WriteByte((byte)((v >> 40) & 255L));
		clientStream.WriteByte((byte)((v >> 48) & 255L));
		clientStream.WriteByte((byte)((v >> 56) & 255L));
	}

	// Token: 0x0600A6AF RID: 42671 RVA: 0x003E0FCD File Offset: 0x003DF1CD
	public static int ReadInt32(Stream clientStream)
	{
		return 0 | (clientStream.ReadByte() & 255) | ((clientStream.ReadByte() & 255) << 8) | ((clientStream.ReadByte() & 255) << 16) | ((clientStream.ReadByte() & 255) << 24);
	}

	// Token: 0x0600A6B0 RID: 42672 RVA: 0x003E100C File Offset: 0x003DF20C
	public static int ReadInt32(byte[] buffer, ref int offset)
	{
		int num = 0;
		int num2 = offset;
		offset = num2 + 1;
		int num3 = num | (int)buffer[num2];
		num2 = offset;
		offset = num2 + 1;
		int num4 = num3 | ((int)buffer[num2] << 8);
		num2 = offset;
		offset = num2 + 1;
		int num5 = num4 | ((int)buffer[num2] << 16);
		num2 = offset;
		offset = num2 + 1;
		return num5 | ((int)buffer[num2] << 24);
	}

	// Token: 0x0600A6B1 RID: 42673 RVA: 0x003E1054 File Offset: 0x003DF254
	public static void Write(Stream clientStream, int v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
		clientStream.WriteByte((byte)((v >> 16) & 255));
		clientStream.WriteByte((byte)((v >> 24) & 255));
	}

	// Token: 0x0600A6B2 RID: 42674 RVA: 0x003E10A4 File Offset: 0x003DF2A4
	public static void Write(byte[] buffer, int v, ref int offset)
	{
		int num = offset;
		offset = num + 1;
		buffer[num] = (byte)(v & 255);
		num = offset;
		offset = num + 1;
		buffer[num] = (byte)((v >> 8) & 255);
		num = offset;
		offset = num + 1;
		buffer[num] = (byte)((v >> 16) & 255);
		num = offset;
		offset = num + 1;
		buffer[num] = (byte)((v >> 24) & 255);
	}

	// Token: 0x0600A6B3 RID: 42675 RVA: 0x003E1105 File Offset: 0x003DF305
	public static ushort ReadUInt16(Stream clientStream)
	{
		return 0 | (ushort)(clientStream.ReadByte() & 255) | (ushort)((clientStream.ReadByte() & 255) << 8);
	}

	// Token: 0x0600A6B4 RID: 42676 RVA: 0x003E1128 File Offset: 0x003DF328
	public static ushort ReadUInt16(byte[] buffer, ref int offset)
	{
		byte b = 0;
		int num = offset;
		offset = num + 1;
		ushort num2 = (ushort)(b | buffer[num]);
		num = offset;
		offset = num + 1;
		return num2 | (ushort)(buffer[num] << 8);
	}

	// Token: 0x0600A6B5 RID: 42677 RVA: 0x003E1153 File Offset: 0x003DF353
	public static void Write(Stream clientStream, ushort v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
	}

	// Token: 0x0600A6B6 RID: 42678 RVA: 0x003E1173 File Offset: 0x003DF373
	public static short ReadInt16(Stream clientStream)
	{
		return 0 | (short)(clientStream.ReadByte() & 255) | (short)((clientStream.ReadByte() & 255) << 8);
	}

	// Token: 0x0600A6B7 RID: 42679 RVA: 0x003E1198 File Offset: 0x003DF398
	public static short ReadInt16(byte[] buffer, ref int offset)
	{
		byte b = 0;
		int num = offset;
		offset = num + 1;
		short num2 = (short)(b | buffer[num]);
		num = offset;
		offset = num + 1;
		return num2 | (short)(buffer[num] << 8);
	}

	// Token: 0x0600A6B8 RID: 42680 RVA: 0x003E1153 File Offset: 0x003DF353
	public static void Write(Stream clientStream, short v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
	}

	// Token: 0x0600A6B9 RID: 42681 RVA: 0x003E11C3 File Offset: 0x003DF3C3
	public static byte ReadByte(Stream clientStream)
	{
		return (byte)clientStream.ReadByte();
	}

	// Token: 0x0600A6BA RID: 42682 RVA: 0x003E11CC File Offset: 0x003DF3CC
	public static byte ReadByte(byte[] buffer, ref int offset)
	{
		int num = offset;
		offset = num + 1;
		return buffer[num];
	}

	// Token: 0x0600A6BB RID: 42683 RVA: 0x003E11E4 File Offset: 0x003DF3E4
	public static void Write(Stream clientStream, byte _b)
	{
		clientStream.WriteByte(_b);
	}

	// Token: 0x0600A6BC RID: 42684 RVA: 0x003E11ED File Offset: 0x003DF3ED
	public static void Write(BinaryWriter _bw, Vector3 _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
		_bw.Write(_v.z);
	}

	// Token: 0x0600A6BD RID: 42685 RVA: 0x003E1216 File Offset: 0x003DF416
	public static Vector3 ReadVector3(BinaryReader _br)
	{
		return new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A6BE RID: 42686 RVA: 0x003E122F File Offset: 0x003DF42F
	public static void Write(BinaryWriter _bw, Vector3i _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
		_bw.Write(_v.z);
	}

	// Token: 0x0600A6BF RID: 42687 RVA: 0x003E1255 File Offset: 0x003DF455
	public static Vector3i ReadVector3i(BinaryReader _br)
	{
		return new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
	}

	// Token: 0x0600A6C0 RID: 42688 RVA: 0x003E126E File Offset: 0x003DF46E
	public static void Write(BinaryWriter _bw, Vector2 _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
	}

	// Token: 0x0600A6C1 RID: 42689 RVA: 0x003E128A File Offset: 0x003DF48A
	public static Vector2 ReadVector2(BinaryReader _br)
	{
		return new Vector2(_br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A6C2 RID: 42690 RVA: 0x003E129D File Offset: 0x003DF49D
	public static void Write(BinaryWriter _bw, Vector2i _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
	}

	// Token: 0x0600A6C3 RID: 42691 RVA: 0x003E12B7 File Offset: 0x003DF4B7
	public static Vector2i ReadVector2i(BinaryReader _br)
	{
		return new Vector2i(_br.ReadInt32(), _br.ReadInt32());
	}

	// Token: 0x0600A6C4 RID: 42692 RVA: 0x003E12CA File Offset: 0x003DF4CA
	public static void Write(BinaryWriter _bw, Quaternion _q)
	{
		_bw.Write(_q.x);
		_bw.Write(_q.y);
		_bw.Write(_q.z);
		_bw.Write(_q.w);
	}

	// Token: 0x0600A6C5 RID: 42693 RVA: 0x003E1300 File Offset: 0x003DF500
	public static Quaternion ReadQuaterion(BinaryReader _br)
	{
		return new Quaternion(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A6C6 RID: 42694 RVA: 0x003E131F File Offset: 0x003DF51F
	public static Color ReadColor(BinaryReader _br)
	{
		return new Color(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A6C7 RID: 42695 RVA: 0x003E133E File Offset: 0x003DF53E
	public static void Write(BinaryWriter _bw, Color _c)
	{
		_bw.Write(_c.r);
		_bw.Write(_c.g);
		_bw.Write(_c.b);
		_bw.Write(_c.a);
	}

	// Token: 0x0600A6C8 RID: 42696 RVA: 0x003E1374 File Offset: 0x003DF574
	public static Color ReadColor32(BinaryReader _br)
	{
		uint num = _br.ReadUInt32();
		return new Color(((num >> 24) & 255U) / 255f, ((num >> 16) & 255U) / 255f, ((num >> 8) & 255U) / 255f, (num & 255U) / 255f);
	}

	// Token: 0x0600A6C9 RID: 42697 RVA: 0x003E13D4 File Offset: 0x003DF5D4
	public static void WriteColor32(BinaryWriter _bw, Color _c)
	{
		uint num = ((uint)(_c.r * 255f) << 24) | ((uint)(_c.g * 255f) << 16) | ((uint)(_c.b * 255f) << 8) | (uint)(_c.a * 255f);
		_bw.Write(num);
	}

	// Token: 0x0600A6CA RID: 42698 RVA: 0x003E1428 File Offset: 0x003DF628
	public static void Write(BinaryWriter _bw, string _s)
	{
		_bw.Write(_s != null);
		if (_s != null)
		{
			_bw.Write(_s);
		}
	}

	// Token: 0x0600A6CB RID: 42699 RVA: 0x003E143E File Offset: 0x003DF63E
	public static string ReadString(BinaryReader _br)
	{
		if (!_br.ReadBoolean())
		{
			return null;
		}
		return _br.ReadString();
	}

	// Token: 0x0600A6CC RID: 42700 RVA: 0x003E1450 File Offset: 0x003DF650
	public static void Write7BitEncodedSignedInt(Stream _stream, int _value)
	{
		long num = (long)_value;
		bool flag = num < 0L;
		num = Math.Abs(num);
		long num2 = (num >> 6) & 33554431L;
		byte b = (byte)(num & 63L);
		if (num2 != 0L)
		{
			b |= 128;
		}
		if (flag)
		{
			b |= 64;
		}
		StreamUtils.Write(_stream, b);
		for (num = num2; num != 0L; num = num2)
		{
			num2 = (num >> 7) & 16777215L;
			b = (byte)(num & 127L);
			if (num2 != 0L)
			{
				b |= 128;
			}
			StreamUtils.Write(_stream, b);
		}
	}

	// Token: 0x0600A6CD RID: 42701 RVA: 0x003E14C8 File Offset: 0x003DF6C8
	public static void Write7BitEncodedInt(Stream _stream, int _value)
	{
		do
		{
			int num = (_value >> 7) & 16777215;
			byte b = (byte)(_value & 127);
			if (num != 0)
			{
				b |= 128;
			}
			StreamUtils.Write(_stream, b);
			_value = num;
		}
		while (_value != 0);
	}

	// Token: 0x0600A6CE RID: 42702 RVA: 0x003E14FC File Offset: 0x003DF6FC
	public static int Read7BitEncodedSignedInt(Stream _stream)
	{
		int num = 0;
		int i = 0;
		byte b = (byte)(_stream.ReadByte() & 255);
		num |= (int)(b & 63);
		i += 6;
		bool flag = (b & 64) > 0;
		if ((b & 128) != 0)
		{
			while (i < 32)
			{
				b = (byte)(_stream.ReadByte() & 255);
				num |= (int)(b & 127) << i;
				i += 7;
				if ((b & 128) == 0)
				{
					if (!flag)
					{
						return num;
					}
					return -num;
				}
			}
			throw new FormatException("Illegal encoding for 7 bit encoded int");
		}
		if (!flag)
		{
			return num;
		}
		return -num;
	}

	// Token: 0x0600A6CF RID: 42703 RVA: 0x003E1580 File Offset: 0x003DF780
	public static int Read7BitEncodedInt(Stream _stream)
	{
		int num = 0;
		int i = 0;
		while (i < 35)
		{
			byte b = (byte)(_stream.ReadByte() & 255);
			num |= (int)(b & 127) << i;
			i += 7;
			if ((b & 128) == 0)
			{
				return num;
			}
		}
		throw new FormatException("Illegal encoding for 7 bit encoded int");
	}

	// Token: 0x0600A6D0 RID: 42704 RVA: 0x003E15CC File Offset: 0x003DF7CC
	public unsafe static Guid ReadGuid(BinaryReader _br)
	{
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		if (_br.Read(span) != 16)
		{
			throw new EndOfStreamException("Failed to read 16 bytes for Guid");
		}
		return new Guid(span);
	}

	// Token: 0x0600A6D1 RID: 42705 RVA: 0x003E1608 File Offset: 0x003DF808
	public unsafe static void Write(BinaryWriter _bw, Guid _guid)
	{
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		if (!_guid.TryWriteBytes(span))
		{
			throw new InvalidOperationException("Failed to write Guid to span");
		}
		_bw.Write(span);
	}

	// Token: 0x0600A6D2 RID: 42706 RVA: 0x003E1644 File Offset: 0x003DF844
	public static void StreamCopy(Stream _source, Stream _destination, byte[] _tempBuf = null, bool _bFlush = true)
	{
		byte[] array = _tempBuf ?? MemoryPools.poolByte.Alloc(4096);
		bool flag = true;
		while (flag)
		{
			int num = _source.Read(array, 0, array.Length);
			if (num > 0)
			{
				_destination.Write(array, 0, num);
			}
			else
			{
				if (_bFlush)
				{
					_destination.Flush();
				}
				flag = false;
			}
		}
		if (_tempBuf == null)
		{
			MemoryPools.poolByte.Free(array);
		}
	}

	// Token: 0x0600A6D3 RID: 42707 RVA: 0x003E16A4 File Offset: 0x003DF8A4
	public static void StreamCopy(Stream _source, Stream _destination, int _length, byte[] _tempBuf = null, bool _bFlush = true)
	{
		byte[] array = _tempBuf ?? MemoryPools.poolByte.Alloc(4096);
		bool flag = true;
		while (flag)
		{
			int num = _source.Read(array, 0, Math.Min(_length, array.Length));
			if (num > 0)
			{
				_destination.Write(array, 0, num);
				_length -= num;
			}
			else
			{
				if (_bFlush)
				{
					_destination.Flush();
				}
				flag = false;
			}
		}
		if (_tempBuf == null)
		{
			MemoryPools.poolByte.Free(array);
		}
	}

	// Token: 0x0600A6D4 RID: 42708 RVA: 0x003E1710 File Offset: 0x003DF910
	public static void WriteStreamToFile(Stream _source, string _fileName)
	{
		using (Stream stream = SdFile.Create(_fileName))
		{
			StreamUtils.StreamCopy(_source, stream, null, true);
		}
	}

	// Token: 0x0600A6D5 RID: 42709 RVA: 0x003E174C File Offset: 0x003DF94C
	public static void WriteStreamToFile(Stream _source, string _fileName, int _length)
	{
		using (Stream stream = SdFile.Create(_fileName))
		{
			StreamUtils.StreamCopy(_source, stream, _length, null, true);
		}
	}

	// Token: 0x0600A6D6 RID: 42710 RVA: 0x003E1788 File Offset: 0x003DF988
	public static MemoryStream ToBlob(Action<PooledBinaryWriter> _write)
	{
		MemoryStream memoryStream = new MemoryStream();
		using (PooledBinaryWriter pooledBinaryWriter = MemoryPools.poolBinaryWriter.AllocSync(false))
		{
			pooledBinaryWriter.SetBaseStream(memoryStream);
			_write(pooledBinaryWriter);
		}
		return memoryStream;
	}

	// Token: 0x0600A6D7 RID: 42711 RVA: 0x003E17D4 File Offset: 0x003DF9D4
	public static void FromBlob(MemoryStream _blob, Action<PooledBinaryReader> _read)
	{
		if (_blob == null || _blob.Length == 0L)
		{
			return;
		}
		ArraySegment<byte> blobData = StreamUtils.GetBlobData(_blob);
		using (MemoryStream memoryStream = new MemoryStream(blobData.Array, blobData.Offset, blobData.Count, false))
		{
			using (PooledBinaryReader pooledBinaryReader = MemoryPools.poolBinaryReader.AllocSync(false))
			{
				pooledBinaryReader.SetBaseStream(memoryStream);
				_read(pooledBinaryReader);
			}
		}
	}

	// Token: 0x0600A6D8 RID: 42712 RVA: 0x003E185C File Offset: 0x003DFA5C
	public static MemoryStream ReadBlob(PooledBinaryReader _br)
	{
		int num = _br.ReadInt32();
		if (num <= 0)
		{
			return new MemoryStream();
		}
		byte[] array = _br.ReadBytes(num);
		return new MemoryStream(array, 0, array.Length, true, true);
	}

	// Token: 0x0600A6D9 RID: 42713 RVA: 0x003E1890 File Offset: 0x003DFA90
	public static void WriteBlob(PooledBinaryWriter _bw, MemoryStream _blob)
	{
		ArraySegment<byte> blobData = StreamUtils.GetBlobData(_blob);
		_bw.Write(blobData.Count);
		_bw.Write(blobData.Array, blobData.Offset, blobData.Count);
	}

	// Token: 0x0600A6DA RID: 42714 RVA: 0x003E18CC File Offset: 0x003DFACC
	[PublicizedFrom(EAccessModifier.Private)]
	public static ArraySegment<byte> GetBlobData(MemoryStream _blob)
	{
		ArraySegment<byte> arraySegment;
		if (_blob.TryGetBuffer(out arraySegment))
		{
			return arraySegment;
		}
		return new ArraySegment<byte>(_blob.ToArray());
	}
}
