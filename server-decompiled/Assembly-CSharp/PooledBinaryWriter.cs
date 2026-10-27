using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x020014B1 RID: 5297
public class PooledBinaryWriter : BinaryWriter, IBinaryReaderOrWriter, IMemoryPoolableObject, IDisposable
{
	// Token: 0x170013AC RID: 5036
	// (get) Token: 0x0600A5C3 RID: 42435 RVA: 0x003DD18B File Offset: 0x003DB38B
	// (set) Token: 0x0600A5C4 RID: 42436 RVA: 0x003DD193 File Offset: 0x003DB393
	public Encoding Encoding
	{
		get
		{
			return this.encoding;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.encoding = value;
			this.encoder = null;
			this.maxBytesPerChar = this.encoding.GetMaxByteCount(1);
		}
	}

	// Token: 0x0600A5C5 RID: 42437 RVA: 0x003DD1C4 File Offset: 0x003DB3C4
	public PooledBinaryWriter()
	{
		this.Encoding = new UTF8Encoding(false, false);
		Interlocked.Increment(ref PooledBinaryWriter.INSTANCES_CREATED);
		Interlocked.Increment(ref PooledBinaryWriter.INSTANCES_LIVE);
		if (PooledBinaryWriter.INSTANCES_LIVE > PooledBinaryWriter.INSTANCES_MAX)
		{
			Interlocked.Exchange(ref PooledBinaryWriter.INSTANCES_MAX, PooledBinaryWriter.INSTANCES_LIVE);
		}
	}

	// Token: 0x0600A5C6 RID: 42438 RVA: 0x003DD238 File Offset: 0x003DB438
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledBinaryWriter()
	{
		Interlocked.Decrement(ref PooledBinaryWriter.INSTANCES_LIVE);
	}

	// Token: 0x0600A5C7 RID: 42439 RVA: 0x003DD26C File Offset: 0x003DB46C
	public void SetBaseStream(Stream _stream)
	{
		if (_stream != null && !_stream.CanWrite)
		{
			throw new ArgumentException("Stream does not support writing or already closed.");
		}
		this.OutStream = _stream;
		this.encoder = null;
	}

	// Token: 0x170013AD RID: 5037
	// (get) Token: 0x0600A5C8 RID: 42440 RVA: 0x003DD292 File Offset: 0x003DB492
	public override Stream BaseStream
	{
		get
		{
			return this.OutStream;
		}
	}

	// Token: 0x0600A5C9 RID: 42441 RVA: 0x003DD29A File Offset: 0x003DB49A
	public override void Flush()
	{
		this.OutStream.Flush();
	}

	// Token: 0x0600A5CA RID: 42442 RVA: 0x003DD2A7 File Offset: 0x003DB4A7
	public override long Seek(int _offset, SeekOrigin _origin)
	{
		return this.OutStream.Seek((long)_offset, _origin);
	}

	// Token: 0x0600A5CB RID: 42443 RVA: 0x003DD2B7 File Offset: 0x003DB4B7
	public override void Write(bool _value)
	{
		this.buffer[0] = ((!_value) ? 0 : 1);
		this.OutStream.Write(this.buffer, 0, 1);
	}

	// Token: 0x0600A5CC RID: 42444 RVA: 0x003DD2DC File Offset: 0x003DB4DC
	public override void Write(byte _value)
	{
		this.OutStream.WriteByte(_value);
	}

	// Token: 0x0600A5CD RID: 42445 RVA: 0x003DD2EA File Offset: 0x003DB4EA
	public override void Write(byte[] _buffer)
	{
		if (_buffer == null)
		{
			throw new ArgumentNullException("_buffer");
		}
		this.OutStream.Write(_buffer, 0, _buffer.Length);
	}

	// Token: 0x0600A5CE RID: 42446 RVA: 0x003DD30A File Offset: 0x003DB50A
	public override void Write(byte[] _buffer, int _index, int _count)
	{
		if (_buffer == null)
		{
			throw new ArgumentNullException("_buffer");
		}
		this.OutStream.Write(_buffer, _index, _count);
	}

	// Token: 0x0600A5CF RID: 42447 RVA: 0x003DD328 File Offset: 0x003DB528
	public override void Write(ReadOnlySpan<byte> _buffer)
	{
		this.OutStream.Write(_buffer);
	}

	// Token: 0x0600A5D0 RID: 42448 RVA: 0x003DD338 File Offset: 0x003DB538
	public override void Write(char _ch)
	{
		this.charBuffer[0] = _ch;
		int bytes = this.encoding.GetBytes(this.charBuffer, 0, 1, this.buffer, 0);
		this.OutStream.Write(this.buffer, 0, bytes);
	}

