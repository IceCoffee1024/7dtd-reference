using System;

// Token: 0x0200148A RID: 5258
public static class SizeUtils
{
	// Token: 0x0600A4CB RID: 42187 RVA: 0x003D5594 File Offset: 0x003D3794
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

	// Token: 0x0600A4CC RID: 42188 RVA: 0x003D5644 File Offset: 0x003D3844
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

	// Token: 0x0600A4CD RID: 42189 RVA: 0x003D56E8 File Offset: 0x003D38E8
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

	// Token: 0x0600A4CE RID: 42190 RVA: 0x003D5804 File Offset: 0x003D3A04
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
