using System;

// Token: 0x02001545 RID: 5445
public static class MetricConversion
{
	// Token: 0x0600AAB9 RID: 43705 RVA: 0x003FBFA0 File Offset: 0x003FA1A0
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

	// Token: 0x04007EC4 RID: 32452
	public const double nanoToMilli = 1E-06;

	// Token: 0x04007EC5 RID: 32453
	public const int kilobyte = 1024;

	// Token: 0x04007EC6 RID: 32454
	public const int megabyte = 1048576;

	// Token: 0x04007EC7 RID: 32455
	public const int gigabyte = 1073741824;

	// Token: 0x04007EC8 RID: 32456
	public const double bytesToKilobyte = 0.0009765625;

	// Token: 0x04007EC9 RID: 32457
	public const double bytesToMegabyte = 9.5367431640625E-07;

	// Token: 0x04007ECA RID: 32458
	public const double bytesToGigabyte = 9.313225746154785E-10;
}
