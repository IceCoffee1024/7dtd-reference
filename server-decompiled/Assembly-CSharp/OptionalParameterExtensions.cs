using System;

// Token: 0x0200149D RID: 5277
public static class OptionalParameterExtensions
{
	// Token: 0x0600A50A RID: 42250 RVA: 0x003D9AD3 File Offset: 0x003D7CD3
	public static T OrIfNullThen<T>(this T element, T defaultValue) where T : class
	{
		if (element != null)
		{
			return element;
		}
		return defaultValue;
	}
}
