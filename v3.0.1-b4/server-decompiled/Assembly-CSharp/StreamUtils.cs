using System;
using System.IO;
using UnityEngine;

// Token: 0x02001494 RID: 5268
public static class StreamUtils
{
	// Token: 0x0600A502 RID: 42242 RVA: 0x003D6E78 File Offset: 0x003D5078
	public static long ReadInt64(Stream clientStream)
	{
		return (long)(clientStream.ReadByte() & 255) | ((long)(clientStream.ReadByte() & 255) << 8) | ((long)(clientStream.ReadByte() & 255) << 16) | ((long)(clientStream.ReadByte() & 255) << 24) | ((long)(clientStream.ReadByte() & 255) << 32) | ((long)(clientStream.ReadByte() & 255) << 40) | ((long)(clientStream.ReadByte() & 255) << 48) | ((long)(clientStream.ReadByte() & 255) << 56);
	}

	// Token: 0x0600A503 RID: 42243 RVA: 0x003D6F08 File Offset: 0x003D5108
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

	// Token: 0x0600A504 RID: 42244 RVA: 0x003D6FA1 File Offset: 0x003D51A1
	public static int ReadInt32(Stream clientStream)
	{
		return 0 | (clientStream.ReadByte() & 255) | ((clientStream.ReadByte() & 255) << 8) | ((clientStream.ReadByte() & 255) << 16) | ((clientStream.ReadByte() & 255) << 24);
	}

	// Token: 0x0600A505 RID: 42245 RVA: 0x003D6FE0 File Offset: 0x003D51E0
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

	// Token: 0x0600A506 RID: 42246 RVA: 0x003D7028 File Offset: 0x003D5228
	public static void Write(Stream clientStream, int v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
		clientStream.WriteByte((byte)((v >> 16) & 255));
		clientStream.WriteByte((byte)((v >> 24) & 255));
	}

	// Token: 0x0600A507 RID: 42247 RVA: 0x003D7078 File Offset: 0x003D5278
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

	// Token: 0x0600A508 RID: 42248 RVA: 0x003D70D9 File Offset: 0x003D52D9
	public static ushort ReadUInt16(Stream clientStream)
	{
		return 0 | (ushort)(clientStream.ReadByte() & 255) | (ushort)((clientStream.ReadByte() & 255) << 8);
	}

	// Token: 0x0600A509 RID: 42249 RVA: 0x003D70FC File Offset: 0x003D52FC
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

	// Token: 0x0600A50A RID: 42250 RVA: 0x003D7127 File Offset: 0x003D5327
	public static void Write(Stream clientStream, ushort v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
	}

	// Token: 0x0600A50B RID: 42251 RVA: 0x003D7147 File Offset: 0x003D5347
	public static short ReadInt16(Stream clientStream)
	{
		return 0 | (short)(clientStream.ReadByte() & 255) | (short)((clientStream.ReadByte() & 255) << 8);
	}

	// Token: 0x0600A50C RID: 42252 RVA: 0x003D716C File Offset: 0x003D536C
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

	// Token: 0x0600A50D RID: 42253 RVA: 0x003D7127 File Offset: 0x003D5327
	public static void Write(Stream clientStream, short v)
	{
		clientStream.WriteByte((byte)(v & 255));
		clientStream.WriteByte((byte)((v >> 8) & 255));
	}

	// Token: 0x0600A50E RID: 42254 RVA: 0x003D7197 File Offset: 0x003D5397
	public static byte ReadByte(Stream clientStream)
	{
		return (byte)clientStream.ReadByte();
	}

	// Token: 0x0600A50F RID: 42255 RVA: 0x003D71A0 File Offset: 0x003D53A0
	public static byte ReadByte(byte[] buffer, ref int offset)
	{
		int num = offset;
		offset = num + 1;
		return buffer[num];
	}

	// Token: 0x0600A510 RID: 42256 RVA: 0x003D71B8 File Offset: 0x003D53B8
	public static void Write(Stream clientStream, byte _b)
	{
		clientStream.WriteByte(_b);
	}

	// Token: 0x0600A511 RID: 42257 RVA: 0x003D71C1 File Offset: 0x003D53C1
	public static void Write(BinaryWriter _bw, Vector3 _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
		_bw.Write(_v.z);
	}

