using System;

// Token: 0x020013D5 RID: 5077
[PublicizedFrom(EAccessModifier.Internal)]
public static class BitConverterLE
{
	// Token: 0x06009F8B RID: 40843 RVA: 0x003C3FBF File Offset: 0x003C21BF
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

	// Token: 0x06009F8C RID: 40844 RVA: 0x003C4000 File Offset: 0x003C2200
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

	// Token: 0x06009F8D RID: 40845 RVA: 0x003C4081 File Offset: 0x003C2281
	public unsafe static void GetBytes(float _value, byte[] _buffer)
	{
		BitConverterLE.GetUIntBytes((byte*)(&_value), _buffer);
	}

	// Token: 0x06009F8E RID: 40846 RVA: 0x003C408C File Offset: 0x003C228C
	public unsafe static void GetBytes(double value, byte[] _buffer)
	{
		BitConverterLE.GetULongBytes((byte*)(&value), _buffer);
	}

	// Token: 0x06009F8F RID: 40847 RVA: 0x003C4098 File Offset: 0x003C2298
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

	// Token: 0x06009F90 RID: 40848 RVA: 0x003C40F0 File Offset: 0x003C22F0
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

	// Token: 0x06009F91 RID: 40849 RVA: 0x003C4134 File Offset: 0x003C2334
	public unsafe static float ToSingle(byte[] _value, int _startIndex)
	{
		float num;
		BitConverterLE.UIntFromBytes((byte*)(&num), _value, _startIndex);
		return num;
	}

	// Token: 0x06009F92 RID: 40850 RVA: 0x003C414C File Offset: 0x003C234C
	public unsafe static double ToDouble(byte[] _value, int _startIndex)
	{
		double num;
		BitConverterLE.ULongFromBytes((byte*)(&num), _value, _startIndex);
		return num;
	}
}
