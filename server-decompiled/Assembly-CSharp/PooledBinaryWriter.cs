using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x0200147F RID: 5247
public class PooledBinaryWriter : BinaryWriter, IBinaryReaderOrWriter, IMemoryPoolableObject, IDisposable
{
	// Token: 0x1700135B RID: 4955
	// (get) Token: 0x0600A4BD RID: 42173 RVA: 0x003D60FB File Offset: 0x003D42FB
	// (set) Token: 0x0600A4BE RID: 42174 RVA: 0x003D6103 File Offset: 0x003D4303
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

	// Token: 0x0600A4BF RID: 42175 RVA: 0x003D6134 File Offset: 0x003D4334
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

	// Token: 0x0600A4C0 RID: 42176 RVA: 0x003D61A8 File Offset: 0x003D43A8
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledBinaryWriter()
	{
		Interlocked.Decrement(ref PooledBinaryWriter.INSTANCES_LIVE);
	}

	// Token: 0x0600A4C1 RID: 42177 RVA: 0x003D61DC File Offset: 0x003D43DC
	public void SetBaseStream(Stream _stream)
	{
		if (_stream != null && !_stream.CanWrite)
		{
			throw new ArgumentException("Stream does not support writing or already closed.");
		}
		this.OutStream = _stream;
		this.encoder = null;
	}

	// Token: 0x1700135C RID: 4956
	// (get) Token: 0x0600A4C2 RID: 42178 RVA: 0x003D6202 File Offset: 0x003D4402
	public override Stream BaseStream
	{
		get
		{
			return this.OutStream;
		}
	}

	// Token: 0x0600A4C3 RID: 42179 RVA: 0x003D620A File Offset: 0x003D440A
	public override void Flush()
	{
		this.OutStream.Flush();
	}

	// Token: 0x0600A4C4 RID: 42180 RVA: 0x003D6217 File Offset: 0x003D4417
	public override long Seek(int _offset, SeekOrigin _origin)
	{
		return this.OutStream.Seek((long)_offset, _origin);
	}

	// Token: 0x0600A4C5 RID: 42181 RVA: 0x003D6227 File Offset: 0x003D4427
	public override void Write(bool _value)
	{
		this.buffer[0] = ((!_value) ? 0 : 1);
		this.OutStream.Write(this.buffer, 0, 1);
	}

	// Token: 0x0600A4C6 RID: 42182 RVA: 0x003D624C File Offset: 0x003D444C
	public override void Write(byte _value)
	{
		this.OutStream.WriteByte(_value);
	}

	// Token: 0x0600A4C7 RID: 42183 RVA: 0x003D625A File Offset: 0x003D445A
	public override void Write(byte[] _buffer)
	{
		if (_buffer == null)
		{
			throw new ArgumentNullException("_buffer");
		}
		this.OutStream.Write(_buffer, 0, _buffer.Length);
	}

	// Token: 0x0600A4C8 RID: 42184 RVA: 0x003D627A File Offset: 0x003D447A
	public override void Write(byte[] _buffer, int _index, int _count)
	{
		if (_buffer == null)
		{
			throw new ArgumentNullException("_buffer");
		}
		this.OutStream.Write(_buffer, _index, _count);
	}

	// Token: 0x0600A4C9 RID: 42185 RVA: 0x003D6298 File Offset: 0x003D4498
	public override void Write(ReadOnlySpan<byte> _buffer)
	{
		this.OutStream.Write(_buffer);
	}

	// Token: 0x0600A4CA RID: 42186 RVA: 0x003D62A8 File Offset: 0x003D44A8
	public override void Write(char _ch)
	{
		this.charBuffer[0] = _ch;
		int bytes = this.encoding.GetBytes(this.charBuffer, 0, 1, this.buffer, 0);
		this.OutStream.Write(this.buffer, 0, bytes);
	}

	// Token: 0x0600A4CB RID: 42187 RVA: 0x003D62EC File Offset: 0x003D44EC
	public override void Write(char[] _chars)
	{
		this.Write(_chars, 0, _chars.Length);
	}

