using System;

// Token: 0x0200146B RID: 5227
public static class OptionalParameterExtensions
{
	// Token: 0x0600A406 RID: 41990 RVA: 0x003D2ABF File Offset: 0x003D0CBF
	public static T OrIfNullThen<T>(this T element, T defaultValue) where T : class
	{
		if (element != null)
		{
			return element;
		}
		return defaultValue;
	}
}
