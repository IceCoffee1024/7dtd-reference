using System;
using System.IO;
using UnityEngine;

// Token: 0x020014AF RID: 5295
public static class StreamUtils
{
	// Token: 0x0600A5FF RID: 42495 RVA: 0x003DC1B8 File Offset: 0x003DA3B8
	public static long ReadInt64(Stream clientStream)
	{
		return (long)(clientStream.ReadByte() & 255) | ((long)(clientStream.ReadByte() & 255) << 8) | ((long)(clientStream.ReadByte() & 255) << 16) | ((long)(clientStream.ReadByte() & 255) << 24) | ((long)(clientStream.ReadByte() & 255) << 32) | ((long)(clientStream.ReadByte() & 255) << 40) | ((long)(clientStream.ReadByte() & 255) << 48) | ((long)(clientStream.ReadByte() & 255) << 56);
	}

	// Token: 0x0600A600 RID: 42496 RVA: 0x003DC248 File Offset: 0x003DA448
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

	// Token: 0x0600A601 RID: 42497 RVA: 0x003DC2E1 File Offset: 0x003DA4E1
	public static int ReadInt32(Stream clientStream)
	{
		return 0 | (clientStream.ReadByte() & 255) | ((clientStream.ReadByte() & 255) << 8) | ((clientStream.ReadByte() & 255) << 16) | ((clientStream.ReadByte() & 255) << 24);
	}

	// Token: 0x0600A602 RID: 42498 RVA: 0x003DC320 File Offset: 0x003DA520
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

	// Token: 0x0600A603 RID: 42499 RVA: 0x003DC368 File Offset: 0x003DA568
	public static void Write(Stream clientStream, int v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
		clientStream.WriteByte((byte)((v >> 16) & 255));
		clientStream.WriteByte((byte)((v >> 24) & 255));
	}

	// Token: 0x0600A604 RID: 42500 RVA: 0x003DC3B8 File Offset: 0x003DA5B8
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

	// Token: 0x0600A605 RID: 42501 RVA: 0x003DC419 File Offset: 0x003DA619
	public static ushort ReadUInt16(Stream clientStream)
	{
		return 0 | (ushort)(clientStream.ReadByte() & 255) | (ushort)((clientStream.ReadByte() & 255) << 8);
	}

	// Token: 0x0600A606 RID: 42502 RVA: 0x003DC43C File Offset: 0x003DA63C
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

	// Token: 0x0600A607 RID: 42503 RVA: 0x003DC467 File Offset: 0x003DA667
	public static void Write(Stream clientStream, ushort v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
	}

	// Token: 0x0600A608 RID: 42504 RVA: 0x003DC487 File Offset: 0x003DA687
	public static short ReadInt16(Stream clientStream)
	{
		return 0 | (short)(clientStream.ReadByte() & 255) | (short)((clientStream.ReadByte() & 255) << 8);
	}

	// Token: 0x0600A609 RID: 42505 RVA: 0x003DC4AC File Offset: 0x003DA6AC
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

	// Token: 0x0600A60A RID: 42506 RVA: 0x003DC467 File Offset: 0x003DA667
	public static void Write(Stream clientStream, short v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
	}

	// Token: 0x0600A60B RID: 42507 RVA: 0x003DC4D7 File Offset: 0x003DA6D7
	public static byte ReadByte(Stream clientStream)
	{
		return (byte)clientStream.ReadByte();
	}

	// Token: 0x0600A60C RID: 42508 RVA: 0x003DC4E0 File Offset: 0x003DA6E0
	public static byte ReadByte(byte[] buffer, ref int offset)
	{
		int num = offset;
		offset = num + 1;
		return buffer[num];
	}

	// Token: 0x0600A60D RID: 42509 RVA: 0x003DC4F8 File Offset: 0x003DA6F8
	public static void Write(Stream clientStream, byte _b)
	{
		clientStream.WriteByte(_b);
	}

	// Token: 0x0600A60E RID: 42510 RVA: 0x003DC501 File Offset: 0x003DA701
	public static void Write(BinaryWriter _bw, Vector3 _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
		_bw.Write(_v.z);
	}