	// Token: 0x0600A5D1 RID: 42449 RVA: 0x003DD37C File Offset: 0x003DB57C
	public override void Write(char[] _chars)
	{
		this.Write(_chars, 0, _chars.Length);
	}

	// Token: 0x0600A5D2 RID: 42450 RVA: 0x003DD389 File Offset: 0x003DB589
	public override void Write(char[] _chars, int _index, int _count)
	{
		if (_chars == null)
		{
			throw new ArgumentNullException("_chars");
		}
		this.Write(_chars.AsSpan<char>(_index, _count));
	}

	// Token: 0x0600A5D3 RID: 42451 RVA: 0x003DD3AC File Offset: 0x003DB5AC
	public override void Write(ReadOnlySpan<char> _chars)
	{
		int num;
		for (int i = 0; i < _chars.Length; i += num)
		{
			num = Math.Min(128 / this.maxBytesPerChar, _chars.Length - i);
			int bytes = this.encoding.GetBytes(_chars.Slice(i, num), this.buffer);
			this.OutStream.Write(this.buffer, 0, bytes);
		}
	}

	// Token: 0x0600A5D4 RID: 42452 RVA: 0x003DD41C File Offset: 0x003DB61C
	public unsafe override void Write(decimal _value)
	{
		byte* ptr = (byte*)(&_value);
		if (BitConverter.IsLittleEndian)
		{
			for (int i = 0; i < 16; i++)
			{
				if (i < 4)
				{
					this.buffer[i + 12] = ptr[i];
				}
				else if (i < 8)
				{
					this.buffer[i + 4] = ptr[i];
				}
				else if (i < 12)
				{
					this.buffer[i - 8] = ptr[i];
				}
				else
				{
					this.buffer[i - 8] = ptr[i];
				}
			}
		}
		else
		{
			for (int j = 0; j < 16; j++)
			{
				if (j < 4)
				{
					this.buffer[15 - j] = ptr[j];
				}
				else if (j < 8)
				{
					this.buffer[15 - j] = ptr[j];
				}
				else if (j < 12)
				{
					this.buffer[11 - j] = ptr[j];
				}
				else
				{
					this.buffer[19 - j] = ptr[j];
				}
			}
		}
		this.OutStream.Write(this.buffer, 0, 16);
	}

	// Token: 0x0600A5D5 RID: 42453 RVA: 0x003DD4FF File Offset: 0x003DB6FF
	public override void Write(double _value)
	{
		BitConverterLE.GetBytes(_value, this.buffer);
		this.OutStream.Write(this.buffer, 0, 8);
	}

	// Token: 0x0600A5D6 RID: 42454 RVA: 0x003DD520 File Offset: 0x003DB720
	public override void Write(short _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.OutStream.Write(this.buffer, 0, 2);
	}

