using System;
using System.IO;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x020014AF RID: 5295
public class PooledBinaryReader : BinaryReader, IBinaryReaderOrWriter, IMemoryPoolableObject, IDisposable
{
	// Token: 0x170013AA RID: 5034
	// (get) Token: 0x0600A588 RID: 42376 RVA: 0x003DC6E6 File Offset: 0x003DA8E6
	public override Stream BaseStream
	{
		get
		{
			return this.baseStream;
		}
	}

	// Token: 0x170013AB RID: 5035
	// (get) Token: 0x0600A589 RID: 42377 RVA: 0x003DC6EE File Offset: 0x003DA8EE
	// (set) Token: 0x0600A58A RID: 42378 RVA: 0x003DC6F6 File Offset: 0x003DA8F6
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

	// Token: 0x0600A58B RID: 42379 RVA: 0x003DC714 File Offset: 0x003DA914
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

	// Token: 0x0600A58C RID: 42380 RVA: 0x003DC79C File Offset: 0x003DA99C
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledBinaryReader()
	{
		Interlocked.Decrement(ref PooledBinaryReader.INSTANCES_LIVE);
	}

	// Token: 0x0600A58D RID: 42381 RVA: 0x003DC7D0 File Offset: 0x003DA9D0
	public void SetBaseStream(Stream _stream)
	{
		if (_stream != null && !_stream.CanRead)
		{
			throw new ArgumentException("The stream doesn't support reading.");
		}
		this.baseStream = _stream;
		this.decoder = null;
	}

	// Token: 0x0600A58E RID: 42382 RVA: 0x003DC7F8 File Offset: 0x003DA9F8
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

	// Token: 0x0600A58F RID: 42383 RVA: 0x003DC857 File Offset: 0x003DAA57
	public override int Read()
	{
		if (this.Read(this.charBuffer, 0, 1) == 0)
		{
			return -1;
		}
		return (int)this.charBuffer[0];
	}

	// Token: 0x0600A590 RID: 42384 RVA: 0x003DC873 File Offset: 0x003DAA73
	public override bool ReadBoolean()
	{
		return this.ReadByte() > 0;
	}

	// Token: 0x0600A591 RID: 42385 RVA: 0x003DC87E File Offset: 0x003DAA7E
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

	// Token: 0x0600A592 RID: 42386 RVA: 0x003DC8A9 File Offset: 0x003DAAA9
	public override sbyte ReadSByte()
	{
		return (sbyte)this.ReadByte();
	}

	// Token: 0x0600A593 RID: 42387 RVA: 0x003DC8B2 File Offset: 0x003DAAB2
	public override char ReadChar()
	{
		int num = this.Read();
		if (num == -1)
		{
			throw new EndOfStreamException();
		}
		return (char)num;
	}

	// Token: 0x0600A594 RID: 42388 RVA: 0x003DC8C5 File Offset: 0x003DAAC5
	public override short ReadInt16()
	{
		this.FillBuffer(2);
		return (short)((int)this.buffer[0] | ((int)this.buffer[1] << 8));
	}

	// Token: 0x0600A595 RID: 42389 RVA: 0x003DC8E2 File Offset: 0x003DAAE2
	public override ushort ReadUInt16()
	{
		this.FillBuffer(2);
		return (ushort)((int)this.buffer[0] | ((int)this.buffer[1] << 8));
	}

	// Token: 0x0600A596 RID: 42390 RVA: 0x003DC8FF File Offset: 0x003DAAFF
	public override int ReadInt32()
	{
		this.FillBuffer(4);
		return (int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24);
	}

	// Token: 0x0600A597 RID: 42391 RVA: 0x003DC8FF File Offset: 0x003DAAFF
	public override uint ReadUInt32()
	{
		this.FillBuffer(4);
		return (uint)((int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24));
	}

	// Token: 0x0600A598 RID: 42392 RVA: 0x003DC934 File Offset: 0x003DAB34
	public override long ReadInt64()
	{
		this.FillBuffer(8);
		uint num = (uint)((int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24));
		return (long)(((ulong)((int)this.buffer[4] | ((int)this.buffer[5] << 8) | ((int)this.buffer[6] << 16) | ((int)this.buffer[7] << 24)) << 32) | (ulong)num);
	}

	// Token: 0x0600A599 RID: 42393 RVA: 0x003DC9A8 File Offset: 0x003DABA8
	public override ulong ReadUInt64()
	{
		this.FillBuffer(8);
		uint num = (uint)((int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24));
		return ((ulong)((int)this.buffer[4] | ((int)this.buffer[5] << 8) | ((int)this.buffer[6] << 16) | ((int)this.buffer[7] << 24)) << 32) | (ulong)num;
	}

