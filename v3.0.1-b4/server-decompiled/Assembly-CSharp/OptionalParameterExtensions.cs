using System;

// Token: 0x02001450 RID: 5200
public static class OptionalParameterExtensions
{
	// Token: 0x0600A309 RID: 41737 RVA: 0x003CD77B File Offset: 0x003CB97B
	public static T OrIfNullThen<T>(this T element, T defaultValue) where T : class
	{
		if (element != null)
		{
			return element;
		}
		return defaultValue;
	}
}