	// Token: 0x0600A5D7 RID: 42455 RVA: 0x003DD54C File Offset: 0x003DB74C
	public override void Write(int _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.buffer[2] = (byte)(_value >> 16);
		this.buffer[3] = (byte)(_value >> 24);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A5D8 RID: 42456 RVA: 0x003DD59C File Offset: 0x003DB79C
	public override void Write(long _value)
	{
		int i = 0;
		int num = 0;
		while (i < 8)
		{
			this.buffer[i] = (byte)(_value >> num);
			i++;
			num += 8;
		}
		this.OutStream.Write(this.buffer, 0, 8);
	}

	// Token: 0x0600A5D9 RID: 42457 RVA: 0x003DD5DD File Offset: 0x003DB7DD
	public override void Write(sbyte _value)
	{
		this.buffer[0] = (byte)_value;
		this.OutStream.Write(this.buffer, 0, 1);
	}

	// Token: 0x0600A5DA RID: 42458 RVA: 0x003DD5FC File Offset: 0x003DB7FC
	public override void Write(float _value)
	{
		BitConverterLE.GetBytes(_value, this.buffer);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A5DB RID: 42459 RVA: 0x003DD620 File Offset: 0x003DB820
	public unsafe override void Write(string _value)
	{
		if (_value == null)
		{
			throw new ArgumentNullException("_value");
		}
		if (this.encoder == null)
		{
			this.encoder = this.encoding.GetEncoder();
		}
		int byteCount;
		fixed (string text = _value)
		{
			char* ptr = text;
			if (ptr != null)
			{
				ptr += RuntimeHelpers.OffsetToStringData / 2;
			}
			byteCount = this.encoder.GetByteCount(ptr, _value.Length, true);
		}
		this.Write7BitEncodedInt(byteCount);
		int num = 128 / this.maxBytesPerChar;
		int num2 = 0;
		int num3;
		for (int i = _value.Length; i > 0; i -= num3)
		{
			num3 = ((i <= num) ? i : num);
			int bytes;
			fixed (string text = _value)
			{
				char* ptr2 = text;
				if (ptr2 != null)
				{
					ptr2 += RuntimeHelpers.OffsetToStringData / 2;
				}
				byte[] array;
				byte* ptr3;
				if ((array = this.buffer) == null || array.Length == 0)
				{
					ptr3 = null;
				}
				else
				{
					ptr3 = &array[0];
				}
				bytes = this.encoder.GetBytes((char*)(void*)((UIntPtr)((void*)ptr2) + num2 * 2), num3, ptr3, 128, num3 == i);
				array = null;
			}
			this.OutStream.Write(this.buffer, 0, bytes);
			num2 += num3;
		}
	}

	// Token: 0x0600A5DC RID: 42460 RVA: 0x003DD520 File Offset: 0x003DB720
	public override void Write(ushort _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.OutStream.Write(this.buffer, 0, 2);
	}

	// Token: 0x0600A5DD RID: 42461 RVA: 0x003DD744 File Offset: 0x003DB944
	public override void Write(uint _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.buffer[2] = (byte)(_value >> 16);
		this.buffer[3] = (byte)(_value >> 24);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A5DE RID: 42462 RVA: 0x003DD794 File Offset: 0x003DB994
	public override void Write(ulong _value)
	{
		int i = 0;
		int num = 0;
		while (i < 8)
		{
			this.buffer[i] = (byte)(_value >> num);
			i++;
			num += 8;
		}
		this.OutStream.Write(this.buffer, 0, 8);
	}

	// Token: 0x0600A5DF RID: 42463 RVA: 0x003DD7D8 File Offset: 0x003DB9D8
	public void Write7BitEncodedSignedInt(int _value)
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
		this.Write(b);
		for (num = num2; num != 0L; num = num2)
		{
			num2 = (num >> 7) & 16777215L;
			b = (byte)(num & 127L);
			if (num2 != 0L)
			{
				b |= 128;
			}
			this.Write(b);
		}
	}

	// Token: 0x0600A5E0 RID: 42464 RVA: 0x003DD850 File Offset: 0x003DBA50
	public new void Write7BitEncodedInt(int _value)
	{
		do
		{
			int num = (_value >> 7) & 16777215;
			byte b = (byte)(_value & 127);
			if (num != 0)
			{
				b |= 128;
			}
			this.Write(b);
			_value = num;
		}
		while (_value != 0);
	}

	// Token: 0x0600A5E1 RID: 42465 RVA: 0x003DD884 File Offset: 0x003DBA84
	[MustUseReturnValue]
	public PooledBinaryWriter.StreamWriteSizeMarker ReserveSizeMarker(PooledBinaryWriter.EMarkerSize _markerSize)
	{
		long position = this.OutStream.Position;
		Array.Clear(this.buffer, 0, (int)_markerSize);
		this.OutStream.Write(this.buffer, 0, (int)_markerSize);
		return new PooledBinaryWriter.StreamWriteSizeMarker(position, _markerSize);
	}

	// Token: 0x0600A5E2 RID: 42466 RVA: 0x003DD8B8 File Offset: 0x003DBAB8
	public void FinalizeSizeMarker(ref PooledBinaryWriter.StreamWriteSizeMarker _sizeMarker)
	{
		long position = this.OutStream.Position;
		long num = position - _sizeMarker.Position;
		if (num < 0L)
		{
			throw new Exception(string.Format("FinalizeMarker position ({0}) before Reserved position ({1})", position, _sizeMarker.Position));
		}
		uint num2;
		switch (_sizeMarker.MarkerSize)
		{
		case PooledBinaryWriter.EMarkerSize.UInt8:
			num2 = 255U;
			goto IL_007D;
		case PooledBinaryWriter.EMarkerSize.UInt16:
			num2 = 65535U;
			goto IL_007D;
		case PooledBinaryWriter.EMarkerSize.UInt32:
			num2 = uint.MaxValue;
			goto IL_007D;
		}
		throw new ArgumentOutOfRangeException("MarkerSize");
		IL_007D:
		long num3 = (long)((ulong)num2);
		if (num > num3)
		{
			throw new Exception(string.Format("Marked size ({0}) exceeding marker type ({1} maximum ({2})", num, _sizeMarker.MarkerSize.ToStringCached<PooledBinaryWriter.EMarkerSize>(), num3));
		}
		this.OutStream.Position = _sizeMarker.Position;
		switch (_sizeMarker.MarkerSize)
		{
		case PooledBinaryWriter.EMarkerSize.UInt8:
			this.Write((byte)num);
			break;
		case PooledBinaryWriter.EMarkerSize.UInt16:
			this.Write((ushort)num);
			break;
		case PooledBinaryWriter.EMarkerSize.UInt32:
			this.Write((uint)num);
			break;
		}
		this.OutStream.Position = position;
	}

	// Token: 0x0600A5E3 RID: 42467 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A5E4 RID: 42468 RVA: 0x003DD9CC File Offset: 0x003DBBCC
	public void Reset()
	{
		this.SetBaseStream(null);
	}

	// Token: 0x0600A5E5 RID: 42469 RVA: 0x003DD9D5 File Offset: 0x003DBBD5
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A5E6 RID: 42470 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A5E7 RID: 42471 RVA: 0x003DD9DD File Offset: 0x003DBBDD
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolBinaryWriter.FreeSync(this);
	}

	// Token: 0x0600A5E8 RID: 42472 RVA: 0x003DD9EA File Offset: 0x003DBBEA
	public bool ReadWrite(bool _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5E9 RID: 42473 RVA: 0x003DD9F4 File Offset: 0x003DBBF4
	public byte ReadWrite(byte _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5EA RID: 42474 RVA: 0x003DD9FE File Offset: 0x003DBBFE
	public sbyte ReadWrite(sbyte _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5EB RID: 42475 RVA: 0x003DDA08 File Offset: 0x003DBC08
	public char ReadWrite(char _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5EC RID: 42476 RVA: 0x003DDA12 File Offset: 0x003DBC12
	public short ReadWrite(short _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5ED RID: 42477 RVA: 0x003DDA1C File Offset: 0x003DBC1C
	public ushort ReadWrite(ushort _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5EE RID: 42478 RVA: 0x003DDA26 File Offset: 0x003DBC26
	public int ReadWrite(int _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5EF RID: 42479 RVA: 0x003DDA30 File Offset: 0x003DBC30
	public uint ReadWrite(uint _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5F0 RID: 42480 RVA: 0x003DDA3A File Offset: 0x003DBC3A
	public long ReadWrite(long _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5F1 RID: 42481 RVA: 0x003DDA44 File Offset: 0x003DBC44
	public ulong ReadWrite(ulong _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5F2 RID: 42482 RVA: 0x003DDA4E File Offset: 0x003DBC4E
	public float ReadWrite(float _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5F3 RID: 42483 RVA: 0x003DDA58 File Offset: 0x003DBC58
	public double ReadWrite(double _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5F4 RID: 42484 RVA: 0x003DDA62 File Offset: 0x003DBC62
	public decimal ReadWrite(decimal _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5F5 RID: 42485 RVA: 0x003DDA6C File Offset: 0x003DBC6C
	public string ReadWrite(string _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A5F6 RID: 42486 RVA: 0x003DDA76 File Offset: 0x003DBC76
	public void ReadWrite(byte[] _buffer, int _index, int _count)
	{
		this.Write(_buffer, _index, _count);
	}

	// Token: 0x0600A5F7 RID: 42487 RVA: 0x003DDA81 File Offset: 0x003DBC81
	public Vector3 ReadWrite(Vector3 _value)
	{
		this.Write(_value.x);
		this.Write(_value.y);
		this.Write(_value.z);
		return _value;
	}

	// Token: 0x04007BB6 RID: 31670
	public static int INSTANCES_LIVE;

	// Token: 0x04007BB7 RID: 31671
	public static int INSTANCES_MAX;

	// Token: 0x04007BB8 RID: 31672
	public static int INSTANCES_CREATED;

	// Token: 0x04007BB9 RID: 31673
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BYTE_BUFFER_SIZE = 128;

	// Token: 0x04007BBA RID: 31674
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHAR_BUFFER_SIZE = 128;

	// Token: 0x04007BBB RID: 31675
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] buffer = new byte[128];

	// Token: 0x04007BBC RID: 31676
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly char[] charBuffer = new char[128];

	// Token: 0x04007BBD RID: 31677
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxBytesPerChar;

	// Token: 0x04007BBE RID: 31678
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoder encoder;

	// Token: 0x04007BBF RID: 31679
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoding encoding;

	// Token: 0x020014B2 RID: 5298
	public enum EMarkerSize
	{
		// Token: 0x04007BC1 RID: 31681
		UInt8 = 1,
		// Token: 0x04007BC2 RID: 31682
		UInt16,
		// Token: 0x04007BC3 RID: 31683
		UInt32 = 4
	}

	// Token: 0x020014B3 RID: 5299
	public readonly struct StreamWriteSizeMarker
	{
		// Token: 0x0600A5F8 RID: 42488 RVA: 0x003DDAA8 File Offset: 0x003DBCA8
		public StreamWriteSizeMarker(long _position, PooledBinaryWriter.EMarkerSize _markerSize)
		{
			this.Position = _position;
			this.MarkerSize = _markerSize;
		}

		// Token: 0x04007BC4 RID: 31684
		public readonly long Position;

		// Token: 0x04007BC5 RID: 31685
		public readonly PooledBinaryWriter.EMarkerSize MarkerSize;
	}
}
