using System;
using System.Text;

// Token: 0x02001393 RID: 5011
public static class ByteLengthUtils
{
	// Token: 0x06009DD0 RID: 40400 RVA: 0x003B9544 File Offset: 0x003B7744
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

	// Token: 0x06009DD1 RID: 40401 RVA: 0x003B956C File Offset: 0x003B776C
	public static int GetBinaryWriterLength(this string text, Encoding encoding)
	{
		int byteCount = encoding.GetByteCount(text);
		return ByteLengthUtils.GetBinaryWriter7BitEncodedIntLength(byteCount) + byteCount;
	}
}
