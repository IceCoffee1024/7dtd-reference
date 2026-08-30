using System;
using System.IO;

// Token: 0x020013A2 RID: 5026
public static class BinaryReaderExtensions
{
	// Token: 0x06009E86 RID: 40582 RVA: 0x003BCFC0 File Offset: 0x003BB1C0
	public static bool TryReadAllBytes(this BinaryReader reader, Span<byte> dest)
	{
		int num;
		return reader.TryReadAllBytes(dest, out num);
	}

	// Token: 0x06009E87 RID: 40583 RVA: 0x003BCFD8 File Offset: 0x003BB1D8
	public static bool TryReadAllBytes(this BinaryReader reader, Span<byte> dest, out int totalBytesRead)
	{
		int num2;
		for (totalBytesRead = 0; totalBytesRead < dest.Length; totalBytesRead += num2)
		{
			Span<byte> span = dest;
			int num = totalBytesRead;
			num2 = reader.Read(span.Slice(num, span.Length - num));
			if (num2 <= 0)
			{
				return false;
			}
		}
		return true;
	}
}
