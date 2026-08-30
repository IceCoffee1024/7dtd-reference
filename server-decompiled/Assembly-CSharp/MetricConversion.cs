using System;

// Token: 0x02001523 RID: 5411
public static class MetricConversion
{
	// Token: 0x0600AA06 RID: 43526 RVA: 0x003F714C File Offset: 0x003F534C
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

	// Token: 0x04007E41 RID: 32321
	public const double nanoToMilli = 1E-06;

	// Token: 0x04007E42 RID: 32322
	public const int kilobyte = 1024;

	// Token: 0x04007E43 RID: 32323
	public const int megabyte = 1048576;

	// Token: 0x04007E44 RID: 32324
	public const int gigabyte = 1073741824;

	// Token: 0x04007E45 RID: 32325
	public const double bytesToKilobyte = 0.0009765625;

	// Token: 0x04007E46 RID: 32326
	public const double bytesToMegabyte = 9.5367431640625E-07;

	// Token: 0x04007E47 RID: 32327
	public const double bytesToGigabyte = 9.313225746154785E-10;
}
