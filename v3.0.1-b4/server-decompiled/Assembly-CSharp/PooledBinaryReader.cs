using System;
using System.IO;
using System.Text;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;

// Token: 0x02001462 RID: 5218
public class PooledBinaryReader : BinaryReader, IBinaryReaderOrWriter, IMemoryPoolableObject, IDisposable
{
	// Token: 0x17001320 RID: 4896
	// (get) Token: 0x0600A386 RID: 41862 RVA: 0x003D0362 File Offset: 0x003CE562
	public override Stream BaseStream
	{
		get
		{
			return this.baseStream;
		}
	}

	// Token: 0x17001321 RID: 4897
	// (get) Token: 0x0600A387 RID: 41863 RVA: 0x003D036A File Offset: 0x003CE56A
	// (set) Token: 0x0600A388 RID: 41864 RVA: 0x003D0372 File Offset: 0x003CE572
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

	// Token: 0x0600A389 RID: 41865 RVA: 0x003D0390 File Offset: 0x003CE590
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

	// Token: 0x0600A38A RID: 41866 RVA: 0x003D0418 File Offset: 0x003CE618
	[PublicizedFrom(EAccessModifier.Protected)]
	public ~PooledBinaryReader()
	{
		Interlocked.Decrement(ref PooledBinaryReader.INSTANCES_LIVE);
	}

	// Token: 0x0600A38B RID: 41867 RVA: 0x003D044C File Offset: 0x003CE64C
	public void SetBaseStream(Stream _stream)
	{
		if (_stream != null && !_stream.CanRead)
		{
			throw new ArgumentException("The stream doesn't support reading.");
		}
		this.baseStream = _stream;
		this.decoder = null;
	}

	// Token: 0x0600A38C RID: 41868 RVA: 0x003D0474 File Offset: 0x003CE674
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

	// Token: 0x0600A38D RID: 41869 RVA: 0x003D04D3 File Offset: 0x003CE6D3
	public override int Read()
	{
		if (this.Read(this.charBuffer, 0, 1) == 0)
		{
			return -1;
		}
		return (int)this.charBuffer[0];
	}

	// Token: 0x0600A38E RID: 41870 RVA: 0x003D04EF File Offset: 0x003CE6EF
	public override bool ReadBoolean()
	{
		return this.ReadByte() > 0;
	}

	// Token: 0x0600A38F RID: 41871 RVA: 0x003D04FA File Offset: 0x003CE6FA
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

	// Token: 0x0600A390 RID: 41872 RVA: 0x003D0525 File Offset: 0x003CE725
	public override sbyte ReadSByte()
	{
		return (sbyte)this.ReadByte();
	}

	// Token: 0x0600A391 RID: 41873 RVA: 0x003D052E File Offset: 0x003CE72E
	public override char ReadChar()
	{
		int num = this.Read();
		if (num == -1)
		{
			throw new EndOfStreamException();
		}
		return (char)num;
	}

	// Token: 0x0600A392 RID: 41874 RVA: 0x003D0541 File Offset: 0x003CE741
	public override short ReadInt16()
	{
		this.FillBuffer(2);
		return (short)((int)this.buffer[0] | ((int)this.buffer[1] << 8));
	}

	// Token: 0x0600A393 RID: 41875 RVA: 0x003D055E File Offset: 0x003CE75E
	public override ushort ReadUInt16()
	{
		this.FillBuffer(2);
		return (ushort)((int)this.buffer[0] | ((int)this.buffer[1] << 8));
	}

	// Token: 0x0600A394 RID: 41876 RVA: 0x003D057B File Offset: 0x003CE77B
	public override int ReadInt32()
	{
		this.FillBuffer(4);
		return (int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24);
	}

	// Token: 0x0600A395 RID: 41877 RVA: 0x003D057B File Offset: 0x003CE77B
	public override uint ReadUInt32()
	{
		this.FillBuffer(4);
		return (uint)((int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24));
	}

	// Token: 0x0600A396 RID: 41878 RVA: 0x003D05B0 File Offset: 0x003CE7B0
	public override long ReadInt64()
	{
		this.FillBuffer(8);
		uint num = (uint)((int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24));
		return (long)(((ulong)((int)this.buffer[4] | ((int)this.buffer[5] << 8) | ((int)this.buffer[6] << 16) | ((int)this.buffer[7] << 24)) << 32) | (ulong)num);
	}

	// Token: 0x0600A397 RID: 41879 RVA: 0x003D0624 File Offset: 0x003CE824
	public override ulong ReadUInt64()
	{
		this.FillBuffer(8);
		uint num = (uint)((int)this.buffer[0] | ((int)this.buffer[1] << 8) | ((int)this.buffer[2] << 16) | ((int)this.buffer[3] << 24));
		return ((ulong)((int)this.buffer[4] | ((int)this.buffer[5] << 8) | ((int)this.buffer[6] << 16) | ((int)this.buffer[7] << 24)) << 32) | (ulong)num;
	}