	// Token: 0x0600A4CC RID: 42188 RVA: 0x003D62F9 File Offset: 0x003D44F9
	public override void Write(char[] _chars, int _index, int _count)
	{
		if (_chars == null)
		{
			throw new ArgumentNullException("_chars");
		}
		this.Write(_chars.AsSpan<char>(_index, _count));
	}

	// Token: 0x0600A4CD RID: 42189 RVA: 0x003D631C File Offset: 0x003D451C
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

	// Token: 0x0600A4CE RID: 42190 RVA: 0x003D638C File Offset: 0x003D458C
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

	// Token: 0x0600A4CF RID: 42191 RVA: 0x003D646F File Offset: 0x003D466F
	public override void Write(double _value)
	{
		BitConverterLE.GetBytes(_value, this.buffer);
		this.OutStream.Write(this.buffer, 0, 8);
	}

	// Token: 0x0600A4D0 RID: 42192 RVA: 0x003D6490 File Offset: 0x003D4690
	public override void Write(short _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.OutStream.Write(this.buffer, 0, 2);
	}

	// Token: 0x0600A4D1 RID: 42193 RVA: 0x003D64BC File Offset: 0x003D46BC
	public override void Write(int _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.buffer[2] = (byte)(_value >> 16);
		this.buffer[3] = (byte)(_value >> 24);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A4D2 RID: 42194 RVA: 0x003D650C File Offset: 0x003D470C
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

	// Token: 0x0600A4D3 RID: 42195 RVA: 0x003D654D File Offset: 0x003D474D
	public override void Write(sbyte _value)
	{
		this.buffer[0] = (byte)_value;
		this.OutStream.Write(this.buffer, 0, 1);
	}

	// Token: 0x0600A4D4 RID: 42196 RVA: 0x003D656C File Offset: 0x003D476C
	public override void Write(float _value)
	{
		BitConverterLE.GetBytes(_value, this.buffer);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A4D5 RID: 42197 RVA: 0x003D6590 File Offset: 0x003D4790
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

	// Token: 0x0600A4D6 RID: 42198 RVA: 0x003D6490 File Offset: 0x003D4690
	public override void Write(ushort _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.OutStream.Write(this.buffer, 0, 2);
	}

	// Token: 0x0600A4D7 RID: 42199 RVA: 0x003D66B4 File Offset: 0x003D48B4
	public override void Write(uint _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.buffer[2] = (byte)(_value >> 16);
		this.buffer[3] = (byte)(_value >> 24);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A4D8 RID: 42200 RVA: 0x003D6704 File Offset: 0x003D4904
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

	// Token: 0x0600A4D9 RID: 42201 RVA: 0x003D6748 File Offset: 0x003D4948
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

	// Token: 0x0600A4DA RID: 42202 RVA: 0x003D67C0 File Offset: 0x003D49C0
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

	// Token: 0x0600A4DB RID: 42203 RVA: 0x003D67F4 File Offset: 0x003D49F4
	[MustUseReturnValue]
	public PooledBinaryWriter.StreamWriteSizeMarker ReserveSizeMarker(PooledBinaryWriter.EMarkerSize _markerSize)
	{
		long position = this.OutStream.Position;
		Array.Clear(this.buffer, 0, (int)_markerSize);
		this.OutStream.Write(this.buffer, 0, (int)_markerSize);
		return new PooledBinaryWriter.StreamWriteSizeMarker(position, _markerSize);
	}

	// Token: 0x0600A4DC RID: 42204 RVA: 0x003D6828 File Offset: 0x003D4A28
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

	// Token: 0x0600A4DD RID: 42205 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A4DE RID: 42206 RVA: 0x003D693C File Offset: 0x003D4B3C
	public void Reset()
	{
		this.SetBaseStream(null);
	}

	// Token: 0x0600A4DF RID: 42207 RVA: 0x003D6945 File Offset: 0x003D4B45
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A4E0 RID: 42208 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A4E1 RID: 42209 RVA: 0x003D694D File Offset: 0x003D4B4D
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolBinaryWriter.FreeSync(this);
	}

	// Token: 0x0600A4E2 RID: 42210 RVA: 0x003D695A File Offset: 0x003D4B5A
	public bool ReadWrite(bool _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4E3 RID: 42211 RVA: 0x003D6964 File Offset: 0x003D4B64
	public byte ReadWrite(byte _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4E4 RID: 42212 RVA: 0x003D696E File Offset: 0x003D4B6E
	public sbyte ReadWrite(sbyte _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4E5 RID: 42213 RVA: 0x003D6978 File Offset: 0x003D4B78
	public char ReadWrite(char _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4E6 RID: 42214 RVA: 0x003D6982 File Offset: 0x003D4B82
	public short ReadWrite(short _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4E7 RID: 42215 RVA: 0x003D698C File Offset: 0x003D4B8C
	public ushort ReadWrite(ushort _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4E8 RID: 42216 RVA: 0x003D6996 File Offset: 0x003D4B96
	public int ReadWrite(int _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4E9 RID: 42217 RVA: 0x003D69A0 File Offset: 0x003D4BA0
	public uint ReadWrite(uint _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4EA RID: 42218 RVA: 0x003D69AA File Offset: 0x003D4BAA
	public long ReadWrite(long _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4EB RID: 42219 RVA: 0x003D69B4 File Offset: 0x003D4BB4
	public ulong ReadWrite(ulong _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4EC RID: 42220 RVA: 0x003D69BE File Offset: 0x003D4BBE
	public float ReadWrite(float _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4ED RID: 42221 RVA: 0x003D69C8 File Offset: 0x003D4BC8
	public double ReadWrite(double _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4EE RID: 42222 RVA: 0x003D69D2 File Offset: 0x003D4BD2
	public decimal ReadWrite(decimal _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4EF RID: 42223 RVA: 0x003D69DC File Offset: 0x003D4BDC
	public string ReadWrite(string _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A4F0 RID: 42224 RVA: 0x003D69E6 File Offset: 0x003D4BE6
	public void ReadWrite(byte[] _buffer, int _index, int _count)
	{
		this.Write(_buffer, _index, _count);
	}

	// Token: 0x0600A4F1 RID: 42225 RVA: 0x003D69F1 File Offset: 0x003D4BF1
	public Vector3 ReadWrite(Vector3 _value)
	{
		this.Write(_value.x);
		this.Write(_value.y);
		this.Write(_value.z);
		return _value;
	}

	// Token: 0x04007AFA RID: 31482
	public static int INSTANCES_LIVE;

	// Token: 0x04007AFB RID: 31483
	public static int INSTANCES_MAX;

	// Token: 0x04007AFC RID: 31484
	public static int INSTANCES_CREATED;

	// Token: 0x04007AFD RID: 31485
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BYTE_BUFFER_SIZE = 128;

	// Token: 0x04007AFE RID: 31486
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHAR_BUFFER_SIZE = 128;

	// Token: 0x04007AFF RID: 31487
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] buffer = new byte[128];

	// Token: 0x04007B00 RID: 31488
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly char[] charBuffer = new char[128];

	// Token: 0x04007B01 RID: 31489
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxBytesPerChar;

	// Token: 0x04007B02 RID: 31490
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoder encoder;

	// Token: 0x04007B03 RID: 31491
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoding encoding;

	// Token: 0x02001480 RID: 5248
	public enum EMarkerSize
	{
		// Token: 0x04007B05 RID: 31493
		UInt8 = 1,
		// Token: 0x04007B06 RID: 31494
		UInt16,
		// Token: 0x04007B07 RID: 31495
		UInt32 = 4
	}

	// Token: 0x02001481 RID: 5249
	public readonly struct StreamWriteSizeMarker
	{
		// Token: 0x0600A4F2 RID: 42226 RVA: 0x003D6A18 File Offset: 0x003D4C18
		public StreamWriteSizeMarker(long _position, PooledBinaryWriter.EMarkerSize _markerSize)
		{
			this.Position = _position;
			this.MarkerSize = _markerSize;
		}

		// Token: 0x04007B08 RID: 31496
		public readonly long Position;

		// Token: 0x04007B09 RID: 31497
		public readonly PooledBinaryWriter.EMarkerSize MarkerSize;
	}
}
