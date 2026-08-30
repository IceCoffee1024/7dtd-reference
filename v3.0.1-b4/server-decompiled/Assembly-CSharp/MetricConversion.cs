using System;

// Token: 0x02001508 RID: 5384
public static class MetricConversion
{
	// Token: 0x0600A909 RID: 43273 RVA: 0x003F1E0C File Offset: 0x003F000C
	public static string ToShortestBytesString(long bytes)
	{
		if (bytes < 1024L)
		{
			return string.Format("{0} B", bytes);
		}
		if (bytes < 1048576L)
		{
			return string.Format("{0:F2} KB", 0.0009765625 * (double)bytes);
		}
		if (bytes < 1073741824L)
		{
			return string.Format("{0:F2} MB", 9.5367431640625E-07 * (double)bytes);
		}
		return string.Format("{0:F2} GB", 9.313225746154785E-10 * (double)bytes);
	}

	// Token: 0x04007D68 RID: 32104
	public const double nanoToMilli = 1E-06;

	// Token: 0x04007D69 RID: 32105
	public const int kilobyte = 1024;

	// Token: 0x04007D6A RID: 32106
	public const int megabyte = 1048576;

	// Token: 0x04007D6B RID: 32107
	public const int gigabyte = 1073741824;

	// Token: 0x04007D6C RID: 32108
	public const double bytesToKilobyte = 0.0009765625;

	// Token: 0x04007D6D RID: 32109
	public const double bytesToMegabyte = 9.5367431640625E-07;

	// Token: 0x04007D6E RID: 32110
	public const double bytesToGigabyte = 9.313225746154785E-10;
}
