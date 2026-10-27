using System;
using System.IO;
using UnityEngine;

// Token: 0x020014E3 RID: 5347
public static class StreamUtils
{
	// Token: 0x0600A705 RID: 42757 RVA: 0x003E32A8 File Offset: 0x003E14A8
	public static long ReadInt64(Stream clientStream)
	{
		return (long)(clientStream.ReadByte() & 255) | ((long)(clientStream.ReadByte() & 255) << 8) | ((long)(clientStream.ReadByte() & 255) << 16) | ((long)(clientStream.ReadByte() & 255) << 24) | ((long)(clientStream.ReadByte() & 255) << 32) | ((long)(clientStream.ReadByte() & 255) << 40) | ((long)(clientStream.ReadByte() & 255) << 48) | ((long)(clientStream.ReadByte() & 255) << 56);
	}

	// Token: 0x0600A706 RID: 42758 RVA: 0x003E3338 File Offset: 0x003E1538
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

	// Token: 0x0600A707 RID: 42759 RVA: 0x003E33D1 File Offset: 0x003E15D1
	public static int ReadInt32(Stream clientStream)
	{
		return 0 | (clientStream.ReadByte() & 255) | ((clientStream.ReadByte() & 255) << 8) | ((clientStream.ReadByte() & 255) << 16) | ((clientStream.ReadByte() & 255) << 24);
	}

	// Token: 0x0600A708 RID: 42760 RVA: 0x003E3410 File Offset: 0x003E1610
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

	// Token: 0x0600A709 RID: 42761 RVA: 0x003E3458 File Offset: 0x003E1658
	public static void Write(Stream clientStream, int v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
		clientStream.WriteByte((byte)((v >> 16) & 255));
		clientStream.WriteByte((byte)((v >> 24) & 255));
	}

	// Token: 0x0600A70A RID: 42762 RVA: 0x003E34A8 File Offset: 0x003E16A8
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

	// Token: 0x0600A70B RID: 42763 RVA: 0x003E3509 File Offset: 0x003E1709
	public static ushort ReadUInt16(Stream clientStream)
	{
		return 0 | (ushort)(clientStream.ReadByte() & 255) | (ushort)((clientStream.ReadByte() & 255) << 8);
	}

	// Token: 0x0600A70C RID: 42764 RVA: 0x003E352C File Offset: 0x003E172C
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

	// Token: 0x0600A70D RID: 42765 RVA: 0x003E3557 File Offset: 0x003E1757
	public static void Write(Stream clientStream, ushort v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
	}

	// Token: 0x0600A70E RID: 42766 RVA: 0x003E3577 File Offset: 0x003E1777
	public static short ReadInt16(Stream clientStream)
	{
		return 0 | (short)(clientStream.ReadByte() & 255) | (short)((clientStream.ReadByte() & 255) << 8);
	}

	// Token: 0x0600A70F RID: 42767 RVA: 0x003E359C File Offset: 0x003E179C
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

	// Token: 0x0600A710 RID: 42768 RVA: 0x003E3557 File Offset: 0x003E1757
	public static void Write(Stream clientStream, short v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
	}

	// Token: 0x0600A711 RID: 42769 RVA: 0x003E35C7 File Offset: 0x003E17C7
	public static byte ReadByte(Stream clientStream)
	{
		return (byte)clientStream.ReadByte();
	}

	// Token: 0x0600A712 RID: 42770 RVA: 0x003E35D0 File Offset: 0x003E17D0
	public static byte ReadByte(byte[] buffer, ref int offset)
	{
		int num = offset;
		offset = num + 1;
		return buffer[num];
	}

	// Token: 0x0600A713 RID: 42771 RVA: 0x003E35E8 File Offset: 0x003E17E8
	public static void Write(Stream clientStream, byte _b)
	{
		clientStream.WriteByte(_b);
	}

	// Token: 0x0600A714 RID: 42772 RVA: 0x003E35F1 File Offset: 0x003E17F1
	public static void Write(BinaryWriter _bw, Vector3 _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
		_bw.Write(_v.z);
	}

