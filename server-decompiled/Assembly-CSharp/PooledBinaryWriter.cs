using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02001464 RID: 5220
public class PooledBinaryWriter : BinaryWriter, IBinaryReaderOrWriter, IMemoryPoolableObject, IDisposable
{
	// Token: 0x17001322 RID: 4898
	// (get) Token: 0x0600A3C0 RID: 41920 RVA: 0x003D0DB7 File Offset: 0x003CEFB7
	// (set) Token: 0x0600A3C1 RID: 41921 RVA: 0x003D0DBF File Offset: 0x003CEFBF
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

	// Token: 0x0600A3C2 RID: 41922 RVA: 0x003D0DF0 File Offset: 0x003CEFF0
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

	// Token: 0x0600A3C3 RID: 41923 RVA: 0x003D0E64 File Offset: 0x003CF064
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledBinaryWriter()
	{
		Interlocked.Decrement(ref PooledBinaryWriter.INSTANCES_LIVE);
	}

	// Token: 0x0600A3C4 RID: 41924 RVA: 0x003D0E98 File Offset: 0x003CF098
	public void SetBaseStream(Stream _stream)
	{
		if (_stream != null && !_stream.CanWrite)
		{
			throw new ArgumentException("Stream does not support writing or already closed.");
		}
		this.OutStream = _stream;
		this.encoder = null;
	}

	// Token: 0x17001323 RID: 4899
	// (get) Token: 0x0600A3C5 RID: 41925 RVA: 0x003D0EBE File Offset: 0x003CF0BE
	public override Stream BaseStream
	{
		get
		{
			return this.OutStream;
		}
	}

	// Token: 0x0600A3C6 RID: 41926 RVA: 0x003D0EC6 File Offset: 0x003CF0C6
	public override void Flush()
	{
		this.OutStream.Flush();
	}

	// Token: 0x0600A3C7 RID: 41927 RVA: 0x003D0ED3 File Offset: 0x003CF0D3
	public override long Seek(int _offset, SeekOrigin _origin)
	{
		return this.OutStream.Seek((long)_offset, _origin);
	}

	// Token: 0x0600A3C8 RID: 41928 RVA: 0x003D0EE3 File Offset: 0x003CF0E3
	public override void Write(bool _value)
	{
		this.buffer[0] = ((!_value) ? 0 : 1);
		this.OutStream.Write(this.buffer, 0, 1);
	}

	// Token: 0x0600A3C9 RID: 41929 RVA: 0x003D0F08 File Offset: 0x003CF108
	public override void Write(byte _value)
	{
		this.OutStream.WriteByte(_value);
	}

	// Token: 0x0600A3CA RID: 41930 RVA: 0x003D0F16 File Offset: 0x003CF116
	public override void Write(byte[] _buffer)
	{
		if (_buffer == null)
		{
			throw new ArgumentNullException("_buffer");
		}
		this.OutStream.Write(_buffer, 0, _buffer.Length);
	}

	// Token: 0x0600A3CB RID: 41931 RVA: 0x003D0F36 File Offset: 0x003CF136
	public override void Write(byte[] _buffer, int _index, int _count)
	{
		if (_buffer == null)
		{
			throw new ArgumentNullException("_buffer");
		}
		this.OutStream.Write(_buffer, _index, _count);
	}

	// Token: 0x0600A3CC RID: 41932 RVA: 0x003D0F54 File Offset: 0x003CF154
	public override void Write(ReadOnlySpan<byte> _buffer)
	{
		this.OutStream.Write(_buffer);
	}

	// Token: 0x0600A3CD RID: 41933 RVA: 0x003D0F64 File Offset: 0x003CF164
	public override void Write(char _ch)
	{
		this.charBuffer[0] = _ch;
		int bytes = this.encoding.GetBytes(this.charBuffer, 0, 1, this.buffer, 0);
		this.OutStream.Write(this.buffer, 0, bytes);
	}

	// Token: 0x0600A3CE RID: 41934 RVA: 0x003D0FA8 File Offset: 0x003CF1A8
	public override void Write(char[] _chars)
	{
		this.Write(_chars, 0, _chars.Length);
	}