	// Token: 0x0600A59A RID: 42394 RVA: 0x003DCA1A File Offset: 0x003DAC1A
	public override float ReadSingle()
	{
		this.FillBuffer(4);
		return BitConverterLE.ToSingle(this.buffer, 0);
	}

	// Token: 0x0600A59B RID: 42395 RVA: 0x003DCA2F File Offset: 0x003DAC2F
	public override double ReadDouble()
	{
		this.FillBuffer(8);
		return BitConverterLE.ToDouble(this.buffer, 0);
	}

	// Token: 0x0600A59C RID: 42396 RVA: 0x003DCA44 File Offset: 0x003DAC44
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

	// Token: 0x0600A59D RID: 42397 RVA: 0x003DCB28 File Offset: 0x003DAD28
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

	// Token: 0x0600A59E RID: 42398 RVA: 0x003DCBE0 File Offset: 0x003DADE0
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

	// Token: 0x0600A59F RID: 42399 RVA: 0x003DCC5C File Offset: 0x003DAE5C
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

	// Token: 0x0600A5A0 RID: 42400 RVA: 0x003DCCBC File Offset: 0x003DAEBC
	public override int Read(Span<char> _buffer)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		int num;
		return this.ReadCharBytes(_buffer, out num);
	}

	// Token: 0x0600A5A1 RID: 42401 RVA: 0x003DCCE5 File Offset: 0x003DAEE5
	public override int Read(Span<byte> _buffer)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		return this.baseStream.Read(_buffer);
	}

	// Token: 0x0600A5A2 RID: 42402 RVA: 0x003DCD08 File Offset: 0x003DAF08
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

	// Token: 0x0600A5A3 RID: 42403 RVA: 0x003DCD84 File Offset: 0x003DAF84
	public void ReadExact(Span<byte> _buffer)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		int num;
		for (int i = 0; i < _buffer.Length; i += num)
		{
			num = this.baseStream.Read(_buffer.Slice(i));
			if (num <= 0)
			{
				throw new EndOfStreamException();
			}
		}
	}

	// Token: 0x0600A5A4 RID: 42404 RVA: 0x003DCDD4 File Offset: 0x003DAFD4
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

	// Token: 0x0600A5A5 RID: 42405 RVA: 0x003DCE48 File Offset: 0x003DB048
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

	// Token: 0x0600A5A6 RID: 42406 RVA: 0x003DCEBC File Offset: 0x003DB0BC
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

	// Token: 0x0600A5A7 RID: 42407 RVA: 0x003DCF00 File Offset: 0x003DB100
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

	// Token: 0x0600A5A8 RID: 42408 RVA: 0x003DCF64 File Offset: 0x003DB164
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

	// Token: 0x0600A5A9 RID: 42409 RVA: 0x003DCFB1 File Offset: 0x003DB1B1
	[PublicizedFrom(EAccessModifier.Private)]
	public int ReadCharBytes(char[] _targetBuffer, int _targetIndex, int _count, out int _bytesRead)
	{
		return this.ReadCharBytes(_targetBuffer.AsSpan<char>(_targetIndex, _count), out _bytesRead);
	}

	// Token: 0x0600A5AA RID: 42410 RVA: 0x003DCFC4 File Offset: 0x003DB1C4
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

	// Token: 0x0600A5AB RID: 42411 RVA: 0x003DD03C File Offset: 0x003DB23C
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

	// Token: 0x0600A5AC RID: 42412 RVA: 0x003DD087 File Offset: 0x003DB287
	public void Flush()
	{
		if (this.baseStream != null)
		{
			this.baseStream.Flush();
		}
	}

	// Token: 0x0600A5AD RID: 42413 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A5AE RID: 42414 RVA: 0x003DD09C File Offset: 0x003DB29C
	public void Reset()
	{
		this.stringBuilder.Length = 0;
		this.baseStream = null;
	}

	// Token: 0x0600A5AF RID: 42415 RVA: 0x003DD0B1 File Offset: 0x003DB2B1
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A5B0 RID: 42416 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A5B1 RID: 42417 RVA: 0x003DD0B9 File Offset: 0x003DB2B9
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolBinaryReader.FreeSync(this);
	}

	// Token: 0x0600A5B2 RID: 42418 RVA: 0x003DD0C6 File Offset: 0x003DB2C6
	public bool ReadWrite(bool _value)
	{
		return this.ReadBoolean();
	}

	// Token: 0x0600A5B3 RID: 42419 RVA: 0x003DD0CE File Offset: 0x003DB2CE
	public byte ReadWrite(byte _value)
	{
		return this.ReadByte();
	}

	// Token: 0x0600A5B4 RID: 42420 RVA: 0x003DD0D6 File Offset: 0x003DB2D6
	public sbyte ReadWrite(sbyte _value)
	{
		return this.ReadSByte();
	}

	// Token: 0x0600A5B5 RID: 42421 RVA: 0x003DD0DE File Offset: 0x003DB2DE
	public char ReadWrite(char _value)
	{
		return this.ReadChar();
	}

	// Token: 0x0600A5B6 RID: 42422 RVA: 0x003DD0E6 File Offset: 0x003DB2E6
	public short ReadWrite(short _value)
	{
		return this.ReadInt16();
	}

	// Token: 0x0600A5B7 RID: 42423 RVA: 0x003DD0EE File Offset: 0x003DB2EE
	public ushort ReadWrite(ushort _value)
	{
		return this.ReadUInt16();
	}

	// Token: 0x0600A5B8 RID: 42424 RVA: 0x003DD0F6 File Offset: 0x003DB2F6
	public int ReadWrite(int _value)
	{
		return this.ReadInt32();
	}

	// Token: 0x0600A5B9 RID: 42425 RVA: 0x003DD0FE File Offset: 0x003DB2FE
	public uint ReadWrite(uint _value)
	{
		return this.ReadUInt32();
	}

	// Token: 0x0600A5BA RID: 42426 RVA: 0x003DD106 File Offset: 0x003DB306
	public long ReadWrite(long _value)
	{
		return this.ReadInt64();
	}

	// Token: 0x0600A5BB RID: 42427 RVA: 0x003DD10E File Offset: 0x003DB30E
	public ulong ReadWrite(ulong _value)
	{
		return this.ReadUInt64();
	}

	// Token: 0x0600A5BC RID: 42428 RVA: 0x003DD116 File Offset: 0x003DB316
	public float ReadWrite(float _value)
	{
		return this.ReadSingle();
	}

	// Token: 0x0600A5BD RID: 42429 RVA: 0x003DD11E File Offset: 0x003DB31E
	public double ReadWrite(double _value)
	{
		return this.ReadDouble();
	}

	// Token: 0x0600A5BE RID: 42430 RVA: 0x003DD126 File Offset: 0x003DB326
	public decimal ReadWrite(decimal _value)
	{
		return this.ReadDecimal();
	}

	// Token: 0x0600A5BF RID: 42431 RVA: 0x003DD12E File Offset: 0x003DB32E
	public string ReadWrite(string _value)
	{
		return this.ReadString();
	}

	// Token: 0x0600A5C0 RID: 42432 RVA: 0x003DD136 File Offset: 0x003DB336
	public void ReadWrite(byte[] _buffer, int _index, int _count)
	{
		this.Read(_buffer, _index, _count);
	}

	// Token: 0x0600A5C1 RID: 42433 RVA: 0x003DD144 File Offset: 0x003DB344
	public Vector3 ReadWrite(Vector3 _value)
	{
		Vector3 vector = _value;
		vector.x = this.ReadSingle();
		vector.y = this.ReadSingle();
		vector.z = this.ReadSingle();
		return vector;
	}

	// Token: 0x04007BA9 RID: 31657
	public static int INSTANCES_LIVE;

	// Token: 0x04007BAA RID: 31658
	public static int INSTANCES_MAX;

	// Token: 0x04007BAB RID: 31659
	public static int INSTANCES_CREATED;

	// Token: 0x04007BAC RID: 31660
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BYTE_BUFFER_SIZE = 128;

	// Token: 0x04007BAD RID: 31661
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHAR_BUFFER_SIZE = 128;

	// Token: 0x04007BAE RID: 31662
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] buffer = new byte[128];

	// Token: 0x04007BAF RID: 31663
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly char[] charBuffer = new char[128];

	// Token: 0x04007BB0 RID: 31664
	[PublicizedFrom(EAccessModifier.Private)]
	public Decoder decoder;

	// Token: 0x04007BB1 RID: 31665
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder stringBuilder = new StringBuilder(128);

	// Token: 0x04007BB2 RID: 31666
	[PublicizedFrom(EAccessModifier.Private)]
	public Stream baseStream;

	// Token: 0x04007BB3 RID: 31667
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoding encoding;

	// Token: 0x020014B0 RID: 5296
	public readonly struct StreamReadSizeMarker
	{
		// Token: 0x0600A5C2 RID: 42434 RVA: 0x003DD17B File Offset: 0x003DB37B
		public StreamReadSizeMarker(long _position, uint _expectedSize)
		{
			this.Position = _position;
			this.ExpectedSize = _expectedSize;
		}

		// Token: 0x04007BB4 RID: 31668
		public readonly long Position;

		// Token: 0x04007BB5 RID: 31669
		public readonly uint ExpectedSize;
	}
}
