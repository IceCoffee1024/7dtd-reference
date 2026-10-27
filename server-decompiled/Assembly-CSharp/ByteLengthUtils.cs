using System;
using System.Text;

// Token: 0x020013E0 RID: 5088
public static class ByteLengthUtils
{
	// Token: 0x06009FD0 RID: 40912 RVA: 0x003C5828 File Offset: 0x003C3A28
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

	// Token: 0x06009FD1 RID: 40913 RVA: 0x003C5850 File Offset: 0x003C3A50
	public static int GetBinaryWriterLength(this string text, Encoding encoding)
	{
		int byteCount = encoding.GetByteCount(text);
		return ByteLengthUtils.GetBinaryWriter7BitEncodedIntLength(byteCount) + byteCount;
	}
}