	// Token: 0x0600A715 RID: 42773 RVA: 0x003E361A File Offset: 0x003E181A
	public static Vector3 ReadVector3(BinaryReader _br)
	{
		return new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A716 RID: 42774 RVA: 0x003E3633 File Offset: 0x003E1833
	public static void Write(BinaryWriter _bw, Vector3i _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
		_bw.Write(_v.z);
	}

	// Token: 0x0600A717 RID: 42775 RVA: 0x003E3659 File Offset: 0x003E1859
	public static Vector3i ReadVector3i(BinaryReader _br)
	{
		return new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
	}

	// Token: 0x0600A718 RID: 42776 RVA: 0x003E3672 File Offset: 0x003E1872
	public static void Write(BinaryWriter _bw, Vector2 _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
	}

	// Token: 0x0600A719 RID: 42777 RVA: 0x003E368E File Offset: 0x003E188E
	public static Vector2 ReadVector2(BinaryReader _br)
	{
		return new Vector2(_br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A71A RID: 42778 RVA: 0x003E36A1 File Offset: 0x003E18A1
	public static void Write(BinaryWriter _bw, Vector2i _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
	}

	// Token: 0x0600A71B RID: 42779 RVA: 0x003E36BB File Offset: 0x003E18BB
	public static Vector2i ReadVector2i(BinaryReader _br)
	{
		return new Vector2i(_br.ReadInt32(), _br.ReadInt32());
	}

	// Token: 0x0600A71C RID: 42780 RVA: 0x003E36CE File Offset: 0x003E18CE
	public static void Write(BinaryWriter _bw, Quaternion _q)
	{
		_bw.Write(_q.x);
		_bw.Write(_q.y);
		_bw.Write(_q.z);
		_bw.Write(_q.w);
	}

	// Token: 0x0600A71D RID: 42781 RVA: 0x003E3704 File Offset: 0x003E1904
	public static Quaternion ReadQuaterion(BinaryReader _br)
	{
		return new Quaternion(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A71E RID: 42782 RVA: 0x003E3723 File Offset: 0x003E1923
	public static Color ReadColor(BinaryReader _br)
	{
		return new Color(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A71F RID: 42783 RVA: 0x003E3742 File Offset: 0x003E1942
	public static void Write(BinaryWriter _bw, Color _c)
	{
		_bw.Write(_c.r);
		_bw.Write(_c.g);
		_bw.Write(_c.b);
		_bw.Write(_c.a);
	}

	// Token: 0x0600A720 RID: 42784 RVA: 0x003E3778 File Offset: 0x003E1978
	public static Color ReadColor32(BinaryReader _br)
	{
		uint num = _br.ReadUInt32();
		return new Color(((num >> 24) & 255U) / 255f, ((num >> 16) & 255U) / 255f, ((num >> 8) & 255U) / 255f, (num & 255U) / 255f);
	}

	// Token: 0x0600A721 RID: 42785 RVA: 0x003E37D8 File Offset: 0x003E19D8
	public static void WriteColor32(BinaryWriter _bw, Color _c)
	{
		uint num = ((uint)(_c.r * 255f) << 24) | ((uint)(_c.g * 255f) << 16) | ((uint)(_c.b * 255f) << 8) | (uint)(_c.a * 255f);
		_bw.Write(num);
	}

	// Token: 0x0600A722 RID: 42786 RVA: 0x003E382C File Offset: 0x003E1A2C
	public static void Write(BinaryWriter _bw, string _s)
	{
		_bw.Write(_s != null);
		if (_s != null)
		{
			_bw.Write(_s);
		}
	}

	// Token: 0x0600A723 RID: 42787 RVA: 0x003E3842 File Offset: 0x003E1A42
	public static string ReadString(BinaryReader _br)
	{
		if (!_br.ReadBoolean())
		{
			return null;
		}
		return _br.ReadString();
	}

	// Token: 0x0600A724 RID: 42788 RVA: 0x003E3854 File Offset: 0x003E1A54
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

	// Token: 0x0600A725 RID: 42789 RVA: 0x003E38CC File Offset: 0x003E1ACC
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

	// Token: 0x0600A726 RID: 42790 RVA: 0x003E3900 File Offset: 0x003E1B00
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

	// Token: 0x0600A727 RID: 42791 RVA: 0x003E3984 File Offset: 0x003E1B84
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

	// Token: 0x0600A728 RID: 42792 RVA: 0x003E39D0 File Offset: 0x003E1BD0
	public unsafe static Guid ReadGuid(BinaryReader _br)
	{
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		if (_br.Read(span) != 16)
		{
			throw new EndOfStreamException("Failed to read 16 bytes for Guid");
		}
		return new Guid(span);
	}

	// Token: 0x0600A729 RID: 42793 RVA: 0x003E3A0C File Offset: 0x003E1C0C
	public unsafe static void Write(BinaryWriter _bw, Guid _guid)
	{
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		if (!_guid.TryWriteBytes(span))
		{
			throw new InvalidOperationException("Failed to write Guid to span");
		}
		_bw.Write(span);
	}

	// Token: 0x0600A72A RID: 42794 RVA: 0x003E3A48 File Offset: 0x003E1C48
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

	// Token: 0x0600A72B RID: 42795 RVA: 0x003E3AA8 File Offset: 0x003E1CA8
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

	// Token: 0x0600A72C RID: 42796 RVA: 0x003E3B14 File Offset: 0x003E1D14
	public static void WriteStreamToFile(Stream _source, string _fileName)
	{
		using (Stream stream = SdFile.Create(_fileName))
		{
			StreamUtils.StreamCopy(_source, stream, null, true);
		}
	}

	// Token: 0x0600A72D RID: 42797 RVA: 0x003E3B50 File Offset: 0x003E1D50
	public static void WriteStreamToFile(Stream _source, string _fileName, int _length)
	{
		using (Stream stream = SdFile.Create(_fileName))
		{
			StreamUtils.StreamCopy(_source, stream, _length, null, true);
		}
	}

	// Token: 0x0600A72E RID: 42798 RVA: 0x003E3B8C File Offset: 0x003E1D8C
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

	// Token: 0x0600A72F RID: 42799 RVA: 0x003E3BD8 File Offset: 0x003E1DD8
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

	// Token: 0x0600A730 RID: 42800 RVA: 0x003E3C60 File Offset: 0x003E1E60
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

	// Token: 0x0600A731 RID: 42801 RVA: 0x003E3C94 File Offset: 0x003E1E94
	public static void WriteBlob(PooledBinaryWriter _bw, MemoryStream _blob)
	{
		ArraySegment<byte> blobData = StreamUtils.GetBlobData(_blob);
		_bw.Write(blobData.Count);
		_bw.Write(blobData.Array, blobData.Offset, blobData.Count);
	}

	// Token: 0x0600A732 RID: 42802 RVA: 0x003E3CD0 File Offset: 0x003E1ED0
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