	// Token: 0x0600A398 RID: 41880 RVA: 0x003D0696 File Offset: 0x003CE896
	public override float ReadSingle()
	{
		this.FillBuffer(4);
		return BitConverterLE.ToSingle(this.buffer, 0);
	}

	// Token: 0x0600A399 RID: 41881 RVA: 0x003D06AB File Offset: 0x003CE8AB
	public override double ReadDouble()
	{
		this.FillBuffer(8);
		return BitConverterLE.ToDouble(this.buffer, 0);
	}

	// Token: 0x0600A39A RID: 41882 RVA: 0x003D06C0 File Offset: 0x003CE8C0
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

	// Token: 0x0600A39B RID: 41883 RVA: 0x003D07A4 File Offset: 0x003CE9A4
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

	// Token: 0x0600A39C RID: 41884 RVA: 0x003D085C File Offset: 0x003CEA5C
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

	// Token: 0x0600A39D RID: 41885 RVA: 0x003D08D8 File Offset: 0x003CEAD8
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

	// Token: 0x0600A39E RID: 41886 RVA: 0x003D0938 File Offset: 0x003CEB38
	public override int Read(Span<char> _buffer)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		int num;
		return this.ReadCharBytes(_buffer, out num);
	}

	// Token: 0x0600A39F RID: 41887 RVA: 0x003D0961 File Offset: 0x003CEB61
	public override int Read(Span<byte> _buffer)
	{
		if (this.baseStream == null)
		{
			throw new IOException("Stream is invalid");
		}
		return this.baseStream.Read(_buffer);
	}

	// Token: 0x0600A3A0 RID: 41888 RVA: 0x003D0984 File Offset: 0x003CEB84
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

	// Token: 0x0600A3A1 RID: 41889 RVA: 0x003D0A00 File Offset: 0x003CEC00
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

	// Token: 0x0600A3A2 RID: 41890 RVA: 0x003D0A74 File Offset: 0x003CEC74
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

	// Token: 0x0600A3A3 RID: 41891 RVA: 0x003D0AE8 File Offset: 0x003CECE8
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

	// Token: 0x0600A3A4 RID: 41892 RVA: 0x003D0B2C File Offset: 0x003CED2C
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

	// Token: 0x0600A3A5 RID: 41893 RVA: 0x003D0B90 File Offset: 0x003CED90
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

	// Token: 0x0600A3A6 RID: 41894 RVA: 0x003D0BDD File Offset: 0x003CEDDD
	[PublicizedFrom(EAccessModifier.Private)]
	public int ReadCharBytes(char[] _targetBuffer, int _targetIndex, int _count, out int _bytesRead)
	{
		return this.ReadCharBytes(_targetBuffer.AsSpan<char>(_targetIndex, _count), out _bytesRead);
	}

	// Token: 0x0600A3A7 RID: 41895 RVA: 0x003D0BF0 File Offset: 0x003CEDF0
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

	// Token: 0x0600A3A8 RID: 41896 RVA: 0x003D0C68 File Offset: 0x003CEE68
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

	// Token: 0x0600A3A9 RID: 41897 RVA: 0x003D0CB3 File Offset: 0x003CEEB3
	public void Flush()
	{
		if (this.baseStream != null)
		{
			this.baseStream.Flush();
		}
	}

	// Token: 0x0600A3AA RID: 41898 RVA: 0x000027FC File Offset: 0x000009FC
	public override void Close()
	{
	}

	// Token: 0x0600A3AB RID: 41899 RVA: 0x003D0CC8 File Offset: 0x003CEEC8
	public void Reset()
	{
		this.stringBuilder.Length = 0;
		this.baseStream = null;
	}

	// Token: 0x0600A3AC RID: 41900 RVA: 0x003D0CDD File Offset: 0x003CEEDD
	public void Cleanup()
	{
		this.Reset();
	}

	// Token: 0x0600A3AD RID: 41901 RVA: 0x000027FC File Offset: 0x000009FC
	[PublicizedFrom(EAccessModifier.Protected)]
	public override void Dispose(bool _disposing)
	{
	}

	// Token: 0x0600A3AE RID: 41902 RVA: 0x003D0CE5 File Offset: 0x003CEEE5
	[PublicizedFrom(EAccessModifier.Private)]
	public void System.IDisposable.Dispose()
	{
		MemoryPools.poolBinaryReader.FreeSync(this);
	}

	// Token: 0x0600A3AF RID: 41903 RVA: 0x003D0CF2 File Offset: 0x003CEEF2
	public bool ReadWrite(bool _value)
	{
		return this.ReadBoolean();
	}

	// Token: 0x0600A3B0 RID: 41904 RVA: 0x003D0CFA File Offset: 0x003CEEFA
	public byte ReadWrite(byte _value)
	{
		return this.ReadByte();
	}

	// Token: 0x0600A3B1 RID: 41905 RVA: 0x003D0D02 File Offset: 0x003CEF02
	public sbyte ReadWrite(sbyte _value)
	{
		return this.ReadSByte();
	}

	// Token: 0x0600A3B2 RID: 41906 RVA: 0x003D0D0A File Offset: 0x003CEF0A
	public char ReadWrite(char _value)
	{
		return this.ReadChar();
	}

	// Token: 0x0600A3B3 RID: 41907 RVA: 0x003D0D12 File Offset: 0x003CEF12
	public short ReadWrite(short _value)
	{
		return this.ReadInt16();
	}

	// Token: 0x0600A3B4 RID: 41908 RVA: 0x003D0D1A File Offset: 0x003CEF1A
	public ushort ReadWrite(ushort _value)
	{
		return this.ReadUInt16();
	}

	// Token: 0x0600A3B5 RID: 41909 RVA: 0x003D0D22 File Offset: 0x003CEF22
	public int ReadWrite(int _value)
	{
		return this.ReadInt32();
	}

	// Token: 0x0600A3B6 RID: 41910 RVA: 0x003D0D2A File Offset: 0x003CEF2A
	public uint ReadWrite(uint _value)
	{
		return this.ReadUInt32();
	}

	// Token: 0x0600A3B7 RID: 41911 RVA: 0x003D0D32 File Offset: 0x003CEF32
	public long ReadWrite(long _value)
	{
		return this.ReadInt64();
	}

	// Token: 0x0600A3B8 RID: 41912 RVA: 0x003D0D3A File Offset: 0x003CEF3A
	public ulong ReadWrite(ulong _value)
	{
		return this.ReadUInt64();
	}

	// Token: 0x0600A3B9 RID: 41913 RVA: 0x003D0D42 File Offset: 0x003CEF42
	public float ReadWrite(float _value)
	{
		return this.ReadSingle();
	}

	// Token: 0x0600A3BA RID: 41914 RVA: 0x003D0D4A File Offset: 0x003CEF4A
	public double ReadWrite(double _value)
	{
		return this.ReadDouble();
	}

	// Token: 0x0600A3BB RID: 41915 RVA: 0x003D0D52 File Offset: 0x003CEF52
	public decimal ReadWrite(decimal _value)
	{
		return this.ReadDecimal();
	}

	// Token: 0x0600A3BC RID: 41916 RVA: 0x003D0D5A File Offset: 0x003CEF5A
	public string ReadWrite(string _value)
	{
		return this.ReadString();
	}

	// Token: 0x0600A3BD RID: 41917 RVA: 0x003D0D62 File Offset: 0x003CEF62
	public void ReadWrite(byte[] _buffer, int _index, int _count)
	{
		this.Read(_buffer, _index, _count);
	}

	// Token: 0x0600A3BE RID: 41918 RVA: 0x003D0D70 File Offset: 0x003CEF70
	public Vector3 ReadWrite(Vector3 _value)
	{
		Vector3 vector = _value;
		vector.x = this.ReadSingle();
		vector.y = this.ReadSingle();
		vector.z = this.ReadSingle();
		return vector;
	}

	// Token: 0x04007A14 RID: 31252
	public static int INSTANCES_LIVE;

	// Token: 0x04007A15 RID: 31253
	public static int INSTANCES_MAX;

	// Token: 0x04007A16 RID: 31254
	public static int INSTANCES_CREATED;

	// Token: 0x04007A17 RID: 31255
	[PublicizedFrom(EAccessModifier.Private)]
	public const int BYTE_BUFFER_SIZE = 128;

	// Token: 0x04007A18 RID: 31256
	[PublicizedFrom(EAccessModifier.Private)]
	public const int CHAR_BUFFER_SIZE = 128;

	// Token: 0x04007A19 RID: 31257
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly byte[] buffer = new byte[128];

	// Token: 0x04007A1A RID: 31258
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly char[] charBuffer = new char[128];

	// Token: 0x04007A1B RID: 31259
	[PublicizedFrom(EAccessModifier.Private)]
	public Decoder decoder;

	// Token: 0x04007A1C RID: 31260
	[PublicizedFrom(EAccessModifier.Private)]
	public readonly StringBuilder stringBuilder = new StringBuilder(128);

	// Token: 0x04007A1D RID: 31261
	[PublicizedFrom(EAccessModifier.Private)]
	public Stream baseStream;

	// Token: 0x04007A1E RID: 31262
	[PublicizedFrom(EAccessModifier.Private)]
	public Encoding encoding;

	// Token: 0x02001463 RID: 5219
	public readonly struct StreamReadSizeMarker
	{
		// Token: 0x0600A3BF RID: 41919 RVA: 0x003D0DA7 File Offset: 0x003CEFA7
		public StreamReadSizeMarker(long _position, uint _expectedSize)
		{
			this.Position = _position;
			this.ExpectedSize = _expectedSize;
		}

		// Token: 0x04007A1F RID: 31263
		public readonly long Position;

		// Token: 0x04007A20 RID: 31264
		public readonly uint ExpectedSize;
	}
}
