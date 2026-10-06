using System;
using System.IO;

// Token: 0x020013C2 RID: 5058
public static class BinaryReaderExtensions
{
	// Token: 0x06009F31 RID: 40753 RVA: 0x003C1B5C File Offset: 0x003BFD5C
	public static bool TryReadAllBytes(this BinaryReader reader, Span<byte> dest)
	{
		int num;
		return reader.TryReadAllBytes(dest, out num);
	}

	// Token: 0x06009F32 RID: 40754 RVA: 0x003C1B74 File Offset: 0x003BFD74
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
