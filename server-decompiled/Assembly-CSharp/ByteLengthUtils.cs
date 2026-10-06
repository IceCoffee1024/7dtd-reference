using System;
using System.Text;

// Token: 0x020013CE RID: 5070
public static class ByteLengthUtils
{
	// Token: 0x06009F78 RID: 40824 RVA: 0x003C3424 File Offset: 0x003C1624
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

	// Token: 0x06009F79 RID: 40825 RVA: 0x003C344C File Offset: 0x003C164C
	public static int GetBinaryWriterLength(this string text, Encoding encoding)
	{
		int byteCount = encoding.GetByteCount(text);
		return ByteLengthUtils.GetBinaryWriter7BitEncodedIntLength(byteCount) + byteCount;
	}
}
