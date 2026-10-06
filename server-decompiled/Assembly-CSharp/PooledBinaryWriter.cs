using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x0200149F RID: 5279
public class PooledBinaryWriter : BinaryWriter, IBinaryReaderOrWriter, IMemoryPoolableObject, IDisposable
{
	// Token: 0x170013A0 RID: 5024
	// (get) Token: 0x0600A56B RID: 42347 RVA: 0x003DAD87 File Offset: 0x003D8F87
	// (set) Token: 0x0600A56C RID: 42348 RVA: 0x003DAD8F File Offset: 0x003D8F8F
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

	// Token: 0x0600A56D RID: 42349 RVA: 0x003DADC0 File Offset: 0x003D8FC0
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

	// Token: 0x0600A56E RID: 42350 RVA: 0x003DAE34 File Offset: 0x003D9034
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledBinaryWriter()
	{
		Interlocked.Decrement(ref PooledBinaryWriter.INSTANCES_LIVE);
	}

	// Token: 0x0600A56F RID: 42351 RVA: 0x003DAE68 File Offset: 0x003D9068
	public void SetBaseStream(Stream _stream)
	{
		if (_stream != null && !_stream.CanWrite)
		{
			throw new ArgumentException("Stream does not support writing or already closed.");
		}
		this.OutStream = _stream;
		this.encoder = null;
	}

	// Token: 0x170013A1 RID: 5025
	// (get) Token: 0x0600A570 RID: 42352 RVA: 0x003DAE8E File Offset: 0x003D908E
	public override Stream BaseStream
	{
		get
		{
			return this.OutStream;
		}
	}

	// Token: 0x0600A571 RID: 42353 RVA: 0x003DAE96 File Offset: 0x003D9096
	public override void Flush()
	{
		this.OutStream.Flush();
	}

	// Token: 0x0600A572 RID: 42354 RVA: 0x003DAEA3 File Offset: 0x003D90A3
	public override long Seek(int _offset, SeekOrigin _origin)
	{
		return this.OutStream.Seek((long)_offset, _origin);
	}

	// Token: 0x0600A573 RID: 42355 RVA: 0x003DAEB3 File Offset: 0x003D90B3
	public override void Write(bool _value)
	{
		this.buffer[0] = ((!_value) ? 0 : 1);
		this.OutStream.Write(this.buffer, 0, 1);
	}

	// Token: 0x0600A574 RID: 42356 RVA: 0x003DAED8 File Offset: 0x003D90D8
	public override void Write(byte _value)
	{
		this.OutStream.WriteByte(_value);
	}

	// Token: 0x0600A575 RID: 42357 RVA: 0x003DAEE6 File Offset: 0x003D90E6
	public override void Write(byte[] _buffer)
	{
		if (_buffer == null)
		{
			throw new ArgumentNullException("_buffer");
		}
		this.OutStream.Write(_buffer, 0, _buffer.Length);
	}

	// Token: 0x0600A576 RID: 42358 RVA: 0x003DAF06 File Offset: 0x003D9106
	public override void Write(byte[] _buffer, int _index, int _count)
	{
		if (_buffer == null)
		{
			throw new ArgumentNullException("_buffer");
		}
		this.OutStream.Write(_buffer, _index, _count);
	}

	// Token: 0x0600A577 RID: 42359 RVA: 0x003DAF24 File Offset: 0x003D9124
	public override void Write(ReadOnlySpan<byte> _buffer)
	{
		this.OutStream.Write(_buffer);
	}

	// Token: 0x0600A578 RID: 42360 RVA: 0x003DAF34 File Offset: 0x003D9134
	public override void Write(char _ch)
	{
		this.charBuffer[0] = _ch;
		int bytes = this.encoding.GetBytes(this.charBuffer, 0, 1, this.buffer, 0);
		this.OutStream.Write(this.buffer, 0, bytes);
	}

	// Token: 0x0600A579 RID: 42361 RVA: 0x003DAF78 File Offset: 0x003D9178
	public override void Write(char[] _chars)
	{
		this.Write(_chars, 0, _chars.Length);
	}

