using System;
using System.IO;

// Token: 0x020013D4 RID: 5076
public static class BinaryReaderExtensions
{
	// Token: 0x06009F89 RID: 40841 RVA: 0x003C3F60 File Offset: 0x003C2160
	public static bool TryReadAllBytes(this BinaryReader reader, Span<byte> dest)
	{
		int num;
		return reader.TryReadAllBytes(dest, out num);
	}

	// Token: 0x06009F8A RID: 40842 RVA: 0x003C3F78 File Offset: 0x003C2178
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
