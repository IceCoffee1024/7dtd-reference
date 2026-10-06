using System;

// Token: 0x0200148B RID: 5259
public static class OptionalParameterExtensions
{
	// Token: 0x0600A4B2 RID: 42162 RVA: 0x003D76CF File Offset: 0x003D58CF
	public static T OrIfNullThen<T>(this T element, T defaultValue) where T : class
	{
		if (element != null)
		{
			return element;
		}
		return defaultValue;
	}
}