	// Token: 0x0600A57A RID: 42362 RVA: 0x003DAF85 File Offset: 0x003D9185
	public override void Write(char[] _chars, int _index, int _count)
	{
		if (_chars == null)
		{
			throw new ArgumentNullException("_chars");
		}
		this.Write(_chars.AsSpan<char>(_index, _count));
	}

	// Token: 0x0600A57B RID: 42363 RVA: 0x003DAFA8 File Offset: 0x003D91A8
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

	// Token: 0x0600A57C RID: 42364 RVA: 0x003DB018 File Offset: 0x003D9218
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

	// Token: 0x0600A57D RID: 42365 RVA: 0x003DB0FB File Offset: 0x003D92FB
	public override void Write(double _value)
	{
		BitConverterLE.GetBytes(_value, this.buffer);
		this.OutStream.Write(this.buffer, 0, 8);
	}

	// Token: 0x0600A57E RID: 42366 RVA: 0x003DB11C File Offset: 0x003D931C
	public override void Write(short _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.OutStream.Write(this.buffer, 0, 2);
	}

	// Token: 0x0600A57F RID: 42367 RVA: 0x003DB148 File Offset: 0x003D9348
	public override void Write(int _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.buffer[2] = (byte)(_value >> 16);
		this.buffer[3] = (byte)(_value >> 24);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A580 RID: 42368 RVA: 0x003DB198 File Offset: 0x003D9398
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

	// Token: 0x0600A581 RID: 42369 RVA: 0x003DB1D9 File Offset: 0x003D93D9
	public override void Write(sbyte _value)
	{
		this.buffer[0] = (byte)_value;
		this.OutStream.Write(this.buffer, 0, 1);
	}

	// Token: 0x0600A582 RID: 42370 RVA: 0x003DB1F8 File Offset: 0x003D93F8
	public override void Write(float _value)
	{
		BitConverterLE.GetBytes(_value, this.buffer);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A583 RID: 42371 RVA: 0x003DB21C File Offset: 0x003D941C
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

	// Token: 0x0600A584 RID: 42372 RVA: 0x003DB11C File Offset: 0x003D931C
	public override void Write(ushort _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.OutStream.Write(this.buffer, 0, 2);
	}

	// Token: 0x0600A585 RID: 42373 RVA: 0x003DB340 File Offset: 0x003D9540
	public override void Write(uint _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.buffer[2] = (byte)(_value >> 16);
		this.buffer[3] = (byte)(_value >> 24);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A586 RID: 42374 RVA: 0x003DB390 File Offset: 0x003D9590
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

	// Token: 0x0600A587 RID: 42375 RVA: 0x003DB3D4 File Offset: 0x003D95D4
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

	// Token: 0x0600A588 RID: 42376 RVA: 0x003DB44C File Offset: 0x003D964C
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

	// Token: 0x0600A589 RID: 42377 RVA: 0x003DB480 File Offset: 0x003D9680
	[MustUseReturnValue]
	public PooledBinaryWriter.StreamWriteSizeMarker ReserveSizeMarker(PooledBinaryWriter.EMarkerSize _markerSize)
	{
		long position = this.OutStream.Position;
		Array.Clear(this.buffer, 0, (int)_markerSize);
		this.OutStream.Write(this.buffer, 0, (int)_markerSize);
		return new PooledBinaryWriter.StreamWriteSizeMarker(position, _markerSize);
	}

	// Token: 0x0600A58A RID: 42378 RVA: 0x003DB4B4 File Offset: 0x003D96B4
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

	// Token: 0x0600A58B RID: 42379 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A58C RID: 42380 RVA: 0x003DB5C8 File Offset: 0x003D97C8
	public void Reset()
	{
		this.SetBaseStream(null);
	}

	// Token: 0x0600A58D RID: 42381 RVA: 0x003DB5D1 File Offset: 0x003D97D1
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A58E RID: 42382 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A58F RID: 42383 RVA: 0x003DB5D9 File Offset: 0x003D97D9
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolBinaryWriter.FreeSync(this);
	}

	// Token: 0x0600A590 RID: 42384 RVA: 0x003DB5E6 File Offset: 0x003D97E6
	public bool ReadWrite(bool _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A591 RID: 42385 RVA: 0x003DB5F0 File Offset: 0x003D97F0
	public byte ReadWrite(byte _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A592 RID: 42386 RVA: 0x003DB5FA File Offset: 0x003D97FA
	public sbyte ReadWrite(sbyte _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A593 RID: 42387 RVA: 0x003DB604 File Offset: 0x003D9804
	public char ReadWrite(char _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A594 RID: 42388 RVA: 0x003DB60E File Offset: 0x003D980E
	public short ReadWrite(short _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A595 RID: 42389 RVA: 0x003DB618 File Offset: 0x003D9818
	public ushort ReadWrite(ushort _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A596 RID: 42390 RVA: 0x003DB622 File Offset: 0x003D9822
	public int ReadWrite(int _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A597 RID: 42391 RVA: 0x003DB62C File Offset: 0x003D982C
	public uint ReadWrite(uint _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A598 RID: 42392 RVA: 0x003DB636 File Offset: 0x003D9836
	public long ReadWrite(long _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A599 RID: 42393 RVA: 0x003DB640 File Offset: 0x003D9840
	public ulong ReadWrite(ulong _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A59A RID: 42394 RVA: 0x003DB64A File Offset: 0x003D984A
	public float ReadWrite(float _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A59B RID: 42395 RVA: 0x003DB654 File Offset: 0x003D9854
	public double ReadWrite(double _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A59C RID: 42396 RVA: 0x003DB65E File Offset: 0x003D985E
	public decimal ReadWrite(decimal _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A59D RID: 42397 RVA: 0x003DB668 File Offset: 0x003D9868
	public string ReadWrite(string _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A59E RID: 42398 RVA: 0x003DB672 File Offset: 0x003D9872
	public void ReadWrite(byte[] _buffer, int _index, int _count)
	{
		this.Write(_buffer, _index, _count);
	}

	// Token: 0x0600A59F RID: 42399 RVA: 0x003DB67D File Offset: 0x003D987D
	public Vector3 ReadWrite(Vector3 _value)
	{
		this.Write(_value.x);
		this.Write(_value.y);
		this.Write(_value.z);
		return _value;
	}

	// Token: 0x04007B75 RID: 31605
	public static int INSTANCES_LIVE;

	// Token: 0x04007B76 RID: 31606
	public static int INSTANCES_MAX;

	// Token: 0x04007B77 RID: 31607
	public static int INSTANCES_CREATED;

	// Token: 0x04007B78 RID: 31608
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BYTE_BUFFER_SIZE = 128;

	// Token: 0x04007B79 RID: 31609
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHAR_BUFFER_SIZE = 128;

	// Token: 0x04007B7A RID: 31610
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] buffer = new byte[128];

	// Token: 0x04007B7B RID: 31611
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly char[] charBuffer = new char[128];

	// Token: 0x04007B7C RID: 31612
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxBytesPerChar;

	// Token: 0x04007B7D RID: 31613
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoder encoder;

	// Token: 0x04007B7E RID: 31614
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoding encoding;

	// Token: 0x020014A0 RID: 5280
	public enum EMarkerSize
	{
		// Token: 0x04007B80 RID: 31616
		UInt8 = 1,
		// Token: 0x04007B81 RID: 31617
		UInt16,
		// Token: 0x04007B82 RID: 31618
		UInt32 = 4
	}

	// Token: 0x020014A1 RID: 5281
	public readonly struct StreamWriteSizeMarker
	{
		// Token: 0x0600A5A0 RID: 42400 RVA: 0x003DB6A4 File Offset: 0x003D98A4
		public StreamWriteSizeMarker(long _position, PooledBinaryWriter.EMarkerSize _markerSize)
		{
			this.Position = _position;
			this.MarkerSize = _markerSize;
		}

		// Token: 0x04007B83 RID: 31619
		public readonly long Position;

		// Token: 0x04007B84 RID: 31620
		public readonly PooledBinaryWriter.EMarkerSize MarkerSize;
	}
}
