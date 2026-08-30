using System;

namespace SharpEXR.Compression
{
	// Token: 0x020016DA RID: 5850
	public static class RLE
	{
		// Token: 0x0600B701 RID: 46849 RVA: 0x0043E408 File Offset: 0x0043C608
		public static int Uncompress(IEXRReader reader, int count, byte[] uncompressed)
		{
			int num = reader.Position + count;
			int num2 = uncompressed.Length;
			int num3 = 0;
			while (reader.Position < num)
			{
				int num4 = (int)((sbyte)reader.ReadByte());
				if (num4 < 0)
				{
					num4 = -num4;
					if (num3 + num4 >= num2)
					{
						reader.Position--;
						return 0;
					}
					reader.CopyBytes(uncompressed, num3, num4);
					num3 += num4;
				}
				else
				{
					if (num3 + num4 + 1 >= num2)
					{
						reader.Position--;
						return 0;
					}
					byte b = reader.ReadByte();
					RLE.MemSet(uncompressed, num3, b, num4 + 1);
					num3 += num4 + 1;
				}
			}
			return num3;
		}

		// Token: 0x0600B702 RID: 46850 RVA: 0x0043E498 File Offset: 0x0043C698
		public static void MemSet(byte[] array, int offset, byte value, int count)
		{
			int num = offset + count;
			for (int i = offset; i < num; i++)
			{
				array[i] = value;
			}
		}
	}
}