	// Token: 0x0600A60F RID: 42511 RVA: 0x003DC52A File Offset: 0x003DA72A
	public static Vector3 ReadVector3(BinaryReader _br)
	{
		return new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A610 RID: 42512 RVA: 0x003DC543 File Offset: 0x003DA743
	public static void Write(BinaryWriter _bw, Vector3i _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
		_bw.Write(_v.z);
	}

	// Token: 0x0600A611 RID: 42513 RVA: 0x003DC569 File Offset: 0x003DA769
	public static Vector3i ReadVector3i(BinaryReader _br)
	{
		return new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
	}

	// Token: 0x0600A612 RID: 42514 RVA: 0x003DC582 File Offset: 0x003DA782
	public static void Write(BinaryWriter _bw, Vector2 _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
	}

	// Token: 0x0600A613 RID: 42515 RVA: 0x003DC59E File Offset: 0x003DA79E
	public static Vector2 ReadVector2(BinaryReader _br)
	{
		return new Vector2(_br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A614 RID: 42516 RVA: 0x003DC5B1 File Offset: 0x003DA7B1
	public static void Write(BinaryWriter _bw, Vector2i _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
	}

	// Token: 0x0600A615 RID: 42517 RVA: 0x003DC5CB File Offset: 0x003DA7CB
	public static Vector2i ReadVector2i(BinaryReader _br)
	{
		return new Vector2i(_br.ReadInt32(), _br.ReadInt32());
	}

	// Token: 0x0600A616 RID: 42518 RVA: 0x003DC5DE File Offset: 0x003DA7DE
	public static void Write(BinaryWriter _bw, Quaternion _q)
	{
		_bw.Write(_q.x);
		_bw.Write(_q.y);
		_bw.Write(_q.z);
		_bw.Write(_q.w);
	}

	// Token: 0x0600A617 RID: 42519 RVA: 0x003DC614 File Offset: 0x003DA814
	public static Quaternion ReadQuaterion(BinaryReader _br)
	{
		return new Quaternion(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A618 RID: 42520 RVA: 0x003DC633 File Offset: 0x003DA833
	public static Color ReadColor(BinaryReader _br)
	{
		return new Color(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A619 RID: 42521 RVA: 0x003DC652 File Offset: 0x003DA852
	public static void Write(BinaryWriter _bw, Color _c)
	{
		_bw.Write(_c.r);
		_bw.Write(_c.g);
		_bw.Write(_c.b);
		_bw.Write(_c.a);
	}

	// Token: 0x0600A61A RID: 42522 RVA: 0x003DC688 File Offset: 0x003DA888
	public static Color ReadColor32(BinaryReader _br)
	{
		uint num = _br.ReadUInt32();
		return new Color(((num >> 24) & 255U) / 255f, ((num >> 16) & 255U) / 255f, ((num >> 8) & 255U) / 255f, (num & 255U) / 255f);
	}

	// Token: 0x0600A61B RID: 42523 RVA: 0x003DC6E8 File Offset: 0x003DA8E8
	public static void WriteColor32(BinaryWriter _bw, Color _c)
	{
		uint num = ((uint)(_c.r * 255f) << 24) | ((uint)(_c.g * 255f) << 16) | ((uint)(_c.b * 255f) << 8) | (uint)(_c.a * 255f);
		_bw.Write(num);
	}

	// Token: 0x0600A61C RID: 42524 RVA: 0x003DC73C File Offset: 0x003DA93C
	public static void Write(BinaryWriter _bw, string _s)
	{
		_bw.Write(_s != null);
		if (_s != null)
		{
			_bw.Write(_s);
		}
	}

	// Token: 0x0600A61D RID: 42525 RVA: 0x003DC752 File Offset: 0x003DA952
	public static string ReadString(BinaryReader _br)
	{
		if (!_br.ReadBoolean())
		{
			return null;
		}
		return _br.ReadString();
	}

	// Token: 0x0600A61E RID: 42526 RVA: 0x003DC764 File Offset: 0x003DA964
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

	// Token: 0x0600A61F RID: 42527 RVA: 0x003DC7DC File Offset: 0x003DA9DC
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

	// Token: 0x0600A620 RID: 42528 RVA: 0x003DC810 File Offset: 0x003DAA10
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

	// Token: 0x0600A621 RID: 42529 RVA: 0x003DC894 File Offset: 0x003DAA94
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

	// Token: 0x0600A622 RID: 42530 RVA: 0x003DC8E0 File Offset: 0x003DAAE0
	public unsafe static Guid ReadGuid(BinaryReader _br)
	{
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		if (_br.Read(span) != 16)
		{
			throw new EndOfStreamException("Failed to read 16 bytes for Guid");
		}
		return new Guid(span);
	}

	// Token: 0x0600A623 RID: 42531 RVA: 0x003DC91C File Offset: 0x003DAB1C
	public unsafe static void Write(BinaryWriter _bw, Guid _guid)
	{
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		if (!_guid.TryWriteBytes(span))
		{
			throw new InvalidOperationException("Failed to write Guid to span");
		}
		_bw.Write(span);
	}

	// Token: 0x0600A624 RID: 42532 RVA: 0x003DC958 File Offset: 0x003DAB58
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

	// Token: 0x0600A625 RID: 42533 RVA: 0x003DC9B8 File Offset: 0x003DABB8
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

	// Token: 0x0600A626 RID: 42534 RVA: 0x003DCA24 File Offset: 0x003DAC24
	public static void WriteStreamToFile(Stream _source, string _fileName)
	{
		using (Stream stream = SdFile.Create(_fileName))
		{
			StreamUtils.StreamCopy(_source, stream, null, true);
		}
	}

	// Token: 0x0600A627 RID: 42535 RVA: 0x003DCA60 File Offset: 0x003DAC60
	public static void WriteStreamToFile(Stream _source, string _fileName, int _length)
	{
		using (Stream stream = SdFile.Create(_fileName))
		{
			StreamUtils.StreamCopy(_source, stream, _length, null, true);
		}
	}
}
