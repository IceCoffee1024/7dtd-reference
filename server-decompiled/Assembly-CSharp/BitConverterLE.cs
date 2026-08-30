using System;

// Token: 0x020013A3 RID: 5027
[PublicizedFrom(EAccessModifier.Internal)]
public static class BitConverterLE
{
	// Token: 0x06009E88 RID: 40584 RVA: 0x003BD01F File Offset: 0x003BB21F
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static void GetUIntBytes(byte* bytes, byte[] _buffer)
	{
		if (BitConverter.IsLittleEndian)
		{
			_buffer[0] = *bytes;
			_buffer[1] = bytes[1];
			_buffer[2] = bytes[2];
			_buffer[3] = bytes[3];
			return;
		}
		_buffer[0] = bytes[3];
		_buffer[1] = bytes[2];
		_buffer[2] = bytes[1];
		_buffer[3] = *bytes;
	}

	// Token: 0x06009E89 RID: 40585 RVA: 0x003BD060 File Offset: 0x003BB260
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static void GetULongBytes(byte* bytes, byte[] _buffer)
	{
		if (BitConverter.IsLittleEndian)
		{
			_buffer[0] = *bytes;
			_buffer[1] = bytes[1];
			_buffer[2] = bytes[2];
			_buffer[3] = bytes[3];
			_buffer[4] = bytes[4];
			_buffer[5] = bytes[5];
			_buffer[6] = bytes[6];
			_buffer[7] = bytes[7];
			return;
		}
		_buffer[0] = bytes[7];
		_buffer[1] = bytes[6];
		_buffer[2] = bytes[5];
		_buffer[3] = bytes[4];
		_buffer[4] = bytes[3];
		_buffer[5] = bytes[2];
		_buffer[6] = bytes[1];
		_buffer[7] = *bytes;
	}

	// Token: 0x06009E8A RID: 40586 RVA: 0x003BD0E1 File Offset: 0x003BB2E1
	public unsafe static void GetBytes(float _value, byte[] _buffer)
	{
		BitConverterLE.GetUIntBytes((byte*)(&_value), _buffer);
	}

	// Token: 0x06009E8B RID: 40587 RVA: 0x003BD0EC File Offset: 0x003BB2EC
	public unsafe static void GetBytes(double value, byte[] _buffer)
	{
		BitConverterLE.GetULongBytes((byte*)(&value), _buffer);
	}

	// Token: 0x06009E8C RID: 40588 RVA: 0x003BD0F8 File Offset: 0x003BB2F8
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static void UIntFromBytes(byte* _dst, byte[] _src, int _startIndex)
	{
		if (BitConverter.IsLittleEndian)
		{
			*_dst = _src[_startIndex];
			_dst[1] = _src[_startIndex + 1];
			_dst[2] = _src[_startIndex + 2];
			_dst[3] = _src[_startIndex + 3];
			return;
		}
		*_dst = _src[_startIndex + 3];
		_dst[1] = _src[_startIndex + 2];
		_dst[2] = _src[_startIndex + 1];
		_dst[3] = _src[_startIndex];
	}

	// Token: 0x06009E8D RID: 40589 RVA: 0x003BD150 File Offset: 0x003BB350
	[PublicizedFrom(EAccessModifier.Private)]
	public unsafe static void ULongFromBytes(byte* _dst, byte[] _src, int _startIndex)
	{
		if (BitConverter.IsLittleEndian)
		{
			for (int i = 0; i < 8; i++)
			{
				_dst[i] = _src[_startIndex + i];
			}
			return;
		}
		for (int j = 0; j < 8; j++)
		{
			_dst[j] = _src[_startIndex + (7 - j)];
		}
	}

	// Token: 0x06009E8E RID: 40590 RVA: 0x003BD194 File Offset: 0x003BB394
	public unsafe static float ToSingle(byte[] _value, int _startIndex)
	{
		float num;
		BitConverterLE.UIntFromBytes((byte*)(&num), _value, _startIndex);
		return num;
	}

	// Token: 0x06009E8F RID: 40591 RVA: 0x003BD1AC File Offset: 0x003BB3AC
	public unsafe static double ToDouble(byte[] _value, int _startIndex)
	{
		double num;
		BitConverterLE.ULongFromBytes((byte*)(&num), _value, _startIndex);
		return num;
	}
}
