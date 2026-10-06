using System;

// Token: 0x020013C3 RID: 5059
[PublicizedFrom(EAccessModifier.Internal)]
public static class BitConverterLE
{
	// Token: 0x06009F33 RID: 40755 RVA: 0x003C1BBB File Offset: 0x003BFDBB
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

	// Token: 0x06009F34 RID: 40756 RVA: 0x003C1BFC File Offset: 0x003BFDFC
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

	// Token: 0x06009F35 RID: 40757 RVA: 0x003C1C7D File Offset: 0x003BFE7D
	public unsafe static void GetBytes(float _value, byte[] _buffer)
	{
		BitConverterLE.GetUIntBytes((byte*)(&_value), _buffer);
	}

	// Token: 0x06009F36 RID: 40758 RVA: 0x003C1C88 File Offset: 0x003BFE88
	public unsafe static void GetBytes(double value, byte[] _buffer)
	{
		BitConverterLE.GetULongBytes((byte*)(&value), _buffer);
	}

	// Token: 0x06009F37 RID: 40759 RVA: 0x003C1C94 File Offset: 0x003BFE94
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

	// Token: 0x06009F38 RID: 40760 RVA: 0x003C1CEC File Offset: 0x003BFEEC
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

	// Token: 0x06009F39 RID: 40761 RVA: 0x003C1D30 File Offset: 0x003BFF30
	public unsafe static float ToSingle(byte[] _value, int _startIndex)
	{
		float num;
		BitConverterLE.UIntFromBytes((byte*)(&num), _value, _startIndex);
		return num;
	}

	// Token: 0x06009F3A RID: 40762 RVA: 0x003C1D48 File Offset: 0x003BFF48
	public unsafe static double ToDouble(byte[] _value, int _startIndex)
	{
		double num;
		BitConverterLE.ULongFromBytes((byte*)(&num), _value, _startIndex);
		return num;
	}
}
