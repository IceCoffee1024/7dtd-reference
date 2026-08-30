using System;
using System.Text;

// Token: 0x020013AE RID: 5038
public static class ByteLengthUtils
{
	// Token: 0x06009ECD RID: 40653 RVA: 0x003BE888 File Offset: 0x003BCA88
	[PublicizedFrom(EAccessModifier.Private)]
	public static int GetBinaryWriter7BitEncodedIntLength(int value)
	{
		int num = 0;
		for (uint num2 = (uint)value; num2 >= 128U; num2 >>= 7)
		{
			num++;
		}
		return num + 1;
	}

	// Token: 0x06009ECE RID: 40654 RVA: 0x003BE8B0 File Offset: 0x003BCAB0
	public static int GetBinaryWriterLength(this string text, Encoding encoding)
	{
		int byteCount = encoding.GetByteCount(text);
		return ByteLengthUtils.GetBinaryWriter7BitEncodedIntLength(byteCount) + byteCount;
	}
}
