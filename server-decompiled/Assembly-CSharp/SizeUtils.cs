using System;

// Token: 0x020014A5 RID: 5285
public static class SizeUtils
{
	// Token: 0x0600A5C8 RID: 42440 RVA: 0x003DA8D8 File Offset: 0x003D8AD8
	public static string FormatSize(this int sizeBytes, bool includeOriginalBytes = false)
	{
		if (Math.Abs(sizeBytes) < 1024)
		{
			return string.Format("{0} B", sizeBytes);
		}
		double num = (double)sizeBytes / 1024.0;
		string text = "kB";
		if (Math.Abs(num) >= 1024.0)
		{
			num /= 1024.0;
			text = "MB";
		}
		if (Math.Abs(num) >= 1024.0)
		{
			num /= 1024.0;
			text = "GB";
		}
		if (!includeOriginalBytes)
		{
			return string.Format("{0:F3} {1}", num, text);
		}
		return string.Format("{0:F3} {1} ({2} B)", num, text, sizeBytes);
	}

	// Token: 0x0600A5C9 RID: 42441 RVA: 0x003DA988 File Offset: 0x003D8B88
	public static string FormatSize(this uint sizeBytes, bool includeOriginalBytes = false)
	{
		if (sizeBytes < 1024U)
		{
			return string.Format("{0} B", sizeBytes);
		}
		double num = sizeBytes / 1024.0;
		string text = "kB";
		if (num >= 1024.0)
		{
			num /= 1024.0;
			text = "MB";
		}
		if (num >= 1024.0)
		{
			num /= 1024.0;
			text = "GB";
		}
		if (!includeOriginalBytes)
		{
			return string.Format("{0:F3} {1}", num, text);
		}
		return string.Format("{0:F3} {1} ({2} B)", num, text, sizeBytes);
	}

	// Token: 0x0600A5CA RID: 42442 RVA: 0x003DAA2C File Offset: 0x003D8C2C
	public static string FormatSize(this long sizeBytes, bool includeOriginalBytes = false)
	{
		if (Math.Abs(sizeBytes) < 1024L)
		{
			return string.Format("{0} B", sizeBytes);
		}
		double num = (double)sizeBytes / 1024.0;
		string text = "kB";
		if (Math.Abs(num) >= 1024.0)
		{
			num /= 1024.0;
			text = "MB";
		}
		if (Math.Abs(num) >= 1024.0)
		{
			num /= 1024.0;
			text = "GB";
		}
		if (Math.Abs(num) >= 1024.0)
		{
			num /= 1024.0;
			text = "TB";
		}
		if (Math.Abs(num) >= 1024.0)
		{
			num /= 1024.0;
			text = "PB";
		}
		if (Math.Abs(num) >= 1024.0)
		{
			num /= 1024.0;
			text = "EB";
		}
		if (!includeOriginalBytes)
		{
			return string.Format("{0:F3} {1}", num, text);
		}
		return string.Format("{0:F3} {1} ({2} B)", num, text, sizeBytes);
	}

	// Token: 0x0600A5CB RID: 42443 RVA: 0x003DAB48 File Offset: 0x003D8D48
	public static string FormatSize(this ulong sizeBytes, bool includeOriginalBytes = false)
	{
		if (sizeBytes < 1024UL)
		{
			return string.Format("{0} B", sizeBytes);
		}
		double num = sizeBytes / 1024.0;
		string text = "kB";
		if (num >= 1024.0)
		{
			num /= 1024.0;
			text = "MB";
		}
		if (num >= 1024.0)
		{
			num /= 1024.0;
			text = "GB";
		}
		if (num >= 1024.0)
		{
			num /= 1024.0;
			text = "TB";
		}
		if (num >= 1024.0)
		{
			num /= 1024.0;
			text = "PB";
		}
		if (num >= 1024.0)
		{
			num /= 1024.0;
			text = "EB";
		}
		if (!includeOriginalBytes)
		{
			return string.Format("{0:F3} {1}", num, text);
		}
		return string.Format("{0:F3} {1} ({2} B)", num, text, sizeBytes);
	}
}
