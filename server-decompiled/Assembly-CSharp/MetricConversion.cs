using System;

// Token: 0x02001557 RID: 5463
public static class MetricConversion
{
	// Token: 0x0600AB11 RID: 43793 RVA: 0x003FE3A4 File Offset: 0x003FC5A4
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

	// Token: 0x04007F05 RID: 32517
	public const double nanoToMilli = 1E-06;

	// Token: 0x04007F06 RID: 32518
	public const int kilobyte = 1024;

	// Token: 0x04007F07 RID: 32519
	public const int megabyte = 1048576;

	// Token: 0x04007F08 RID: 32520
	public const int gigabyte = 1073741824;

	// Token: 0x04007F09 RID: 32521
	public const double bytesToKilobyte = 0.0009765625;

	// Token: 0x04007F0A RID: 32522
	public const double bytesToMegabyte = 9.5367431640625E-07;

	// Token: 0x04007F0B RID: 32523
	public const double bytesToGigabyte = 9.313225746154785E-10;
}
