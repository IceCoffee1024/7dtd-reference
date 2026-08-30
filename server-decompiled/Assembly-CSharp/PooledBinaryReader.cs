using System;
using System.IO;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x0200147D RID: 5245
public class PooledBinaryReader : BinaryReader, IBinaryReaderOrWriter, IMemoryPoolableObject, IDisposable
{
	// Token: 0x17001359 RID: 4953
	// (get) Token: 0x0600A483 RID: 42115 RVA: 0x003D56A6 File Offset: 0x003D38A6
	public override Stream BaseStream
	{
		get
		{
			return this.baseStream;
		}
	}

	// Token: 0x1700135A RID: 4954
	// (get) Token: 0x0600A484 RID: 42116 RVA: 0x003D56AE File Offset: 0x003D38AE
	// (set) Token: 0x0600A485 RID: 42117 RVA: 0x003D56B6 File Offset: 0x003D38B6
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
			this.decoder = null;
		}
	}

	// Token: 0x0600A486 RID: 42118 RVA: 0x003D56D4 File Offset: 0x003D38D4
	public PooledBinaryReader()
		: base(Stream.Null)
	{
		this.Encoding = new UTF8Encoding(false, false);
		Interlocked.Increment(ref PooledBinaryReader.INSTANCES_CREATED);
		Interlocked.Increment(ref PooledBinaryReader.INSTANCES_LIVE);
		if (PooledBinaryReader.INSTANCES_LIVE > PooledBinaryReader.INSTANCES_MAX)
		{
			Interlocked.Exchange(ref PooledBinaryReader.INSTANCES_MAX, PooledBinaryReader.INSTANCES_LIVE);
		}
	}

	// Token: 0x0600A487 RID: 42119 RVA: 0x003D575C File Offset: 0x003D395C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledBinaryReader()
	{
		Interlocked.Decrement(ref PooledBinaryReader.INSTANCES_LIVE);
	}

	// Token: 0x0600A488 RID: 42120 RVA: 0x003D5790 File Offset: 0x003D3990
	public void SetBaseStream(Stream _stream)
	{
		if (_stream != null && !_stream.CanRead)
		{
			throw new ArgumentException("The stream doesn't support reading.");
		}
		this.baseStream = _stream;
		this.decoder = null;
	}

	// Token: 0x0600A489 RID: 42121 RVA: 0x003D57B8 File Offset: 0x003D39B8
	public override int PeekChar()
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		if (!this.baseStream.CanSeek)
		{
			return -1;
		}
		int num;
		bool flag = this.ReadCharBytes(this.charBuffer, 0, 1, out num) != 0;
		this.baseStream.Position -= (long)num;
		if (!flag)
		{
			return -1;
		}
		return (int)this.charBuffer[0];
	}

	// Token: 0x0600A48A RID: 42122 RVA: 0x003D5817 File Offset: 0x003D3A17
	public override int Read()
	{
		if (this.Read(this.charBuffer, 0, 1) == 0)
		{
			return -1;
		}
		return (int)this.charBuffer[0];
	}

	// Token: 0x0600A48B RID: 42123 RVA: 0x003D5833 File Offset: 0x003D3A33
	public override bool ReadBoolean()
	{
		return this.ReadByte() > 0;
	}

	// Token: 0x0600A48C RID: 42124 RVA: 0x003D583E File Offset: 0x003D3A3E
	public override byte ReadByte()
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		int num = this.baseStream.ReadByte();
		if (num == -1)
		{
			throw new EndOfStreamException();
		}
		return (byte)num;
	}

	// Token: 0x0600A48D RID: 42125 RVA: 0x003D5869 File Offset: 0x003D3A69
	public override sbyte ReadSByte()
	{
		return (sbyte)this.ReadByte();
	}

	// Token: 0x0600A48E RID: 42126 RVA: 0x003D5872 File Offset: 0x003D3A72
	public override char ReadChar()
	{
		int num = this.Read();
		if (num == -1)
		{
			throw new EndOfStreamException();
		}
		return (char)num;
	}

	// Token: 0x0600A48F RID: 42127 RVA: 0x003D5885 File Offset: 0x003D3A85
	public override short ReadInt16()
	{
		this.FillBuffer(2);
		return (short)((int)this.buffer[0] | ((int)this.buffer[1] << 8));
	}

	// Token: 0x0600A490 RID: 42128 RVA: 0x003D58A2 File Offset: 0x003D3AA2
	public override ushort ReadUInt16()
	{
		this.FillBuffer(2);
		return (ushort)((int)this.buffer[0] | ((int)this.buffer[1] << 8));
	}

	// Token: 0x0600A491 RID: 42129 RVA: 0x003D58BF File Offset: 0x003D3ABF
	public override int ReadInt32()
	{
		this.FillBuffer(4);
		return (int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24);
	}

	// Token: 0x0600A492 RID: 42130 RVA: 0x003D58BF File Offset: 0x003D3ABF
	public override uint ReadUInt32()
	{
		this.FillBuffer(4);
		return (uint)((int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24));
	}

	// Token: 0x0600A493 RID: 42131 RVA: 0x003D58F4 File Offset: 0x003D3AF4
	public override long ReadInt64()
	{
		this.FillBuffer(8);
		uint num = (uint)((int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24));
		return (long)(((ulong)((int)this.buffer[4] | ((int)this.buffer[5] << 8) | ((int)this.buffer[6] << 16) | ((int)this.buffer[7] << 24)) << 32) | (ulong)num);
	}

	// Token: 0x0600A494 RID: 42132 RVA: 0x003D5968 File Offset: 0x003D3B68
	public override ulong ReadUInt64()
	{
		this.FillBuffer(8);
		uint num = (uint)((int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24));
		return ((ulong)((int)this.buffer[4] | ((int)this.buffer[5] << 8) | ((int)this.buffer[6] << 16) | ((int)this.buffer[7] << 24)) << 32) | (ulong)num;
	}

	// Token: 0x0600A495 RID: 42133 RVA: 0x003D59DA File Offset: 0x003D3BDA
	public override float ReadSingle()
	{
		this.FillBuffer(4);
		return BitConverterLE.ToSingle(this.buffer, 0);
	}

	// Token: 0x0600A496 RID: 42134 RVA: 0x003D59EF File Offset: 0x003D3BEF
	public override double ReadDouble()
	{
		this.FillBuffer(8);
		return BitConverterLE.ToDouble(this.buffer, 0);
	}

	// Token: 0x0600A497 RID: 42135 RVA: 0x003D5A04 File Offset: 0x003D3C04
	public unsafe override decimal ReadDecimal()
	{
		this.FillBuffer(16);
		decimal num;
		byte* ptr = (byte*)(&num);
		if (BitConverter.IsLittleEndian)
		{
			for (int i = 0; i < 16; i++)
			{
				if (i < 4)
				{
					ptr[i + 8] = this.buffer[i];
				}
				else if (i < 8)
				{
					ptr[i + 8] = this.buffer[i];
				}
				else if (i < 12)
				{
					ptr[i - 4] = this.buffer[i];
				}
				else if (i < 16)
				{
					ptr[i - 12] = this.buffer[i];
				}
			}
		}
		else
		{
			for (int j = 0; j < 16; j++)
			{
				if (j < 4)
				{
					ptr[11 - j] = this.buffer[j];
				}
				else if (j < 8)
				{
					ptr[19 - j] = this.buffer[j];
				}
				else if (j < 12)
				{
					ptr[15 - j] = this.buffer[j];
				}
				else if (j < 16)
				{
					ptr[15 - j] = this.buffer[j];
				}
			}
		}
		return num;
	}

	// Token: 0x0600A498 RID: 42136 RVA: 0x003D5AE8 File Offset: 0x003D3CE8
	public override string ReadString()
	{
		int num = this.Read7BitEncodedInt();
		if (num < 0)
		{
			throw new IOException("Invalid binary file (string len < 0)");
		}
		if (num == 0)
		{
			return string.Empty;
		}
		this.stringBuilder.Length = 0;
		this.stringBuilder.EnsureCapacity(num);
		do
		{
			int num2 = ((num <= 128) ? num : 128);
			this.FillBuffer(num2);
			if (this.decoder == null)
			{
				this.decoder = this.encoding.GetDecoder();
			}
			int chars = this.decoder.GetChars(this.buffer, 0, num2, this.charBuffer, 0);
			this.stringBuilder.Append(this.charBuffer, 0, chars);
			num -= num2;
		}
		while (num > 0);
		return this.stringBuilder.ToString();
	}

	// Token: 0x0600A499 RID: 42137 RVA: 0x003D5BA0 File Offset: 0x003D3DA0
	public override int Read(char[] _buffer, int _index, int _count)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		if (_buffer == null)
		{
			throw new ArgumentNullException("_buffer", "_buffer is null");
		}
		if (_index < 0)
		{
			throw new ArgumentOutOfRangeException("_index", "_index is less than 0");
		}
		if (_count < 0)
		{
			throw new ArgumentOutOfRangeException("_count", "_count is less than 0");
		}
		if (_buffer.Length - _index < _count)
		{
			throw new ArgumentException("buffer is too small");
		}
		int num;
		return this.ReadCharBytes(_buffer, _index, _count, out num);
	}

	// Token: 0x0600A49A RID: 42138 RVA: 0x003D5C1C File Offset: 0x003D3E1C
	[Obsolete("char[] ReadChars (int) allocates memory. Try using int Read (char[], int, int) instead.")]
	public override char[] ReadChars(int _count)
	{
		if (_count < 0)
		{
			throw new ArgumentOutOfRangeException("_count", "_count is less than 0");
		}
		if (_count == 0)
		{
			return new char[0];
		}
		char[] array = new char[_count];
		int num = this.Read(array, 0, _count);
		if (num == 0)
		{
			throw new EndOfStreamException();
		}
		if (num != array.Length)
		{
			char[] array2 = new char[num];
			Array.Copy(array, 0, array2, 0, num);
			return array2;
		}
		return array;
	}

	// Token: 0x0600A49B RID: 42139 RVA: 0x003D5C7C File Offset: 0x003D3E7C
	public override int Read(Span<char> _buffer)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		int num;
		return this.ReadCharBytes(_buffer, out num);
	}

	// Token: 0x0600A49C RID: 42140 RVA: 0x003D5CA5 File Offset: 0x003D3EA5
	public override int Read(Span<byte> _buffer)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		return this.baseStream.Read(_buffer);
	}

	// Token: 0x0600A49D RID: 42141 RVA: 0x003D5CC8 File Offset: 0x003D3EC8
	public override int Read(byte[] _buffer, int _index, int _count)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		if (_buffer == null)
		{
			throw new ArgumentNullException("_buffer", "_buffer is null");
		}
		if (_index < 0)
		{
			throw new ArgumentOutOfRangeException("_index", "_index is less than 0");
		}
		if (_count < 0)
		{
			throw new ArgumentOutOfRangeException("_count", "_count is less than 0");
		}
		if (_buffer.Length - _index < _count)
		{
			throw new ArgumentException("buffer is too small");
		}
		return this.baseStream.Read(_buffer, _index, _count);
	}

	// Token: 0x0600A49E RID: 42142 RVA: 0x003D5D44 File Offset: 0x003D3F44
	[Obsolete("byte[] ReadBytes (int) allocates memory. Try using int Read (byte[], int, int) instead.")]
	public override byte[] ReadBytes(int _count)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		if (_count < 0)
		{
			throw new ArgumentOutOfRangeException("_count", "_count is less than 0");
		}
		byte[] array = new byte[_count];
		int i;
		int num;
		for (i = 0; i < _count; i += num)
		{
			num = this.baseStream.Read(array, i, _count - i);
			if (num == 0)
			{
				break;
			}
		}
		if (i != _count)
		{
			byte[] array2 = new byte[i];
			Buffer.BlockCopy(array, 0, array2, 0, i);
			return array2;
		}
		return array;
	}

	// Token: 0x0600A49F RID: 42143 RVA: 0x003D5DB8 File Offset: 0x003D3FB8
	public int Read7BitEncodedSignedInt()
	{
		int num = 0;
		int i = 0;
		byte b = this.ReadByte();
		num |= (int)(b & 63);
		i += 6;
		bool flag = (b & 64) > 0;
		if ((b & 128) != 0)
		{
			while (i < 32)
			{
				b = this.ReadByte();
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

	// Token: 0x0600A4A0 RID: 42144 RVA: 0x003D5E2C File Offset: 0x003D402C
	public new int Read7BitEncodedInt()
	{
		int num = 0;
		int i = 0;
		while (i < 35)
		{
			byte b = this.ReadByte();
			num |= (int)(b & 127) << i;
			i += 7;
			if ((b & 128) == 0)
			{
				return num;
			}
		}
		throw new FormatException("Illegal encoding for 7 bit encoded int");
	}

	// Token: 0x0600A4A1 RID: 42145 RVA: 0x003D5E70 File Offset: 0x003D4070
	[MustUseReturnValue]
	public PooledBinaryReader.StreamReadSizeMarker ReadSizeMarker(PooledBinaryWriter.EMarkerSize _markerSize)
	{
		long position = this.baseStream.Position;
		uint num;
		switch (_markerSize)
		{
		case PooledBinaryWriter.EMarkerSize.UInt8:
			num = (uint)this.ReadByte();
			goto IL_004C;
		case PooledBinaryWriter.EMarkerSize.UInt16:
			num = (uint)this.ReadUInt16();
			goto IL_004C;
		case PooledBinaryWriter.EMarkerSize.UInt32:
			num = this.ReadUInt32();
			goto IL_004C;
		}
		throw new ArgumentOutOfRangeException("_markerSize");
		IL_004C:
		uint num2 = num;
		return new PooledBinaryReader.StreamReadSizeMarker(position, num2);
	}

	// Token: 0x0600A4A2 RID: 42146 RVA: 0x003D5ED4 File Offset: 0x003D40D4
	public bool ValidateSizeMarker(ref PooledBinaryReader.StreamReadSizeMarker _sizeMarker, out uint _bytesReceived, bool _fixPosition = true)
	{
		long num = this.baseStream.Position - _sizeMarker.Position;
		_bytesReceived = (uint)num;
		if (num == (long)((ulong)_sizeMarker.ExpectedSize))
		{
			return true;
		}
		if (_fixPosition)
		{
			this.baseStream.Position = _sizeMarker.Position + (long)((ulong)_sizeMarker.ExpectedSize);
		}
		return false;
	}

	// Token: 0x0600A4A3 RID: 42147 RVA: 0x003D5F21 File Offset: 0x003D4121
	[PublicizedFrom(EAccessModifier.Private)]
	public int ReadCharBytes(char[] _targetBuffer, int _targetIndex, int _count, out int _bytesRead)
	{
		return this.ReadCharBytes(_targetBuffer.AsSpan<char>(_targetIndex, _count), out _bytesRead);
	}

	// Token: 0x0600A4A4 RID: 42148 RVA: 0x003D5F34 File Offset: 0x003D4134
	[PublicizedFrom(EAccessModifier.Private)]
	public int ReadCharBytes(Span<char> _targetBuffer, out int _bytesRead)
	{
		int i = 0;
		_bytesRead = 0;
		while (i < _targetBuffer.Length)
		{
			int num = 0;
			int chars;
			do
			{
				int num2 = this.baseStream.ReadByte();
				if (num2 == -1)
				{
					return i;
				}
				this.buffer[num++] = (byte)num2;
				_bytesRead++;
				chars = this.encoding.GetChars(this.buffer.AsSpan<byte>(0, num), _targetBuffer.Slice(i));
			}
			while (chars <= 0);
			i++;
		}
		return i;
	}

	// Token: 0x0600A4A5 RID: 42149 RVA: 0x003D5FAC File Offset: 0x003D41AC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void FillBuffer(int _numBytes)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		int num;
		for (int i = 0; i < _numBytes; i += num)
		{
			num = this.baseStream.Read(this.buffer, i, _numBytes - i);
			if (num == 0)
			{
				throw new EndOfStreamException();
			}
		}
	}

	// Token: 0x0600A4A6 RID: 42150 RVA: 0x003D5FF7 File Offset: 0x003D41F7
	public void Flush()
	{
		if (this.baseStream != null)
		{
			this.baseStream.Flush();
		}
	}

	// Token: 0x0600A4A7 RID: 42151 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A4A8 RID: 42152 RVA: 0x003D600C File Offset: 0x003D420C
	public void Reset()
	{
		this.stringBuilder.Length = 0;
		this.baseStream = null;
	}

	// Token: 0x0600A4A9 RID: 42153 RVA: 0x003D6021 File Offset: 0x003D4221
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A4AA RID: 42154 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A4AB RID: 42155 RVA: 0x003D6029 File Offset: 0x003D4229
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolBinaryReader.FreeSync(this);
	}

	// Token: 0x0600A4AC RID: 42156 RVA: 0x003D6036 File Offset: 0x003D4236
	public bool ReadWrite(bool _value)
	{
		return this.ReadBoolean();
	}

	// Token: 0x0600A4AD RID: 42157 RVA: 0x003D603E File Offset: 0x003D423E
	public byte ReadWrite(byte _value)
	{
		return this.ReadByte();
	}

	// Token: 0x0600A4AE RID: 42158 RVA: 0x003D6046 File Offset: 0x003D4246
	public sbyte ReadWrite(sbyte _value)
	{
		return this.ReadSByte();
	}

	// Token: 0x0600A4AF RID: 42159 RVA: 0x003D604E File Offset: 0x003D424E
	public char ReadWrite(char _value)
	{
		return this.ReadChar();
	}

	// Token: 0x0600A4B0 RID: 42160 RVA: 0x003D6056 File Offset: 0x003D4256
	public short ReadWrite(short _value)
	{
		return this.ReadInt16();
	}

	// Token: 0x0600A4B1 RID: 42161 RVA: 0x003D605E File Offset: 0x003D425E
	public ushort ReadWrite(ushort _value)
	{
		return this.ReadUInt16();
	}

	// Token: 0x0600A4B2 RID: 42162 RVA: 0x003D6066 File Offset: 0x003D4266
	public int ReadWrite(int _value)
	{
		return this.ReadInt32();
	}

	// Token: 0x0600A4B3 RID: 42163 RVA: 0x003D606E File Offset: 0x003D426E
	public uint ReadWrite(uint _value)
	{
		return this.ReadUInt32();
	}

	// Token: 0x0600A4B4 RID: 42164 RVA: 0x003D6076 File Offset: 0x003D4276
	public long ReadWrite(long _value)
	{
		return this.ReadInt64();
	}

	// Token: 0x0600A4B5 RID: 42165 RVA: 0x003D607E File Offset: 0x003D427E
	public ulong ReadWrite(ulong _value)
	{
		return this.ReadUInt64();
	}

	// Token: 0x0600A4B6 RID: 42166 RVA: 0x003D6086 File Offset: 0x003D4286
	public float ReadWrite(float _value)
	{
		return this.ReadSingle();
	}

	// Token: 0x0600A4B7 RID: 42167 RVA: 0x003D608E File Offset: 0x003D428E
	public double ReadWrite(double _value)
	{
		return this.ReadDouble();
	}

	// Token: 0x0600A4B8 RID: 42168 RVA: 0x003D6096 File Offset: 0x003D4296
	public decimal ReadWrite(decimal _value)
	{
		return this.ReadDecimal();
	}

	// Token: 0x0600A4B9 RID: 42169 RVA: 0x003D609E File Offset: 0x003D429E
	public string ReadWrite(string _value)
	{
		return this.ReadString();
	}

	// Token: 0x0600A4BA RID: 42170 RVA: 0x003D60A6 File Offset: 0x003D42A6
	public void ReadWrite(byte[] _buffer, int _index, int _count)
	{
		this.Read(_buffer, _index, _count);
	}

	// Token: 0x0600A4BB RID: 42171 RVA: 0x003D60B4 File Offset: 0x003D42B4
	public Vector3 ReadWrite(Vector3 _value)
	{
		Vector3 vector = _value;
		vector.x = this.ReadSingle();
		vector.y = this.ReadSingle();
		vector.z = this.ReadSingle();
		return vector;
	}

	// Token: 0x04007AED RID: 31469
	public static int INSTANCES_LIVE;

	// Token: 0x04007AEE RID: 31470
	public static int INSTANCES_MAX;

	// Token: 0x04007AEF RID: 31471
	public static int INSTANCES_CREATED;

	// Token: 0x04007AF0 RID: 31472
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BYTE_BUFFER_SIZE = 128;

	// Token: 0x04007AF1 RID: 31473
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHAR_BUFFER_SIZE = 128;

	// Token: 0x04007AF2 RID: 31474
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] buffer = new byte[128];

	// Token: 0x04007AF3 RID: 31475
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly char[] charBuffer = new char[128];

	// Token: 0x04007AF4 RID: 31476
	[PublicizedFrom(EAccessModifier.Private)]
	public Decoder decoder;

	// Token: 0x04007AF5 RID: 31477
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder stringBuilder = new StringBuilder(128);

	// Token: 0x04007AF6 RID: 31478
	[PublicizedFrom(EAccessModifier.Private)]
	public Stream baseStream;

	// Token: 0x04007AF7 RID: 31479
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoding encoding;

	// Token: 0x0200147E RID: 5246
	public readonly struct StreamReadSizeMarker
	{
		// Token: 0x0600A4BC RID: 42172 RVA: 0x003D60EB File Offset: 0x003D42EB
		public StreamReadSizeMarker(long _position, uint _expectedSize)
		{
			this.Position = _position;
			this.ExpectedSize = _expectedSize;
		}

		// Token: 0x04007AF8 RID: 31480
		public readonly long Position;

		// Token: 0x04007AF9 RID: 31481
		public readonly uint ExpectedSize;
	}
}
