using System;

// Token: 0x02001388 RID: 5000
[PublicizedFrom(EAccessModifier.Internal)]
public static class BitConverterLE
{
	// Token: 0x06009D8B RID: 40331 RVA: 0x003B7CDB File Offset: 0x003B5EDB
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

	// Token: 0x06009D8C RID: 40332 RVA: 0x003B7D1C File Offset: 0x003B5F1C
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

	// Token: 0x06009D8D RID: 40333 RVA: 0x003B7D9D File Offset: 0x003B5F9D
	public unsafe static void GetBytes(float _value, byte[] _buffer)
	{
		BitConverterLE.GetUIntBytes((byte*)(&_value), _buffer);
	}

	// Token: 0x06009D8E RID: 40334 RVA: 0x003B7DA8 File Offset: 0x003B5FA8
	public unsafe static void GetBytes(double value, byte[] _buffer)
	{
		BitConverterLE.GetULongBytes((byte*)(&value), _buffer);
	}

	// Token: 0x06009D8F RID: 40335 RVA: 0x003B7DB4 File Offset: 0x003B5FB4
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

	// Token: 0x06009D90 RID: 40336 RVA: 0x003B7E0C File Offset: 0x003B600C
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

	// Token: 0x06009D91 RID: 40337 RVA: 0x003B7E50 File Offset: 0x003B6050
	public unsafe static float ToSingle(byte[] _value, int _startIndex)
	{
		float num;
		BitConverterLE.UIntFromBytes((byte*)(&num), _value, _startIndex);
		return num;
	}

	// Token: 0x06009D92 RID: 40338 RVA: 0x003B7E68 File Offset: 0x003B6068
	public unsafe static double ToDouble(byte[] _value, int _startIndex)
	{
		double num;
		BitConverterLE.ULongFromBytes((byte*)(&num), _value, _startIndex);
		return num;
	}
}
