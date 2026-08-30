using System;
using System.Globalization;

// Token: 0x020014A6 RID: 5286
public static class StringUtils
{
	// Token: 0x0600A5F7 RID: 42487 RVA: 0x003DBF26 File Offset: 0x003DA126
	[PublicizedFrom(EAccessModifier.Private)]
	static StringUtils()
	{
		Localization.LanguageSelected += StringUtils.OnLanguageSelected;
		StringUtils.OnLanguageSelected(Localization.RequestedLanguage);
	}

	// Token: 0x0600A5F8 RID: 42488 RVA: 0x003DBF54 File Offset: 0x003DA154
	[PublicizedFrom(EAccessModifier.Private)]
	public static void OnLanguageSelected(string _lang)
	{
		string text = Localization.Get("cultureInfoName", false, null);
		if (string.IsNullOrEmpty(text))
		{
			Log.Warning("No culture info name given for selected language: " + _lang);
			return;
		}
		TextInfo textInfo;
		try
		{
			textInfo = CultureInfo.GetCultureInfo(text).TextInfo;
		}
		catch (Exception)
		{
			Log.Warning(string.Concat(new string[] { "No culture info found for given name: ", text, " (language: ", _lang, ")" }));
			return;
		}
		if (textInfo.CultureName != StringUtils.textInfo.CultureName)
		{
			StringUtils.textInfo = textInfo;
			Log.Out("Updated culture for display texts");
		}
	}

	// Token: 0x0600A5F9 RID: 42489 RVA: 0x003DC000 File Offset: 0x003DA200
	public static string ToLowerWithUserLocale(this string _value)
	{
		return StringUtils.textInfo.ToLower(_value);
	}

	// Token: 0x0600A5FA RID: 42490 RVA: 0x003DC00D File Offset: 0x003DA20D
	public static string ToUpperWithUserLocale(this string _value)
	{
		return StringUtils.textInfo.ToUpper(_value);
	}

	// Token: 0x04007B03 RID: 31491
	[PublicizedFrom(EAccessModifier.Private)]
	public static TextInfo textInfo = Utils.StandardCulture.TextInfo;
}
