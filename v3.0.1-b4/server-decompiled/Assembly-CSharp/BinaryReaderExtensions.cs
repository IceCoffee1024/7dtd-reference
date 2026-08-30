using System;
using System.IO;

// Token: 0x02001387 RID: 4999
public static class BinaryReaderExtensions
{
	// Token: 0x06009D89 RID: 40329 RVA: 0x003B7C7C File Offset: 0x003B5E7C
	public static bool TryReadAllBytes(this BinaryReader reader, Span<byte> dest)
	{
		int num;
		return reader.TryReadAllBytes(dest, out num);
	}

	// Token: 0x06009D8A RID: 40330 RVA: 0x003B7C94 File Offset: 0x003B5E94
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