	// Token: 0x0600A512 RID: 42258 RVA: 0x003D71EA File Offset: 0x003D53EA
	public static Vector3 ReadVector3(BinaryReader _br)
	{
		return new Vector3(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A513 RID: 42259 RVA: 0x003D7203 File Offset: 0x003D5403
	public static void Write(BinaryWriter _bw, Vector3i _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
		_bw.Write(_v.z);
	}

	// Token: 0x0600A514 RID: 42260 RVA: 0x003D7229 File Offset: 0x003D5429
	public static Vector3i ReadVector3i(BinaryReader _br)
	{
		return new Vector3i(_br.ReadInt32(), _br.ReadInt32(), _br.ReadInt32());
	}

	// Token: 0x0600A515 RID: 42261 RVA: 0x003D7242 File Offset: 0x003D5442
	public static void Write(BinaryWriter _bw, Vector2 _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
	}

	// Token: 0x0600A516 RID: 42262 RVA: 0x003D725E File Offset: 0x003D545E
	public static Vector2 ReadVector2(BinaryReader _br)
	{
		return new Vector2(_br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A517 RID: 42263 RVA: 0x003D7271 File Offset: 0x003D5471
	public static void Write(BinaryWriter _bw, Vector2i _v)
	{
		_bw.Write(_v.x);
		_bw.Write(_v.y);
	}

	// Token: 0x0600A518 RID: 42264 RVA: 0x003D728B File Offset: 0x003D548B
	public static Vector2i ReadVector2i(BinaryReader _br)
	{
		return new Vector2i(_br.ReadInt32(), _br.ReadInt32());
	}

	// Token: 0x0600A519 RID: 42265 RVA: 0x003D729E File Offset: 0x003D549E
	public static void Write(BinaryWriter _bw, Quaternion _q)
	{
		_bw.Write(_q.x);
		_bw.Write(_q.y);
		_bw.Write(_q.z);
		_bw.Write(_q.w);
	}

	// Token: 0x0600A51A RID: 42266 RVA: 0x003D72D4 File Offset: 0x003D54D4
	public static Quaternion ReadQuaterion(BinaryReader _br)
	{
		return new Quaternion(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A51B RID: 42267 RVA: 0x003D72F3 File Offset: 0x003D54F3
	public static Color ReadColor(BinaryReader _br)
	{
		return new Color(_br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle(), _br.ReadSingle());
	}

	// Token: 0x0600A51C RID: 42268 RVA: 0x003D7312 File Offset: 0x003D5512
	public static void Write(BinaryWriter _bw, Color _c)
	{
		_bw.Write(_c.r);
		_bw.Write(_c.g);
		_bw.Write(_c.b);
		_bw.Write(_c.a);
	}

	// Token: 0x0600A51D RID: 42269 RVA: 0x003D7348 File Offset: 0x003D5548
	public static Color ReadColor32(BinaryReader _br)
	{
		uint num = _br.ReadUInt32();
		return new Color(((num >> 24) & 255U) / 255f, ((num >> 16) & 255U) / 255f, ((num >> 8) & 255U) / 255f, (num & 255U) / 255f);
	}

	// Token: 0x0600A51E RID: 42270 RVA: 0x003D73A8 File Offset: 0x003D55A8
	public static void WriteColor32(BinaryWriter _bw, Color _c)
	{
		uint num = ((uint)(_c.r * 255f) << 24) | ((uint)(_c.g * 255f) << 16) | ((uint)(_c.b * 255f) << 8) | (uint)(_c.a * 255f);
		_bw.Write(num);
	}

	// Token: 0x0600A51F RID: 42271 RVA: 0x003D73FC File Offset: 0x003D55FC
	public static void Write(BinaryWriter _bw, string _s)
	{
		_bw.Write(_s != null);
		if (_s != null)
		{
			_bw.Write(_s);
		}
	}

	// Token: 0x0600A520 RID: 42272 RVA: 0x003D7412 File Offset: 0x003D5612
	public static string ReadString(BinaryReader _br)
	{
		if (!_br.ReadBoolean())
		{
			return null;
		}
		return _br.ReadString();
	}

	// Token: 0x0600A521 RID: 42273 RVA: 0x003D7424 File Offset: 0x003D5624
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

	// Token: 0x0600A522 RID: 42274 RVA: 0x003D749C File Offset: 0x003D569C
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

	// Token: 0x0600A523 RID: 42275 RVA: 0x003D74D0 File Offset: 0x003D56D0
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

	// Token: 0x0600A524 RID: 42276 RVA: 0x003D7554 File Offset: 0x003D5754
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

	// Token: 0x0600A525 RID: 42277 RVA: 0x003D75A0 File Offset: 0x003D57A0
	public unsafe static Guid ReadGuid(BinaryReader _br)
	{
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		if (_br.Read(span) != 16)
		{
			throw new EndOfStreamException("Failed to read 16 bytes for Guid");
		}
		return new Guid(span);
	}

	// Token: 0x0600A526 RID: 42278 RVA: 0x003D75DC File Offset: 0x003D57DC
	public unsafe static void Write(BinaryWriter _bw, Guid _guid)
	{
		Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
		if (!_guid.TryWriteBytes(span))
		{
			throw new InvalidOperationException("Failed to write Guid to span");
		}
		_bw.Write(span);
	}

	// Token: 0x0600A527 RID: 42279 RVA: 0x003D7618 File Offset: 0x003D5818
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

	// Token: 0x0600A528 RID: 42280 RVA: 0x003D7678 File Offset: 0x003D5878
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

	// Token: 0x0600A529 RID: 42281 RVA: 0x003D76E4 File Offset: 0x003D58E4
	public static void WriteStreamToFile(Stream _source, string _fileName)
	{
		using (Stream stream = SdFile.Create(_fileName))
		{
			StreamUtils.StreamCopy(_source, stream, null, true);
		}
	}

	// Token: 0x0600A52A RID: 42282 RVA: 0x003D7720 File Offset: 0x003D5920
	public static void WriteStreamToFile(Stream _source, string _fileName, int _length)
	{
		using (Stream stream = SdFile.Create(_fileName))
		{
			StreamUtils.StreamCopy(_source, stream, _length, null, true);
		}
	}
}