	// Token: 0x0600A3CF RID: 41935 RVA: 0x003D0FB5 File Offset: 0x003CF1B5
	public override void Write(char[] _chars, int _index, int _count)
	{
		if (_chars == null)
		{
			throw new ArgumentNullException("_chars");
		}
		this.Write(_chars.AsSpan<char>(_index, _count));
	}

	// Token: 0x0600A3D0 RID: 41936 RVA: 0x003D0FD8 File Offset: 0x003CF1D8
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

	// Token: 0x0600A3D1 RID: 41937 RVA: 0x003D1048 File Offset: 0x003CF248
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

	// Token: 0x0600A3D2 RID: 41938 RVA: 0x003D112B File Offset: 0x003CF32B
	public override void Write(double _value)
	{
		BitConverterLE.GetBytes(_value, this.buffer);
		this.OutStream.Write(this.buffer, 0, 8);
	}

	// Token: 0x0600A3D3 RID: 41939 RVA: 0x003D114C File Offset: 0x003CF34C
	public override void Write(short _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.OutStream.Write(this.buffer, 0, 2);
	}

	// Token: 0x0600A3D4 RID: 41940 RVA: 0x003D1178 File Offset: 0x003CF378
	public override void Write(int _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.buffer[2] = (byte)(_value >> 16);
		this.buffer[3] = (byte)(_value >> 24);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A3D5 RID: 41941 RVA: 0x003D11C8 File Offset: 0x003CF3C8
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

	// Token: 0x0600A3D6 RID: 41942 RVA: 0x003D1209 File Offset: 0x003CF409
	public override void Write(sbyte _value)
	{
		this.buffer[0] = (byte)_value;
		this.OutStream.Write(this.buffer, 0, 1);
	}

	// Token: 0x0600A3D7 RID: 41943 RVA: 0x003D1228 File Offset: 0x003CF428
	public override void Write(float _value)
	{
		BitConverterLE.GetBytes(_value, this.buffer);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A3D8 RID: 41944 RVA: 0x003D124C File Offset: 0x003CF44C
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

	// Token: 0x0600A3D9 RID: 41945 RVA: 0x003D114C File Offset: 0x003CF34C
	public override void Write(ushort _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.OutStream.Write(this.buffer, 0, 2);
	}

	// Token: 0x0600A3DA RID: 41946 RVA: 0x003D1370 File Offset: 0x003CF570
	public override void Write(uint _value)
	{
		this.buffer[0] = (byte)_value;
		this.buffer[1] = (byte)(_value >> 8);
		this.buffer[2] = (byte)(_value >> 16);
		this.buffer[3] = (byte)(_value >> 24);
		this.OutStream.Write(this.buffer, 0, 4);
	}

	// Token: 0x0600A3DB RID: 41947 RVA: 0x003D13C0 File Offset: 0x003CF5C0
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

	// Token: 0x0600A3DC RID: 41948 RVA: 0x003D1404 File Offset: 0x003CF604
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

	// Token: 0x0600A3DD RID: 41949 RVA: 0x003D147C File Offset: 0x003CF67C
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

	// Token: 0x0600A3DE RID: 41950 RVA: 0x003D14B0 File Offset: 0x003CF6B0
	[MustUseReturnValue]
	public PooledBinaryWriter.StreamWriteSizeMarker ReserveSizeMarker(PooledBinaryWriter.EMarkerSize _markerSize)
	{
		long position = this.OutStream.Position;
		Array.Clear(this.buffer, 0, (int)_markerSize);
		this.OutStream.Write(this.buffer, 0, (int)_markerSize);
		return new PooledBinaryWriter.StreamWriteSizeMarker(position, _markerSize);
	}

	// Token: 0x0600A3DF RID: 41951 RVA: 0x003D14E4 File Offset: 0x003CF6E4
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

	// Token: 0x0600A3E0 RID: 41952 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A3E1 RID: 41953 RVA: 0x003D15F8 File Offset: 0x003CF7F8
	public void Reset()
	{
		this.SetBaseStream(null);
	}

	// Token: 0x0600A3E2 RID: 41954 RVA: 0x003D1601 File Offset: 0x003CF801
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A3E3 RID: 41955 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A3E4 RID: 41956 RVA: 0x003D1609 File Offset: 0x003CF809
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolBinaryWriter.FreeSync(this);
	}

	// Token: 0x0600A3E5 RID: 41957 RVA: 0x003D1616 File Offset: 0x003CF816
	public bool ReadWrite(bool _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3E6 RID: 41958 RVA: 0x003D1620 File Offset: 0x003CF820
	public byte ReadWrite(byte _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3E7 RID: 41959 RVA: 0x003D162A File Offset: 0x003CF82A
	public sbyte ReadWrite(sbyte _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3E8 RID: 41960 RVA: 0x003D1634 File Offset: 0x003CF834
	public char ReadWrite(char _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3E9 RID: 41961 RVA: 0x003D163E File Offset: 0x003CF83E
	public short ReadWrite(short _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3EA RID: 41962 RVA: 0x003D1648 File Offset: 0x003CF848
	public ushort ReadWrite(ushort _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3EB RID: 41963 RVA: 0x003D1652 File Offset: 0x003CF852
	public int ReadWrite(int _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3EC RID: 41964 RVA: 0x003D165C File Offset: 0x003CF85C
	public uint ReadWrite(uint _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3ED RID: 41965 RVA: 0x003D1666 File Offset: 0x003CF866
	public long ReadWrite(long _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3EE RID: 41966 RVA: 0x003D1670 File Offset: 0x003CF870
	public ulong ReadWrite(ulong _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3EF RID: 41967 RVA: 0x003D167A File Offset: 0x003CF87A
	public float ReadWrite(float _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3F0 RID: 41968 RVA: 0x003D1684 File Offset: 0x003CF884
	public double ReadWrite(double _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3F1 RID: 41969 RVA: 0x003D168E File Offset: 0x003CF88E
	public decimal ReadWrite(decimal _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3F2 RID: 41970 RVA: 0x003D1698 File Offset: 0x003CF898
	public string ReadWrite(string _value)
	{
		this.Write(_value);
		return _value;
	}

	// Token: 0x0600A3F3 RID: 41971 RVA: 0x003D16A2 File Offset: 0x003CF8A2
	public void ReadWrite(byte[] _buffer, int _index, int _count)
	{
		this.Write(_buffer, _index, _count);
	}

	// Token: 0x0600A3F4 RID: 41972 RVA: 0x003D16AD File Offset: 0x003CF8AD
	public Vector3 ReadWrite(Vector3 _value)
	{
		this.Write(_value.x);
		this.Write(_value.y);
		this.Write(_value.z);
		return _value;
	}

	// Token: 0x04007A21 RID: 31265
	public static int INSTANCES_LIVE;

	// Token: 0x04007A22 RID: 31266
	public static int INSTANCES_MAX;

	// Token: 0x04007A23 RID: 31267
	public static int INSTANCES_CREATED;

	// Token: 0x04007A24 RID: 31268
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BYTE_BUFFER_SIZE = 128;

	// Token: 0x04007A25 RID: 31269
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHAR_BUFFER_SIZE = 128;

	// Token: 0x04007A26 RID: 31270
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] buffer = new byte[128];

	// Token: 0x04007A27 RID: 31271
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly char[] charBuffer = new char[128];

	// Token: 0x04007A28 RID: 31272
	[PublicizedFrom(EAccessModifier.Private)]
	public int maxBytesPerChar;

	// Token: 0x04007A29 RID: 31273
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoder encoder;

	// Token: 0x04007A2A RID: 31274
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoding encoding;

	// Token: 0x02001465 RID: 5221
	public enum EMarkerSize
	{
		// Token: 0x04007A2C RID: 31276
		UInt8 = 1,
		// Token: 0x04007A2D RID: 31277
		UInt16,
		// Token: 0x04007A2E RID: 31278
		UInt32 = 4
	}

	// Token: 0x02001466 RID: 5222
	public readonly struct StreamWriteSizeMarker
	{
		// Token: 0x0600A3F5 RID: 41973 RVA: 0x003D16D4 File Offset: 0x003CF8D4
		public StreamWriteSizeMarker(long _position, PooledBinaryWriter.EMarkerSize _markerSize)
		{
			this.Position = _position;
			this.MarkerSize = _markerSize;
		}

		// Token: 0x04007A2F RID: 31279
		public readonly long Position;

		// Token: 0x04007A30 RID: 31280
		public readonly PooledBinaryWriter.EMarkerSize MarkerSize;
	}
}
